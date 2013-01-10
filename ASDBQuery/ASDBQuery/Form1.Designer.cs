namespace ASDBQuery
{
    partial class ASDB
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if(_db!=null)
                _db.Close();
            if (_dbCH != null)
                _dbCH.Close();

            _threadProcess.Abort();
            
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ASDB));
            this.Bt_Search = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tb_Description = new System.Windows.Forms.TextBox();
            this.bt_fields = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_Field = new System.Windows.Forms.TextBox();
            this.Tb_table = new System.Windows.Forms.TextBox();
            this.BT_Excel = new System.Windows.Forms.Button();
            this.P_EXCEL = new System.Windows.Forms.ProgressBar();
            this.button1 = new System.Windows.Forms.Button();
            this.l_percentage = new System.Windows.Forms.Label();
            this.l_tableName = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.bt_cihui = new System.Windows.Forms.Button();
            this.tb_CH = new System.Windows.Forms.TextBox();
            this.bt_Preference = new System.Windows.Forms.Button();
            this.bt_preference_ALL = new System.Windows.Forms.Button();
            this.BT_NoteBook = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.TSM_CIHUI = new System.Windows.Forms.ToolStripMenuItem();
            this.TSM_Preference = new System.Windows.Forms.ToolStripMenuItem();
            this.TSM_PreferenceALL = new System.Windows.Forms.ToolStripMenuItem();
            this.TSM_NoteBook = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Bt_Search
            // 
            this.Bt_Search.Location = new System.Drawing.Point(6, 99);
            this.Bt_Search.Name = "Bt_Search";
            this.Bt_Search.Size = new System.Drawing.Size(207, 23);
            this.Bt_Search.TabIndex = 1;
            this.Bt_Search.Text = "Search Table(Enter)";
            this.Bt_Search.UseVisualStyleBackColor = true;
            this.Bt_Search.Click += new System.EventHandler(this.Bt_Search_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tb_Description);
            this.groupBox1.Controls.Add(this.bt_fields);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tb_Field);
            this.groupBox1.Controls.Add(this.Tb_table);
            this.groupBox1.Controls.Add(this.Bt_Search);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(492, 138);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search";
            // 
            // tb_Description
            // 
            this.tb_Description.Location = new System.Drawing.Point(113, 56);
            this.tb_Description.Name = "tb_Description";
            this.tb_Description.Size = new System.Drawing.Size(100, 21);
            this.tb_Description.TabIndex = 9;
            this.tb_Description.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tb_Description_KeyUp);
            // 
            // bt_fields
            // 
            this.bt_fields.Location = new System.Drawing.Point(261, 99);
            this.bt_fields.Name = "bt_fields";
            this.bt_fields.Size = new System.Drawing.Size(188, 23);
            this.bt_fields.TabIndex = 8;
            this.bt_fields.Text = "Search Fields(Enter)";
            this.bt_fields.UseVisualStyleBackColor = true;
            this.bt_fields.Click += new System.EventHandler(this.bt_fields_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(212, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "Field(Fuzzy)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(111, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "Desc(Fuzzy)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "Table(Fuzzy)";
            // 
            // tb_Field
            // 
            this.tb_Field.Location = new System.Drawing.Point(261, 56);
            this.tb_Field.Name = "tb_Field";
            this.tb_Field.Size = new System.Drawing.Size(188, 21);
            this.tb_Field.TabIndex = 3;
            this.tb_Field.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tb_Field_KeyUp);
            // 
            // Tb_table
            // 
            this.Tb_table.Location = new System.Drawing.Point(6, 55);
            this.Tb_table.Name = "Tb_table";
            this.Tb_table.Size = new System.Drawing.Size(95, 21);
            this.Tb_table.TabIndex = 2;
            this.Tb_table.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Tb_table_KeyUp);
            // 
            // BT_Excel
            // 
            this.BT_Excel.Location = new System.Drawing.Point(105, 50);
            this.BT_Excel.Name = "BT_Excel";
            this.BT_Excel.Size = new System.Drawing.Size(118, 23);
            this.BT_Excel.TabIndex = 0;
            this.BT_Excel.Text = "Process EXCEL";
            this.BT_Excel.UseVisualStyleBackColor = true;
            this.BT_Excel.Click += new System.EventHandler(this.BT_Excel_Click);
            // 
            // P_EXCEL
            // 
            this.P_EXCEL.Location = new System.Drawing.Point(7, 79);
            this.P_EXCEL.Name = "P_EXCEL";
            this.P_EXCEL.Size = new System.Drawing.Size(286, 23);
            this.P_EXCEL.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(9, 20);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(284, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "DB Active";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // l_percentage
            // 
            this.l_percentage.AutoSize = true;
            this.l_percentage.Location = new System.Drawing.Point(7, 109);
            this.l_percentage.Name = "l_percentage";
            this.l_percentage.Size = new System.Drawing.Size(11, 12);
            this.l_percentage.TabIndex = 3;
            this.l_percentage.Text = "%";
            // 
            // l_tableName
            // 
            this.l_tableName.AutoSize = true;
            this.l_tableName.Location = new System.Drawing.Point(120, 109);
            this.l_tableName.Name = "l_tableName";
            this.l_tableName.Size = new System.Drawing.Size(11, 12);
            this.l_tableName.TabIndex = 4;
            this.l_tableName.Text = ":";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.l_tableName);
            this.groupBox2.Controls.Add(this.l_percentage);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.P_EXCEL);
            this.groupBox2.Controls.Add(this.BT_Excel);
            this.groupBox2.Location = new System.Drawing.Point(12, 156);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(299, 118);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "DB";
            // 
            // bt_cihui
            // 
            this.bt_cihui.Location = new System.Drawing.Point(325, 222);
            this.bt_cihui.Name = "bt_cihui";
            this.bt_cihui.Size = new System.Drawing.Size(187, 52);
            this.bt_cihui.TabIndex = 7;
            this.bt_cihui.Text = "词汇";
            this.bt_cihui.UseVisualStyleBackColor = true;
            this.bt_cihui.Click += new System.EventHandler(this.bt_cihui_Click);
            // 
            // tb_CH
            // 
            this.tb_CH.Location = new System.Drawing.Point(325, 181);
            this.tb_CH.Name = "tb_CH";
            this.tb_CH.Size = new System.Drawing.Size(187, 21);
            this.tb_CH.TabIndex = 8;
            // 
            // bt_Preference
            // 
            this.bt_Preference.Location = new System.Drawing.Point(325, 296);
            this.bt_Preference.Name = "bt_Preference";
            this.bt_Preference.Size = new System.Drawing.Size(190, 37);
            this.bt_Preference.TabIndex = 9;
            this.bt_Preference.Text = "Preference";
            this.bt_Preference.UseVisualStyleBackColor = true;
            this.bt_Preference.Click += new System.EventHandler(this.bt_Preference_Click);
            // 
            // bt_preference_ALL
            // 
            this.bt_preference_ALL.Location = new System.Drawing.Point(325, 339);
            this.bt_preference_ALL.Name = "bt_preference_ALL";
            this.bt_preference_ALL.Size = new System.Drawing.Size(190, 40);
            this.bt_preference_ALL.TabIndex = 10;
            this.bt_preference_ALL.Text = "Preference ALL";
            this.bt_preference_ALL.UseVisualStyleBackColor = true;
            this.bt_preference_ALL.Click += new System.EventHandler(this.bt_preference_ALL_Click);
            // 
            // BT_NoteBook
            // 
            this.BT_NoteBook.Location = new System.Drawing.Point(29, 314);
            this.BT_NoteBook.Name = "BT_NoteBook";
            this.BT_NoteBook.Size = new System.Drawing.Size(272, 65);
            this.BT_NoteBook.TabIndex = 11;
            this.BT_NoteBook.Text = "NoteBook";
            this.BT_NoteBook.UseVisualStyleBackColor = true;
            this.BT_NoteBook.Click += new System.EventHandler(this.BT_NoteBook_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "伤不起";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick);
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSM_CIHUI,
            this.TSM_Preference,
            this.TSM_PreferenceALL,
            this.TSM_NoteBook,
            this.helpToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 136);
            // 
            // TSM_CIHUI
            // 
            this.TSM_CIHUI.Name = "TSM_CIHUI";
            this.TSM_CIHUI.Size = new System.Drawing.Size(152, 22);
            this.TSM_CIHUI.Text = "词汇";
            this.TSM_CIHUI.Click += new System.EventHandler(this.TSM_CIHUI_Click);
            // 
            // TSM_Preference
            // 
            this.TSM_Preference.Name = "TSM_Preference";
            this.TSM_Preference.Size = new System.Drawing.Size(152, 22);
            this.TSM_Preference.Text = "偏好";
            this.TSM_Preference.Click += new System.EventHandler(this.TSM_Preference_Click);
            // 
            // TSM_PreferenceALL
            // 
            this.TSM_PreferenceALL.Name = "TSM_PreferenceALL";
            this.TSM_PreferenceALL.Size = new System.Drawing.Size(152, 22);
            this.TSM_PreferenceALL.Text = "偏好全";
            this.TSM_PreferenceALL.Click += new System.EventHandler(this.TSM_PreferenceALL_Click);
            // 
            // TSM_NoteBook
            // 
            this.TSM_NoteBook.Name = "TSM_NoteBook";
            this.TSM_NoteBook.Size = new System.Drawing.Size(152, 22);
            this.TSM_NoteBook.Text = "笔记本";
            this.TSM_NoteBook.Click += new System.EventHandler(this.TSM_NoteBook_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // ASDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 391);
            this.Controls.Add(this.BT_NoteBook);
            this.Controls.Add(this.bt_preference_ALL);
            this.Controls.Add(this.bt_Preference);
            this.Controls.Add(this.tb_CH);
            this.Controls.Add(this.bt_cihui);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ASDB";
            this.Text = "ASDB";
            this.SizeChanged += new System.EventHandler(this.ASDB_SizeChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ASDB_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Bt_Search;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_Field;
        private System.Windows.Forms.TextBox Tb_table;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bt_fields;
        private System.Windows.Forms.Button BT_Excel;
        private System.Windows.Forms.ProgressBar P_EXCEL;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label l_percentage;
        private System.Windows.Forms.Label l_tableName;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button bt_cihui;
        private System.Windows.Forms.TextBox tb_CH;
        private System.Windows.Forms.TextBox tb_Description;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button bt_Preference;
        private System.Windows.Forms.Button bt_preference_ALL;
        private System.Windows.Forms.Button BT_NoteBook;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem TSM_CIHUI;
        private System.Windows.Forms.ToolStripMenuItem TSM_Preference;
        private System.Windows.Forms.ToolStripMenuItem TSM_PreferenceALL;
        private System.Windows.Forms.ToolStripMenuItem TSM_NoteBook;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
    }
}

