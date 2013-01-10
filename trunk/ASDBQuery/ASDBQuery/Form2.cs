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
using System.Runtime.Remoting.Messaging;

namespace ASDBQuery
{
    public partial class Form2 : Form
    {
        ASField[] _rootField;
        ASField[] asArray;
        string CurrentEdit;
        string _tableName;

        int pageSize = 0;     //每页显示行数
        int nMax = 0;         //总记录数
        int pageCount = 0;    //页数＝总记录数/每页显示行数
        int pageCurrent = 0;   //当前页号

        private delegate void SavebakeHandler(DataGridViewCellEventArgs e);

        SavebakeHandler _saveDele;

        int update_row, update_col;
        public static IntPtr hand;

        public static string BulkCNWords;

        public Form2()
        {
            InitializeComponent();
            dg_fields.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            _saveDele = new SavebakeHandler(SaveChange);
            hand = this.Handle;
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case messaging.UPDATE_FORM2:
                    System.Reflection.PropertyInfo pr = typeof(ASField).GetProperties()[update_col];
                    if (pr.GetValue(asArray[update_row], null) != null)
                        ts_update.Text = "第: " + (update_row + 1).ToString() + "行的  " + pr.GetValue(asArray[update_row], null).ToString() + "   值已更新！";
                    else
                        ts_update.Text = "第: " + (update_row + 1).ToString() + "行的  " + "" + "   值已更新！";
                    break;
                case messaging.BULK_UPDATE_COMPLETE:
                    ts_update.Text = "更行完毕";
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

            asArray = new ASField[nEndPos - nStartPos];
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

        public void UpdateGrid(ASField[] source)
        {
            _rootField = source;
            label1.Text = "共：" + source.Length.ToString() + " 个";
            InitDataSet(_rootField.Length);
        }

        public void UpdateGrid(object collection)
        {
            IList l = (IList)collection; _rootField = new ASField[l.Count];
            for (int i = 0; i < l.Count; i++)
            {
                _rootField[i] = (ASField)l[i];
            }
            label1.Text = "共：" + _rootField.Length.ToString() + " 个";
            InitDataSet(_rootField.Length);
        }
        public void ChangeFormName(string Name)
        {
            this.Text = this.Text + "                 Table Name: " + Name;
            _tableName = Name;
            this.Refresh();
        }

        private void SaveChange(DataGridViewCellEventArgs e)
        {
            ASField field = (ASField)asArray[e.RowIndex];
            ASDB._db.Store(field);
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
                messaging.SendMessage(Form2.hand, messaging.UPDATE_FORM2, 0, 0);
            }
            catch (Exception ex)
            {
                throw ex;
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

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (toolStripTextBox1.Text != null && toolStripTextBox1.Text != string.Empty)
                pageCurrent = Convert.ToInt32(toolStripTextBox1.Text);
            else
                pageCurrent = 1;
            LoadData();
        }

        private void toolStripTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                toolStripButton1.PerformClick();
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (ASDB._query!=null)
            {
                string fieldSearch = toolStripTextBox2.Text.Trim();
                ASDB._query.Constrain(typeof(ASField));
                IQuery q = ASDB._query.Descend("_name");
                q.Constrain(fieldSearch).Like();
                IQuery p = ASDB._query.Descend("_tableName");
                q.Constraints().And(p.Constrain(_tableName).Equal());
                IObjectSet result = ASDB._query.Execute(); ASDB._query = ASDB._db.Query();

                asArray = new ASField[result.Count];
                for (int i = 0; i < result.Count; i++)
                {
                    asArray[i] = (ASField)result[i];
                }
                label1.Text = "共：" + asArray.Length.ToString() + " 个";
                UpdateGrid(asArray);
            }
        }

        private void toolStripTextBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                toolStripButton2.PerformClick();
            }
        }

        private void Form2_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
