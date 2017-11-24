using System;
using System.Collections.Generic;
using System.Text;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;

namespace InvalidDataProcessing.Common
{
    public class NOPIHelper
    {

        public void CreateExcelForData(double[] data, string title, string fileNamePath, string sheetName)
        {
            //HSSFWorkbook workbook = new HSSFWorkbook();
            XSSFWorkbook workbook = new XSSFWorkbook();

            ISheet sheet = workbook.CreateSheet(sheetName);

            IRow rowHeader = sheet.CreateRow(0);
            ICellStyle style = workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;

            ICell cellHeader = rowHeader.CreateCell(0);
            cellHeader.SetCellValue(title);
            cellHeader.CellStyle = style;

            //for (int i = 0; i < data.Length; i++)
            for (int i = 0; i < 1048570; i++)
            {
                IRow row = sheet.CreateRow(i + 1);

                ICell cell = row.CreateCell(0);
                cell.SetCellValue(data[i]);
                cell.CellStyle = style;
            }
            FileStream fs = new FileStream(fileNamePath, FileMode.Create);
            workbook.Write(fs);
            fs.Close();
            workbook = null;
        }

        public void CreateExcelForData(Int64[] data, string title, string fileNamePath, string sheetName)
        {
            //HSSFWorkbook workbook = new HSSFWorkbook();
            XSSFWorkbook workbook = new XSSFWorkbook();

            ISheet sheet = workbook.CreateSheet(sheetName);

            IRow rowHeader = sheet.CreateRow(0);
            ICellStyle style = workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;

            ICell cellHeader = rowHeader.CreateCell(0);
            cellHeader.SetCellValue(title);
            cellHeader.CellStyle = style;

            //for (int i = 0; i < data.Length; i++)
            for (int i = 0; i < 1048570; i++)
            {
                IRow row = sheet.CreateRow(i + 1);

                ICell cell = row.CreateCell(0);
                cell.SetCellValue(data[i]);
                cell.CellStyle = style;
            }
            FileStream fs = new FileStream(fileNamePath, FileMode.Create);
            workbook.Write(fs);
            fs.Close();
            workbook = null;
        }
    }
}
