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
    public partial class HelpForm : Form
    {
        ExcelHelper.WorkSheetHandler _workProcess;
        IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();

        static string _dbFileName = "HelpInfo.yap";
        readonly static string YapFileName = System.IO.Path.Combine(CommonHelper.GetAssemblyPath() + _dbFileName);

        delegate void ReadExcelHandler(string excelPath, int[] SheetIndex, ExcelHelper.WorkSheetHandler[] wsCall);

        public static IntPtr _handle;
        public static string _nowProcessing;
        public static int Total;

        public static IObjectContainer _db;
        public static IQuery _query;

        HelpInfor[] _rootField;
        HelpInfor[] asArray;
        HelpInfor CurrentEdit;

        int pageSize = 0;     //每页显示行数
        int nMax = 0;         //总记录数
        int pageCount = 0;    //页数＝总记录数/每页显示行数
        int pageCurrent = 0;   //当前页号


        List<string> _combList = new List<string>();

        delegate void SaveHelpHandler(int rowIndex);
        static int _rowIndex = 0;
        SaveHelpHandler _saveHandler;

        public HelpForm()
        {
            InitializeComponent();

            _workProcess = new ExcelHelper.WorkSheetHandler(ExcelProcessing);
            _handle = this.Handle;

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.AllowUserToAddRows = true;

#region db init
            try
            {
                config.Common.ObjectClass(typeof(HelpInfor)).CascadeOnUpdate(true);
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
            _saveHandler = new SaveHelpHandler(SaveBack);
            config.Common.ObjectClass(typeof(HelpInfor)).CascadeOnUpdate(true);
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
                    this.Text = "------------->完成" + " "+CurrentEdit.ELEM_NBR+" "+"更新!!!";
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

            asArray = new HelpInfor[nEndPos - nStartPos];

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
            if (asArray.Length > 0)
                richTextBox1.Text = asArray[0].ELEM_DEFINITION;
            else
                richTextBox1.Text = "";
            UpdateButton();
            toolStripTextBox2.Focus();
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
            HelpInfor p = asArray[rIndex];
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
                SaveHelpHandler dele = (SaveHelpHandler)result.AsyncDelegate;
                dele.EndInvoke(asResult);
                messaging.SendMessage(HelpForm._handle, messaging.PREFERENCE_SAVED, 0, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
        public void UpdateGrid(HelpInfor[] collection)
        {
            _rootField = collection;
            InitDataSet(_rootField.Length);
        }

        void RefreshCatalogue()
        {
            IQuery q = _db.Query();
            q.Constrain(typeof(HelpInfor));
            IQuery q2 = q.Descend("_catalogue");
            IObjectSet result = q.Execute();
            foreach (HelpInfor c in result)
            {
                string s = c.ELEM_NBR;
                if (!_combList.Contains(s))
                    _combList.Add(s);
            }
        }

        #region excel
        private void ExcelProcessCallBack(IAsyncResult asResult)
        {
            try
            {
                AsyncResult result = (AsyncResult)asResult;
                ReadExcelHandler dele = (ReadExcelHandler)result.AsyncDelegate;
                dele.EndInvoke(asResult);
                messaging.SendMessage(HelpForm._handle, messaging.UPDATE_PREFERENCE_OVER_ALL, 0, 0);
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

            Total = rowCount - 1;
            messaging.SendMessage(HelpForm._handle, messaging.UPDATE_PREFERENCE_BEGIN_ALL, 0, 0);

            Dictionary<string, string> _dic = ExcelHelper.MapFieldNameAndColumAlfa(typeof(HelpInfor), new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" });
            IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
            config.Common.ObjectClass(typeof(HelpInfor)).CascadeOnUpdate(true);

            HelpInfor p = null;

            if (System.IO.File.Exists(YapFileName))
                System.IO.File.Delete(YapFileName);

            IObjectContainer db = Db4oEmbedded.OpenFile(config, YapFileName);

            try
            {
                PropertyInfo[] ps = typeof(HelpInfor).GetProperties();
                for (int r = 2; r < rowCount+1; r++)
                {
                    int i = 0;
                    p = new HelpInfor();
                    foreach (KeyValuePair<string, string> pair in _dic)
                    {
                        string value = ExcelHelper.GetString2(ws, pair.Value, r);
                        ps[i].SetValue(p, value.Trim(), null);
                        i++;
                    }
                    db.Store(p);
                    messaging.SendMessage(HelpForm._handle, messaging.UPDATE_PREFERENCE_ALL, 0, 0);
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

                if (toolStripTextBox2.Text.StartsWith("~"))
                    _q = CommonHelper.GetQuery(_db, typeof(HelpInfor), "_ELEM_NAME", toolStripTextBox2.Text.Remove(0, 1));
                else if (toolStripTextBox2.Text.StartsWith("$"))
                {
                    _q = CommonHelper.GetQuery(_db, typeof(HelpInfor), "_ELEM_DEFINITION", toolStripTextBox2.Text.Remove(0, 1));
                }
                else
                {
                    _q = CommonHelper.GetQuery(_db, typeof(HelpInfor), "_ELEM_NBR", toolStripTextBox2.Text);
                }

                IObjectSet result1 = _q.Execute();

                HelpInfor[] chArray = new HelpInfor[result1.Count];
                for (int i = 0; i < result1.Count; i++)
                {
                    chArray[i] = (HelpInfor)result1[i];
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

        //private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    IQuery _q;
        //    if (!toolStripTextBox2.Text.StartsWith("~"))
        //        _q = CommonHelper.GetQuery(_db, typeof(HelpInfor), "_preferenceName", toolStripTextBox2.Text);
        //    else
        //        _q = CommonHelper.GetQuery(_db, typeof(HelpInfor), "_descList", toolStripTextBox2.Text.Remove(0, 1));

        //    if (toolStripComboBox1.Text != null && toolStripComboBox1.Text != string.Empty)
        //    {
        //        IQuery q2 = _q.Descend("_catalogue");
        //        _q.Constraints().And(q2.Constrain(toolStripComboBox1.Text).Equal());
        //    }

        //    IObjectSet result1 = _q.Execute();

        //    HelpInfor[] chArray = new HelpInfor[result1.Count];
        //    for (int i = 0; i < result1.Count; i++)
        //    {
        //        chArray[i] = (HelpInfor)result1[i];
        //    }
        //    lb_total.Text = "共：" + chArray.Length + ".";
        //    UpdateGrid(chArray);
        //}

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                CurrentEdit = asArray[e.RowIndex];
                _rowIndex = e.RowIndex;

                richTextBox1.Clear();

                string Help_Tig = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

                IQuery p = _db.Query();
                p.Constrain(typeof(HelpInfor));

                IQuery q = p.Descend("_ELEM_NBR");
                q.Constrain(Help_Tig).Contains();

                IObjectSet result = p.Execute();

                if (result.Count > 0)
                {
                    richTextBox1.Clear();

                    richTextBox1.Text = ((HelpInfor)result[0]).ELEM_DEFINITION;
                    richTextBox1.Refresh();
                }
            }
        }

        private void richTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.S)
            {
                asArray[_rowIndex].ELEM_DEFINITION = richTextBox1.Text;
                _saveHandler.BeginInvoke(_rowIndex,SaveCallBack, null);
            }
        }
    }
}
