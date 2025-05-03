using IonosLedWebMvc.Ver2.Data;
using IonosLedWebMvc.Ver2.Dtos;
using IonosLedWebMvc.Ver2.Models;
using Microsoft.EntityFrameworkCore;

namespace IonosLedWebMvc.Ver2.Repos
{
    public class LampRepo : ILampRepo
    {

        private readonly ApplicationContext _context;

        public LampRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IQueryable<LedLamp> GetAllAsync()
        {
            var lamps = _context.LedLamps
                .Include(p => p.LabelPrintUser)
                .Include(p => p.AssemblingUser)
                .Include(p => p.CheckingPackagingUser)
                .Include(p => p.CutUser)
                .Include(p => p.DrillUser)
                .Include(p => p.MountingUser)
                .Include(p => p.SolderingUser)
                .Include(p => p.Model);
            return lamps;
        }
    }
}
