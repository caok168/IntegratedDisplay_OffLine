using System;
using System.Collections.Generic;
using System.Text;
using DataProcessAdvance;
using MathWorks.MATLAB.NET.Arrays;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;

namespace InvalidDataProcessing
{
    /// <summary>
    /// 计算概率密度和累积分布
    /// </summary>
    public class CalculateDistrubtionClass
    {
        public AccelerateNew.AccelerateNewClass anc;

        public CalculateDistrubtionClass()
        {
            anc = new AccelerateNew.AccelerateNewClass();
        }

        public void CalDistrubtionProcess(double[] dwx, double[] dwvelo, string folderPath, string fileName, double rmsMean, string fileName_tii)
        {
            MWNumericArray wx = new MWNumericArray(dwx);
            Array.Clear(dwx, 0, dwx.Length);
            MWNumericArray wvelo = new MWNumericArray(dwvelo);
            Array.Clear(dwvelo, 0, dwvelo.Length);

            //MWArray resultMW = anc.sub_calculate_distrubtion_with_one_output(wx, wvelo);
            MWArray resultMW = anc.sub_calculate_rms_distrubtion(wx, wvelo);
            Array resultArray = resultMW.ToArray();

            int count = resultArray.Length / 3;
            double[] d1 = new double[count];
            double[] d2 = new double[count];
            double[] d3 = new double[count];

            double[,] resultArray_A = (double[,])resultArray;

            for (int i = 0; i < count; i++)
            {
                d1[i] = resultArray_A[i, 0];
                d2[i] = resultArray_A[i, 1];
                d3[i] = resultArray_A[i, 2];
            }

            MWArray resultMW_tii = anc.sub_calculate_tii_distrubtion(wx, wvelo, rmsMean);
            Array resultArray_tii = resultMW_tii.ToArray();

            int count_tii = resultArray_tii.Length / 3;
            double[] d1_tii = new double[count_tii];
            double[] d2_tii = new double[count_tii];
            double[] d3_tii = new double[count_tii];

            double[,] resultArray_A_tii = (double[,])resultArray_tii;

            for (int i = 0; i < count_tii; i++)
            {
                d1_tii[i] = resultArray_A_tii[i, 0];
                d2_tii[i] = resultArray_A_tii[i, 1];
                d3_tii[i] = resultArray_A_tii[i, 2];
            }

            CreateExcel(d1, d2, d3, folderPath, fileName);

            CreateExcel(d1_tii, d2_tii, d3_tii, folderPath, fileName_tii);
        }

        public void CalDistrubtionProcess_Prev(double[] dwx, double[] dwvelo, string folderPath, string fileName)
        {
            MWNumericArray wx = new MWNumericArray(dwx);
            Array.Clear(dwx, 0, dwx.Length);
            MWNumericArray wvelo = new MWNumericArray(dwvelo);
            Array.Clear(dwvelo, 0, dwvelo.Length);

            MWArray resultMW = anc.sub_calculate_distrubtion_with_one_output(wx, wvelo);
            Array resultArray = resultMW.ToArray();
            int count = resultArray.Length / 3;
            double[] d1 = new double[count];
            double[] d2 = new double[count];
            double[] d3 = new double[count];

            double[,] resultArray_A = (double[,])resultArray;

            for (int i = 0; i < count; i++)
            {
                d1[i] = resultArray_A[i, 0];
                d2[i] = resultArray_A[i, 1];
                d3[i] = resultArray_A[i, 2];
            }

            CreateExcel(d1, d2, d3, folderPath, fileName);
        }

        private void CreateExcel(double[] d1,double[] d2,double[] d3,string folderPath,string fileName)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();

            IRow rowHeader = sheet.CreateRow(0);
            ICellStyle style = workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;

            ICell cellHeader1 = rowHeader.CreateCell(0);
            cellHeader1.SetCellValue("信号值");
            cellHeader1.CellStyle = style;

            ICell cellHeader2 = rowHeader.CreateCell(1);
            cellHeader2.SetCellValue("概率密度");
            cellHeader2.CellStyle = style;

            ICell cellHeader3 = rowHeader.CreateCell(2);
            cellHeader3.SetCellValue("累积分布");
            cellHeader3.CellStyle = style;

            for (int i = 0; i < d1.Length; i++)
            {
                IRow row = sheet.CreateRow(i + 1);

                ICell cell1 = row.CreateCell(0);
                cell1.SetCellValue(d1[i]);
                cell1.CellStyle = style;

                ICell cell2 = row.CreateCell(1);
                cell2.SetCellValue(d2[i]);
                cell2.CellStyle = style;

                ICell cell3 = row.CreateCell(2);
                cell3.SetCellValue(d3[i]);
                cell3.CellStyle = style;
            }

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            string fileNamePath = folderPath + "\\" + fileName + ".csv";
            FileStream fs = new FileStream(fileNamePath, FileMode.Create);
            workbook.Write(fs);
            fs.Close();
            workbook = null;
        }
    }
}
