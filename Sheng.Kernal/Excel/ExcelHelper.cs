/*
********************************************************************
*
*    曹旭升（sheng.c）
*    E-mail: cao.silhouette@msn.com
*    QQ: 279060597
*    https://github.com/iccb1013
*    http://shengxunwei.com
*
*    © Copyright 2017
*
********************************************************************/


using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sheng.Kernal
{
    public class ExcelHelper
    {
        public static void Export(ExcelExportArgs args)
        {
            if (args.ColumnDefine == null || args.ColumnDefine.Count == 0)
                throw new ArgumentException("没有指定 ColumnDefine");

            if (String.IsNullOrEmpty(args.FilePath))
                throw new ArgumentException("没有指定 FilePath");


            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();

            int nextRowIndex = 0;
            int nextCellIndex = 0;

            //创建表头
            IRow row = sheet.CreateRow(nextRowIndex);
            nextRowIndex++;

            if (args.RowNumber)
            {
                ICell cell = row.CreateCell(nextCellIndex);
                cell.SetCellValue("序号");
                nextCellIndex++;
            }
            
            foreach (var item in args.ColumnDefine)
            {
                ICell cell = row.CreateCell(nextCellIndex);
                cell.SetCellValue(item.Title);

                nextCellIndex++;
            }

            //写入内容
            if (args.DataList != null)
            {
                int rowNumber = 0;
                foreach (var data in args.DataList)
                {
                    if (data == null)
                        continue;

                    rowNumber++;

                    nextCellIndex = 0;

                    row = sheet.CreateRow(nextRowIndex);

                    if (args.RowNumber)
                    {
                        ICell cell = row.CreateCell(nextCellIndex);
                        cell.SetCellValue(rowNumber.ToString());
                        nextCellIndex++;
                    }

                    foreach (var column in args.ColumnDefine)
                    {
                        object value = ReflectionHelper.GetPropertyValue(data, column.DataProperty);
                        if (value != null)
                        {
                            ICell cell = row.CreateCell(nextCellIndex);
                            if (column.FormatValueFunc == null)
                            {
                                cell.SetCellValue(value.ToString());
                            }else
                            {
                                cell.SetCellValue(column.FormatValueFunc(value));
                            }

                        }
                        nextCellIndex++;
                    }

                    nextRowIndex++;
                }
            }

            FileStream fsExportFile = new FileStream(args.FilePath, FileMode.OpenOrCreate, FileAccess.Write);
            workbook.Write(fsExportFile);

        }

        public static void ExportByDictionary(ExcelExportByDictionaryArgs args)
        {
            if (args.ColumnDefine == null || args.ColumnDefine.Count == 0)
                throw new ArgumentException("没有指定 ColumnDefine");

            if (String.IsNullOrEmpty(args.FilePath))
                throw new ArgumentException("没有指定 FilePath");


            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();

            int nextRowIndex = 0;
            int nextCellIndex = 0;

            //创建表头
            IRow row = sheet.CreateRow(nextRowIndex);
            nextRowIndex++;

            if (args.RowNumber)
            {
                ICell cell = row.CreateCell(nextCellIndex);
                cell.SetCellValue("序号");
                nextCellIndex++;
            }

            foreach (var item in args.ColumnDefine)
            {
                ICell cell = row.CreateCell(nextCellIndex);
                cell.SetCellValue(item.Title);

                nextCellIndex++;
            }

            //写入内容
            if (args.DataList != null)
            {
                int rowNumber = 0;
                foreach (var data in args.DataList)
                {
                    if (data == null)
                        continue;

                    rowNumber++;

                    nextCellIndex = 0;

                    row = sheet.CreateRow(nextRowIndex);

                    if (args.RowNumber)
                    {
                        ICell cell = row.CreateCell(nextCellIndex);
                        cell.SetCellValue(rowNumber.ToString());
                        nextCellIndex++;
                    }

                    foreach (var column in args.ColumnDefine)
                    {
                        if (data.ContainsKey(column.DataProperty))
                        {
                            object value = data[column.DataProperty];
                            if (value != null)
                            {
                                ICell cell = row.CreateCell(nextCellIndex);
                                if (column.FormatValueFunc == null)
                                {
                                    cell.SetCellValue(value.ToString());
                                }
                                else
                                {
                                    cell.SetCellValue(column.FormatValueFunc(value));
                                }
                            }
                        }

                        nextCellIndex++;
                    }

                    nextRowIndex++;
                }
            }

            FileStream fsExportFile = new FileStream(args.FilePath, FileMode.OpenOrCreate, FileAccess.Write);
            workbook.Write(fsExportFile);

        }

    }
}
