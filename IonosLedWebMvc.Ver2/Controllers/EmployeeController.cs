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
    public class EmployeeController : Controller
    {
        private readonly ApplicationContext _context;

        public EmployeeController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Employee
        public async Task<IActionResult> Index()
        {
            var usersList = await _context.Users.Include(u => u.Role).ToListAsync();
            return View(usersList.Select(u => EmployeeDto.FromUser(u)));
        }

        // GET: Employee/Details/5
        public async Task<IActionResult> Details(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(EmployeeDto.FromUser(user));
        }

        // GET: Employee/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Pin,RoleName")] EmployeeDto employeeDto)
        {
            if (ModelState.IsValid)
            {
                
                _context.Add(employeeDto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employeeDto);
        }

        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeDto = await _context.EmployeeDto.FindAsync(id);
            if (employeeDto == null)
            {
                return NotFound();
            }
            return View(employeeDto);
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(uint id, [Bind("Id,Name,Pin,RoleName")] EmployeeDto employeeDto)
        {
            if (id != employeeDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employeeDto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeDtoExists(employeeDto.Id))
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
            return View(employeeDto);
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeDto = await _context.EmployeeDto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employeeDto == null)
            {
                return NotFound();
            }

            return View(employeeDto);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(uint id)
        {
            var employeeDto = await _context.EmployeeDto.FindAsync(id);
            if (employeeDto != null)
            {
                _context.EmployeeDto.Remove(employeeDto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeDtoExists(uint id)
        {
            return _context.EmployeeDto.Any(e => e.Id == id);
        }
    }
}
