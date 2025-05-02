
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IonosLedWebMvc.Ver2.Data;
using IonosLedWebMvc.Ver2.Models;

namespace IonosLedWebMvc.Ver2.Controllers
{
    public class LedLampsController : Controller
    {
        private readonly ApplicationContext _context;

        public LedLampsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: LedLamps
        public async Task<IActionResult> Index()
        {
            var applicationContext = _context.LedLamps.Include(l => l.AssemblingUser).Include(l => l.CheckingPackagingUser).Include(l => l.CutUser).Include(l => l.DrillUser).Include(l => l.LabelPrintUser).Include(l => l.Model).Include(l => l.MountingUser).Include(l => l.SolderingUser);
            return View(await applicationContext.Take(100).ToListAsync());
        }

        // GET: LedLamps/Details/5
        public async Task<IActionResult> Details(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ledLamp = await _context.LedLamps
                .Include(l => l.AssemblingUser)
                .Include(l => l.CheckingPackagingUser)
                .Include(l => l.CutUser)
                .Include(l => l.DrillUser)
                .Include(l => l.LabelPrintUser)
                .Include(l => l.Model)
                .Include(l => l.MountingUser)
                .Include(l => l.SolderingUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ledLamp == null)
            {
                return NotFound();
            }

            return View(ledLamp);
        }

        // GET: LedLamps/Create
        public IActionResult Create()
        {
            ViewData["AssemblingUserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["CheckingPackagingUserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["CutUserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["DrillUserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["LabelPrintUserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["ModelId"] = new SelectList(_context.LampModels, "Id", "Id");
            ViewData["MountingUserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["SolderingUserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: LedLamps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ModelId,Spec,BitrixOrder,Comment,LabelPrintUserId,LabelPrintTs,CutUserId,AlProfileCutTs,DrillUserId,AlProfileDrillTs,MountingUserId,LedModuleMountingTs,SolderingUserId,LightSolderingTs,AssemblingUserId,LightAssemblingTs,CheckingPackagingUserId,LightCheckingPackagingTs")] LedLamp ledLamp)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ledLamp);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AssemblingUserId"] = new SelectList(_context.Users, "Id", "Id", ledLamp.AssemblingUserId);
            ViewData["CheckingPackagingUserId"] = new SelectList(_context.Users, "Id", "Id", ledLamp.CheckingPackagingUserId);
            ViewData["CutUserId"] = new SelectList(_context.Users, "Id", "Id", ledLamp.CutUserId);
            ViewData["DrillUserId"] = new SelectList(_context.Users, "Id", "Id", ledLamp.DrillUserId);
            ViewData["LabelPrintUserId"] = new SelectList(_context.Users, "Id", "Id", ledLamp.LabelPrintUserId);
            ViewData["ModelId"] = new SelectList(_context.LampModels, "Id", "Id", ledLamp.ModelId);
            ViewData["MountingUserId"] = new SelectList(_context.Users, "Id", "Id", ledLamp.MountingUserId);
            ViewData["SolderingUserId"] = new SelectList(_context.Users, "Id", "Id", ledLamp.SolderingUserId);
            return View(ledLamp);
        }

        // GET: LedLamps/Edit/5
        public async Task<IActionResult> Edit(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ledLamp = await _context.LedLamps.FindAsync(id);
            if (ledLamp == null)
            {
                return NotFound();
            }
            ViewData["AssemblingUserId"] = new SelectList(_context.Users, "Id", "Id", ledLamp.AssemblingUserId);
            ViewData["CheckingPackagingUserId"] = new SelectList(_context.Users, "Id", "Id", ledLamp.CheckingPackagingUserId);
            ViewData["CutUserId"] = new SelectList(_context.Users, "Id", "Id", ledLamp.CutUserId);
            ViewData["DrillUserId"] = new SelectList(_context.Users, "Id", "Id", ledLamp.DrillUserId);
            ViewData["LabelPrintUserId"] = new SelectList(_context.Users, "Id", "Id", ledLamp.LabelPrintUserId);
            ViewData["ModelId"] = new SelectList(_context.LampModels, "Id", "Id", ledLamp.ModelId);
            ViewData["MountingUserId"] = new SelectList(_context.Users, "Id", "Id", ledLamp.MountingUserId);
            ViewData["SolderingUserId"] = new SelectList(_context.Users, "Id", "Id", ledLamp.SolderingUserId);
            return View(ledLamp);
        }

        // POST: LedLamps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(uint id, [Bind("Id,ModelId,Spec,BitrixOrder,Comment,LabelPrintUserId,LabelPrintTs,CutUserId,AlProfileCutTs,DrillUserId,AlProfileDrillTs,MountingUserId,LedModuleMountingTs,SolderingUserId,LightSolderingTs,AssemblingUserId,LightAssemblingTs,CheckingPackagingUserId,LightCheckingPackagingTs")] LedLamp ledLamp)
        {
            if (id != ledLamp.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ledLamp);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LedLampExists(ledLamp.Id))
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
            ViewData["AssemblingUserId"] = new SelectList(_context.Users, "Id", "Id", ledLamp.AssemblingUserId);
            ViewData["CheckingPackagingUserId"] = new SelectList(_context.Users, "Id", "Id", ledLamp.CheckingPackagingUserId);
            ViewData["CutUserId"] = new SelectList(_context.Users, "Id", "Id", ledLamp.CutUserId);
            ViewData["DrillUserId"] = new SelectList(_context.Users, "Id", "Id", ledLamp.DrillUserId);
            ViewData["LabelPrintUserId"] = new SelectList(_context.Users, "Id", "Id", ledLamp.LabelPrintUserId);
            ViewData["ModelId"] = new SelectList(_context.LampModels, "Id", "Id", ledLamp.ModelId);
            ViewData["MountingUserId"] = new SelectList(_context.Users, "Id", "Id", ledLamp.MountingUserId);
            ViewData["SolderingUserId"] = new SelectList(_context.Users, "Id", "Id", ledLamp.SolderingUserId);
            return View(ledLamp);
        }

        // GET: LedLamps/Delete/5
        public async Task<IActionResult> Delete(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ledLamp = await _context.LedLamps
                .Include(l => l.AssemblingUser)
                .Include(l => l.CheckingPackagingUser)
                .Include(l => l.CutUser)
                .Include(l => l.DrillUser)
                .Include(l => l.LabelPrintUser)
                .Include(l => l.Model)
                .Include(l => l.MountingUser)
                .Include(l => l.SolderingUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ledLamp == null)
            {
                return NotFound();
            }

            return View(ledLamp);
        }

        // POST: LedLamps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(uint id)
        {
            var ledLamp = await _context.LedLamps.FindAsync(id);
            if (ledLamp != null)
            {
                _context.LedLamps.Remove(ledLamp);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LedLampExists(uint id)
        {
            return _context.LedLamps.Any(e => e.Id == id);
        }
    }
}
