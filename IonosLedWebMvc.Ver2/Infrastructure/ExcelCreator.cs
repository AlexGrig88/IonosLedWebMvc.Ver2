using ClosedXML.Excel;
using IonosLedWebMvc.Ver2.Dtos;
using IonosLedWebMvc.Ver2.Models.Entities;

namespace IonosLedWebMvc.Ver2.Infrastructure
{
    public class ExcelCreator
    {
        public static string fileName = "Details_For_Salary_UserAuth.xlsx";

        public static void GererateAndSaveFile(List<LedLampDto> lamps, string fileName, string employeeName, decimal? salary, DateTime startDt, DateTime endDt)
        {

            var headers = new List<string>() 
                { " Сер.№", "Спека", "Модель", "BitrixНомер", "Коммент", "ПечатьЭтикеткиTs", "Печать",
                    "НарезкаTs", "Нарезка", "СверлениеTs", "Сверление", "МонтажTs", "Монтаж",
                    "ПайкаTs", "Пайка", "СборкаTs", "Сборка", "УпаковкаTs", "Упаковка"};

 

            using var wbook = new XLWorkbook();
            var ws = wbook.Worksheets.Add("Детализация по светильникам");
            ws.Cell(1, 1).Value = $"Сотрудник {employeeName}. Расчитанная сумма с {startDt:D} по {endDt:D} составила: {salary} руб.";
            ws.Cell(1, 1).Style.Font.Italic = true;
            ws.Cell(1, 1).Style.Font.FontColor = XLColor.Red;

            // Для определения неиспользованных для вывода данных столбцов
            Dictionary<int, bool> switchesColVisible = new Dictionary<int, bool>();

            for (int i = 0; i < headers.Count; i++) {
                ws.Cell(2, i + 1).Value = headers[i];
                switchesColVisible[i + 1] = false;
            }
            for (int i = 1; i <= 5; i++) 
                switchesColVisible[i] = true;


            int col = 1, deltaRow = 3;
            for (int i = 0; i < lamps.Count; i++)
            {
                LedLampDto curr = lamps[i];
                ws.Cell(i + deltaRow, col).Value = curr.Id;
                ws.Cell(i + deltaRow, col + 1).Value = curr.Spec;
                ws.Cell(i + deltaRow, col + 2).Value = curr.Model != null ? curr.Model.ModelName : "";

                ws.Cell(i + deltaRow, col + 3).Value = curr.BitrixOrder;
                ws.Cell(i + deltaRow, col + 4).Value = curr.Comment;

                if (curr.LabelPrintUser != null && curr.LabelPrintUser.Name == employeeName) {
                    if (curr.LabelPrintTs == null) {
                        ws.Cell(i + deltaRow, col + 5).Value = "";
                    }
                    else {
                        ws.Cell(i + deltaRow, col + 5).Value = curr.LabelPrintTs.ToString();
                    }
                    ws.Cell(i + deltaRow, col + 6).Value =  curr.LabelPrintUser.Name;
                    switchesColVisible[6] = true;
                    switchesColVisible[7] = true;
                }

                if (curr.CutUser != null && curr.CutUser.Name == employeeName) {
                    ws.Cell(i + deltaRow, col + 7).Value = curr.AlProfileCutTs == null ? "" : curr.AlProfileCutTs.ToString();
                    ws.Cell(i + deltaRow, col + 8).Value = curr.CutUser.Name;
                    switchesColVisible[8] = true;
                    switchesColVisible[9] = true;
                }

                if (curr.DrillUser != null  && curr.DrillUser.Name == employeeName) {
                    ws.Cell(i + deltaRow, col + 9).Value = curr.AlProfileDrillTs == null ? "" : curr.AlProfileDrillTs.ToString();
                    ws.Cell(i + deltaRow, col + 10).Value = curr.DrillUser.Name;
                    switchesColVisible[10] = true;
                    switchesColVisible[11] = true;
                }

                if (curr.MountingUser != null && curr.MountingUser.Name == employeeName) {
                    ws.Cell(i + deltaRow, col + 11).Value = curr.LedModuleMountingTs == null ? "" : curr.LedModuleMountingTs.ToString();
                    ws.Cell(i + deltaRow, col + 12).Value = curr.MountingUser.Name;
                    switchesColVisible[12] = true;
                    switchesColVisible[13] = true;
                }

                if (curr.SolderingUser != null && curr.SolderingUser.Name == employeeName) {
                    ws.Cell(i + deltaRow, col + 13).Value = curr.LightSolderingTs == null ? "" : curr.LightSolderingTs.ToString();
                    ws.Cell(i + deltaRow, col + 14).Value = curr.SolderingUser.Name;
                    switchesColVisible[14] = true;
                    switchesColVisible[15] = true;
                }

                if (curr.AssemblingUser != null && curr.AssemblingUser.Name == employeeName) {
                    ws.Cell(i + deltaRow, col + 15).Value = curr.LightAssemblingTs == null ? "" : curr.LightAssemblingTs.ToString();
                    ws.Cell(i + deltaRow, col + 16).Value =  curr.AssemblingUser.Name;
                    switchesColVisible[16] = true;
                    switchesColVisible[17] = true;
                }

                if (curr.CheckingPackagingUser != null && curr.CheckingPackagingUser.Name == employeeName) {
                    ws.Cell(i + deltaRow, col + 17).Value = curr.LightCheckingPackagingTs == null ? "" : curr.LightCheckingPackagingTs.ToString();
                    ws.Cell(i + deltaRow, col + 18).Value =  curr.CheckingPackagingUser.Name;
                    switchesColVisible[18] = true;
                    switchesColVisible[19] = true;
                }
            }

            for (int i = 1; i <= headers.Count; i++) {
                if (switchesColVisible[i])
                    ws.Column(i).Width = 20;
                else
                    ws.Column(i).Collapse();
            }

            for (int i = 1; i <= headers.Count; i++) {
                ws.Cell(2, i).Style.Fill.BackgroundColor = XLColor.Aqua;
            }

            wbook.SaveAs(fileName);
        }

    }
}
