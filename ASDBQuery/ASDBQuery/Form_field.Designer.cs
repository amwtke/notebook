namespace ASDBQuery
{
    partial class Form_field
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_field));
            this.aSFieldBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dg_fields = new System.Windows.Forms.DataGridView();
            this.tableNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cNDescriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataLengthDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataPrecisionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataScaleDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nullableDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cHARCOLDECLLENGTHDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cHARLENGTHDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cHARUSEDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.ts_update = new System.Windows.Forms.ToolStripLabel();
            ((System.ComponentModel.ISupportInitialize)(this.aSFieldBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_fields)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            this.SuspendLayout();
            // 
            // aSFieldBindingSource
            // 
            this.aSFieldBindingSource.DataSource = typeof(ASTableDefinition.ASField);
            // 
            // dg_fields
            // 
            this.dg_fields.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dg_fields.AutoGenerateColumns = false;
            this.dg_fields.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_fields.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.tableNameDataGridViewTextBoxColumn,
            this.columnNameDataGridViewTextBoxColumn,
            this.cNDescriptionDataGridViewTextBoxColumn,
            this.dataTypeDataGridViewTextBoxColumn,
            this.dataLengthDataGridViewTextBoxColumn,
            this.dataPrecisionDataGridViewTextBoxColumn,
            this.dataScaleDataGridViewTextBoxColumn,
            this.nullableDataGridViewTextBoxColumn,
            this.columIDDataGridViewTextBoxColumn,
            this.cHARCOLDECLLENGTHDataGridViewTextBoxColumn,
            this.cHARLENGTHDataGridViewTextBoxColumn,
            this.cHARUSEDDataGridViewTextBoxColumn});
            this.dg_fields.DataSource = this.aSFieldBindingSource;
            this.dg_fields.Location = new System.Drawing.Point(12, 28);
            this.dg_fields.Name = "dg_fields";
            this.dg_fields.RowHeadersWidth = 60;
            this.dg_fields.RowTemplate.Height = 23;
            this.dg_fields.Size = new System.Drawing.Size(1221, 410);
            this.dg_fields.TabIndex = 1;
            this.dg_fields.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dg_fields_CellBeginEdit);
            this.dg_fields.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_fields_CellDoubleClick);
            this.dg_fields.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_fields_CellEndEdit);
            this.dg_fields.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dg_fields_KeyDown);
            // 
            // tableNameDataGridViewTextBoxColumn
            // 
            this.tableNameDataGridViewTextBoxColumn.DataPropertyName = "TableName";
            this.tableNameDataGridViewTextBoxColumn.HeaderText = "TableName";
            this.tableNameDataGridViewTextBoxColumn.Name = "tableNameDataGridViewTextBoxColumn";
            // 
            // columnNameDataGridViewTextBoxColumn
            // 
            this.columnNameDataGridViewTextBoxColumn.DataPropertyName = "ColumnName";
            this.columnNameDataGridViewTextBoxColumn.HeaderText = "ColumnName";
            this.columnNameDataGridViewTextBoxColumn.Name = "columnNameDataGridViewTextBoxColumn";
            // 
            // cNDescriptionDataGridViewTextBoxColumn
            // 
            this.cNDescriptionDataGridViewTextBoxColumn.DataPropertyName = "CNDescription";
            this.cNDescriptionDataGridViewTextBoxColumn.HeaderText = "CNDescription";
            this.cNDescriptionDataGridViewTextBoxColumn.Name = "cNDescriptionDataGridViewTextBoxColumn";
            // 
            // dataTypeDataGridViewTextBoxColumn
            // 
            this.dataTypeDataGridViewTextBoxColumn.DataPropertyName = "DataType";
            this.dataTypeDataGridViewTextBoxColumn.HeaderText = "DataType";
            this.dataTypeDataGridViewTextBoxColumn.Name = "dataTypeDataGridViewTextBoxColumn";
            // 
            // dataLengthDataGridViewTextBoxColumn
            // 
            this.dataLengthDataGridViewTextBoxColumn.DataPropertyName = "DataLength";
            this.dataLengthDataGridViewTextBoxColumn.HeaderText = "DataLength";
            this.dataLengthDataGridViewTextBoxColumn.Name = "dataLengthDataGridViewTextBoxColumn";
            // 
            // dataPrecisionDataGridViewTextBoxColumn
            // 
            this.dataPrecisionDataGridViewTextBoxColumn.DataPropertyName = "DataPrecision";
            this.dataPrecisionDataGridViewTextBoxColumn.HeaderText = "DataPrecision";
            this.dataPrecisionDataGridViewTextBoxColumn.Name = "dataPrecisionDataGridViewTextBoxColumn";
            // 
            // dataScaleDataGridViewTextBoxColumn
            // 
            this.dataScaleDataGridViewTextBoxColumn.DataPropertyName = "DataScale";
            this.dataScaleDataGridViewTextBoxColumn.HeaderText = "DataScale";
            this.dataScaleDataGridViewTextBoxColumn.Name = "dataScaleDataGridViewTextBoxColumn";
            // 
            // nullableDataGridViewTextBoxColumn
            // 
            this.nullableDataGridViewTextBoxColumn.DataPropertyName = "Nullable";
            this.nullableDataGridViewTextBoxColumn.HeaderText = "Nullable";
            this.nullableDataGridViewTextBoxColumn.Name = "nullableDataGridViewTextBoxColumn";
            // 
            // columIDDataGridViewTextBoxColumn
            // 
            this.columIDDataGridViewTextBoxColumn.DataPropertyName = "ColumID";
            this.columIDDataGridViewTextBoxColumn.HeaderText = "ColumID";
            this.columIDDataGridViewTextBoxColumn.Name = "columIDDataGridViewTextBoxColumn";
            // 
            // cHARCOLDECLLENGTHDataGridViewTextBoxColumn
            // 
            this.cHARCOLDECLLENGTHDataGridViewTextBoxColumn.DataPropertyName = "CHAR_COL_DECL_LENGTH";
            this.cHARCOLDECLLENGTHDataGridViewTextBoxColumn.HeaderText = "CHAR_COL_DECL_LENGTH";
            this.cHARCOLDECLLENGTHDataGridViewTextBoxColumn.Name = "cHARCOLDECLLENGTHDataGridViewTextBoxColumn";
            // 
            // cHARLENGTHDataGridViewTextBoxColumn
            // 
            this.cHARLENGTHDataGridViewTextBoxColumn.DataPropertyName = "CHAR_LENGTH";
            this.cHARLENGTHDataGridViewTextBoxColumn.HeaderText = "CHAR_LENGTH";
            this.cHARLENGTHDataGridViewTextBoxColumn.Name = "cHARLENGTHDataGridViewTextBoxColumn";
            // 
            // cHARUSEDDataGridViewTextBoxColumn
            // 
            this.cHARUSEDDataGridViewTextBoxColumn.DataPropertyName = "CHAR_USED";
            this.cHARUSEDDataGridViewTextBoxColumn.HeaderText = "CHAR_USED";
            this.cHARUSEDDataGridViewTextBoxColumn.Name = "cHARUSEDDataGridViewTextBoxColumn";
            // 
            // bindingNavigator1
            // 
            this.bindingNavigator1.AddNewItem = null;
            this.bindingNavigator1.BindingSource = this.aSFieldBindingSource;
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
            this.toolStripButton1,
            this.ts_update});
            this.bindingNavigator1.Location = new System.Drawing.Point(0, 0);
            this.bindingNavigator1.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.bindingNavigator1.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.bindingNavigator1.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.bindingNavigator1.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = null;
            this.bindingNavigator1.Size = new System.Drawing.Size(1245, 25);
            this.bindingNavigator1.TabIndex = 2;
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
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // ts_update
            // 
            this.ts_update.Name = "ts_update";
            this.ts_update.Size = new System.Drawing.Size(0, 22);
            // 
            // Form_field
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1245, 450);
            this.Controls.Add(this.bindingNavigator1);
            this.Controls.Add(this.dg_fields);
            this.Name = "Form_field";
            this.Text = "Form_field";
            ((System.ComponentModel.ISupportInitialize)(this.aSFieldBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_fields)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingSource aSFieldBindingSource;
        private System.Windows.Forms.DataGridView dg_fields;
        private System.Windows.Forms.DataGridViewTextBoxColumn tableNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cNDescriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataLengthDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataPrecisionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataScaleDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nullableDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn columIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cHARCOLDECLLENGTHDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cHARLENGTHDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cHARUSEDDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.ToolStripLabel ts_update;
    }
}