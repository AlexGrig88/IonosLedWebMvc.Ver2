using IonosLedWebMvc.Ver2.Data;
using IonosLedWebMvc.Ver2.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IonosLedWebMvc.Ver2.Controllers
{
    public class RoleController : Controller
    {
        private readonly ApplicationContext _context;

        public RoleController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Role
        public async Task<IActionResult> Index()
        {
            var roles = await _context.Roles.ToListAsync();
            return View(roles.Select(r => RoleDto.FromRole(r)));
        }


        // GET: Role/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Role/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RoleName,PermissionSettings,PermissionAlProfileCut,PermissionAlProfileDrill,PermissionLedModuleMounting,PermissionLightSoldering,PermissionLightAssembling,PermissionLightCheckingPackaging,PermissionChiefLightProduction")] RoleDto roleDto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(roleDto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(roleDto);
        }

        // GET: Role/Edit/5
        public async Task<IActionResult> Edit(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roleDto = await _context.RoleDto.FindAsync(id);
            if (roleDto == null)
            {
                return NotFound();
            }
            return View(roleDto);
        }

        // POST: Role/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(uint id, [Bind("Id,RoleName,PermissionSettings,PermissionAlProfileCut,PermissionAlProfileDrill,PermissionLedModuleMounting,PermissionLightSoldering,PermissionLightAssembling,PermissionLightCheckingPackaging,PermissionChiefLightProduction")] RoleDto roleDto)
        {
            if (id != roleDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(roleDto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleDtoExists(roleDto.Id))
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
            return View(roleDto);
        }

        // GET: Role/Delete/5
        public async Task<IActionResult> Delete(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roleDto = await _context.RoleDto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (roleDto == null)
            {
                return NotFound();
            }

            return View(roleDto);
        }

        // POST: Role/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(uint id)
        {
            var roleDto = await _context.RoleDto.FindAsync(id);
            if (roleDto != null)
            {
                _context.RoleDto.Remove(roleDto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoleDtoExists(uint id)
        {
            return _context.RoleDto.Any(e => e.Id == id);
        }
    }
}
