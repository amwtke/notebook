using System;
using System.Collections;
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
using System.Reflection;
using System.Runtime.Remoting.Messaging;

namespace ASDBQuery
{
    public partial class Form_Notebook : Form
    {
        IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
        static string _dbFileName = "NoteBook.yap";
        readonly static string YapFileName = System.IO.Path.Combine(CommonHelper.GetAssemblyPath() + _dbFileName);
        public static IObjectContainer _db;
        static string _searchString = "";
        NoteBook[] _rootField;
        NoteBook[] asArray;
        string CurrentEdit;
        

        int pageSize = 0;     //每页显示行数
        int nMax = 0;         //总记录数
        int pageCount = 0;    //页数＝总记录数/每页显示行数
        int pageCurrent = 0;   //当前页号

        private delegate void SavebakeHandler(DataGridViewCellEventArgs e);
        SavebakeHandler _saveDele;
        int update_row, update_col;

        delegate void SavePreferenceHandler(int rowIndex);
        static int _rowIndex = 0;
        SavePreferenceHandler _saveHandler;

        public static IntPtr hand;
        static ArrayList _comboList = null;
        public static string SelectedCatalogue = string.Empty;
        public Form_Notebook()
        {
            InitializeComponent();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.AllowUserToAddRows = true;
            _saveDele = new SavebakeHandler(SaveChange);
            hand = this.Handle;
            toolStripComboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

            #region db init
            try
            {
                config.Common.ObjectClass(typeof(NoteBook)).CascadeOnUpdate(true);

                _db = Db4oEmbedded.OpenFile(config, YapFileName);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            #endregion

            _saveHandler = new SavePreferenceHandler(SaveBack);

            IQuery _q = _db.Query();
            _q.Constrain(typeof(NoteBook));
            IQuery temp = _q.Descend("_item");
            IObjectSet result = _q.Execute();
            NoteBook[] chArray = new NoteBook[result.Count];
            for (int i = 0; i < result.Count; i++)
            {
                chArray[i] = (NoteBook)result[i];
            }
            UpdateGrid(chArray);
            toolStripButton3.Text = string.Empty;
        }

        #region save callback
        void SaveBack(int rIndex)
        {
            NoteBook p = asArray[rIndex];
            if (p != null)
            {
                _db.Store(p);
                _db.Commit();
            }
        }
        void SaveCallBack_forContent(IAsyncResult asResult)
        {
            try
            {
                AsyncResult result = (AsyncResult)asResult;
                SavePreferenceHandler dele = (SavePreferenceHandler)result.AsyncDelegate;
                dele.EndInvoke(asResult);
                messaging.SendMessage(Form_Notebook.hand, messaging.UPDATE_NOTEBOOK_TEXT, 0, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case messaging.UPDATE_NOTEBOOK:
                    this.Text = "第:" + update_row + " 行更行！";
                    break;

                case messaging.REFRESH_NOTE_CATEGORY:
                    RefreshCatalogue();
                    break;
                case messaging.UPDATE_NOTEBOOK_TEXT:
                    this.Text = "已更新内容:" + asArray[_rowIndex].Item + "!"+" -----"+DateTime.Now.ToString();
                    break;
            }
            base.WndProc(ref m);
        }

        void RefreshCatalogue()
        {
            IQuery temp = _db.Query();
            temp.Constrain(typeof(NoteBook));
            temp.Descend("_category");

            IObjectSet result = temp.Execute();
            if (result.Count > 0)
            {
                _comboList = new ArrayList();
                foreach (object o in result)
                {
                    NoteBook nb = (NoteBook)o;
                    if (!string.IsNullOrEmpty(nb.Category) && !_comboList.Contains(nb.Category))
                    {
                        _comboList.Add(nb.Category);
                    }
                }
                toolStripComboBox1.Items.Clear();
                toolStripComboBox1.Items.AddRange(ComboList.ToArray());
            }
        }

        public static ArrayList ComboList
        {
            get
            {
                return _comboList;
            }
        }

        #region Paging
        private void InitDataSet(int maxRow)
        {
            pageSize = 20;      //设置页面行数
            nMax = maxRow;
            pageCount = (nMax / pageSize);    //计算出总页数
            if ((nMax % pageSize) > 0) pageCount++;
            pageCurrent = 1;    //当前页数从1开始
            LoadData();
        }

        private void InitDataSet2(int maxRow)
        {
            pageSize = 20;      //设置页面行数
            nMax = maxRow;
            pageCount = (nMax / pageSize);    //计算出总页数
            if ((nMax % pageSize) > 0) pageCount++;

            pageCurrent = pageCount;

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

            asArray = new NoteBook[nEndPos - nStartPos];

            if (_rootField != null && _rootField.Length!=0)
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

            noteBookBindingSource2.DataSource = new object();
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
            bindingNavigatorAddNewItem.Enabled = true;
            bindingNavigatorDeleteItem.Enabled = true;

            toolStripTextBox1.Text = pageCurrent.ToString();
            bindingNavigatorCountItem.Text = pageCount.ToString();
        }

        public void UpdateGrid(NoteBook[] collection)
        {
            _rootField = collection;

            InitDataSet(_rootField.Length);
        }
        #endregion

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                CurrentEdit = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            else
                CurrentEdit = null;
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (_rootField != null)
            {
                update_col = e.ColumnIndex;
                update_row = e.RowIndex;

                if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value!=null 
                    && CurrentEdit != dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString())
                {
                    _saveDele.BeginInvoke(e, SaveCallBack, e);
                }
            }
        }

