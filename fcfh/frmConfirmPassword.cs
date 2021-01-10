using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fcfh
{
    public partial class frmConfirmPassword : Form
    {
        private string PW;
        public frmConfirmPassword(string OriginalPassword)
        {
            PW = OriginalPassword;
            InitializeComponent();
            if (string.IsNullOrEmpty(PW))
            {
                Text = "Password required";
                lblPrompt.Text = "A password is required";
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(PW))
            {
                if(!string.IsNullOrEmpty(tbConfirmPassword.Text))
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("Please enter a password", "No Password", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else if (PW == tbConfirmPassword.Text)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Your password does not match", "Invalid Password", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
