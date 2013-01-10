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
using System.Runtime.Remoting.Messaging;
using ExcelTools;

namespace ASDBQuery
{
    public partial class Form_Preference : Form
    {
        ExcelHelper.WorkSheetHandler _workProcess;
        IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();

        static string _dbFileName = "Preference.yap";
        readonly static string YapFileName = System.IO.Path.Combine(CommonHelper.GetAssemblyPath() +_dbFileName);

        delegate void ReadExcelHandler(string excelPath, int[] SheetIndex,ExcelHelper.WorkSheetHandler[] wsCall);

        public static IntPtr _handle;
        public static string _nowProcessing;
        public static int Total;

        public static IObjectContainer _db;
        public static IQuery _query;

        Preference[] _rootField;
        Preference[] asArray;

        int pageSize = 0;     //每页显示行数
        int nMax = 0;         //总记录数
        int pageCount = 0;    //页数＝总记录数/每页显示行数
        int pageCurrent = 0;   //当前页号

        public Form_Preference()
        {
            InitializeComponent();
            _workProcess = new ExcelHelper.WorkSheetHandler(ExcelProcessing);
            _handle = this.Handle;

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.AllowUserToAddRows = true;

            try
            {
                config.Common.ObjectClass(typeof(Preference)).CascadeOnUpdate(true);
                if (System.IO.File.Exists(YapFileName))
                {
                    _db = Db4oEmbedded.OpenFile(config, YapFileName);
                    bt_importexcel.Visible = false;
                    pb_excel.Visible = false;
                    lb_percent.Visible = false;
                    lb_now.Visible = false;
                }
                else
                {
                    bt_importexcel.Visible = true;
                    pb_excel.Visible = true;
                    lb_percent.Visible = true;
                    lb_now.Visible = true;

                    throw new Exception("找不到数据文件！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case messaging.UPDATE_PREFERENCE:
                    pb_excel.Value = pb_excel.Value + 1;
                    lb_percent.Text = ((pb_excel.Value * 100) / pb_excel.Maximum).ToString();
                    lb_now.Text = _nowProcessing;
                    break;
                case messaging.UPDATE_PREFERENCE_OVER:
                    lb_now.Text = "COMPLETED!";
                    break;
                case messaging.UPDATE_PREFERENCE_BEGIN:
                    pb_excel.Minimum = 0;
                    pb_excel.Maximum = Total;
                    break;
            }
            base.WndProc(ref m);
        }

        #region paging
        private void InitDataSet(int maxRow)
        {
            pageSize = 20;      //设置页面行数
            nMax = maxRow;
            pageCount = (nMax / pageSize);    //计算出总页数
            if ((nMax % pageSize) > 0) pageCount++;
            pageCurrent = 1;    //当前页数从1开始
            LoadData();
        }

        private void LoadData()
        {
            int nStartPos = 0;   //当前页面开始记录行
            int nEndPos = 0;     //当前页面结束记录行


            //DataTable dtTemp = dtInfo.Clone();   //克隆DataTable结构框架

            if (pageCurrent == pageCount)
            {
                nEndPos = nMax;
            }
            else
            {
                nEndPos = pageSize * pageCurrent;
            }

            nStartPos = pageSize * (pageCurrent - 1);

            asArray = new Preference[nEndPos - nStartPos];

            if (_rootField != null && _rootField.Length != 0)
            {
                //从元数据源复制记录行
                int j = 0;
                for (int i = nStartPos; i < nEndPos; i++)
                {
                    asArray[j] = _rootField[i];
                    j++;
                }
            }
            else
            {
                asArray = _rootField;
            }

            aSTableBindingSource.DataSource = new object();
            dataGridView1.DataSource = asArray;
            dataGridView1.Refresh();
            UpdateButton();
        }

        private void UpdateButton()
        {
            if (pageCurrent == pageCount)
                bindingNavigatorMoveNextItem.Enabled = false;
            else
                bindingNavigatorMoveNextItem.Enabled = true;

            if (pageCurrent == 1)
                bindingNavigatorMovePreviousItem.Enabled = false;
            else
                bindingNavigatorMovePreviousItem.Enabled = true;

            bindingNavigatorMoveLastItem.Enabled = true;
            bindingNavigatorMoveFirstItem.Enabled = true;

            toolStripTextBox1.Text = pageCurrent.ToString();
            bindingNavigatorCountItem.Text = pageCount.ToString();
        }
        #endregion

        public void UpdateGrid(Preference[] collection)
        {
            _rootField = collection;
            InitDataSet(_rootField.Length);
        }

        #region excel
        private void ExcelProcessCallBack(IAsyncResult asResult)
        {
            try
            {
                AsyncResult result = (AsyncResult)asResult;
                ReadExcelHandler dele = (ReadExcelHandler)result.AsyncDelegate;
                dele.EndInvoke(asResult);
                messaging.SendMessage(Form_Preference._handle, messaging.UPDATE_PREFERENCE_OVER, 0, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void ExcelProcessing(Excel.Worksheet ws)
        {
            int rowCount = ws.UsedRange.Rows.Count;
            int colCount = ws.UsedRange.Columns.Count;

            if (rowCount <= 0)
                throw new Exception("文件中没有数据记录");

            Form_Preference.Total = rowCount - 1;
            messaging.SendMessage(Form_Preference._handle, messaging.UPDATE_PREFERENCE_BEGIN, 0, 0);

            Dictionary<string, string> _dic = ExcelHelper.MapFieldNameAndColumAlfa(typeof(Preference), new string[] { "A", "B", "C", "D" });

            IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
            config.Common.ObjectClass(typeof(Preference)).CascadeOnUpdate(true);

            bool isNextPreference = false;
            Preference p = null;

            if (System.IO.File.Exists(YapFileName))
                System.IO.File.Delete(YapFileName);

            IObjectContainer db = Db4oEmbedded.OpenFile(config, YapFileName);

            try
            {
                PropertyInfo[] ps = typeof(Preference).GetProperties();
                for (int r = 3; r < rowCount; r++)
                {
                    int i = 0;

                    foreach (KeyValuePair<string, string> pair in _dic)
                    {
                        string value = ExcelHelper.GetString(ws, pair.Value, r);

                        if (pair.Key == "PreferenceName")
                        {
                            isNextPreference = string.IsNullOrEmpty(value) ? false : true;
                            _nowProcessing = value;
                        }
                        else
                        {
                            isNextPreference = false;
                        }


                        if (isNextPreference)
                        {
                            if (p != null)
                            {
                                db.Store(p);
                            }

                            p = new Preference(null, null, null, new ArrayList());

                            ps[i].SetValue(p, value, null);
                        }
                        else
                        {
                            if (i == 3 && !string.IsNullOrEmpty(value))
                                p.DescArray.Add(value);
                            else
                            {
                                if (!string.IsNullOrEmpty(value))
                                    ps[i].SetValue(p, value, null);
                            }
                        }

                        i++;
                    }
                    messaging.SendMessage(Form_Preference._handle, messaging.UPDATE_PREFERENCE, 0, 0);
                }
                db.Store(p);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                db.Close();
            }

        }
        #endregion
        private void bt_importexcel_Click(object sender, EventArgs e)
        {
            string Path = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = CommonHelper.GetAssemblyPath();
            openFileDialog1.Filter = "All files (*.*)|*.*|Excel files (*.xls)|*.xls";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                Path = openFileDialog1.FileName;

            ReadExcelHandler reader = new ReadExcelHandler(ExcelHelper.ReadExcel);
            reader.BeginInvoke(Path, new int[] { 1 }, new ExcelHelper.WorkSheetHandler[] { _workProcess }, ExcelProcessCallBack, null);
        }

        private void toolStripTextBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                IQuery _q;
                if (!toolStripTextBox2.Text.StartsWith("~"))
                    _q = CommonHelper.GetQuery(_db, typeof(Preference), "_preferenceName", toolStripTextBox2.Text);
                else
                    _q = CommonHelper.GetQuery(_db, typeof(Preference), "_Description", toolStripTextBox2.Text.Remove(0, 1));


                IObjectSet result1 = _q.Execute();

                Preference[] chArray = new Preference[result1.Count];
                for (int i = 0; i < result1.Count; i++)
                {
                    chArray[i] = (Preference)result1[i];
                }

                lb_total.Text = "共：" + chArray.Length + " 条记录";
                UpdateGrid(chArray);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                richTextBox1.Clear();

                string tableName = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                IQuery p = _db.Query();
                p.Constrain(typeof(Preference));
               IQuery q =  p.Descend("_preferenceName");
               q.Constrain(tableName).Contains();
                IObjectSet result = p.Execute();

               //IQuery _q = CommonHelper.GetQuery(_db, typeof(Preference), "_preferenceName", tableName);
               //IObjectSet result = _q.Execute();
               
                if (result.Count > 0)
                {
                    richTextBox1.Clear();

                    List<string> lines = new List<string>();

                    foreach (Preference pp in result)
                    {
                        for (int i = 0; i < pp.DescArray.Count; i++)
                            lines.Add((string)pp.DescArray[i]);
                        lines.Add("=============================================================================");
                    }
                    richTextBox1.Lines = lines.ToArray<string>();
                    richTextBox1.Refresh();
                }
                //_query = _db.Query();
            }
        }

        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
            pageCurrent = 1;
            LoadData();
        }

        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            pageCurrent = --pageCurrent;
            LoadData();
        }

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            pageCurrent = ++pageCurrent;
            LoadData();
        }

        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
            pageCurrent = pageCount;
            LoadData();
        }
    }
}
