using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Collections;
using System.Threading;
using System.Runtime.Remoting.Messaging;

namespace ExcelTools
{
    public static class ExcelHelper
    {
        public delegate void WorkSheetHandler(Excel.Worksheet ws);

        static Microsoft.Office.Interop.Excel.Application excel = null;
        static Excel.Workbooks wbs = null;
        static Excel.Workbook wb = null;
        static Excel.Worksheet[] ws = null;
        static Excel.Range range1 = null;
        static object Nothing = System.Reflection.Missing.Value;

        static ExcelHelper()
        {
        }

        public static string GetString(Excel.Worksheet ws, string colAlfa, int offset)
        {
            if (ws.get_Range(colAlfa + offset.ToString()).Value != null)
                return Convert.ToString(ws.get_Range(colAlfa + offset.ToString()).Value);
            else
                return null;
        }

        public static string GetString2(Excel.Worksheet ws, string colAlfa, int offset)
        {
            object o = ws.get_Range(colAlfa + offset.ToString()).Value;
            if ( o!= null)
                return Convert.ToString(o);
            else
                return null;
        }

        public static Dictionary<string, string> MapFieldNameAndColumAlfa(Type T, string[] alfaCol)
        {
            Dictionary<string, string> _dic = new Dictionary<string, string>();
            PropertyInfo[] pis = T.GetProperties();
            if (pis.Length == alfaCol.Length)
            {
                for (int i = 0; i < pis.Length; i++)
                {
                    _dic.Add(pis[i].Name, alfaCol[i]);
                }
                return _dic;
            }
            return null;
        }

        public static void ReadExcel(string excelPath, int[] SheetIndex, WorkSheetHandler[] wsCall)
        {
            //YapFileName = System.IO.Path.Combine(CommonHelper.GetAssemblyPath() + dbFilename);

            try
            {
                excel = new Excel.Application();
                excel.UserControl = true;
                excel.DisplayAlerts = false;
                excel.Application.Workbooks.Open(excelPath, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing);

                wbs = excel.Workbooks;
                wb = wbs[1];//工作簿

                if (SheetIndex.Length < wb.Worksheets.Count)
                {
                    ws = new Excel.Worksheet[wb.Worksheets.Count];

                    for (int i = 0; i < SheetIndex.Length; i++)
                    {
                        ws[i] = (Excel.Worksheet)wb.Worksheets[SheetIndex[i]];
                    }
                }

                if (SheetIndex.Length == wsCall.Length)
                {
                    for (int j = 0; j < SheetIndex.Length; j++)
                    {
                        if (wsCall[j] != null)
                            wsCall[j](ws[j]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (excel != null)
                {
                    if (wbs != null)
                    {
                        if (wb != null)
                        {
                            if (ws != null)
                            {
                                if (range1 != null)
                                {
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(range1);
                                    range1 = null;
                                }

                                foreach (Excel.Worksheet w in ws)
                                {
                                    if (w != null)
                                        System.Runtime.InteropServices.Marshal.ReleaseComObject(w);
                                }

                                ws = null;
                            }
                            wb.Close(false, Nothing, Nothing);
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(wb);
                            wb = null;
                        }
                        wbs.Close();
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(wbs);
                        wbs = null;
                    }
                    excel.Application.Workbooks.Close();
                    excel.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                    excel = null;
                    GC.Collect();
                }
            }
        }

        public static string GetAssemblyPath()
        {
            string _CodeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            _CodeBase = _CodeBase.Substring(8, _CodeBase.Length - 8);    // 8是 file:// 的长度
            string[] arrSection = _CodeBase.Split(new char[] { '/' });

            string _FolderPath = "";
            for (int i = 0; i < arrSection.Length - 1; i++)
            {
                _FolderPath += arrSection[i] + "/";
            }
            return _FolderPath;
        }
    }
}
