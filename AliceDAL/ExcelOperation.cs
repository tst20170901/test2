using System;
using System.Collections.Generic;
using System.Text;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using System.IO;
using System.Collections;
using System.Reflection;
using System.Data;

namespace AliceDAL
{
    public class ExcelOperation
    {
        public static void ExportExcel(DataTable dgv, string title, string filePath)
        {
            //保存信息
            //SaveFileDialog saveFileDialog = new SaveFileDialog();
            //saveFileDialog.Filter = "Execl files (*.xls)|*.xls";
            //saveFileDialog.FilterIndex = 0;
            //saveFileDialog.RestoreDirectory = true;
            //saveFileDialog.CreatePrompt = true;
            //saveFileDialog.Title = "导出至Excel文件";
            //saveFileDialog.FileName = title + "-" + DateTime.Now.ToString("yyyy-MM-dd"); ;
            //if (saveFileDialog.ShowDialog() == DialogResult.Cancel)//zly;如果选择提醒导出
            //{
            //    return;
            //}
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            // 表单名
            NPOI.SS.UserModel.Sheet sheet = workbook.CreateSheet("My Sheet");

            //DataGridView行数
            int rowCount = dgv.Rows.Count;
            //DataGridView列数
            int colCount = dgv.Columns.Count;

            //数据表格样式
            NPOI.SS.UserModel.CellStyle dataStyle = workbook.CreateCellStyle();
            dataStyle.BorderBottom = NPOI.SS.UserModel.CellBorderType.THIN;
            dataStyle.BorderLeft = NPOI.SS.UserModel.CellBorderType.THIN;
            dataStyle.BorderTop = NPOI.SS.UserModel.CellBorderType.THIN;
            dataStyle.BorderRight = NPOI.SS.UserModel.CellBorderType.THIN;
            NPOI.SS.UserModel.Font font1 = workbook.CreateFont();
            font1.FontHeight = 14 * 14;
            dataStyle.SetFont(font1);
            dataStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
            // 标题表格样式
            NPOI.SS.UserModel.CellStyle titleStyle = workbook.CreateCellStyle();
            titleStyle.BorderBottom = NPOI.SS.UserModel.CellBorderType.THIN;
            titleStyle.BorderLeft = NPOI.SS.UserModel.CellBorderType.THIN;
            titleStyle.BorderTop = NPOI.SS.UserModel.CellBorderType.THIN;
            titleStyle.BorderRight = NPOI.SS.UserModel.CellBorderType.THIN;
            NPOI.SS.UserModel.Font font = workbook.CreateFont();
            font.FontHeight = 18 * 18;
            titleStyle.SetFont(font);
            titleStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
            //数据表标题           
            NPOI.SS.UserModel.Row titleRow = sheet.CreateRow(0);
            titleRow.Height = 30 * 20;
            NPOI.SS.Util.CellRangeAddress rangTitle = new NPOI.SS.Util.CellRangeAddress(0, 0, 0, colCount - 1);
            sheet.AddMergedRegion(rangTitle);
            //数据表列头 
            NPOI.SS.UserModel.Row colNameRow = sheet.CreateRow(1);
            for (int i = 0; i < colCount; i++)
            {
                //标题行
                NPOI.SS.UserModel.Cell titleCel = titleRow.CreateCell(i);
                titleCel.CellStyle = titleStyle;
                if (i == 0)
                {
                    titleCel.SetCellValue(title);
                }
            }
            NPOI.SS.UserModel.Row dataRows = sheet.CreateRow(1);
            for (int k = 0; k < colCount; k++)
            {
                //把数据保存到二维数组  
                NPOI.SS.UserModel.Cell cel = dataRows.CreateCell(k);
                string valueStr = dgv.Columns[k].ColumnName.ToString();
                cel.CellStyle = dataStyle;
                cel.SetCellValue(valueStr);
            }
            //数据表数据
            for (int j = 0; j < rowCount; j++)
            {
                NPOI.SS.UserModel.Row dataRow = sheet.CreateRow(j + 2);
                for (int k = 0; k < colCount; k++)
                {
                    //if (dgv.Columns[k].Visible != false && dgv.Rows[j].Cells[k].Value != null)
                    //{
                    //把数据保存到二维数组  
                    NPOI.SS.UserModel.Cell cel = dataRow.CreateCell(k);
                    string valueStr = dgv.Rows[j][k].ToString();
                    cel.CellStyle = dataStyle;
                    //设置数据，判断类型
                    int valueInt = 0;
                    double valueDouble = 0.0;
                    DateTime valueDateTime = DateTime.Now;
                    if (int.TryParse(valueStr, out valueInt))
                    {
                        cel.SetCellValue(valueInt);
                    }
                    else if (double.TryParse(valueStr, out valueDouble))
                    {
                        cel.SetCellValue(valueDouble);
                    }
                    else if (DateTime.TryParse(valueStr, out valueDateTime))
                    {
                        cel.SetCellValue(valueDateTime);
                    }
                    else
                    {
                        cel.SetCellValue(valueStr);
                    }
                    //设置列的宽度
                    int colwidth = sheet.GetColumnWidth(k);
                    if (colwidth < valueStr.Length * 18 * 25)
                    {
                        //如果 之前的宽度比现在的宽度大，则不修改宽度
                        sheet.SetColumnWidth(k, valueStr.Length * 18 * 23);
                    }
                    //}
                }
            }
            //保存文件
            FileStream file = new FileStream(filePath, FileMode.Create);
            try
            {
                workbook.Write(ms);
                workbook.Write(file);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                file.Close();
                workbook = null;
                ms.Close();
                ms.Dispose();
            }
        }
        public static void jspc(DataTable dgv, string title, string filePath)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            // 表单名
            NPOI.SS.UserModel.Sheet sheet = workbook.CreateSheet("My Sheet");

