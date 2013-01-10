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
using System.Runtime.Remoting.Messaging;

namespace ASDBQuery
{
    public partial class Form_field : Form
    {
        ASField[] _rootField;
        ASField[] asArray;
        string CurrentEdit;

        int pageSize = 0;     //每页显示行数
        int nMax = 0;         //总记录数
        int pageCount = 0;    //页数＝总记录数/每页显示行数
        int pageCurrent = 0;   //当前页号


        private delegate void SavebakeHandler(DataGridViewCellEventArgs e);
        private delegate void CommitHandler();

        SavebakeHandler _saveDele;
        CommitHandler _commit;

        int update_row, update_col;
        public static IntPtr hand;

        public static string BulkCNWords;

        public Form_field()
        {
            InitializeComponent();
            this.dg_fields.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            hand = this.Handle;
            _saveDele = new SavebakeHandler(SaveChange);
            _commit = new CommitHandler(CommitFun);
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case messaging.FIELDUPDATE:
                    System.Reflection.PropertyInfo pr = typeof(ASField).GetProperties()[update_col];
                    if (pr.GetValue(asArray[update_row], null) != null)
                        ts_update.Text = "第: " + (update_row + 1).ToString() + "行的  " + pr.GetValue(asArray[update_row], null).ToString() + "   值已更新！";
                    else
                        ts_update.Text = "第: " + (update_row + 1).ToString() + "行的  " + "" + "   值已更新！";
                    break;
                case messaging.BULK_UPDATE_COMPLETE:
                    ts_update.Text = "更行完毕";
                    UpdateGrid(_rootField);
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
            asArray = new ASField[pageSize];
            //DataTable dtTemp = dtInfo.Clone();   //克隆DataTable结构框架

            if (pageCurrent == pageCount)
            {
                nEndPos = nMax;
            }
            else
            {
                nEndPos = pageSize * pageCurrent;
            }

            nStartPos = pageSize*(pageCurrent-1);
            //lblPageCount.Text = pageCount.ToString();
            //txtCurrentPage.Text = Convert.ToString(pageCurrent);


            //从元数据源复制记录行
            if (_rootField != null && _rootField.Length > 0)
            {
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

            aSFieldBindingSource.DataSource = new object();
            
            dg_fields.DataSource = asArray;
            dg_fields.Refresh();
            UpdateButton();
        }

        public void UpdateGrid(ASField[] source)
        {
            _rootField = source;
            InitDataSet(_rootField.Length);
            //this.dg_fields.DataSource = source;
            //this.dg_fields.Refresh();
        }

        private void dg_fields_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                string tableName = dg_fields.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
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

        private void dg_fields_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (dg_fields.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                CurrentEdit = dg_fields.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            else
                CurrentEdit = null;
        }

        private void dg_fields_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            update_col = e.ColumnIndex;
            update_row = e.RowIndex;
            _saveDele.BeginInvoke(e, SaveCallBack, e);
        }

        private void SaveChange(DataGridViewCellEventArgs e)
        {
            ASField field = (ASField)asArray[e.RowIndex];
            ASDB._db.Store(field);
            ASDB._db.Commit();
        }

        private void CommitFun()
        {
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
                messaging.SendMessage(Form_field.hand, messaging.FIELDUPDATE, 0, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void CommitCallBack(IAsyncResult asResult)
        {
            try
            {
                AsyncResult result = (AsyncResult)asResult;
                CommitHandler dele = (CommitHandler)result.AsyncDelegate;
                dele.EndInvoke(asResult);
                messaging.SendMessage(Form_field.hand, messaging.BULK_UPDATE_COMPLETE, 0, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

        private void UpdateButton()
        {
            if(pageCurrent == pageCount)
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

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (toolStripTextBox1.Text != null && toolStripTextBox1.Text != string.Empty)
                pageCurrent = Convert.ToInt32(toolStripTextBox1.Text);
            else
                pageCurrent = 1;
            LoadData();
        }

        private void dg_fields_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.R)
            {
                BulkReplace b = new BulkReplace();
                if (b.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    foreach (ASField f in _rootField)
                    {
                        f.CNDescription = BulkCNWords;
                        ASDB._db.Store(f);
                    }
                }
                _commit.BeginInvoke(CommitCallBack, null);
            }
        }
    }
}