        private void SaveChange(DataGridViewCellEventArgs e)
        {
            NoteBook nb = asArray[e.RowIndex];
            nb.UpdateTime = DateTime.Now;
            _db.Store(nb);
            _db.Commit();
        }

        private void SaveCallBack(IAsyncResult asResult)
        {
            try
            {
                AsyncResult result = (AsyncResult)asResult;
                DataGridViewCellEventArgs e = (DataGridViewCellEventArgs)result.AsyncState;
                SavebakeHandler dele = (SavebakeHandler)result.AsyncDelegate;
                dele.EndInvoke(asResult);
                messaging.SendMessage(Form_Notebook.hand, messaging.UPDATE_NOTEBOOK, 0, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            PickCatalogue_NoteBook pickForm = new PickCatalogue_NoteBook();
            if (pickForm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                List<string> temp =new List<string>();temp.Add("ADD TEXT");
                _db.Store(new NoteBook("...", SelectedCatalogue, "...", DateTime.Now, temp));

                
                IQuery _main = _db.Query();
                _main.Constrain(typeof(NoteBook));
                IQuery p = _main.Descend("_updateTime");
                IQuery q = _main.Descend("_category");
                p.Constraints().And(q.Constrain(SelectedCatalogue).Equal());

                IObjectSet result = _main.Execute(); 

                _rootField = new NoteBook[result.Count];
                for (int i = 0; i < result.Count; i++)
                {
                    _rootField[i] = (NoteBook)result[i];
                }
                ts_total.Text = "共：" + _rootField.Length + " 条记录";

                //只显示最后一页
                InitDataSet2(_rootField.Length);
            }
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            PropertyInfo[] ps = typeof(NoteBook).GetProperties();
            if (dataGridView1.SelectedRows.Count > 0)
            {
                for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                {
                    NoteBook temp = new NoteBook();

                    for(int j=0;j<ps.Length-1;j++)
                    {
                        ps[j].SetValue(temp, dataGridView1.SelectedRows[i].Cells[j].Value, null);
                    }
                    
                    IObjectSet result = _db.QueryByExample(temp);
                    temp = (NoteBook)result[0];
                    _db.Delete(temp); 
                }
            }

            //重新载入列表
            IQuery _query = _db.Query();
            CommonHelper.ConstrainByType(_query, typeof(NoteBook));
            IQuery q = CommonHelper.Descend(_query, "_item");
            //q.Constrain(ASDB.CH_filter).Like();
            IObjectSet result1 = _query.Execute();

            NoteBook[] chArray = new NoteBook[result1.Count];
            for (int i = 0; i < result1.Count; i++)
            {
                chArray[i] = (NoteBook)result1[i];
            }
            ts_total.Text = "共：" + chArray.Length + " 条记录";
            UpdateGrid(chArray);
        }

        #region Move Next serial
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (toolStripTextBox1.Text != null && toolStripTextBox1.Text != string.Empty)
                pageCurrent = Convert.ToInt32(toolStripTextBox1.Text);
            else
                pageCurrent = 1;
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

        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            pageCurrent = --pageCurrent;
            LoadData();
        }

        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
            pageCurrent = 1;
            LoadData();
        }
        #endregion

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            IQuery _q;
            if (!toolStripTextBox2.Text.StartsWith("~"))
            {
                _q = CommonHelper.GetQuery(_db, typeof(NoteBook), "_item", toolStripTextBox2.Text);
            }
            else
            {
                _q = CommonHelper.GetQuery(_db, typeof(NoteBook), "_description", toolStripTextBox2.Text.Remove(0, 1));
                if (toolStripButton3.Text == string.Empty)
                {
                    toolStripTextBox3.Text = toolStripTextBox2.Text.Remove(0, 1);
                    _searchString = toolStripTextBox3.Text;
                }
            }

