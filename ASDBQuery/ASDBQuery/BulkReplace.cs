using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ASDBQuery
{
    public partial class BulkReplace : Form
    {
        public BulkReplace()
        {
            InitializeComponent();
            bt_ok.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void bt_ok_Click(object sender, EventArgs e)
        {
            Form_field.BulkCNWords = textBox1.Text;
        }

        private void BulkReplace_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                bt_ok.PerformClick();
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                bt_ok.PerformClick();
            }
        }
    }
}
