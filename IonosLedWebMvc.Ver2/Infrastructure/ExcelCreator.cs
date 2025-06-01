using ClosedXML.Excel;
using IonosLedWebMvc.Ver2.Dtos;
using IonosLedWebMvc.Ver2.Models.Entities;

namespace IonosLedWebMvc.Ver2.Infrastructure
{
    public class ExcelCreator
    {
        public static string fileName = "Details_For_Salary_UserAuth.xlsx";

        public static void GererateAndSaveFile(List<LedLampDto> lamps, string fileName, string employeeName, decimal? salary)
        {

            var headers = new List<string>() 
                { " Сер.№", "Спека", "Модель", "BitrixНомер", "Коммент", "ПечатьЭтикеткиTs", "Печать",
                    "НарезкаTs", "Нарезка", "СверлениеTs", "Сверление", "МонтажTs", "Монтаж",
                    "ПайкаTs", "Пайка", "СборкаTs", "Сборка", "УпаковкаTs", "Упаковка"};


            using var wbook = new XLWorkbook();
            var ws = wbook.Worksheets.Add("Детализация по светильникам");
            ws.Cell(1, 1).Value = $"Сотрудник {employeeName}: {salary} руб.";
            ws.Cell(1, 1).Style.Font.Italic = true;
            ws.Cell(1, 1).Style.Font.FontColor = XLColor.Red;

            for (int i = 0; i < headers.Count; i++) {
                ws.Cell(2, i + 1).Value = headers[i];
            }

            int col = 1;
            for (int i = 0; i < lamps.Count; i++)
            {
                LedLampDto curr = lamps[i];
                ws.Cell(i + 3, col).Value = curr.Id;
                ws.Cell(i + 3, col + 1).Value = curr.Spec;
                ws.Cell(i + 3, col + 2).Value = curr.Model != null ? curr.Model.ModelName : "";

                ws.Cell(i + 3, col + 3).Value = curr.BitrixOrder;
                ws.Cell(i + 3, col + 4).Value = curr.Comment;

                if (curr.LabelPrintUser != null && curr.LabelPrintUser.Name == employeeName) {
                    if (curr.LabelPrintTs == null) {
                        ws.Cell(i + 3, col + 5).Value = "";
                    }
                    else {
                        ws.Cell(i + 3, col + 5).Value = curr.LabelPrintTs.ToString();
                    }
                    ws.Cell(i + 3, col + 6).Value =  curr.LabelPrintUser.Name;
                }

                if (curr.CutUser != null && curr.CutUser.Name == employeeName) {
                    ws.Cell(i + 3, col + 7).Value = curr.AlProfileCutTs == null ? "" : curr.AlProfileCutTs.ToString();
                    ws.Cell(i + 3, col + 8).Value = curr.CutUser.Name;
                }

                if (curr.DrillUser != null  && curr.DrillUser.Name == employeeName) {
                    ws.Cell(i + 3, col + 9).Value = curr.AlProfileDrillTs == null ? "" : curr.AlProfileDrillTs.ToString();
                    ws.Cell(i + 3, col + 10).Value = curr.DrillUser.Name;
                }

                if (curr.MountingUser != null && curr.MountingUser.Name == employeeName) {
                    ws.Cell(i + 3, col + 11).Value = curr.LedModuleMountingTs == null ? "" : curr.LedModuleMountingTs.ToString();
                    ws.Cell(i + 3, col + 12).Value = curr.MountingUser.Name;
                }

                if (curr.SolderingUser != null && curr.SolderingUser.Name == employeeName) {
                    ws.Cell(i + 3, col + 13).Value = curr.LightSolderingTs == null ? "" : curr.LightSolderingTs.ToString();
                    ws.Cell(i + 3, col + 14).Value = curr.SolderingUser.Name;
                }

                if (curr.AssemblingUser != null && curr.AssemblingUser.Name == employeeName) {
                    ws.Cell(i + 3, col + 15).Value = curr.LightAssemblingTs == null ? "" : curr.LightAssemblingTs.ToString();
                    ws.Cell(i + 3, col + 16).Value =  curr.AssemblingUser.Name;
                }

                if (curr.CheckingPackagingUser != null && curr.CheckingPackagingUser.Name == employeeName) {
                    ws.Cell(i + 3, col + 17).Value = curr.LightCheckingPackagingTs == null ? "" : curr.LightCheckingPackagingTs.ToString();
                    ws.Cell(i + 3, col + 18).Value =  curr.CheckingPackagingUser.Name;
                }
            }

            for (int i = 1; i <= headers.Count; i++) {
                ws.Column(i).Width = 20;

            }

            for (int i = 1; i <= headers.Count; i++) {
                ws.Cell(2, i).Style.Fill.BackgroundColor = XLColor.Aqua;
            }

            wbook.SaveAs(fileName);
        }

    }
}
