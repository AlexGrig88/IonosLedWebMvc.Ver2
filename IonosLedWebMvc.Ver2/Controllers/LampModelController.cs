using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IonosLedWebMvc.Ver2.Data;
using IonosLedWebMvc.Ver2.Dtos;
using IonosLedWebMvc.Ver2.Services;
using IonosLedWebMvc.Ver2.Models;
using IonosLedWebMvc.Ver2.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Vml;

namespace IonosLedWebMvc.Ver2.Controllers
{
    public class LampModelController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly LampService _lampService;
        private readonly LampModelService _lampModelService;

        public LampModelController(ApplicationContext context, LampService lampService, LampModelService lampModelService)
        {
            _context = context;
            _lampService = lampService;
            _lampModelService = lampModelService;
        }

        // GET: LampModel
        public async Task<IActionResult> Index()
        {
            var models = await _context.LampModels.ToListAsync();
            var modelNameToCount = await _lampService.GetCountedModelsAsync();
            return View(models.Select(m => LampModelDto.FromLampModel(m, modelNameToCount)).OrderByDescending(m => m.CountReleased));
        }



		// GET: LampModel/Create
		[Authorize]
		public IActionResult Create()
        {
            return View();
        }

        // POST: LampModel/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,ModelName,Sections,CutPrice,DrillPrice,MountPrice,SolderPrice,AssemblyPrice,CheckPrice")] LampModelDto lampModelDto)
        {
            if (ModelState.IsValid)
            {
                _context.LampModels.Add(LampModelDto.ToLampModel(lampModelDto));
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lampModelDto);
        }

        // GET: LampModel/Edit/5
        // resultFilesInfo - параметр вида "resultUploads;FileName1,FileExtensions1,FileSize1;FileName2,FileExtensions2,FileSize2;..."
        [Authorize]
		public async Task<IActionResult> Edit(uint? id, string? resultImgUpload, string? resultFilesUpload)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foundModel = await _context.LampModels.FindAsync(id);
            if (foundModel == null)
            {
                return NotFound();
            }
            ViewBag.ImgUploadSuccess = resultImgUpload;
            ViewBag.FilesUploadSuccess = resultFilesUpload;

            
            var filesWithSizesAsStr = await _lampModelService.GetAllFilesByModelId(id);
            List<FileRecordForView> fileRecords = _lampModelService.ParseFileRecordDB(filesWithSizesAsStr);
            ViewBag.FileRecords = fileRecords; 

            return View(LampModelDto.FromLampModel(foundModel));
        }

        // POST: LampModel/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> Edit(uint id, [Bind("Id,ModelName,Sections,CutPrice,DrillPrice,MountPrice,SolderPrice,AssemblyPrice,CheckPrice")] LampModelDto lampModelDto)
        {
            if (id != lampModelDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var currModel = LampModelDto.ToLampModel(lampModelDto);
					currModel.Id = id;
                    _context.LampModels.Update(currModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LampModelExists(lampModelDto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(lampModelDto);
        }

		// GET: LampModel/Delete/5
		[Authorize]
		public async Task<IActionResult> Delete(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foundModel = await _context.LampModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (foundModel == null)
            {
                return NotFound();
            }

            return View(LampModelDto.FromLampModel(foundModel));
        }

        // POST: LampModel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> DeleteConfirmed(uint id)
        {
            var foundModel = await _context.LampModels.FindAsync(id);
            if (foundModel != null)
            {
                _context.LampModels.Remove(foundModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        // GET: LampModel/Details/5
        public async Task<IActionResult> Details(uint? id, string modelId, string? startDate, string? endDate, int pageNumber)
        {
            int correctPageNumber = pageNumber < 1 ? 1 : pageNumber;

            if (id == null) {
                return NotFound();
            }

            var lampModel = await _context.LampModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lampModel == null) {
                return NotFound();
            }

            var lampModelDetails = await _context.LampModelDetails.FirstOrDefaultAsync(m => m.LampModelId == (int)id);
            ViewData["ImageName"] = lampModelDetails?.ImageName;

            if (startDate == null && endDate == null) {
                ViewBag.StartDate = $"{DateTime.Now.Date:s}";
                ViewBag.EndDate = $"{DateTime.Now.Date:s}";
                return View(new LampModelDetailsPageDto(LampModelDto.FromLampModel(lampModel, notes: lampModelDetails?.Note), null));
            }
            // если применяем фильтрацию, то Id получаем через скрытое строковое поле modelId
            var mId = uint.Parse(modelId);
            DateTime endDt = DateTime.Parse(endDate);
            DateTime startDt = DateTime.Parse(startDate);
            ViewBag.StartDate = $"{startDt:s}";
            ViewBag.EndDate = $"{endDt:s}";
            var lamps = _lampService.GetAllByModelAndDate(mId, startDt, endDt);

            int pageSize = 40;
            var lampsPaginated = await PaginatedList<LedLamp>.CreateAsync(lamps, correctPageNumber, pageSize);

            // для расчета полученного числа записей необходимо получить значение колличества записей на последней странице, т.к. она может не полная
            var lastPageNumber = lampsPaginated.TotalPages < 1 ? 1 : lampsPaginated.TotalPages;
            var lastPage = await PaginatedList<LedLamp>.CreateAsync(lamps, lastPageNumber, pageSize);
            var totalRecords = lampsPaginated.TotalPages * pageSize - (pageSize - lastPage.Items.Count);
            ViewBag.TotalRecords = totalRecords < 0 ? 0 : totalRecords;

            return View(new LampModelDetailsPageDto(LampModelDto.FromLampModel(lampModel, notes: lampModelDetails?.Note), lampsPaginated));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ImageUpload(IFormFile imageFile, uint id)
        {
            if (ModelState.IsValid) {
                //Save image to wwwroot/image
                string result = await _lampModelService.SaveOrChangeImageFile(imageFile, id);
                //Insert record
                //_context.Add(imageModel);
                //await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Edit), new { id = id, resultImgUpload = result });
            }
            return RedirectToAction(nameof(Edit), new { id = id });
        }

        [Authorize]
        [HttpPost("FilesUpload")]
        public async Task<IActionResult> FilesUpload(List<IFormFile> files, uint id)
        {
            if (ModelState.IsValid && files.Count > 0) {
                var resultUpload = await _lampModelService.SaveFiles(files, id);
                return RedirectToAction(nameof(Edit), new { id = id, resultFilesUpload = resultUpload });
            }
            return RedirectToAction(nameof(Edit), new { id = id });

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.
            /*return Ok(new { count = files.Count, size, filePaths });*/
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddNote(string noteArea, uint id)
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(noteArea)) {
                string result = await _lampModelService.AddNoteToDb(noteArea, (int)id);
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Edit), new { id = id });
        }

        [Authorize]
        public async Task<IActionResult> DeleteAttachmentFile(uint? id, string? fileName)
        {
            string result = "";
            if (fileName != null) {
                result = await _lampModelService.DeleteFileFromDetailsById(id, fileName);
            }
            return RedirectToAction(nameof(Edit), new { id = id });
        }


        private bool LampModelExists(uint id)
        {
            return _context.LampModels.Any(e => e.Id == id);
        }



    }
}
