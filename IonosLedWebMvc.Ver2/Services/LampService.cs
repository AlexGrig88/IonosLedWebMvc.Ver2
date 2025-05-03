using IonosLedWebMvc.Ver2.Data;
using IonosLedWebMvc.Ver2.Models;
using IonosLedWebMvc.Ver2.Repos;
using System.Text;

namespace IonosLedWebMvc.Ver2.Services
{
    public class LampService
    {
        private readonly ApplicationContext _context;
        private readonly ILampRepo _lampRepository;

        public LampService(ApplicationContext context, ILampRepo lampRepo)
        {
            _context = context;
            _lampRepository = lampRepo;
        }

        public IQueryable<LedLamp> GetLampsTimeFiltering(DateTime startDt, DateTime endDt) => _lampRepository.GetAllAsync()
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


        // метод будет терминировать с преобразованием в список полученный в параметре поток ламп, чтобы рассчитать зп, затем сформирует точно такой же запрос
        // для тполучения такого же потока ламп и вернёт его вместе с рассчитанными зп для всех сотрудников.
/*        public (IQueryable<LedLamp>, List<EmployeeSalary>) CalculateSalary(IQueryable<LedLamp> lamps, DateTime startDt, DateTime endDt, string employeeName, string modelName)
        {

            List<LedLamp> lampWithoutModel = new List<LedLamp>();         // Список для хранения записей без указания модели 

            Dictionary<uint?, string> dictUserIdToUserName = new Dictionary<uint?, string>();
            Dictionary<uint?, decimal> dictUserIdToSalary = new Dictionary<uint?, decimal>();

            var _emploeeList = _context.Users.ToList();
            foreach (var emp in _emploeeList) {  // инициализация зп работников 0.00 рублями по их Id и карта Id к имени работника
                dictUserIdToSalary.Add(emp.Id, 0.0m);
                dictUserIdToUserName.Add(emp.Id, emp.Name);
            }

            List<ModelLedWithJobAndPrices> modelsLedWithJobAndPrices = CreateListMapOfJobToPrices();


            var products = lamps.ToList();

            foreach (var p in products) {
                if (!p.ModelId.HasValue) {        // если не указана модель, то сохраняем её и переходим к следующей записи
                    lampWithoutModel.Add(p);
                    continue;
                }
                else {
                    var modelWithJobs = modelsLedWithJobAndPrices.Find(m => m.ModelId == p.ModelId);        // находим модель связанную со стоимостями её работ
                    if (p.AlProfileCutTs != null && p.AlProfileCutUser != null) {
                        if (p.AlProfileCutTs >= startDate && p.AlProfileCutTs < endDate) {                  // Валидируем каждое поле на диапазон, т.к. мы выбирали записи по логике "или"
                            dictUserIdToSalary[p.AlProfileCutUser] += modelWithJobs.DictJobIdToPrices[1];   // по найденной раннее модели берём стоимость работы 
                        }
                    }

                    if (p.AlProfileDrillTs != null && p.AlProfileDrillUser != null) {
                        if (p.AlProfileDrillTs >= startDate && p.AlProfileDrillTs < endDate) {
                            dictUserIdToSalary[p.AlProfileDrillUser] += modelWithJobs.DictJobIdToPrices[2];
                        }
                    }

                    if (p.LedModuleMountingTs != null && p.LedModuleMountingUser != null) {
                        if (p.LedModuleMountingTs >= startDate && p.LedModuleMountingTs < endDate) {
                            dictUserIdToSalary[p.LedModuleMountingUser] += modelWithJobs.DictJobIdToPrices[3];
                        }
                    }

                    if (p.LightSolderingTs != null && p.LightSolderingUser != null) {
                        if (p.LightSolderingTs >= startDate && p.LightSolderingTs < endDate) {
                            dictUserIdToSalary[p.LightSolderingUser] += modelWithJobs.DictJobIdToPrices[4];
                        }
                    }

                    if (p.LightAssemblingTs != null && p.LightAssemblingUser != null) {
                        if (p.LightAssemblingTs >= startDate && p.LightAssemblingTs < endDate) {
                            dictUserIdToSalary[p.LightAssemblingUser] += modelWithJobs.DictJobIdToPrices[5];
                        }
                    }

                    if (p.LightCheckingPackagingTs != null && p.LightCheckingPackagingUser != null) {
                        if (p.LightCheckingPackagingTs >= startDate && p.LightCheckingPackagingTs < endDate) {
                            dictUserIdToSalary[p.LightCheckingPackagingUser] += modelWithJobs.DictJobIdToPrices[6];
                        }
                    }
                }
            }

        }*/

        // создание списка объектов, представляющих собой соответствие Id модели с картой работ (вид работы и цены за неё)
        public List<ModelLedWithJobAndPrices> CreateListMapOfJobToPrices()
        {
            var models = _context.LampModels.ToList();
            List<ModelLedWithJobAndPrices> modelsJobToPrices = new List<ModelLedWithJobAndPrices>();
            foreach (var model in models) {
                var dictJobIdToPrice = new Dictionary<int, decimal>
                {
                    { 1, model.Sections * model.CutPrice },
                    { 2, model.Sections * model.DrillPrice },
                    { 3, model.Sections * model.MountPrice },
                    { 4, model.Sections * model.SolderPrice },
                    { 5, model.Sections * model.AssemblyPrice },
                    { 6, model.Sections * model.CheckPrice }
                };
                modelsJobToPrices.Add(new ModelLedWithJobAndPrices(model.Id, dictJobIdToPrice));
            }
            return modelsJobToPrices;
        }
    }

    public class ModelLedWithJobAndPrices
    {
        public uint ModelId { get; set; }
        public Dictionary<int, decimal> DictJobIdToPrices { get; set; }


        public ModelLedWithJobAndPrices(uint modelId, Dictionary<int, decimal> dictJobIdToPrices)
        {
            ModelId = modelId;
            DictJobIdToPrices = dictJobIdToPrices;
        }

        public string ToStr()
        {
            StringBuilder res = new StringBuilder($"ModelId = {ModelId.ToString()};\t\t");
            foreach (var pair in DictJobIdToPrices) {
                res.Append($"JobId = {pair.Key};\t Price = {pair.Value};\t");
            }
            return res.ToString();
        }

    }

    
}
