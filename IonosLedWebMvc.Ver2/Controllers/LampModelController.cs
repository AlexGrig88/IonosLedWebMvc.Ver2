using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IonosLedWebMvc.Ver2.Data;
using IonosLedWebMvc.Ver2.Dtos;

namespace IonosLedWebMvc.Ver2.Controllers
{
    public class LampModelController : Controller
    {
        private readonly ApplicationContext _context;

        public LampModelController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: LampModel
        public async Task<IActionResult> Index()
        {
            var models = await _context.LampModels.ToListAsync();
            return View(models.Select(LampModelDto.FromLampModel));
        }

        // GET: LampModel/Details/5
        public async Task<IActionResult> Details(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lampModelDto = await _context.LampModelDto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lampModelDto == null)
            {
                return NotFound();
            }

            return View(lampModelDto);
        }

        // GET: LampModel/Create
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
        public async Task<IActionResult> Edit(uint? id)
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
            return View(LampModelDto.FromLampModel(foundModel));
        }

        // POST: LampModel/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        private bool LampModelExists(uint id)
        {
            return _context.LampModels.Any(e => e.Id == id);
        }
    }
}
