using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Vml;
using IonosLedWebMvc.Ver2.Data;
using IonosLedWebMvc.Ver2.Models.Entities;

namespace IonosLedWebMvc.Ver2.Services
{
    public class LampModelService
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ApplicationContext _context;

        public LampModelService(IWebHostEnvironment hostEnvironment, ApplicationContext context)
        {
            _hostEnvironment = hostEnvironment;
            _context = context;
        }

        public async Task<string> SaveOrChangeImageFile(IFormFile imageFile, uint id)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            string fileName = System.IO.Path.GetFileNameWithoutExtension(imageFile.FileName);
            string extension = System.IO.Path.GetExtension(imageFile.FileName);
            fileName = $"Model{id}" + extension;
            string path = System.IO.Path.Combine(wwwRootPath + "/images/lamp_models/", fileName);
            using (var fileStream = new FileStream(path, FileMode.Create)) {
                await imageFile.CopyToAsync(fileStream);
            }
            var lampModelDetails = _context.LampModelDetails.FirstOrDefault(m => m.LampModelId == (int)id);
            if (lampModelDetails == null) {
                _context.LampModelDetails.Add(new LampModelDetails((int)id, imgName: fileName));
                await _context.SaveChangesAsync();
            }
            else {
                lampModelDetails.ImageName = fileName;
                _context.LampModelDetails.Update(lampModelDetails);
                await _context.SaveChangesAsync();
            }
            return "Ok";
        }

        public async Task<string> AddNoteToDb(string noteArea, int id)
        {
            string newNote = $"{DateTime.Now:d}: {noteArea}\n";
            var lampModelDetails = _context.LampModelDetails.FirstOrDefault(m => m.LampModelId == id);
            if (lampModelDetails == null) {
                _context.LampModelDetails.Add(new LampModelDetails(id, note: newNote));
                await _context.SaveChangesAsync();
            }
            else {
                lampModelDetails.Note = newNote + lampModelDetails.Note;
                _context.LampModelDetails.Update(lampModelDetails);
                await _context.SaveChangesAsync();
            }
            return "Ok";
        }
    }
}
