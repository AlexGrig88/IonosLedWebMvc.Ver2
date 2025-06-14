﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IonosLedWebMvc.Ver2.Data;
using IonosLedWebMvc.Ver2.Dtos;
using IonosLedWebMvc.Ver2.Infrastructure;
using IonosLedWebMvc.Ver2.Services;
using IonosLedWebMvc.Ver2.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using IonosLedWebMvc.Ver2.Models;

namespace IonosLedWebMvc.Ver2.Controllers
{
	public class EmployeeController : Controller
    {
        private const string PREFIX_OK_RESULT = "Поздравляем! ";
        private const string PREFIX_BAD_RESULT = "Готово! ";
        private readonly ApplicationContext _context;
        private readonly UserEventsService _eventsService;
        private readonly UserAuthService _userAuthService;

        private List<string> _rolesList = new List<string>();

        public EmployeeController(ApplicationContext context, UserEventsService eventsService, UserAuthService userAuthService)
        {
            _context = context;
            _eventsService = eventsService;
            _userAuthService = userAuthService;
        }

        // GET: Employee
        public async Task<IActionResult> Index(string? userActionResult)
        {
            var usersList = await _context.Users.Include(u => u.Role).ToListAsync();
           
            ViewData["UserActionResult"] = userActionResult;
            ViewData["OK"] = PREFIX_OK_RESULT;
            // возьмём последние события за последнюю неделю
            var userIdToEvent = await _eventsService.GetLastEventFor(new TimeSpan(31, 0, 0, 0), usersList);
            var employees = usersList.Select(u => EmployeeDto.FromUser(u, userIdToEvent)).OrderBy(u => u.Name);
            var userAuthList = await _userAuthService.FindAll();
            return View(new EmployeeWithUserAuth() { EmployeeList = employees, UserAuthList = userAuthList });
        }

		// GET: Employee/Details/5
		[Authorize]
		public async Task<IActionResult> Details(uint? id)
        {
           
            if (id == null)
            {
                return NotFound();
            }

            var foundUser = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (foundUser == null)
            {
                return NotFound();
            }
            // возьмём последние события за последнюю неделю
            var userEventsList = await _eventsService.GetAllEventsFor(new TimeSpan(31, 0, 0, 0), foundUser);

            return View(EmployeeDto.FromUserWithEvents(foundUser, userEventsList.Select(UserEventDto.FromUserEvent).ToList()));
        }

		// GET: Employee/Create
		[Authorize]
		public IActionResult Create()
        {
            if (_rolesList.Count == 0)
                _rolesList = _context.Roles.Select(r => r.RoleName).ToList();
            ViewBag.RolesList = _rolesList;
            return View();
        }

        // POST: Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> Create([Bind("Id,Name,Pin,RoleName")] EmployeeDto employeeDto)
        {
            if (ModelState.IsValid)
            {
                User newUser = GetUserFromEmployeeDto(employeeDto);
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { userActionResult = $"{PREFIX_OK_RESULT}Сотрудник успешно создан." });
            }
            return View(employeeDto);
        }

		// GET: Employee/Edit/5
		[Authorize]
		public async Task<IActionResult> Edit(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allUsers = await _context.Users.Include(u => u.Role).ToListAsync();
            var foundUser = allUsers.Find(u => u.Id == id);
            if (foundUser == null)
            {
                return NotFound();
            }
            if (_rolesList.Count == 0) {
                _rolesList = _context.Roles.Select(r => r.RoleName).ToList();
            }
            HelperFunctions.Swap<string>(_rolesList, 0, _rolesList.FindIndex(r => r == foundUser.Role.RoleName));
            ViewBag.RolesList = _rolesList;
            return View(EmployeeDto.FromUser(foundUser));
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> Edit(uint id, [Bind("Id,Name,Pin,RoleName")] EmployeeDto employeeDto)
        {
            if (id != employeeDto.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    User currUser = GetUserFromEmployeeDto(employeeDto);
                    currUser.Id = employeeDto.Id;
                    _context.Users.Update(currUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employeeDto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { userActionResult = $"{PREFIX_OK_RESULT}Данные сотрудника успешно изменены." });
            }
            return View(employeeDto);
        }

        /*		// GET: Employee/Delete/5
                [Authorize]
                public async Task<IActionResult> Delete(uint? id)
                {
                    if (id == null) return NotFound();

                    var foundUser = await _context.Users.Include(u => u.Role)
                        .FirstOrDefaultAsync(u => u.Id == id);
                    if (foundUser == null)
                    {
                        return NotFound();
                    }

                    return View(EmployeeDto.FromUser(foundUser));
                }*/

        // POST: Employee/Delete/5
        /*        [HttpPost, ActionName("Delete")]
                [ValidateAntiForgeryToken]*/
        [HttpPost]
		[Authorize]
		public async Task<IActionResult> DeleteConfirmed(int id)
        {
            int res = id;
            var foundUser = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);
            if (foundUser != null) {
                _context.Users.Remove(foundUser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { userActionResult = $"{PREFIX_BAD_RESULT} Сотрудник успешно удален." });
        }

        private bool EmployeeExists(uint id) => _context.Users.Any(e => e.Id == id);
   

        private User GetUserFromEmployeeDto(EmployeeDto emp)
        {
            Role? targetRole = _context.Roles.FirstOrDefault(r => r.RoleName == emp.RoleName);
            return new User()
            {
                Name = emp.Name,
                Pin = emp.Pin,
                RoleId = targetRole.Id
            };
        }
    }
}