            string[] strY = { "rank", "TeacherName", "DeptName", "ClassName", "SD", "AQ", "GC", "YJ", "TP", "FJ", "AllCount" };
            string[] strName = { "名次", "姓名", "年级", "班级", "师德", "安全", "教学过程", "教学业绩", "他评", "附加分", "总分" };
            //DataGridView行数
            int rowCount = dgv.Rows.Count;
            //DataGridView列数
            int colCount = strName.Length;

            //数据表格样式
            NPOI.SS.UserModel.CellStyle dataStyle = workbook.CreateCellStyle();
            dataStyle.BorderBottom = NPOI.SS.UserModel.CellBorderType.THIN;
            dataStyle.BorderLeft = NPOI.SS.UserModel.CellBorderType.THIN;
            dataStyle.BorderTop = NPOI.SS.UserModel.CellBorderType.THIN;
            dataStyle.BorderRight = NPOI.SS.UserModel.CellBorderType.THIN;
            NPOI.SS.UserModel.Font font1 = workbook.CreateFont();
            font1.FontHeight = 14 * 14;
            dataStyle.SetFont(font1);
            dataStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
            // 标题表格样式
            NPOI.SS.UserModel.CellStyle titleStyle = workbook.CreateCellStyle();
            titleStyle.BorderBottom = NPOI.SS.UserModel.CellBorderType.THIN;
            titleStyle.BorderLeft = NPOI.SS.UserModel.CellBorderType.THIN;
            titleStyle.BorderTop = NPOI.SS.UserModel.CellBorderType.THIN;
            titleStyle.BorderRight = NPOI.SS.UserModel.CellBorderType.THIN;
            NPOI.SS.UserModel.Font font = workbook.CreateFont();
            font.FontHeight = 18 * 18;
            titleStyle.SetFont(font);
            titleStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
            //数据表标题           
            NPOI.SS.UserModel.Row titleRow = sheet.CreateRow(0);
            titleRow.Height = 30 * 20;
            NPOI.SS.Util.CellRangeAddress rangTitle = new NPOI.SS.Util.CellRangeAddress(0, 0, 0, colCount - 1);
            sheet.AddMergedRegion(rangTitle);
            //数据表列头 
            NPOI.SS.UserModel.Row colNameRow = sheet.CreateRow(1);

