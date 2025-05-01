using IonosLedWebMvc.Ver2.Data;
using IonosLedWebMvc.Ver2.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

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
                _context.Roles.Add(RoleDto.ToRole(roleDto));
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

            var foundRole = await _context.Roles.FindAsync(id);
            if (foundRole == null)
            {
                return NotFound();
            }
            return View(RoleDto.FromRole(foundRole));
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
                    var currRole = RoleDto.ToRole(roleDto);
                    currRole.Id = id;
                    _context.Roles.Update(currRole);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleExists(roleDto.Id))
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

            var foundRole = await _context.Roles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (foundRole == null)
            {
                return NotFound();
            }

            return View(RoleDto.FromRole(foundRole));
        }

        // POST: Role/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(uint id)
        {
            var foundRole = await _context.Roles.FindAsync(id);

            if (foundRole != null) {
                _context.Roles.Remove(foundRole);
            }

            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException) {
                return View("ExceptionView");
            }
            return RedirectToAction(nameof(Index));
        }

        private bool RoleExists(uint id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }
    }
}
