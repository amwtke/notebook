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
    public partial class From_Preference_ALL : Form
    {
        ExcelHelper.WorkSheetHandler _workProcess;
        IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();

        static string _dbFileName = "Preference_ALL.yap";
        readonly static string YapFileName = System.IO.Path.Combine(CommonHelper.GetAssemblyPath() + _dbFileName);

        delegate void ReadExcelHandler(string excelPath, int[] SheetIndex, ExcelHelper.WorkSheetHandler[] wsCall);

        public static IntPtr _handle;
        public static string _nowProcessing;
        public static int Total;

        public static IObjectContainer _db;
        public static IQuery _query;

        PreferenceAll[] _rootField;
        PreferenceAll[] asArray;
        PreferenceAll CurrentEdit;

        int pageSize = 0;     //每页显示行数
        int nMax = 0;         //总记录数
        int pageCount = 0;    //页数＝总记录数/每页显示行数
        int pageCurrent = 0;   //当前页号


        List<string> _combList = new List<string>();

        delegate void SavePreferenceHandler(int rowIndex);
        static int _rowIndex = 0;
        SavePreferenceHandler _saveHandler;

        public From_Preference_ALL()
        {
            InitializeComponent();

            _workProcess = new ExcelHelper.WorkSheetHandler(ExcelProcessing);
            _handle = this.Handle;

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.AllowUserToAddRows = true;


#region db init
            try
            {
                config.Common.ObjectClass(typeof(Preference)).CascadeOnUpdate(true);
                if (System.IO.File.Exists(YapFileName))
                {
                    _db = Db4oEmbedded.OpenFile(config, YapFileName);
                    BT_Import.Visible = false;
                    pb_excel.Visible = false;
                    lb_percent.Visible = false;
                    lb_now.Visible = false;
                }
                else
                {
                    BT_Import.Visible = true;
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
#endregion
            if(_db!=null)
                RefreshCatalogue();

            toolStripComboBox1.DropDownStyle = ComboBoxStyle.DropDown;
            toolStripComboBox1.AutoCompleteMode = AutoCompleteMode.Append;
            toolStripComboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;

            _saveHandler = new SavePreferenceHandler(SaveBack);
            config.Common.ObjectClass(typeof(PreferenceAll)).CascadeOnUpdate(true);
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case messaging.UPDATE_PREFERENCE_ALL:
                    pb_excel.Value = pb_excel.Value + 1;
                    lb_percent.Text = ((pb_excel.Value * 100) / pb_excel.Maximum).ToString();
                    lb_now.Text = _nowProcessing;
                    break;
                case messaging.UPDATE_PREFERENCE_OVER_ALL:
                    lb_now.Text = "COMPLETED!";
                    break;
                case messaging.UPDATE_PREFERENCE_BEGIN_ALL:
                    pb_excel.Minimum = 0;
                    pb_excel.Maximum = Total;
                    break;
                case messaging.PREFERENCE_SAVED:
                    this.Text = "------------->完成" + " "+CurrentEdit.PreferenceName+" "+"更新!!!";
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

            asArray = new PreferenceAll[nEndPos - nStartPos];

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

            preferenceAllBindingSource1.DataSource = new object();
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

        #region save callback
        void SaveBack(int rIndex)
        {
            PreferenceAll p = asArray[rIndex];
            if (p != null)
            {
                _db.Store(p);
                _db.Commit();
            }
        }
        void SaveCallBack(IAsyncResult asResult)
        {
            try
            {
                AsyncResult result = (AsyncResult)asResult;
                SavePreferenceHandler dele = (SavePreferenceHandler)result.AsyncDelegate;
                dele.EndInvoke(asResult);
                messaging.SendMessage(From_Preference_ALL._handle, messaging.PREFERENCE_SAVED, 0, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
        public void UpdateGrid(PreferenceAll[] collection)
        {
            _rootField = collection;
            InitDataSet(_rootField.Length);
        }

        void RefreshCatalogue()
        {
            IQuery q = _db.Query();
            q.Constrain(typeof(PreferenceAll));
            IQuery q2 = q.Descend("_catalogue");
            IObjectSet result = q.Execute();
            foreach (PreferenceAll c in result)
            {
                string s = c.Catalogue;
                if (!_combList.Contains(s))
                    _combList.Add(s);
            }
            toolStripComboBox1.Items.Clear();
            toolStripComboBox1.Items.AddRange(_combList.ToArray());
        }

        #region excel
        private void ExcelProcessCallBack(IAsyncResult asResult)
        {
            try
            {
                AsyncResult result = (AsyncResult)asResult;
                ReadExcelHandler dele = (ReadExcelHandler)result.AsyncDelegate;
                dele.EndInvoke(asResult);
                messaging.SendMessage(From_Preference_ALL._handle, messaging.UPDATE_PREFERENCE_OVER_ALL, 0, 0);
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

            From_Preference_ALL.Total = rowCount - 1;
            messaging.SendMessage(From_Preference_ALL._handle, messaging.UPDATE_PREFERENCE_BEGIN_ALL, 0, 0);

            Dictionary<string, string> _dic = ExcelHelper.MapFieldNameAndColumAlfa(typeof(PreferenceAll), new string[] { "A", "B", "C", "D", "E", "F" });

            IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
            config.Common.ObjectClass(typeof(PreferenceAll)).CascadeOnUpdate(true);

            bool isNextPreference = false;
            string P_value = null;

            PreferenceAll p = null;

            if (System.IO.File.Exists(YapFileName))
                System.IO.File.Delete(YapFileName);

            IObjectContainer db = Db4oEmbedded.OpenFile(config, YapFileName);

            try
            {
                PropertyInfo[] ps = typeof(PreferenceAll).GetProperties();
                for (int r = 2; r < rowCount+1; r++)
                {
                    int i = 0;

                    foreach (KeyValuePair<string, string> pair in _dic)
                    {
                        string value = ExcelHelper.GetString(ws, pair.Value, r);

                        #region 两个值一同判断
                        if (pair.Key == "PreferenceName")
                        {
                            if (P_value == null)
                            {
                                isNextPreference = true;
                            }
                            else
                            {
                                isNextPreference = P_value.Equals(value) ? false : true;
                                if (!isNextPreference)
                                {
                                    isNextPreference = !p.Catalogue.Equals(ExcelHelper.GetString(ws, "E", r));
                                }
                            }
                            P_value = value;
                            _nowProcessing = value;
                        }
                        else
                        {
                            isNextPreference = false;
                        }
                        #endregion

                        if (isNextPreference)
                        {
                            if (p != null)
                            {
                                if (p.Catalogue == null) 
                                {
                                    Console.WriteLine();
                                }
                                else
                                    db.Store(p);
                            }

                            p = new PreferenceAll(null, null, null, null, null, new List<string>());

                            ps[i].SetValue(p, value, null);
                        }
                        else
                        {
                            if (i == 5 && !string.IsNullOrEmpty(value))
                                p.DescriptionList.Add(value);
                            else
                            {
                                if (!string.IsNullOrEmpty(value))
                                    ps[i].SetValue(p, value, null);
                            }
                        }

                        i++;
                    }
                    messaging.SendMessage(From_Preference_ALL._handle, messaging.UPDATE_PREFERENCE_ALL, 0, 0);
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

        private void BT_Import_Click(object sender, EventArgs e)
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
                    _q = CommonHelper.GetQuery(_db, typeof(PreferenceAll), "_preferenceName", toolStripTextBox2.Text);
                else
                    _q = CommonHelper.GetQuery(_db, typeof(PreferenceAll), "_descList", toolStripTextBox2.Text.Remove(0, 1));

                if (toolStripComboBox1.Text != null && toolStripComboBox1.Text != string.Empty)
                {
                    IQuery q2 = _q.Descend("_catalogue");
                    _q.Constraints().And(q2.Constrain(toolStripComboBox1.Text).Equal());
                }

                IObjectSet result1 = _q.Execute();

                PreferenceAll[] chArray = new PreferenceAll[result1.Count];
                for (int i = 0; i < result1.Count; i++)
                {
                    chArray[i] = (PreferenceAll)result1[i];
                }

                lb_total.Text = "共：" + chArray.Length+".";
                UpdateGrid(chArray);
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

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            IQuery _q;
            if (!toolStripTextBox2.Text.StartsWith("~"))
                _q = CommonHelper.GetQuery(_db, typeof(PreferenceAll), "_preferenceName", toolStripTextBox2.Text);
            else
                _q = CommonHelper.GetQuery(_db, typeof(PreferenceAll), "_descList", toolStripTextBox2.Text.Remove(0, 1));

            if (toolStripComboBox1.Text != null && toolStripComboBox1.Text != string.Empty)
            {
                IQuery q2 = _q.Descend("_catalogue");
                _q.Constraints().And(q2.Constrain(toolStripComboBox1.Text).Equal());
            }

            IObjectSet result1 = _q.Execute();

            PreferenceAll[] chArray = new PreferenceAll[result1.Count];
            for (int i = 0; i < result1.Count; i++)
            {
                chArray[i] = (PreferenceAll)result1[i];
            }
            lb_total.Text = "共：" + chArray.Length + ".";
            UpdateGrid(chArray);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                CurrentEdit = asArray[e.RowIndex];
                _rowIndex = e.RowIndex;

                richTextBox1.Clear();

                string PreferenceName = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                string catalogueName = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 4].Value.ToString();
                IQuery p = _db.Query();
                p.Constrain(typeof(PreferenceAll));

                IQuery q = p.Descend("_preferenceName");
                q.Constrain(PreferenceName).Contains();


                IQuery q2 = p.Descend("_catalogue");
                p.Constraints().And(q2.Constrain(catalogueName).Equal());

                IObjectSet result = p.Execute();

                if (result.Count > 0)
                {
                    richTextBox1.Clear();

                    List<string> lines = new List<string>();

                    this.Text = "======>共：" + result.Count + "个记录。";

                    foreach (PreferenceAll pp in result)
                    {
                        for (int i = 0; i < pp.DescriptionList.Count; i++)
                            lines.Add((string)pp.DescriptionList[i]);
                        //lines.Add("=============================================================================");
                    }
                    richTextBox1.Lines = lines.ToArray<string>();
                    richTextBox1.Refresh();
                }
            }
        }

        private void richTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.S)
            {
                asArray[_rowIndex].DescriptionList.Clear();

                for (int i = 0; i < richTextBox1.Lines.Length; i++)
                {
                    asArray[_rowIndex].DescriptionList.Add(richTextBox1.Lines[i]);
                }

                _saveHandler.BeginInvoke(_rowIndex,SaveCallBack, null);
            }
        }

    }
}