            for (int i = 0; i < colCount; i++)
            {
                //标题行
                NPOI.SS.UserModel.Cell titleCel = titleRow.CreateCell(i);
                titleCel.CellStyle = titleStyle;
                if (i == 0)
                {
                    titleCel.SetCellValue(title);
                }
            }
            NPOI.SS.UserModel.Row dataRows = sheet.CreateRow(1);
            for (int k = 0; k < colCount; k++)
            {
                //把数据保存到二维数组
                NPOI.SS.UserModel.Cell cel = dataRows.CreateCell(k);
                string valueStr = strName.ToString();
                cel.CellStyle = dataStyle;
                cel.SetCellValue(valueStr);
            }
            //数据表数据
            for (int j = 0; j < rowCount; j++)
            {
                NPOI.SS.UserModel.Row dataRow = sheet.CreateRow(j + 2);
                for (int k = 0; k < colCount; k++)
                {
                    //if (dgv.Columns[k].Visible != false && dgv.Rows[j].Cells[k].Value != null)
                    //{
                    //把数据保存到二维数组  
                    NPOI.SS.UserModel.Cell cel = dataRow.CreateCell(k);
                    string valueStr = dgv.Rows[j][strY[k]].ToString();
                    cel.CellStyle = dataStyle;
                    //设置数据，判断类型
                    int valueInt = 0;
                    double valueDouble = 0.0;
                    DateTime valueDateTime = DateTime.Now;
                    if (int.TryParse(valueStr, out valueInt))
                    {
                        cel.SetCellValue(valueInt);
                    }
                    else if (double.TryParse(valueStr, out valueDouble))
                    {
                        cel.SetCellValue(valueDouble);
                    }
                    else if (DateTime.TryParse(valueStr, out valueDateTime))
                    {
                        cel.SetCellValue(valueDateTime);
                    }
                    else
                    {
                        cel.SetCellValue(valueStr);
                    }
                    //设置列的宽度
                    int colwidth = sheet.GetColumnWidth(k);
                    if (colwidth < valueStr.Length * 18 * 25)
                    {
                        //如果 之前的宽度比现在的宽度大，则不修改宽度
                        sheet.SetColumnWidth(k, valueStr.Length * 18 * 23);
                    }
                    //}
                }
            }
            //保存文件
            FileStream file = new FileStream(filePath, FileMode.Create);
            try
            {
                workbook.Write(ms);
                workbook.Write(file);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                file.Close();
                workbook = null;
                ms.Close();
                ms.Dispose();
            }
        }

        public static DataTable ImportExcel(string filePath)
        {
            HSSFWorkbook hssfworkbook;
            try
            {
                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    hssfworkbook = new HSSFWorkbook(file);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            NPOI.SS.UserModel.Sheet sheet = hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            DataTable dt = new DataTable();       //给DdataTable添加表头
            for (int j = 0; j < (sheet.GetRow(0).LastCellNum); j++)
            {
                dt.Columns.Add(Convert.ToChar(((int)'A') + j).ToString());
            }     //读取数据  
            while (rows.MoveNext())
            {
                HSSFRow row = (HSSFRow)rows.Current;
                DataRow dr = dt.NewRow();
                for (int i = 0; i < row.LastCellNum; i++)
                {
                    NPOI.SS.UserModel.Cell cell = row.GetCell(i);
                    if (cell == null)
                    {
                        dr[i] = null;
                    }
                    else
                    {
                        dr[i] = cell.ToString();
                    }
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}
