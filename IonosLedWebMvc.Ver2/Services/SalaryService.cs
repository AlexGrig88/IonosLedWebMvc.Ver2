using IonosLedWebMvc.Ver2.Data;
using IonosLedWebMvc.Ver2.Dtos;
using IonosLedWebMvc.Ver2.Models;
using IonosLedWebMvc.Ver2.Models.Entities;
using System.Text;

namespace IonosLedWebMvc.Ver2.Services
{
    public class SalaryService
    {
        private readonly ApplicationContext _context;

        public SalaryService(ApplicationContext context)
        {
            _context = context;
        }

        public (List<EmployeeSalary> employeeSalaryList, List<LedLamp> lampWithoutModelList) CalculateSalary(List<LedLamp> products, DateTime startDate, DateTime endDate)
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

            foreach (var p in products) {
                if (!p.ModelId.HasValue) {        // если не указана модель, то сохраняем её и переходим к следующей записи
                    lampWithoutModel.Add(p);
                    continue;
                }
                else {
                    var modelWithJobs = modelsLedWithJobAndPrices.Find(m => m.ModelId == p.ModelId);        // находим модель связанную со стоимостями её работ
                    if (p.AlProfileCutTs != null && p.CutUserId != null) {
                        if (p.AlProfileCutTs >= startDate && p.AlProfileCutTs < endDate) {                  // Валидируем каждое поле на диапазон, т.к. мы выбирали записи по логике "или"
                            dictUserIdToSalary[p.CutUserId] += modelWithJobs.DictJobIdToPrices[1];   // по найденной раннее модели берём стоимость работы 
                        }
                    }

                    if (p.AlProfileDrillTs != null && p.DrillUserId != null) {
                        if (p.AlProfileDrillTs >= startDate && p.AlProfileDrillTs < endDate) {
                            dictUserIdToSalary[p.DrillUserId] += modelWithJobs.DictJobIdToPrices[2];
                        }
                    }

                    if (p.LedModuleMountingTs != null && p.MountingUserId != null) {
                        if (p.LedModuleMountingTs >= startDate && p.LedModuleMountingTs < endDate) {
                            dictUserIdToSalary[p.MountingUserId] += modelWithJobs.DictJobIdToPrices[3];
                        }
                    }

                    if (p.LightSolderingTs != null && p.SolderingUserId != null) {
                        if (p.LightSolderingTs >= startDate && p.LightSolderingTs < endDate) {
                            dictUserIdToSalary[p.SolderingUserId] += modelWithJobs.DictJobIdToPrices[4];
                        }
                    }

                    if (p.LightAssemblingTs != null && p.AssemblingUserId != null) {
                        if (p.LightAssemblingTs >= startDate && p.LightAssemblingTs < endDate) {
                            dictUserIdToSalary[p.AssemblingUserId] += modelWithJobs.DictJobIdToPrices[5];
                        }
                    }

                    if (p.LightCheckingPackagingTs != null && p.CheckingPackagingUserId != null) {
                        if (p.LightCheckingPackagingTs >= startDate && p.LightCheckingPackagingTs < endDate) {
                            dictUserIdToSalary[p.CheckingPackagingUserId] += modelWithJobs.DictJobIdToPrices[6];
                        }
                    }
                }
            }

            if (lampWithoutModel.Count > 0) {       // если есть записи без моделей показываем их и печатаем общее колличество таких записей
                
            }

            var emploeeSalaries = new List<EmployeeSalary>();   // создаём и заполняем список для отображения результатов подсчета 
            foreach (var pair in dictUserIdToSalary)
            {
                emploeeSalaries.Add(new EmployeeSalary($"{pair.Key}. {dictUserIdToUserName[pair.Key]}", pair.Value));
            }

            return (emploeeSalaries, lampWithoutModel);

        }

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
