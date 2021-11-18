using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using Linkedin.Entities;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;


namespace Linkedin.Services
{
    class ExcelWorker
    {

        public void SaveIntoExcel(List<Profile> _profiles)
        {

            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet1 = workbook.CreateSheet("Accounts");

            IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("Имя");
            row1.CreateCell(1).SetCellValue("Должность");
            row1.CreateCell(2).SetCellValue("Рабочая почта");
            row1.CreateCell(3).SetCellValue("Место работы");
            row1.CreateCell(4).SetCellValue("Линк");

            for (var index = 0; index < _profiles.Count; index++)
            {
                var profile = _profiles[index];
                IRow row2 = sheet1.CreateRow(index + 1);
                row2.CreateCell(0).SetCellValue(profile.name);
                row2.CreateCell(1).SetCellValue(profile.current_title);
                row2.CreateCell(2).SetCellValue(profile.Emails?.FirstOrDefault()?.email);
                row2.CreateCell(3).SetCellValue(profile.current_employer);
                row2.CreateCell(4).SetCellValue(profile.linkedin_url);
            }
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Files");
            Directory.CreateDirectory(path);
            FileStream sw = File.Create(Path.Combine(path, $"result_{DateTime.Now.ToString("yyyy.MM.dd_hh.mm.ss")}.xlsx"));
            workbook.Write(sw);
            sw.Close();
        }
        public List<string> GetListOfNames(string path)
        {
            IWorkbook book;

            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            book = new XSSFWorkbook(fs);

            var sheet = book.GetSheetAt(0);
            var rowIndex = 1;
            var ListOfFirstCellsValues = new List<string>();
            while (!string.IsNullOrEmpty(sheet.GetRow(rowIndex).GetCell(0).StringCellValue))
            {
                var row = sheet.GetRow(rowIndex);
                var cell = row.GetCell(0);
                ListOfFirstCellsValues.Add(cell.StringCellValue);
                rowIndex++;
            }

            return ListOfFirstCellsValues;
        }
    }
}