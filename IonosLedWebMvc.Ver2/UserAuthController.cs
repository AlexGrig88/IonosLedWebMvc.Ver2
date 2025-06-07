using IonosLedWebMvc.Ver2.Data;
using IonosLedWebMvc.Ver2.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IonosLedWebMvc.Ver2
{
    public class UserAuthController : Controller
    {
        private readonly ApplicationContext _context;

        public UserAuthController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: UserAuth
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserAuths.ToListAsync());
        }

        // GET: UserAuth/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userAuth = await _context.UserAuths
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userAuth == null)
            {
                return NotFound();
            }

            return View(userAuth);
        }

        // GET: UserAuth/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserAuth/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Username,Email,PasswordHash,Phone,ProdGroup,URole,AvatarImg,BirthDate,RegisterDate")] UserAuth userAuth)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userAuth);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userAuth);
        }

        // GET: UserAuth/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userAuth = await _context.UserAuths.FindAsync(id);
            if (userAuth == null)
            {
                return NotFound();
            }
            return View(userAuth);
        }

        // POST: UserAuth/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Username,Email,PasswordHash,Phone,ProdGroup,URole,AvatarImg,BirthDate,RegisterDate")] UserAuth userAuth)
        {
            if (id != userAuth.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userAuth);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserAuthExists(userAuth.Id))
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
            return View(userAuth);
        }

        // GET: UserAuth/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userAuth = await _context.UserAuths
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userAuth == null)
            {
                return NotFound();
            }

            return View(userAuth);
        }

        // POST: UserAuth/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userAuth = await _context.UserAuths.FindAsync(id);
            if (userAuth != null)
            {
                _context.UserAuths.Remove(userAuth);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserAuthExists(int id)
        {
            return _context.UserAuths.Any(e => e.Id == id);
        }
    }
}
