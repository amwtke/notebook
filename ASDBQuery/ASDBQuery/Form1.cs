using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ASTableDefinition;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Query;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Collections;
using System.Threading;

namespace ASDBQuery
{
    public partial class ASDB : Form
    {
        static IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
        public static IObjectContainer _db;
        public static IQuery _query;
        static string Path = CommonHelper.GetAssemblyPath() + @"asdb.yap";

        static ParameterizedThreadStart ParStart = new ParameterizedThreadStart(ExcelHelper.ReadExcel);
        private Thread _threadProcess = new Thread(ParStart);
        public static IntPtr hand;
        public static string tableProcessing;
        public static int ItemNum;
        
        public static IObjectContainer _dbCH;
        public static IQuery _queryCH;
        static IEmbeddedConfiguration _configCH = Db4oEmbedded.NewConfiguration();
        static string PathCH = CommonHelper.GetAssemblyPath() + @"CH.yap";
        public static string CH_filter;

        public ASDB()
        {
            InitializeComponent();
            this.KeyPreview = true;
            hand = this.Handle;
            P_EXCEL.Value = 0;
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case messaging.PROCESSBAR:
                    this.P_EXCEL.Value++;
                    P_EXCEL.Refresh();
                    l_percentage.Text = ((P_EXCEL.Value * 100) / P_EXCEL.Maximum).ToString();
                    l_percentage.Refresh();
                    l_tableName.Text = tableProcessing;
                    l_tableName.Refresh();
                    break;
                case messaging.PROCESSBAR_BEGIN:
                    this.P_EXCEL.Value = 0;
                    this.P_EXCEL.Maximum = ItemNum;
                    break;
                case messaging.PROCESSBAR_COMPLETED:
                    this.P_EXCEL.Value = 0;
                    MessageBox.Show("完工！");
                    break;
                case messaging.CLOSE_CIHUI:
                    TSM_CIHUI.Enabled = true;
                    break;
                case messaging.CLOSE_NOTEBOOK:
                    TSM_NoteBook.Enabled = true;
                    break;
                case messaging.CLOSE_PREFERENCE:
                    TSM_Preference.Enabled = true;
                    break;
                case messaging.CLOSE_PREFERENCE_ALL:
                    TSM_PreferenceALL.Enabled = true;
                    break;
                case messaging.CLOSE_HELP:
                    helpToolStripMenuItem.Enabled = true;
                    break;
            }
            base.WndProc(ref m);
        }

        private void Bt_Search_Click(object sender, EventArgs e)
        {
            if (_query != null)
            {
                IObjectSet result;

                //CommonHelper.ConstrainByType(_query, typeof(ASTable));
                //IQuery temp = CommonHelper.Descend(_query, "_tableName");
                //CommonHelper.Contain(temp, Tb_table.Text.ToUpper());
                //IObjectSet result = _query.Execute();
                _query = CommonHelper.GetQuery(_db, typeof(ASTable), "_tableName", Tb_table.Text.Trim());

                if (tb_Description.Text != string.Empty)
                {
                    //IQuery q = CommonHelper.GetQuery(_db, typeof(ASTable), "_description", tb_Description.Text);
                    IQuery temp = _query.Descend("_description");
                     _query.Constraints().And(temp.Constrain(tb_Description.Text).Like());
                    //temp.Constrain(tb_Description.Text).Like();
                }

                result = _query.Execute();
                ArrayList asArray = new ArrayList();
                for (int i = 0; i < result.Count; i++)
                {
                    asArray.Add((ASTable)result[i]);
                }

                Form_tabel ft = new Form_tabel();
                ft.Show();
                ft.UpdateGrid(asArray);
            }
        }

        private void ASDB_KeyDown(object sender, KeyEventArgs e)
        {
            if (_query != null)
            {
                if (e.KeyCode == System.Windows.Forms.Keys.Enter)
                {
                    if (Tb_table.Text != null)
                    {
                        CommonHelper.ConstrainByType(_query, typeof(ASTable));
                        IQuery temp = CommonHelper.Descend(_query, "_tableName");
                        CommonHelper.Contain(temp, Tb_table.Text.ToUpper());
                    }

                    IObjectSet result = _query.Execute();
                    _query = _db.Query();
                }
                else
                {
                    if (e.KeyCode == System.Windows.Forms.Keys.F5)
                    {
                    }
                }
            }
        }