            if (toolStripComboBox1.Text != null && toolStripComboBox1.Text != string.Empty)
            {
                IQuery q2 = _q.Descend("_category");
                _q.Constraints().And(q2.Constrain(toolStripComboBox1.Text).Equal());
            }

            IObjectSet result1 = _q.Execute();

            NoteBook[] chArray = new NoteBook[result1.Count];
            for (int i = 0; i < result1.Count; i++)
            {
                chArray[i] = (NoteBook)result1[i];
            }

            ts_total.Text = "共：" + chArray.Length + " 条记录";
            UpdateGrid(chArray);

            //update rich,显示第一个。
            List<string> lines = new List<string>();
            if (chArray != null && chArray.Length > 0)
            {
                richTextBox1.Clear();

                lines.AddRange(chArray[0].Content);
                richTextBox1.Lines = lines.ToArray();
                richTextBox1.Refresh();
                MarkSearch(richTextBox1, _searchString);
                _rowIndex = 0;
            }
        }

        private void MarkSearch(RichTextBox _textBox, string searchString)
        {
            int k = 1; int m = 0;int index = 0;
            //选中所有的RichTextBox的内容
            this.richTextBox1.SelectAll();

            //改变RichTextBox的选中的字体颜色
            this.richTextBox1.SelectionColor = Color.Black;
            //改变RichTextBox的选中的字体的背景颜色
            this.richTextBox1.SelectionBackColor = Color.White;
            this.richTextBox1.Select(0, 0);

            if (!string.IsNullOrEmpty(searchString))
            {
                m = System.Text.RegularExpressions.Regex.Matches(richTextBox1.Text, searchString, System.Text.RegularExpressions.RegexOptions.IgnoreCase).Count;
                toolStripLabel1.Text = "共："+m.ToString()+"个！";
                if (richTextBox1.Text != string.Empty)
                {
                    if (k <= m)
                    {
                        while ((index = richTextBox1.Find(searchString, index, RichTextBoxFinds.None)) >= 0)
                        {
                            richTextBox1.SelectionColor = Color.Red;
                            richTextBox1.SelectionBackColor = Color.Yellow;
                            richTextBox1.Focus();
                            richTextBox1.Select(index, searchString.Length);
                            //richTextBox1.ScrollToCaret();
                            index++;
                            k++;
                            if (k == m)
                            {
                                richTextBox1.Select(0, 0);
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void toolStripTextBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                toolStripButton2.PerformClick();
            }
        }

        private void toolStripComboBox1_DropDown(object sender, EventArgs e)
        {
            RefreshCatalogue();
        }

        private void toolStripTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                toolStripButton1.PerformClick();
            }
        }


        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            IQuery _q;
            if (!toolStripTextBox2.Text.StartsWith("~"))
                _q = CommonHelper.GetQuery(_db, typeof(NoteBook), "_item", toolStripTextBox2.Text);
            else
                _q = CommonHelper.GetQuery(_db, typeof(NoteBook), "_description", toolStripTextBox2.Text.Remove(0, 1));

            if (toolStripComboBox1.Text != null && toolStripComboBox1.Text != string.Empty)
            {
                IQuery q2 = _q.Descend("_category");
                _q.Constraints().And(q2.Constrain(toolStripComboBox1.Text).Equal());
            }

            IObjectSet result1 = _q.Execute();

            NoteBook[] chArray = new NoteBook[result1.Count];
            for (int i = 0; i < result1.Count; i++)
            {
                chArray[i] = (NoteBook)result1[i];
            }
            ts_total.Text = "共："+chArray.Length+" 条记录";
            UpdateGrid(chArray);

            //update rich,显示第一个。
            if (chArray != null && chArray.Length > 0)
            {
                richTextBox1.Clear();
                richTextBox1.Lines = chArray[0].Content.ToArray<string>();
                richTextBox1.Refresh();
                _rowIndex = 0;
            }
            MarkSearch(richTextBox1, _searchString);
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control &&
                e.KeyCode == Keys.D)
            {
                bindingNavigatorAddNewItem.PerformClick();
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                _rowIndex = e.RowIndex;
                richTextBox1.Clear();

                string itemName = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                string catalogueName = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value.ToString();
                DateTime time = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 3].Value);

