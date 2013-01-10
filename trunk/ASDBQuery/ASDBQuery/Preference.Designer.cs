namespace ASDBQuery
{
    partial class Form_Preference
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            messaging.SendMessage(ASDB.hand, messaging.CLOSE_PREFERENCE, 0, 0);
            if (_db != null)
                _db.Close();

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Preference));
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripTextBox2 = new System.Windows.Forms.ToolStripTextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.pb_excel = new System.Windows.Forms.ProgressBar();
            this.bt_importexcel = new System.Windows.Forms.Button();
            this.lb_percent = new System.Windows.Forms.Label();
            this.lb_now = new System.Windows.Forms.Label();
            this.aSTableBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.preferenceNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bPRValueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cNNCValueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.preferenceBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lb_total = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.aSTableBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.preferenceBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // bindingNavigator1
            // 
            this.bindingNavigator1.AddNewItem = null;
            this.bindingNavigator1.CountItem = this.bindingNavigatorCountItem;
            this.bindingNavigator1.DeleteItem = null;
            this.bindingNavigator1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.toolStripTextBox1,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2,
            this.toolStripTextBox2});
            this.bindingNavigator1.Location = new System.Drawing.Point(0, 0);
            this.bindingNavigator1.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.bindingNavigator1.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.bindingNavigator1.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.bindingNavigator1.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = null;
            this.bindingNavigator1.Size = new System.Drawing.Size(774, 25);
            this.bindingNavigator1.TabIndex = 1;
            this.bindingNavigator1.Text = "bindingNavigator1";
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(32, 22);
            this.bindingNavigatorCountItem.Text = "/ {0}";
            this.bindingNavigatorCountItem.ToolTipText = "总项数";
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveFirstItem.Text = "移到第一条记录";
            this.bindingNavigatorMoveFirstItem.Click += new System.EventHandler(this.bindingNavigatorMoveFirstItem_Click);
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMovePreviousItem.Text = "移到上一条记录";
            this.bindingNavigatorMovePreviousItem.Click += new System.EventHandler(this.bindingNavigatorMovePreviousItem_Click);
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(100, 25);
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveNextItem.Text = "移到下一条记录";
            this.bindingNavigatorMoveNextItem.Click += new System.EventHandler(this.bindingNavigatorMoveNextItem_Click);
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveLastItem.Text = "移到最后一条记录";
            this.bindingNavigatorMoveLastItem.Click += new System.EventHandler(this.bindingNavigatorMoveLastItem_Click);
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripTextBox2
            // 
            this.toolStripTextBox2.Name = "toolStripTextBox2";
            this.toolStripTextBox2.Size = new System.Drawing.Size(300, 25);
            this.toolStripTextBox2.KeyUp += new System.Windows.Forms.KeyEventHandler(this.toolStripTextBox2_KeyUp);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Location = new System.Drawing.Point(12, 245);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(750, 242);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            // 
            // pb_excel
            // 
            this.pb_excel.Location = new System.Drawing.Point(105, 205);
            this.pb_excel.Name = "pb_excel";
            this.pb_excel.Size = new System.Drawing.Size(651, 23);
            this.pb_excel.TabIndex = 3;
            // 
            // bt_importexcel
            // 
            this.bt_importexcel.Location = new System.Drawing.Point(12, 205);
            this.bt_importexcel.Name = "bt_importexcel";
            this.bt_importexcel.Size = new System.Drawing.Size(75, 23);
            this.bt_importexcel.TabIndex = 4;
            this.bt_importexcel.Text = "导入";
            this.bt_importexcel.UseVisualStyleBackColor = true;
            this.bt_importexcel.Click += new System.EventHandler(this.bt_importexcel_Click);
            // 
            // lb_percent
            // 
            this.lb_percent.AutoSize = true;
            this.lb_percent.Location = new System.Drawing.Point(103, 231);
            this.lb_percent.Name = "lb_percent";
            this.lb_percent.Size = new System.Drawing.Size(11, 12);
            this.lb_percent.TabIndex = 5;
            this.lb_percent.Text = "%";
            // 
            // lb_now
            // 
            this.lb_now.AutoSize = true;
            this.lb_now.Location = new System.Drawing.Point(381, 231);
            this.lb_now.Name = "lb_now";
            this.lb_now.Size = new System.Drawing.Size(23, 12);
            this.lb_now.TabIndex = 6;
            this.lb_now.Text = "===";
            // 
            // aSTableBindingSource
            // 
            this.aSTableBindingSource.DataSource = typeof(ASTableDefinition.ASTable);
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.preferenceNameDataGridViewTextBoxColumn,
            this.bPRValueDataGridViewTextBoxColumn,
            this.cNNCValueDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.preferenceBindingSource;
            this.dataGridView1.Location = new System.Drawing.Point(12, 36);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(750, 163);
            this.dataGridView1.TabIndex = 7;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            // 
            // preferenceNameDataGridViewTextBoxColumn
            // 
            this.preferenceNameDataGridViewTextBoxColumn.DataPropertyName = "PreferenceName";
            this.preferenceNameDataGridViewTextBoxColumn.HeaderText = "PreferenceName";
            this.preferenceNameDataGridViewTextBoxColumn.Name = "preferenceNameDataGridViewTextBoxColumn";
            // 
            // bPRValueDataGridViewTextBoxColumn
            // 
            this.bPRValueDataGridViewTextBoxColumn.DataPropertyName = "BPR_Value";
            this.bPRValueDataGridViewTextBoxColumn.HeaderText = "BPR_Value";
            this.bPRValueDataGridViewTextBoxColumn.Name = "bPRValueDataGridViewTextBoxColumn";
            // 
            // cNNCValueDataGridViewTextBoxColumn
            // 
            this.cNNCValueDataGridViewTextBoxColumn.DataPropertyName = "CNNC_Value";
            this.cNNCValueDataGridViewTextBoxColumn.HeaderText = "CNNC_Value";
            this.cNNCValueDataGridViewTextBoxColumn.Name = "cNNCValueDataGridViewTextBoxColumn";
            // 
            // preferenceBindingSource
            // 
            this.preferenceBindingSource.DataSource = typeof(ASTableDefinition.Preference);
            // 
            // lb_total
            // 
            this.lb_total.AutoSize = true;
            this.lb_total.Location = new System.Drawing.Point(593, 13);
            this.lb_total.Name = "lb_total";
            this.lb_total.Size = new System.Drawing.Size(29, 12);
            this.lb_total.TabIndex = 8;
            this.lb_total.Text = "共：";
            // 
            // Form_Preference
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(774, 499);
            this.Controls.Add(this.lb_total);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.lb_now);
            this.Controls.Add(this.lb_percent);
            this.Controls.Add(this.bt_importexcel);
            this.Controls.Add(this.pb_excel);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.bindingNavigator1);
            this.Name = "Form_Preference";
            this.Text = "Preference";
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.aSTableBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.preferenceBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.ProgressBar pb_excel;
        private System.Windows.Forms.Button bt_importexcel;
        private System.Windows.Forms.Label lb_percent;
        private System.Windows.Forms.Label lb_now;
        private System.Windows.Forms.BindingSource aSTableBindingSource;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn preferenceNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bPRValueDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cNNCValueDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource preferenceBindingSource;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox2;
        private System.Windows.Forms.Label lb_total;
    }
}