        private void bt_fields_Click(object sender, EventArgs e)
        {
            if (_query != null)
            {
                if (this.tb_Field.Text != null)
                {
                    CommonHelper.ConstrainByType(_query, typeof(ASField));
                    IQuery temp = CommonHelper.Descend(_query, "_name");
                    CommonHelper.Contain(temp, tb_Field.Text.ToUpper().Trim());
                }

                IObjectSet result = _query.Execute(); _query = _db.Query();

                ASField[] asArray = new ASField[result.Count];
                for (int i = 0; i < result.Count; i++)
                {
                    asArray[i] = (ASField)result[i];
                }
                

                Form_field ff = new Form_field();
                ff.Show();
                ff.UpdateGrid(asArray);
            }
        }

        private void BT_Excel_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = CommonHelper.GetAssemblyPath();
            openFileDialog1.Filter = "All files (*.*)|*.*|Excel files (*.xls)|*.xls";
            //openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                Path = openFileDialog1.FileName;

            ThreadObject TO = new ThreadObject(Path, new int[] { 1, 2 }, this.P_EXCEL);
            _threadProcess.Start(TO);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = CommonHelper.GetAssemblyPath();
            if (!System.IO.File.Exists(CommonHelper.GetAssemblyPath() + @"asdb.yap"))
            {
                openFileDialog1.Filter = "All files (*.*)|*.*|Yap files (*.yap)|*.yap";
                //openFileDialog1.FilterIndex = 2;
                openFileDialog1.RestoreDirectory = true;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    Path = openFileDialog1.FileName;
            }
            else
                Path = CommonHelper.GetAssemblyPath() + @"asdb.yap";

