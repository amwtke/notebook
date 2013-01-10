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
    public partial class Form_CH : Form
    {
        CH[] _rootField;
        CH[] asArray;
        string CurrentEdit;

        int pageSize = 0;     //每页显示行数
        int nMax = 0;         //总记录数
        int pageCount = 0;    //页数＝总记录数/每页显示行数
        int pageCurrent = 0;   //当前页号

        private delegate void SavebakeHandler(DataGridViewCellEventArgs e);
        SavebakeHandler _saveDele;
        int update_row, update_col;
        public static IntPtr hand;

        static ArrayList _comboList = null;
        public static string SelectedCatalogue = string.Empty;
        public Form_CH()
        {
            InitializeComponent();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.AllowUserToAddRows = true;
            _saveDele = new SavebakeHandler(SaveChange);
            hand = this.Handle;
            //if(ComboList!=null && ComboList.Count>0 )
            // toolStripComboBox1.Items.AddRange(ComboList.ToArray());
            //toolStripComboBox1.AutoCompleteMode = AutoCompleteMode.Append;
            //toolStripComboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
            toolStripComboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        public void SetFocus()
        {
            toolStripTextBox2.Focus();
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case messaging.CHUPDATE:
                    System.Reflection.PropertyInfo pr = typeof(CH).GetProperties()[update_col];
                    if (pr.GetValue(asArray[update_row], null) != null)
                    {
                        this.Text = "词汇                     第: " + (update_row + 1).ToString() + "行的  " + pr.GetValue(asArray[update_row], null).ToString() + "   值已更新！";
                    }
                    else
                    {
                        this.Text = "词汇                     第: " + (update_row + 1).ToString() + "行的  " + "" + "   值已更新！";
                    }
                    break;

                case messaging.REFRESHCATALOUGE:
                    RefreshCatalogue();
                    break;
            }
            base.WndProc(ref m);
        }

        void RefreshCatalogue()
        {
            IQuery temp = ASDB._dbCH.Query();
            temp.Constrain(typeof(CH));
            temp.Descend("_catalogue");

            IObjectSet result = temp.Execute();
            if (result.Count > 0)
            {
                _comboList = new ArrayList();
                foreach (object o in result)
                {
                    CH ch = (CH)o;
                    if (!string.IsNullOrEmpty(ch.CataLogue) && !_comboList.Contains(ch.CataLogue))
                    {
                        _comboList.Add(ch.CataLogue);
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
                //if (_comboList == null)
                //{
                //    RefreshCatalogue();
                //}
                return _comboList;
            }
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

            asArray = new CH[nEndPos - nStartPos];

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

            cHBindingSource2.DataSource = new object();
            dataGridView1.DataSource = asArray;
            dataGridView1.Refresh();
            UpdateButton();

            SetFocus();
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
        public void UpdateGrid(CH[] collection)
        {
            _rootField = collection;

            InitDataSet(_rootField.Length);
        }

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
                //string cellValue = null;
                //if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                //    cellValue = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                //if (CurrentEdit != cellValue)
                //{
                //    System.Reflection.PropertyInfo pr = typeof(CH).GetProperties()[e.ColumnIndex];
                //    CH oldValue = new CH();
                //    pr.SetValue(oldValue, CurrentEdit, null);

                //    IObjectSet temp =  ASDB._dbCH.QueryByExample(oldValue);
                //    CH ch = (CH)temp.Next();

                //    pr.SetValue(ch,cellValue, null);

                //}
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
            CH ch = asArray[e.RowIndex];
            ch.UpdateTime = DateTime.Now;
            ASDB._dbCH.Store(ch);
            ASDB._dbCH.Commit();
        }

        private void SaveCallBack(IAsyncResult asResult)
        {
            try
            {
                AsyncResult result = (AsyncResult)asResult;
                DataGridViewCellEventArgs e = (DataGridViewCellEventArgs)result.AsyncState;
                SavebakeHandler dele = (SavebakeHandler)result.AsyncDelegate;
                dele.EndInvoke(asResult);
                messaging.SendMessage(Form_CH.hand, messaging.CHUPDATE, 0, 0);
                //System.Reflection.PropertyInfo pr = typeof(CH).GetProperties()[e.ColumnIndex];
                //ts_update.Text = "第: " + (e.RowIndex + 1).ToString() + "行的  " + pr.GetValue(asArray[e.RowIndex], null).ToString() + "   值已更新！";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            PickCatalogue pickForm = new PickCatalogue();
            if (pickForm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                ASDB._dbCH.Store((new CH("...", "...", "...", SelectedCatalogue, DateTime.Now)));


                ASDB._queryCH.Constrain(typeof(CH));
                IQuery p = ASDB._queryCH.Descend("_updateTime");
                IQuery q = ASDB._queryCH.Descend("_catalogue");
                p.Constraints().And(q.Constrain(SelectedCatalogue).Equal());

                IObjectSet result = ASDB._queryCH.Execute(); ASDB._queryCH = ASDB._dbCH.Query();

                _rootField = new CH[result.Count];
                for (int i = 0; i < result.Count; i++)
                {
                    _rootField[i] = (CH)result[i];
                }
                ts_total.Text = "共：" + _rootField.Length + " 条记录";

                //只显示最后一页
                InitDataSet2(_rootField.Length);
                
            }
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            PropertyInfo[] ps = typeof(CH).GetProperties();
            if (dataGridView1.SelectedRows.Count > 0)
            {
                for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                {
                    CH temp =new CH();
                    int j = 0;
                    foreach (PropertyInfo p in ps)
                    {
                        ps[j].SetValue(temp, dataGridView1.SelectedRows[i].Cells[j].Value, null);
                        j++;
                    }
                    
                    IObjectSet result = ASDB._dbCH.QueryByExample(temp);
                    temp = (CH)result[0];
                    ASDB._dbCH.Delete(temp); 
                }
            }

            //重新载入列表
            CommonHelper.ConstrainByType(ASDB._queryCH, typeof(CH));
            IQuery q = CommonHelper.Descend(ASDB._queryCH, "_en");
            q.Constrain(ASDB.CH_filter).Like();
            IObjectSet result1 = ASDB._queryCH.Execute();
            ASDB._queryCH = ASDB._dbCH.Query();
            CH[] chArray = new CH[result1.Count];
            for (int i = 0; i < result1.Count; i++)
            {
                chArray[i] = (CH)result1[i];
            }
            ts_total.Text = "共：" + chArray.Length + " 条记录";
            UpdateGrid(chArray);
        }

        private void DeleteFromRootArray(int indexToRemove)
        {
            if (indexToRemove < _rootField.Length)
            {
                for (int j = indexToRemove; j < _rootField.Length - 1; indexToRemove++)
                {
                    _rootField[indexToRemove] = _rootField[indexToRemove + 1];
                }
            }
            _rootField[_rootField.Length - 1] = null;

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

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            IQuery _q;
            if(!toolStripTextBox2.Text.StartsWith("~"))
                _q= CommonHelper.GetQuery(ASDB._dbCH, typeof(CH), "_en", toolStripTextBox2.Text);
            else
                _q = CommonHelper.GetQuery(ASDB._dbCH, typeof(CH), "_cn", toolStripTextBox2.Text.Remove(0,1));

            if (toolStripComboBox1.Text != null && toolStripComboBox1.Text != string.Empty)
            {
                IQuery q2 = _q.Descend("_catalogue");
                _q.Constraints().And(q2.Constrain(toolStripComboBox1.Text).Equal());
            }

            IObjectSet result1 = _q.Execute();

            CH[] chArray = new CH[result1.Count];
            for (int i = 0; i < result1.Count; i++)
            {
                chArray[i] = (CH)result1[i];
            }

            ts_total.Text = "共：" + chArray.Length + " 条记录";
            UpdateGrid(chArray);
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

        //private void toolStripComboBox1_TextChanged(object sender, EventArgs e)
        //{
        //    CommonHelper.ConstrainByType(ASDB._queryCH, typeof(CH));
        //    IQuery q = CommonHelper.Descend(ASDB._queryCH, "_catalogue");
        //    if (toolStripComboBox1.Text != null && toolStripComboBox1.Text != string.Empty)
        //        q.Constrain(toolStripComboBox1.Text).Equal();

        //    IObjectSet result1 = ASDB._queryCH.Execute();
        //    ASDB._queryCH = ASDB._dbCH.Query();

        //    CH[] chArray = new CH[result1.Count];
        //    for (int i = 0; i < result1.Count; i++)
        //    {
        //        chArray[i] = (CH)result1[i];
        //    }

        //    UpdateGrid(chArray);
        //}

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CommonHelper.ConstrainByType(ASDB._queryCH, typeof(CH));
            //IQuery q = CommonHelper.Descend(ASDB._queryCH, "_catalogue");
            //if (toolStripComboBox1.Text != null && toolStripComboBox1.Text != string.Empty)
            //    q.Constrain(toolStripComboBox1.Text).Equal();

            //IObjectSet result1 = ASDB._queryCH.Execute();
            //ASDB._queryCH = ASDB._dbCH.Query();

            //CH[] chArray = new CH[result1.Count];
            //for (int i = 0; i < result1.Count; i++)
            //{
            //    chArray[i] = (CH)result1[i];
            //}

            //UpdateGrid(chArray);
            IQuery _q;
            if (!toolStripTextBox2.Text.StartsWith("~"))
                _q = CommonHelper.GetQuery(ASDB._dbCH, typeof(CH), "_en", toolStripTextBox2.Text);
            else
                _q = CommonHelper.GetQuery(ASDB._dbCH, typeof(CH), "_cn", toolStripTextBox2.Text.Remove(0, 1));

            if (toolStripComboBox1.Text != null && toolStripComboBox1.Text != string.Empty)
            {
                IQuery q2 = _q.Descend("_catalogue");
                _q.Constraints().And(q2.Constrain(toolStripComboBox1.Text).Equal());
            }

            IObjectSet result1 = _q.Execute();

            CH[] chArray = new CH[result1.Count];
            for (int i = 0; i < result1.Count; i++)
            {
                chArray[i] = (CH)result1[i];
            }
            ts_total.Text = "共："+chArray.Length+" 条记录";
            UpdateGrid(chArray);
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control &&
                e.KeyCode == Keys.D)
            {
                bindingNavigatorAddNewItem.PerformClick();
            }
        }

    }
}
