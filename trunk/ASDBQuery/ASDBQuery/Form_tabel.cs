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
using System.Reflection;
using System.Runtime.Remoting.Messaging;

namespace ASDBQuery
{
    public partial class Form_tabel : Form
    {
        System.Collections.ArrayList _rootTable;
        System.Collections.ArrayList asArray;
        string CurrentEdit;

        int pageSize = 0;     //每页显示行数
        int nMax = 0;         //总记录数
        int pageCount = 0;    //页数＝总记录数/每页显示行数
        int pageCurrent = 0;   //当前页号

        private delegate void SavebakeHandler(DataGridViewCellEventArgs e);
        SavebakeHandler _saveDele;
        int update_row, update_col;
        public static IntPtr hand;

        public Form_tabel()
        {
            InitializeComponent();
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            _saveDele = new SavebakeHandler(SaveChange);
            hand = this.Handle;
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case messaging.TABLEUPDATE:
                    System.Reflection.PropertyInfo pr = typeof(ASTable).GetProperties()[update_col];
                    if(pr.GetValue(asArray[update_row], null)!=null)
                        ts_update.Text = "第: " + (update_row + 1).ToString() + "行的  " + pr.GetValue(asArray[update_row], null).ToString() + "   值已更新！";
                    else
                        ts_update.Text = "第: " + (update_row + 1).ToString() + "行的  " + "" + "   值已更新！";
                    break;
            }
            base.WndProc(ref m);
        }

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
            asArray = new System.Collections.ArrayList();
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
            //lblPageCount.Text = pageCount.ToString();
            //txtCurrentPage.Text = Convert.ToString(pageCurrent);


            //从元数据源复制记录行
            if (_rootTable != null && _rootTable.Count > 0)
            {
                int j = 0;
                for (int i = nStartPos; i < nEndPos; i++)
                {
                    asArray.Add(_rootTable[i]);
                    j++;
                }
            }
            else
            {
                asArray = _rootTable;
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

        public void UpdateGrid(System.Collections.ArrayList source)
        {
            _rootTable = source;
            InitDataSet(_rootTable.Count);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.ColumnIndex == 0 && ASDB._query!=null)
            //{
            //    string tableName = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            //    CommonHelper.ConstrainByType(ASDB._query, typeof(ASField));
            //    IQuery temp = CommonHelper.Descend(ASDB._query, "_tableName");
            //    CommonHelper.Contain(temp, tableName);
            //    IObjectSet result = ASDB._query.Execute();

            //    //if (result.Count > 0 && result.Count == 1)
            //    //{
            //    //    Form2 f2 = new Form2();
            //    //    f2.Show();
            //    //    f2.ChangeFormName(tableName);
            //    //    ASTable tableObject = (ASTable)result[0];
            //    //    f2.UpdateGrid(tableObject.Fields);
            //    //}
            //    ASDB._query = ASDB._db.Query();

            //    //IObjectSet result = _query.Execute(); _query = _db.Query();

            //    ASField[] asArray = new ASField[result.Count];
            //    for (int i = 0; i < result.Count; i++)
            //    {
            //        asArray[i] = (ASField)result[i];
            //    }

            //    if (asArray.Length > 0)
            //    {
            //        Form_field ff = new Form_field();

            //        ff.UpdateGrid(asArray);
            //        ff.Show();
            //    }
            //}

            if (e.ColumnIndex == 0)
            {
                string tableName = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                CommonHelper.ConstrainByType(ASDB._query, typeof(ASTable));
                IQuery temp = CommonHelper.Descend(ASDB._query, "_tableName");
                CommonHelper.Contain(temp, tableName);
                IObjectSet result = ASDB._query.Execute();

                if (result.Count > 0 && result.Count == 1)
                {
                    Form2 f2 = new Form2();
                    f2.Show();
                    f2.ChangeFormName(tableName);
                    ASTable tableObject = (ASTable)result[0];
                    f2.UpdateGrid(tableObject.Fields);
                }
                ASDB._query = ASDB._db.Query();
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            update_col = e.ColumnIndex;
            update_row = e.RowIndex;
            _saveDele.BeginInvoke(e, SaveCallBack, e);
        }

        private void SaveChange(DataGridViewCellEventArgs e)
        {
            ASTable table = (ASTable)asArray[e.RowIndex];
            ASDB._db.Store(table);
            ASDB._db.Commit();
        }

        private void SaveCallBack(IAsyncResult asResult)
        {
            try
            {
                AsyncResult result = (AsyncResult)asResult;
                DataGridViewCellEventArgs e = (DataGridViewCellEventArgs)result.AsyncState;
                SavebakeHandler dele = (SavebakeHandler)result.AsyncDelegate;
                dele.EndInvoke(asResult);
                messaging.SendMessage(Form_tabel.hand, messaging.TABLEUPDATE, 0, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                CurrentEdit = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            else
                CurrentEdit = null;
        }

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

        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            pageCurrent = --pageCurrent;
            LoadData();
        }

        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
            pageCurrent = pageCount;
            LoadData();
        }

        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
            pageCurrent = 1;
            LoadData();
        }
    }
}

