using IonosLedWebMvc.Ver2.Data;
using IonosLedWebMvc.Ver2.Models.Entities;
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

        public IQueryable<LedLamp> GetAllAsQueryable()
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
        public IQueryable<LedLamp> GetLabelPrintAsQueryable() => _context.LedLamps.Include(p => p.LabelPrintUser).Include(p => p.Model);
        public IQueryable<LedLamp> GetAssemblyAsQueryable() => _context.LedLamps.Include(p => p.AssemblingUser).Include(p => p.Model);
        public IQueryable<LedLamp> GetCutAsQueryable() => _context.LedLamps.Include(p => p.CutUser).Include(p => p.Model);
        public IQueryable<LedLamp> GetDrillAsQueryable() => _context.LedLamps.Include(p => p.DrillUser).Include(p => p.Model);
        public IQueryable<LedLamp> GetMountAsQueryable() => _context.LedLamps.Include(p => p.MountingUser).Include(p => p.Model);
        public IQueryable<LedLamp> GetSolderAsQueryable() => _context.LedLamps.Include(p => p.SolderingUser).Include(p => p.Model);
        public IQueryable<LedLamp> GetPackegAsQueryable() => _context.LedLamps.Include(p => p.CheckingPackagingUser).Include(p => p.Model);



        public IQueryable<LedLamp> GetAllReleased() => _context.LedLamps.Where(lamp => lamp.LightCheckingPackagingTs != null);

		// отличие от GetDateFiltering в том, что мы не соединяем ни с какими таблицами
		public IQueryable<LedLamp> GetAllRealesedForThePeriodAsync(DateTime startDt, DateTime endDt) =>
            _context.LedLamps.Where(lamp => lamp.LightCheckingPackagingTs != null && lamp.LightCheckingPackagingTs >= startDt && lamp.LightCheckingPackagingTs < endDt);

        public IQueryable<LedLamp> GetDateFiltering(DateTime startDt, DateTime endDt) => 
            _context.LedLamps
                .Include(p => p.LabelPrintUser)
                .Include(p => p.CheckingPackagingUser)
                .Where(lamp => lamp.LightCheckingPackagingTs != null && lamp.LightCheckingPackagingTs >= startDt && lamp.LightCheckingPackagingTs < endDt);
    }
}