            try
            {
                config.Common.ObjectClass(typeof(ASTable)).CascadeOnUpdate(true);
                if (System.IO.File.Exists(Path))
                {
                    _db = Db4oEmbedded.OpenFile(config, Path);
                    _query = _db.Query();
                }
                else
                {
                    throw new Exception("找不到数据文件！");
                }

                button1.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public enum Join
        {
            AND,
            OR,
            NOT,
        }

        public static class ExcelHelper
        {
            static string[] columns = { "B", "C", "D", "G", "H", "I", "J", "K", "W", "AA", "AB" };
            static Microsoft.Office.Interop.Excel.Application excel = null;
            static Excel.Workbooks wbs = null;
            static Excel.Workbook wb = null;
            static Excel.Worksheet ws = null;
            static Excel.Range range1 = null;
            static object Nothing = System.Reflection.Missing.Value;

            static Dictionary<string, string> _ColumnDic;

            readonly static string YapFileName = System.IO.Path.Combine(CommonHelper.GetAssemblyPath() + @"asdb.yap");
            static ExcelHelper()
            {
                _ColumnDic = new Dictionary<string, string>();

                Type asFields = typeof(ASField);
                PropertyInfo[] fs = asFields.GetProperties();
                for (int i = 1; i < 12; i++)
                {
                    _ColumnDic.Add(fs[i].Name, columns[i - 1]);
                }

                if (System.IO.File.Exists(YapFileName))
                    System.IO.File.Delete(YapFileName);
            }

            public static void ReadExcel(object o)
            {
                ThreadObject argObj = (ThreadObject)o;

                //Sheet 1
                try
                {
                    excel = new Excel.Application();
                    excel.UserControl = true;
                    excel.DisplayAlerts = false;
                    excel.Application.Workbooks.Open(argObj.FilePath, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing);

                    wbs = excel.Workbooks;
                    wb = wbs[1];


                    ws = (Excel.Worksheet)wb.Worksheets[argObj.SheetIndex[0]];

                    int rowCount = ws.UsedRange.Rows.Count;
                    int colCount = ws.UsedRange.Columns.Count;

                    if (rowCount <= 0)
                        throw new Exception("文件中没有数据记录");

                    ASDB.ItemNum = rowCount - 1;
                    messaging.FireMessage(messaging.PROCESSBAR_BEGIN);

                    IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
                    config.Common.ObjectClass(typeof(ASTable)).CascadeOnUpdate(true);
                    using (IObjectContainer db = Db4oEmbedded.OpenFile(config, YapFileName))
                    {
                        for (int r = 2; r < rowCount; r++)
                        {
                            //table section
                            ASTable aT = null;
                            string tableName = (string)ws.get_Range("A" + r.ToString()).Value;
                            Console.WriteLine("Now: " + tableName);

                            IQuery query = db.Query();
                            query.Constrain(typeof(ASTable));
                            IQuery tableNameQuery = query.Descend("_tableName");
                            tableNameQuery.Constrain(tableName);
                            IObjectSet result = query.Execute();
                            if (result.Count > 1) { throw new Exception("多插入了！"); }
                            if (result.Count == 1 && result[0] is ASTable)
                            {
                                aT = (ASTable)result[0];

                                //field section
                                ASField asF = new ASField();
                                asF.TableName = tableName;
                                PropertyInfo[] ps = typeof(ASField).GetProperties();
                                int i = 1;
                                foreach (KeyValuePair<string, string> pair in _ColumnDic)
                                {
                                    string index = pair.Value + r.ToString();
                                    object oTemp = ws.get_Range(index).Value;
                                    if (oTemp == null)
                                        ps[i].SetValue(asF, oTemp, null);
                                    else
                                        ps[i].SetValue(asF, oTemp.ToString(), null);
                                    i++;
                                }
                                aT.Fields.Add(asF);
                                db.Store(aT);
                            }
                            else
                            {
                                aT = new ASTable(tableName, new ArrayList(), string.Empty);

                                //field section
                                ASField asF = new ASField();
                                asF.TableName = tableName;
                                PropertyInfo[] ps = typeof(ASField).GetProperties();
                                int i = 1;
                                foreach (KeyValuePair<string, string> pair in _ColumnDic)
                                {
                                    string index = pair.Value + r.ToString();
                                    object oTemp = ws.get_Range(index).Value;
                                    if (oTemp == null)
                                        ps[i].SetValue(asF, oTemp, null);
                                    else
                                        ps[i].SetValue(asF, oTemp.ToString(), null);
                                    i++;
                                }
                                aT.Fields.Add(asF);


                                db.Store(aT);
                            }

                            ASDB.tableProcessing = tableName;
                            messaging.FireMessage(messaging.PROCESSBAR);
                        }

                        //sheet 2
                        Console.WriteLine("Begin sheet 2");
                        ws = (Excel.Worksheet)wb.Worksheets[argObj.SheetIndex[1]];

                        rowCount = ws.UsedRange.Rows.Count;
                        colCount = ws.UsedRange.Columns.Count;

                        ASDB.ItemNum = rowCount - 1;
                        messaging.FireMessage(messaging.PROCESSBAR_BEGIN);

                        for (int i = 2; i < rowCount; i++)
                        {
                            IQuery query2 = db.Query();
                            query2.Constrain(typeof(ASTable));
                            IQuery q2 = query2.Descend("_tableName");

                            string tableName = (string)ws.get_Range("A" + i.ToString()).Value;
                            if (tableName != null && tableName != string.Empty)
                            {
                                q2.Constrain(tableName);
                                IObjectSet result = query2.Execute();
                                if (result.Count == 1)
                                {
                                    ASTable table = (ASTable)result[0];
                                    table.Description = (string)ws.get_Range("C" + i.ToString()).Value;
                                    table.Description_CN = (string)ws.get_Range("D" + i.ToString()).Value;
                                    table.Panels = (string)ws.get_Range("E" + i.ToString()).Value;
                                    db.Store(table);
                                }
                                Console.WriteLine("sheet 2" + tableName);
                            }

                            ASDB.tableProcessing = tableName;
                            messaging.FireMessage(messaging.PROCESSBAR);
                        }
                    };
                    messaging.FireMessage(messaging.PROCESSBAR_COMPLETED);
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
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(ws);
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

        public class ThreadObject
        {
            string _filePath;
            int[] _sheetIndex;
            System.Windows.Forms.ProgressBar _bar;
            public ThreadObject(string filePath, int[] sheetIndex, System.Windows.Forms.ProgressBar bar)
            {
                _filePath = filePath;
                _sheetIndex = sheetIndex;
                _bar = bar;
            }
            public string FilePath
            {
                get
                {
                    return _filePath;
                }
            }

            public int[] SheetIndex
            {
                get
                {
                    return _sheetIndex;
                }
            }

            public System.Windows.Forms.ProgressBar ProcessBar
            {
                get
                {
                    return _bar;
                }
                set
                {
                    _bar = value;
                }
            }
        }

        private void bt_cihui_Click(object sender, EventArgs e)
        {
            try
            {
                if (_dbCH == null || _queryCH == null)
                {
                    _configCH.Common.ObjectClass(typeof(CH)).CascadeOnUpdate(true);

                    _dbCH = Db4oEmbedded.OpenFile(_configCH, PathCH);
                    _queryCH = _dbCH.Query();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (_dbCH != null || _queryCH != null)
            {
                if (this.tb_CH.Text != null)
                {
                    CH_filter = tb_CH.Text;

                    CommonHelper.ConstrainByType(_queryCH, typeof(CH));
                    IQuery temp = CommonHelper.Descend(_queryCH, "_en");
                    temp.Constrain(CH_filter).Like();
                    //CommonHelper.Contain(temp, tb_CH.Text);
                }

                IObjectSet result = _queryCH.Execute(); 
                _queryCH = _dbCH.Query();

                CH[] chArray = new CH[result.Count];
                for (int i = 0; i < result.Count; i++)
                {
                    chArray[i] = (CH)result[i];
                }


                Form_CH fc = new Form_CH();
                fc.Show();
                if (chArray != null || chArray.Length != 0)
                {
                    fc.UpdateGrid(chArray);
                }
            }
        }

        private void Tb_table_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
                Bt_Search.PerformClick();
        }

        private void tb_Description_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
                Bt_Search.PerformClick();
        }

        private void tb_Field_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                bt_fields.PerformClick();
        }

        private void bt_Preference_Click(object sender, EventArgs e)
        {
            Form_Preference p = new Form_Preference();
            p.Show();
        }

        private void bt_preference_ALL_Click(object sender, EventArgs e)
        {
            From_Preference_ALL p = new From_Preference_ALL();
            p.Show();
        }

        private void BT_NoteBook_Click(object sender, EventArgs e)
        {
            Form_Notebook n = new Form_Notebook();
            n.WindowState = FormWindowState.Maximized;
            n.Show();
        }

        private void ASDB_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this.notifyIcon1.Visible = true;
            } 
        }

        #region xianshi
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.Visible = true;
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void TSM_CIHUI_Click(object sender, EventArgs e)
        {
            try
            {
                if (_dbCH == null || _queryCH == null)
                {
                    _configCH.Common.ObjectClass(typeof(CH)).CascadeOnUpdate(true);

                    _dbCH = Db4oEmbedded.OpenFile(_configCH, PathCH);
                    _queryCH = _dbCH.Query();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (_dbCH != null || _queryCH != null)
            {
                if (this.tb_CH.Text != null)
                {
                    CH_filter = tb_CH.Text;

                    CommonHelper.ConstrainByType(_queryCH, typeof(CH));
                    IQuery temp = CommonHelper.Descend(_queryCH, "_en");
                    temp.Constrain(CH_filter).Like();
                    //CommonHelper.Contain(temp, tb_CH.Text);
                }

                IObjectSet result = _queryCH.Execute();
                _queryCH = _dbCH.Query();

                CH[] chArray = new CH[result.Count];
                for (int i = 0; i < result.Count; i++)
                {
                    chArray[i] = (CH)result[i];
                }


                Form_CH fc = new Form_CH();
                fc.Show();
                if (chArray != null || chArray.Length != 0)
                {
                    fc.UpdateGrid(chArray);
                }

            }

            TSM_CIHUI.Enabled = false;
        }

        private void TSM_Preference_Click(object sender, EventArgs e)
        {
            Form_Preference p = new Form_Preference();
            p.Show();
            TSM_Preference.Enabled = false;
        }

        private void TSM_PreferenceALL_Click(object sender, EventArgs e)
        {
            From_Preference_ALL p = new From_Preference_ALL();
            p.Show();
            TSM_PreferenceALL.Enabled = false;
        }

        private void TSM_NoteBook_Click(object sender, EventArgs e)
        {
            Form_Notebook n = new Form_Notebook();
            n.WindowState = FormWindowState.Maximized;
            n.Show();
            TSM_NoteBook.Enabled = false;
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
                contextMenuStrip1.Show();
        }
        #endregion

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpForm f = new
HelpForm();
            f.Show();
            helpToolStripMenuItem.Enabled = false;
        }
    }

    public static class CommonHelper
    {
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

        public static IConstraint ConstrainByType(IQuery query, Type t)
        {
            if (query != null)
                return query.Constrain(t);
            else
                throw new Exception("Query is null in ConstrainByType!");
        }

        public static IQuery Descend(IQuery query, string fieldName)
        {
            if (query != null)
            {
                return query.Descend(fieldName);
            }
            return null;
        }
        public static IConstraint Contain(IQuery query, string containString)
        {
            if (query != null)
            {
                return query.Constrain(containString).Contains();
            }
            throw new Exception("Query is null in Contain!");
        }

        public static IObjectSet GetResultLike(IQuery q,Type T,string filedName,string sreachWord)
        {
            CommonHelper.ConstrainByType(q, T);
            IQuery temp = CommonHelper.Descend(q, filedName);
            IConstraint _iconstrain = CommonHelper.Contain(temp, sreachWord);

            return q.Execute();
        }

        public static IQuery GetQuery(IObjectContainer db,Type T, string fieldName, string keyWord)
        {
            IQuery q = db.Query();
            q.Constrain(T);
            IQuery temp = q.Descend(fieldName);
            temp.Constrain(keyWord).Like();
            return q;
        }

        public static IConstraint GetConstrain(IObjectContainer db, Type T, string fieldName, string keyWord)
        {
            IQuery q = db.Query();
            q.Constrain(T);
            return q.Descend(fieldName).Constrain(keyWord).Like();
        }
    }
}
