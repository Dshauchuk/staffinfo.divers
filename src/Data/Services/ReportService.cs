using AutoMapper;
using Microsoft.Office.Interop.Excel;
using Staffinfo.Divers.Data.Repositories.Contracts;
using Staffinfo.Divers.Data.Services.Contracts;
using Staffinfo.Divers.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace Staffinfo.Divers.Data.Services
{
    public class ReportService : IReportService
    {
        [DllImportAttribute("user32.dll", EntryPoint = "GetWindowThreadProcessId")]
        public static extern int GetWindowThreadProcessId([InAttribute()] IntPtr hWnd, out int lpdwProcessId);

        private readonly IDiverRepository _diverRepository;
        private readonly IReportDataService _reportDataService;
        private readonly IMapper _mapper;

        public ReportService(IDiverRepository diverRepository, IReportDataService reportDataService, IMapper mapper)
        {
            _diverRepository = diverRepository;
            _reportDataService = reportDataService;
            _mapper = mapper;
        }

        public async Task<byte[]> GenerateExcelReport()
        {
            Application xlApp = new Application();
            Workbook xlWorkBook = null;
            Worksheet xlWorkSheet = null;
            try
            {
                object misValue = Missing.Value;

                xlWorkBook = xlApp.Workbooks.Add(misValue);

                xlWorkSheet = xlWorkBook.Worksheets.get_Item(1);

                string[] colNames = new string[]{
                    "Название станции",
                    "Ф.И.О",
                    "Дата рождения",
                    "Мед. освидетельствование",
                    "Всего часов",
                    "Всего часов в этом году"
                };

                for (int i = 0; i < colNames.Length; i++)
                {
                    xlWorkSheet.Cells[1, i + 1] = colNames[i];
                    (xlWorkSheet.Cells[1, i + 1] as Excel.Range).Font.Bold = true;
                    (xlWorkSheet.Cells[1, i + 1] as Excel.Range).Borders.LineStyle = XlLineStyle.xlContinuous;
                }

                var diversReportData = (await _reportDataService.GetReportDatas()).ToArray();

                for(int i = 0, row = 2; i < diversReportData.Length; i++, row++)
                {
                    xlWorkSheet.Cells[row, 1] = diversReportData[i].StationName;
                    xlWorkSheet.Cells[row, 2] = diversReportData[i].Name;
                    xlWorkSheet.Cells[row, 3] = diversReportData[i].BirthDate;
                    xlWorkSheet.Cells[row, 4] = diversReportData[i].MedicalExaminationDate;
                    xlWorkSheet.Cells[row, 5] = diversReportData[i].TotalHours;
                    xlWorkSheet.Cells[row, 6] = diversReportData[i].HoursThisYear;
                }
                xlWorkSheet.UsedRange.Columns.AutoFit();

                string fileName = "csharp-Excel.xlsx";
                var path = Path.Combine(Directory.GetCurrentDirectory(), @"AppData\Temp\", fileName); //AppDomain.CurrentDomain.BaseDirectory + fileName; Directory.GetCurrentDirectory()
                if (File.Exists(path))
                    File.Delete(path);
                xlWorkBook.SaveAs(path,
                    XlFileFormat.xlWorkbookDefault,
                    Type.Missing, Type.Missing, false, false, XlSaveAsAccessMode.xlNoChange,
                    XlSaveConflictResolution.xlLocalSessionChanges, Type.Missing, Type.Missing);
                xlWorkBook.Close(true, Missing.Value, Missing.Value);

                byte[] fileBytes = File.ReadAllBytes(path);
                File.Delete(path);

                return fileBytes;
            }
            finally
            {
                if (xlWorkSheet != null)
                    Marshal.FinalReleaseComObject(xlWorkSheet);

                if (xlWorkBook != null)
                {
                    Marshal.FinalReleaseComObject(xlWorkBook);
                }

                if (xlApp != null)
                {
                    int ExcelPID = 0;
                    int Hwnd = 0;
                    Hwnd = xlApp.Hwnd;
                    System.Diagnostics.Process ExcelProcess;
                    GetWindowThreadProcessId((IntPtr)Hwnd, out ExcelPID);
                    ExcelProcess = System.Diagnostics.Process.GetProcessById(ExcelPID);

                    xlApp.Quit();
                    Marshal.FinalReleaseComObject(xlApp);

                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    ExcelProcess.Kill();
                    ExcelProcess = null;
                }
            }
        }
    }
}
