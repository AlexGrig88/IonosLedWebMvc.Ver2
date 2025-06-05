using IonosLedWebMvc.Ver2.Data;
using IonosLedWebMvc.Ver2.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace IonosLedWebMvc.Ver2.Controllers
{
    public class RoleController : Controller
    {
        private const string PREFIX_OK_RESULT = "Поздравляем! ";
        private const string PREFIX_BAD_RESULT = "Готово! ";
        private readonly ApplicationContext _context;

        public RoleController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Role
        public async Task<IActionResult> Index(string? userActionResult)
        {
            var roles = await _context.Roles.ToListAsync();
            ViewData["UserActionResult"] = userActionResult;
            ViewData["OK"] = PREFIX_OK_RESULT;
            return View(roles.Select(r => RoleDto.FromRole(r)));
        }


		// GET: Role/Create
		[Authorize]
		public IActionResult Create()
        {
            return View();
        }

        // POST: Role/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> Create([Bind("Id,RoleName,PermissionSettings,PermissionAlProfileCut,PermissionAlProfileDrill,PermissionLedModuleMounting,PermissionLightSoldering,PermissionLightAssembling,PermissionLightCheckingPackaging,PermissionChiefLightProduction")] RoleDto roleDto)
        {
            if (ModelState.IsValid)
            {
                _context.Roles.Add(RoleDto.ToRole(roleDto));
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { userActionResult = $"{PREFIX_OK_RESULT} Должность успешно создана." });
            }
            return View(roleDto);
        }

		// GET: Role/Edit/5
		[Authorize]
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
		[Authorize]
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
                return RedirectToAction(nameof(Index), new { userActionResult = $"{PREFIX_OK_RESULT} Должность успешно изменена." });
            }
            return View(roleDto);
        }

		// GET: Role/Delete/5
		[Authorize]
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
		[Authorize]
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
            return RedirectToAction(nameof(Index), new { userActionResult = $"{PREFIX_BAD_RESULT} Должность удалена." });
        }

        private bool RoleExists(uint id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }
    }
}
