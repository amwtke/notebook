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
    public partial class PickCatalogue_NoteBook : Form
    {
        static int _lastPickedIndex = -1;
        public PickCatalogue_NoteBook()
        {
            InitializeComponent();

            messaging.SendMessage(Form_Notebook.hand, messaging.REFRESH_NOTE_CATEGORY, 0, 0);
            bt_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            System.Threading.Thread.Sleep(500);

            comboBox1.AutoCompleteMode = AutoCompleteMode.Append;
            comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
            if (Form_Notebook.ComboList != null && Form_Notebook.ComboList.Count > 0)
                comboBox1.Items.AddRange(Form_Notebook.ComboList.ToArray());

            if (_lastPickedIndex != -1)
                comboBox1.SelectedIndex = _lastPickedIndex;
        }

        private void bt_OK_Click(object sender, EventArgs e)
        {
            Form_Notebook.SelectedCatalogue = comboBox1.Text;
            _lastPickedIndex = comboBox1.SelectedIndex;
        }

        private void comboBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
                bt_OK.PerformClick();
        }
    }
}