                IQuery p = _db.Query();
                p.Constrain(typeof(NoteBook));

                IQuery q = p.Descend("_item");
                q.Constrain(itemName).Contains();


                IQuery q2 = p.Descend("_category");
                p.Constraints().And(q2.Constrain(catalogueName).Equal());

                IQuery q3 = p.Descend("_updateTime");
                p.Constraints().And(q3.Constrain(time).Equal());

                IObjectSet result = p.Execute();

                if (result.Count > 0)
                {
                    richTextBox1.Clear();

                    List<string> lines = new List<string>();

                    this.Text = "======>共：" + result.Count + "个记录。";

                    foreach (NoteBook nb in result)
                    {
                        lines.AddRange(nb.Content);
                        
                    }
                    StringBuilder sb = new StringBuilder();
                    foreach (string s in lines)
                    {
                        sb.Append(s);
                    }
                    richTextBox1.Lines = lines.ToArray();
                    MarkSearch(richTextBox1, _searchString);
                    richTextBox1.Refresh();
                }
            }
        }

        private void richTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                bt_update.PerformClick();
            }
        }

        private void bt_update_Click(object sender, EventArgs e)
        {
            try
            {
                asArray[_rowIndex].Content.Clear();

                //for (int i = 0; i < richTextBox1.Lines.Length; i++)
                //{
                //    asArray[_rowIndex].Content.Add(richTextBox1.Lines[i]);
                //}
                asArray[_rowIndex].Content.Add(richTextBox1.Text);
                _saveHandler.BeginInvoke(_rowIndex, SaveCallBack_forContent, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            _searchString = toolStripTextBox3.Text;
            MarkSearch(richTextBox1, _searchString);
            toolStripTextBox3.Focus();
        }

        private void toolStripTextBox3_KeyUp(object sender, KeyEventArgs e)
        {
            _searchString = toolStripTextBox3.Text;
            MarkSearch(richTextBox1, _searchString);
            toolStripTextBox3.Focus();
        }

    }
}
