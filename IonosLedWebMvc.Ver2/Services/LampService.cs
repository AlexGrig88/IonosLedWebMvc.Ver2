using DocumentFormat.OpenXml.InkML;
using IonosLedWebMvc.Ver2.Data;
using IonosLedWebMvc.Ver2.Models.Entities;
using IonosLedWebMvc.Ver2.Repos;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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

        public IQueryable<LedLamp> GetLampsTimeFiltering(DateTime startDt, DateTime endDt)
        {
           return  _lampRepository.GetAllAsQueryable().Where(p => p.LabelPrintTs >= startDt && p.LabelPrintTs < endDt ||
                                        p.AlProfileCutTs >= startDt && p.AlProfileCutTs < endDt ||
                                        p.AlProfileDrillTs >= startDt && p.AlProfileDrillTs < endDt ||
                                        p.LedModuleMountingTs >= startDt && p.LedModuleMountingTs < endDt ||
                                        p.LightSolderingTs >= startDt && p.LightSolderingTs < endDt ||
                                        p.LightAssemblingTs >= startDt && p.LightAssemblingTs < endDt ||
                                        p.LightCheckingPackagingTs >= startDt && p.LightCheckingPackagingTs < endDt);
        }

        public IQueryable<LedLamp> GetLabelPrintTimeFiltering(DateTime startDt, DateTime endDt) => _lampRepository.GetLabelPrintAsQueryable().Where(p => p.LabelPrintTs >= startDt && p.LabelPrintTs < endDt);
        public IQueryable<LedLamp> GetCutTimeFiltering(DateTime startDt, DateTime endDt) => _lampRepository.GetCutAsQueryable().Where(p => p.AlProfileCutTs >= startDt && p.AlProfileCutTs < endDt);
        public IQueryable<LedLamp> GetDrillTimeFiltering(DateTime startDt, DateTime endDt) => _lampRepository.GetDrillAsQueryable().Where(p => p.AlProfileDrillTs >= startDt && p.AlProfileDrillTs < endDt);
        public IQueryable<LedLamp> GetMountTimeFiltering(DateTime startDt, DateTime endDt) => _lampRepository.GetMountAsQueryable().Where(p => p.LedModuleMountingTs >= startDt && p.LedModuleMountingTs < endDt);
        public IQueryable<LedLamp> GetAssemblyTimeFiltering(DateTime startDt, DateTime endDt) => _lampRepository.GetAssemblyAsQueryable().Where(p => p.LightAssemblingTs >= startDt && p.LightAssemblingTs < endDt);
        public IQueryable<LedLamp> GetSolderTimeFiltering(DateTime startDt, DateTime endDt) => _lampRepository.GetSolderAsQueryable().Where(p => p.LightSolderingTs >= startDt && p.LightSolderingTs < endDt);
        public IQueryable<LedLamp> GetPackegTimeFiltering(DateTime startDt, DateTime endDt) => _lampRepository.GetPackegAsQueryable().Where(p => p.LightCheckingPackagingTs >= startDt && p.LightCheckingPackagingTs < endDt);


        public IQueryable<LedLamp> GetLampsTimeAndEmployeeFiltering(DateTime startDt, DateTime endDt, string employeeName, string operationFlag)
        {
            return operationFlag switch {
                "all" => GetLampsTimeFiltering(startDt, endDt).Where(p => (p.LabelPrintUser != null && p.LabelPrintUser.Name == employeeName) ||
                                (p.CutUser != null && p.CutUser.Name == employeeName) ||
                                (p.DrillUser != null && p.DrillUser.Name == employeeName) ||
                                (p.MountingUser != null && p.MountingUser.Name == employeeName) ||
                                (p.AssemblingUser != null && p.AssemblingUser.Name == employeeName) ||
                                (p.SolderingUser != null && p.SolderingUser.Name == employeeName) ||
                                (p.CheckingPackagingUser != null && p.CheckingPackagingUser.Name == employeeName)
                                ),
                "print" => GetLabelPrintTimeFiltering(startDt, endDt).Where(p => p.LabelPrintUser != null && p.LabelPrintUser.Name == employeeName),
                "cut" => GetCutTimeFiltering(startDt, endDt).Where(p => p.CutUser != null && p.CutUser.Name == employeeName),
                "drill" => GetDrillTimeFiltering(startDt, endDt).Where(p => p.DrillUser != null && p.DrillUser.Name == employeeName),
                "mount" => GetMountTimeFiltering(startDt, endDt).Where(p => p.MountingUser != null && p.MountingUser.Name == employeeName),
                "solder" => GetSolderTimeFiltering(startDt, endDt).Where(p => p.SolderingUser != null && p.SolderingUser.Name == employeeName),
                "assembly" => GetAssemblyTimeFiltering(startDt, endDt).Where(p => p.AssemblingUser != null && p.AssemblingUser.Name == employeeName),
                "packeg" => GetPackegTimeFiltering(startDt, endDt).Where(p => p.CheckingPackagingUser != null && p.CheckingPackagingUser.Name == employeeName),
                _ => throw new NotImplementedException("Ошибка!!!, проверить!")
            };
        }


/*        public IQueryable<LedLamp> GetLampsTimeAndEmployeeFiltering(DateTime startDt, DateTime endDt, string employeeName) => GetLampsTimeFiltering(startDt, endDt)
            .Where(p => (p.LabelPrintUser != null && p.LabelPrintUser.Name == employeeName) ||
                            (p.CutUser != null && p.CutUser.Name == employeeName) ||
                            (p.DrillUser != null && p.DrillUser.Name == employeeName) ||
                            (p.MountingUser != null && p.MountingUser.Name == employeeName) ||
                            (p.AssemblingUser != null && p.AssemblingUser.Name == employeeName) ||
                            (p.SolderingUser != null && p.SolderingUser.Name == employeeName) ||
                            (p.CheckingPackagingUser != null && p.CheckingPackagingUser.Name == employeeName)
                            );*/

        public IQueryable<LedLamp> GetLampsTimeAndEmployeeAndModelFiltering(DateTime startDt, DateTime endDt, string employeeName, string modelName, string operationFlag) =>
            GetLampsTimeAndEmployeeFiltering(startDt, endDt, employeeName, operationFlag).Where(p => p.Model != null && p.Model.ModelName == modelName);

        public IQueryable<LedLamp> GetLampsTimeAndAndModelFiltering(DateTime startDt, DateTime endDt, string modelName, string operationFlag)
        {
            IQueryable<LedLamp> filterList = operationFlag switch
            {
                "all" => GetLampsTimeFiltering(startDt, endDt),

                "print" => GetLabelPrintTimeFiltering(startDt, endDt),
                "cut" => GetCutTimeFiltering(startDt, endDt),
                "drill" => GetDrillTimeFiltering(startDt, endDt),
                "mount" => GetMountTimeFiltering(startDt, endDt),
                "solder" => GetSolderTimeFiltering(startDt, endDt),
                "assembly" => GetAssemblyTimeFiltering(startDt, endDt),
                "packeg" => GetPackegTimeFiltering(startDt, endDt),
                _ => throw new NotImplementedException("Ошибка!!!, проверить!")
            };
            return filterList.Where(p => p.Model != null && p.Model.ModelName == modelName);
        }
             

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

        public IQueryable<LedLamp> GetAllByModelAndDate(uint modelId, DateTime startDt, DateTime endDt) => 
                    _lampRepository.GetDateFiltering(startDt, endDt).Where(lamp => lamp.ModelId == modelId);

    }
    
}
