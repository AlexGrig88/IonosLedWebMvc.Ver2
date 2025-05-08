using IonosLedWebMvc.Ver2.Data;
using IonosLedWebMvc.Ver2.Models;
using IonosLedWebMvc.Ver2.Repos;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace IonosLedWebMvc.Ver2.Services
{
    public class LampService
    {
        private readonly ILampRepo _lampRepository;

        public LampService(ApplicationContext context, ILampRepo lampRepo)
        {
            _lampRepository = lampRepo;
        }

        public IQueryable<LedLamp> GetLampsTimeFiltering(DateTime startDt, DateTime endDt) => _lampRepository.GetAllAsQueryable()
            .Where(p => p.LabelPrintTs >= startDt && p.LabelPrintTs < endDt ||
                p.AlProfileCutTs >= startDt && p.AlProfileCutTs < endDt ||
                p.AlProfileDrillTs >= startDt && p.AlProfileDrillTs < endDt ||
                p.LedModuleMountingTs >= startDt && p.LedModuleMountingTs < endDt ||
                p.LightSolderingTs >= startDt && p.LightSolderingTs < endDt ||
                p.LightAssemblingTs >= startDt && p.LightAssemblingTs < endDt ||
                p.LightCheckingPackagingTs >= startDt && p.LightCheckingPackagingTs < endDt
                );

        public IQueryable<LedLamp> GetLampsTimeAndEmployeeFiltering(DateTime startDt, DateTime endDt, string employeeName) => GetLampsTimeFiltering(startDt, endDt)
            .Where(p => (p.LabelPrintUser != null && p.LabelPrintUser.Name == employeeName) ||
                            (p.CutUser != null && p.CutUser.Name == employeeName) ||
                            (p.DrillUser != null && p.DrillUser.Name == employeeName) ||
                            (p.MountingUser != null && p.MountingUser.Name == employeeName) ||
                            (p.AssemblingUser != null && p.AssemblingUser.Name == employeeName) ||
                            (p.SolderingUser != null && p.SolderingUser.Name == employeeName) ||
                            (p.CheckingPackagingUser != null && p.CheckingPackagingUser.Name == employeeName)
                            );

        public IQueryable<LedLamp> GetLampsTimeAndEmployeeAndModelFiltering(DateTime startDt, DateTime endDt, string employeeName, string modelName) =>
            GetLampsTimeAndEmployeeFiltering(startDt, endDt, employeeName).Where(p => p.Model != null && p.Model.ModelName == modelName);

        public IQueryable<LedLamp> GetLampsTimeAndAndModelFiltering(DateTime startDt, DateTime endDt, string modelName) =>
             GetLampsTimeFiltering(startDt, endDt).Where(p => p.Model != null && p.Model.ModelName == modelName);

        public IQueryable<LedLamp> GetLampsSearchBitrixNum(uint bitrixNum) => _lampRepository.GetAllAsQueryable().Where(p => p.BitrixOrder != null && p.BitrixOrder == bitrixNum);

        public async Task<Dictionary<uint, int>> GetCountedModelsAsync()
        {
            var modelsToCount = await _lampRepository.GetAllReleased()
                .GroupBy(lamp => lamp.ModelId)
                .Select(g => new { ModelId = g.Key, Count = g.Count() })
                .ToListAsync();

            var dictModelToCount = new Dictionary<uint, int>();
            foreach (var mc in modelsToCount)
            {
/*                string modelName = models.FirstOrDefault(m => m.Id == mc.ModelId)?.ModelName ?? "";
*/                dictModelToCount.Add(mc.ModelId ?? 0, mc.Count);
            }
            return dictModelToCount;
        }

    }
    
}
