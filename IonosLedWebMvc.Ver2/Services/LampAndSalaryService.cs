using IonosLedWebMvc.Ver2.Data;
using IonosLedWebMvc.Ver2.Models;
using IonosLedWebMvc.Ver2.Repos;

namespace IonosLedWebMvc.Ver2.Services
{
    public class LampAndSalaryService
    {
        private readonly ApplicationContext _context;
        private readonly ILampRepo _lampRepository;

        public LampAndSalaryService(ApplicationContext context, ILampRepo lampRepo)
        {
            _context = context;
            _lampRepository = lampRepo;
        }

        public async Task<IQueryable<LedLamp>> GetAllLampsTimeFilter(DateTime startDt, DateTime endDt)
        {
            var lamps = _lampRepository.GetAllAsync();
            return lamps;
        }
    }
}
