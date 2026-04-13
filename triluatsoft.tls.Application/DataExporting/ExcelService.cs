using Castle.Core.Logging;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.OldTable.dto;

namespace triluatsoft.tls.DataExporting
{
    public class ExcelService
    {
        public static void DataSetsToExcel(List<FeeDataExport> exportData, string path, string filename, string language)
        {
            string TitleLabel = string.Empty;
            string RefLabel = string.Empty;
            string AdjusterLabel = string.Empty;
            string RateLabel = string.Empty;
            string ExchangeLabel = string.Empty;
            string DateHeader = string.Empty;
            string DesHeader = string.Empty;
            string TimeHeader = string.Empty;
            string AmountHeader = string.Empty;
            string TotalTimeLabel = string.Empty;
            string RateVNDLabel = string.Empty;
            string TotalAmountLabel = string.Empty;

            if (language == "EN")
            {
                TitleLabel = "TIMESHEET";
                RefLabel = "Our Ref.";
                AdjusterLabel = "Adjuster";
                RateLabel = "Rate";
                ExchangeLabel = "Exchange Rate";
                DateHeader = "Date";
                DesHeader = "Description";
                TimeHeader = "Time (hh:mm)";
                AmountHeader = "Amount (VND)";
                TotalTimeLabel = "Total time spent";
                RateVNDLabel = "Rate (VND/hour)";
                TotalAmountLabel = "Professional fee";
            } else
            {
                TitleLabel = "THỜI GIAN LÀM VIỆC";
                RefLabel = "Tham chiếu";
                AdjusterLabel = "Giám định";
                RateLabel = "Mức phí";
                ExchangeLabel = "Tỷ giá quy đổi";
                DateHeader = "Ngày";
                DesHeader = "Mô tả công việc";
                TimeHeader = "Thời gian (hh:mm)";
                AmountHeader = "Thành tiền (VNĐ)";
                TotalTimeLabel = "Tổng thời gian";
                RateVNDLabel = "Mức phí quy đổi (VNĐ/giờ)";
                TotalAmountLabel = "Phí giám định";
            }

            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            //Excel.Workbook xlWorkBook;
            //Excel.Worksheet xlWorkSheet;


            object misValue = System.Reflection.Missing.Value;
            //Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            //Workbook xlWorkbook = xlApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Add(misValue);
            Microsoft.Office.Interop.Excel.Sheets xlSheets = null;
            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = null;
            if (!Directory.Exists(path))   // CHECK IF THE FOLDER EXISTS. IF NOT, CREATE A NEW FOLDER.
            {
                Directory.CreateDirectory(path);
            }

            File.Delete(Path.Combine(path, filename)); // DELETE THE FILE BEFORE CREATING A NEW ONE.


            foreach (FeeDataExport expData in exportData)
            {
                if (expData.dataset.Tables[0].Rows.Count > 0)
                {
                    System.Data.DataTable dataTable = expData.dataset.Tables[0];
                    int rowNo = dataTable.Rows.Count;
                    int columnNo = dataTable.Columns.Count;
                    int colIndex = 0;

                    NullLogger.Instance.Debug(expData.dataset.DataSetName);
                    //Create Excel Sheets
                    xlSheets = xlWorkbook.Sheets;
                    xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlSheets.Add(xlSheets[1], Type.Missing, Type.Missing, Type.Missing);
                    xlWorksheet.Name = expData.dataset.DataSetName;

                    xlWorksheet.Columns[1].ColumnWidth = 13;
                    xlWorksheet.Columns[2].ColumnWidth = 0.5;
                    xlWorksheet.Columns[3].ColumnWidth = 42;
                    xlWorksheet.Columns[4].ColumnWidth = 10;
                    xlWorksheet.Columns[5].ColumnWidth = 14;

                    xlApp.Cells[1, 1] = TitleLabel;

                    //Set sep line
                    Microsoft.Office.Interop.Excel.Range bl_range = xlWorksheet.Range[xlApp.Cells[2, 1], xlApp.Cells[2, columnNo]];
                    bl_range.EntireRow.RowHeight = 7;

                    xlApp.Cells[3, 1] = RefLabel;
                    xlApp.Cells[4, 1] = AdjusterLabel;
                    xlApp.Cells[5, 1] = RateLabel;
                    xlApp.Cells[6, 1] = ExchangeLabel;

                    xlApp.Cells[3, 2] = ":";
                    xlApp.Cells[4, 2] = ":";
                    xlApp.Cells[5, 2] = ":";
                    xlApp.Cells[6, 2] = ":";

                    xlApp.Cells[3, 3] = expData.Refer;
                    xlApp.Cells[4, 3] = expData.Adjuster;
                    if (language == "EN")
                    {
                        xlApp.Cells[5, 3] = "USD$" + expData.userFee.ToString() + "/hrs";
                    }
                    else
                    {
                        xlApp.Cells[5, 3] = "USD$" + expData.userFee.ToString() + "/giờ";
                    }


                    xlApp.Cells[6, 4] = "USD/VND=";
                    xlApp.Cells[6, 5] = expData.exchangeRate;

                    xlApp.Cells[rowNo + 8, 4] = "----------";
                    xlApp.Cells[rowNo + 9, 3] = TotalTimeLabel;
                    xlApp.Cells[rowNo + 9, 4] = expData.TotalWorkTime;
                    xlApp.Cells[rowNo + 10, 3] = RateVNDLabel;
                    xlApp.Cells[rowNo + 10, 5] = expData.userFeeVND;

                    xlApp.Cells[rowNo + 11, 3] = TotalAmountLabel;
                    xlApp.Cells[rowNo + 11, 5] = expData.TotalAmount;

                    //Format header
                    Microsoft.Office.Interop.Excel.Range hd_range = xlWorksheet.Range[xlApp.Cells[1, 1], xlApp.Cells[5, columnNo]];

                    hd_range.EntireRow.Font.Name = "Times New Roman";
                    hd_range.EntireRow.Font.Bold = true;
                    hd_range.EntireRow.Font.Size = 12;

                    xlWorksheet.Range[xlApp.Cells[6, 1], xlApp.Cells[6, 1]].Font.Name = "Times New Roman";
                    xlWorksheet.Range[xlApp.Cells[6, 1], xlApp.Cells[6, 1]].Font.Bold = true;
                    xlWorksheet.Range[xlApp.Cells[6, 1], xlApp.Cells[6, 1]].Font.Size = 12;

                    xlWorksheet.Range[xlApp.Cells[6, 4], xlApp.Cells[6, 5]].Font.Name = "Times New Roman";
                    xlWorksheet.Range[xlApp.Cells[6, 4], xlApp.Cells[6, 5]].Font.Bold = false;
                    xlWorksheet.Range[xlApp.Cells[6, 4], xlApp.Cells[6, 5]].Font.Italic = true;
                    xlWorksheet.Range[xlApp.Cells[6, 4], xlApp.Cells[6, 5]].Font.Size = 12;

                    xlWorksheet.Range[xlApp.Cells[6, 5], xlApp.Cells[6, 5]].NumberFormat = "* #,##0;[Red]-* #,##0";

                    //Merg Tiltle
                    Microsoft.Office.Interop.Excel.Range tt_range = xlWorksheet.Range[xlApp.Cells[1, 1], xlApp.Cells[1, columnNo]];

                    tt_range.Merge(true);
                    tt_range.HorizontalAlignment = XlHAlign.xlHAlignCenter;

                    //Format footer
                    Microsoft.Office.Interop.Excel.Range ft_range = xlWorksheet.Range[xlApp.Cells[rowNo + 8, 1], xlApp.Cells[rowNo + 9, columnNo]];

                    ft_range.EntireRow.Font.Name = "Times New Roman";
                    ft_range.EntireRow.Font.Bold = true;
                    ft_range.EntireRow.Font.Size = 12;

                    xlWorksheet.Range[xlApp.Cells[rowNo + 9, 1], xlApp.Cells[rowNo + 9, 5]].Font.Name = "Times New Roman";
                    xlWorksheet.Range[xlApp.Cells[rowNo + 9, 1], xlApp.Cells[rowNo + 9, 5]].Font.Bold = true;
                    xlWorksheet.Range[xlApp.Cells[rowNo + 9, 1], xlApp.Cells[rowNo + 9, 5]].Font.Size = 12;

                    xlWorksheet.Range[xlApp.Cells[rowNo + 10, 1], xlApp.Cells[rowNo + 10, 5]].Font.Name = "Times New Roman";
                    xlWorksheet.Range[xlApp.Cells[rowNo + 10, 1], xlApp.Cells[rowNo + 10, 5]].Font.Bold = false;
                    xlWorksheet.Range[xlApp.Cells[rowNo + 10, 1], xlApp.Cells[rowNo + 10, 5]].Font.Size = 12;
                    xlWorksheet.Range[xlApp.Cells[rowNo + 10, 5], xlApp.Cells[rowNo + 10, 5]].NumberFormat = "* #,##0;[Red]-* #,##0";

                    xlWorksheet.Range[xlApp.Cells[rowNo + 11, 1], xlApp.Cells[rowNo + 11, 5]].Font.Name = "Times New Roman";
                    xlWorksheet.Range[xlApp.Cells[rowNo + 11, 1], xlApp.Cells[rowNo + 11, 5]].Font.Bold = true;
                    xlWorksheet.Range[xlApp.Cells[rowNo + 11, 1], xlApp.Cells[rowNo + 11, 5]].Font.Size = 12;
                    xlWorksheet.Range[xlApp.Cells[rowNo + 11, 5], xlApp.Cells[rowNo + 11, 5]].NumberFormat = "* #,##0;[Red]-* #,##0";

                    //Generate Field Names
                    //foreach (DataColumn dataColumn in dataTable.Columns)
                    //{
                    //    colIndex++;
                    //    if (colIndex != 2)
                    //    {
                    //        xlApp.Cells[7, colIndex] = dataColumn.ColumnName;
                    //    }                    
                    //}
                    xlApp.Cells[7, 1] = DateHeader;
                    xlApp.Cells[7, 3] = DesHeader;
                    xlApp.Cells[7, 4] = TimeHeader;
                    xlApp.Cells[7, 5] = AmountHeader;

                    //Format header
                    Microsoft.Office.Interop.Excel.Range shd_range = xlWorksheet.Range[xlApp.Cells[7, 1], xlApp.Cells[7, columnNo]];

                    shd_range.EntireRow.Font.Name = "Times New Roman";
                    shd_range.EntireRow.Font.Bold = true;
                    shd_range.EntireRow.Font.Size = 12;
                    shd_range.WrapText = true;
                    shd_range.HorizontalAlignment = XlHAlign.xlHAlignCenter;

                    object[,] objData = new object[rowNo, columnNo];

                    //Convert DataSet to Cell Data
                    for (int row = 0; row < rowNo; row++)
                    {
                        for (int col = 0; col < columnNo; col++)
                        {
                            objData[row, col] = dataTable.Rows[row][col];
                        }
                    }

                    //Add the Data
                    Microsoft.Office.Interop.Excel.Range range = xlWorksheet.Range[xlApp.Cells[8, 1], xlApp.Cells[rowNo + 7, columnNo]];
                    range.Value2 = objData;

                    range.EntireRow.Font.Name = "Times New Roman";
                    range.EntireRow.Font.Bold = false;
                    range.EntireRow.Font.Size = 12;
                    range.WrapText = true;
                    range.Columns[1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    range.EntireColumn[4].HorizontalAlignment = XlHAlign.xlHAlignCenter;

                    //Format Data Type of Columns 
                    colIndex = 0;
                    foreach (DataColumn dataColumn in dataTable.Columns)
                    {
                        colIndex++;
                        string format = "0";
                        switch (dataColumn.DataType.Name)
                        {
                            case "Boolean":
                                break;
                            case "Byte":
                                break;
                            case "Char":
                                break;
                            case "DateTime":
                                format = "dd/mm/yyyy";
                                break;
                            case "Decimal":
                                //format = "$* #,##0.00;[Red]-$* #,##0.00";
                                format = "* #,##0;[Red]-* #,##0";
                                break;
                            case "Double":
                                break;
                            case "Int16":
                                format = "0";
                                break;
                            case "Int32":
                                format = "0";
                                break;
                            case "Int64":
                                format = "0";
                                break;
                            case "SByte":
                                break;
                            case "Single":
                                break;
                            case "TimeSpan":
                                break;
                            case "UInt16":
                                break;
                            case "UInt32":
                                break;
                            case "UInt64":
                                break;
                            //case "String":
                            //    format = "@";
                            //    break;
                            default: //String
                                break;
                        }

                        //Format the Column accodring to Data Type
                        xlWorksheet.Range[xlApp.Cells[8, colIndex], xlApp.Cells[rowNo + 7, colIndex]].NumberFormat = format;
                    }
                    //endhvtam-
                }
            }

            //Remove the Default Worksheet
            ((Microsoft.Office.Interop.Excel.Worksheet)xlApp.ActiveWorkbook.Sheets[xlApp.ActiveWorkbook.Sheets.Count]).Delete();

            //Save
            //xlWorkBook.SaveAs(fileName,
            //    System.Reflection.Missing.Value,
            //    System.Reflection.Missing.Value,
            //    System.Reflection.Missing.Value,
            //    System.Reflection.Missing.Value,
            //    System.Reflection.Missing.Value,
            //    XlSaveAsAccessMode.xlNoChange,
            //    System.Reflection.Missing.Value,
            //    System.Reflection.Missing.Value,
            //    System.Reflection.Missing.Value,
            //    System.Reflection.Missing.Value,
            //    System.Reflection.Missing.Value);
            xlWorkbook.SaveAs(Path.Combine(path, filename));
            //xlWorkbook.SaveAs(path + filename, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            //xlWorkbook.Close();
            //xlApp.Quit();
            //GC.Collect();

            xlWorkbook.Close(false, Type.Missing, Type.Missing);

            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(xlWorkbook);
            xlApp.Quit();
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(xlApp);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public static void FillDatatableToExcel(System.Data.DataTable data, string rsFile, string templateFile, string sheetName, int beginRow, int beginCol,
               int beginFooterRow, int footerRow, int FooterCol, int rowFrom, int colFrom, string From, int rowTo, int colTo, string To, int addHeader,
               int laRow, int laCol, string laName)
        {
            if (File.Exists(rsFile))
            {
                File.Delete(rsFile);
            }
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            object misValue = System.Reflection.Missing.Value;            
            Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(templateFile);
            //Excel.Workbook xlWorkbook = xlApp.Workbooks.Add(misValue);
            Microsoft.Office.Interop.Excel.Sheets xlSheets = null;
            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = null;

            if (rowFrom > 0 && From != "" & colFrom > 0)
            {
                xlApp.Cells[rowFrom, colFrom] = From;
            }
            if (rowTo > 0 && To != "" & colTo > 0)
            {
                xlApp.Cells[rowTo, colTo] = To;
            }

            if (laRow > 0 && laCol > 0 && laName != null && laName != "")
            {
                xlApp.Cells[laRow, laCol] = laName;
            }

            int rowNo = data.Rows.Count;
            int columnNo = data.Columns.Count;
            int colIndex = 0;

            //Create Excel Sheets
            xlSheets = xlWorkbook.Sheets;
            //xlWorksheet = (Excel.Worksheet)xlSheets.Add(xlSheets[1], Type.Missing, Type.Missing, Type.Missing);
            xlWorksheet = xlSheets[sheetName];
            //xlWorksheet.Name = sheetName;
            object[,] objFooter = new object[footerRow, FooterCol];
            for (int row = 0; row < footerRow; row++)
            {
                for (int col = 1; col < FooterCol; col++)
                {
                    object temp = xlApp.Cells[beginFooterRow + row, col].Value;
                    if (temp != null)
                    {
                        objFooter[row, col] = temp;
                    }
                    else
                    {
                        objFooter[row, col] = string.Empty;
                    }
                    //objFooter[row, col] = xlApp.Cells[beginFooterRow + row, col].Value == null ? "" : xlApp.Cells[beginFooterRow + row, col].Value;
                }
            }
            //Generate Field Names
            if (addHeader != 0)
            {
                colIndex = beginCol;
                foreach (DataColumn dataColumn in data.Columns)
                {
                    xlApp.Cells[beginRow, colIndex] = dataColumn.ColumnName;
                    xlApp.Cells[beginRow, colIndex].Font.Name = "Times New Roman";
                    xlApp.Cells[beginRow, colIndex].Font.Bold = true;
                    xlApp.Cells[beginRow, colIndex].Font.Size = 14;
                    xlApp.Cells[beginRow, colIndex].VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    xlApp.Cells[beginRow, colIndex].Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    colIndex++;
                }
            }




            object[,] objData = new object[rowNo, columnNo];

            //Convert DataSet to Cell Data
            for (int row = 0; row < rowNo; row++)
            {
                for (int col = 0; col < columnNo; col++)
                {
                    objData[row, col] = data.Rows[row][col];
                }
            }



            //Add the Data


            Microsoft.Office.Interop.Excel.Range range = xlWorksheet.Range[xlApp.Cells[beginRow + 1, beginCol], xlApp.Cells[beginRow + rowNo, beginCol + columnNo - 1]];
            range.Value2 = objData;

            range.EntireRow.Font.Name = "Times New Roman";
            range.EntireRow.Font.Bold = false;
            range.EntireRow.Font.Size = 12;
            range.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

            //add Footer
            Microsoft.Office.Interop.Excel.Range footer = xlWorksheet.Range[xlApp.Cells[beginRow + rowNo + 1, 1], xlApp.Cells[beginRow + rowNo + footerRow, FooterCol]];


            footer.Value2 = objFooter;


            //Format Data Type of Columns 
            colIndex = beginCol;
            foreach (DataColumn dataColumn in data.Columns)
            {

                string format = "0";
                switch (dataColumn.DataType.Name)
                {
                    case "Boolean":
                        break;
                    case "Byte":
                        break;
                    case "Char":
                        break;
                    case "DateTime":
                        format = "dd/mm/yyyy";
                        break;
                    case "Decimal":
                        //format = "$* #,##0.00;[Red]-$* #,##0.00";
                        format = "* #,##0.00;[Red]-* #,##0.00";
                        break;
                    case "Double":
                        break;
                    case "Int16":
                        format = "0";
                        break;
                    case "Int32":
                        format = "0";
                        break;
                    case "Int64":
                        format = "0";
                        break;
                    case "SByte":
                        break;
                    case "Single":
                        break;
                    case "TimeSpan":
                        break;
                    case "UInt16":
                        break;
                    case "UInt32":
                        break;
                    case "UInt64":
                        break;
                    //case "String":
                    //    format = "@";
                    //    break;
                    default: //String
                        break;
                }
                //Format the Column accodring to Data Type
                xlWorksheet.Range[xlApp.Cells[beginRow + 1, colIndex], xlApp.Cells[beginRow + rowNo, colIndex]].NumberFormat = format;
                colIndex++;
            }
            //endhvtam-



            //Remove the Default Worksheet
            //((Excel.Worksheet)xlApp.ActiveWorkbook.Sheets[xlApp.ActiveWorkbook.Sheets.Count]).Delete();

            //Save
            //xlWorkBook.SaveAs(fileName,
            //    System.Reflection.Missing.Value,
            //    System.Reflection.Missing.Value,
            //    System.Reflection.Missing.Value,
            //    System.Reflection.Missing.Value,
            //    System.Reflection.Missing.Value,
            //    XlSaveAsAccessMode.xlNoChange,
            //    System.Reflection.Missing.Value,
            //    System.Reflection.Missing.Value,
            //    System.Reflection.Missing.Value,
            //    System.Reflection.Missing.Value,
            //    System.Reflection.Missing.Value);
            xlWorkbook.SaveAs(rsFile, Type.Missing, Type.Missing, Type.Missing,
            false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
            Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            //xlWorkbook.SaveAs(path + filename, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            //xlWorkbook.Close();
            xlWorkbook.Close(false, Type.Missing, Type.Missing);

            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(xlWorkbook);
            xlApp.Quit();
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(xlApp);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
