using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Linkedin.Entities;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;


namespace Linkedin.Services
{
    class ExcelSaver
    {
        public ExcelSaver(List<Profile> profiles)
        {
            this._profiles = profiles;
        }

        private List<Profile> _profiles;
        public  void SaveIntoExcel()
        {
          
                IWorkbook workbook = new XSSFWorkbook();
                ISheet sheet1 = workbook.CreateSheet("Sheet1");

                IRow row1 = sheet1.CreateRow(0);
                row1.CreateCell(0).SetCellValue("Имя");
                row1.CreateCell(1).SetCellValue("Должность");
                row1.CreateCell(2).SetCellValue("Рабочая почта");
                row1.CreateCell(3).SetCellValue("Место работы");
                row1.CreateCell(4).SetCellValue("Линк");

                for (var index = 0; index < _profiles.Count; index++)
                {
                    var profile = _profiles[index];
                    IRow row2 = sheet1.CreateRow(index+1);
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
    }
}