using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StartUp
{
    public partial class frmSetPassword : Form
    {
        public UserManager m_xmlusers;
        public string UserName;

        public frmSetPassword()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //if(this.NewPassword.Text == string.Empty 
            //    || this.ConfirmNewPassword.Text==string.Empty)
            //{
            //     MessageBox.Show(this, "", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //     return;
            //}

            if(this.NewPassword.Text != this.ConfirmNewPassword.Text)
            {

                MessageBox.Show(this, "两次输入的密码不一致", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //m_xmlusers.
            m_xmlusers.SetPasswordOfUser(UserName, NewPassword.Text);
            this.btnOK.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

     
        private void SetPassword_Load(object sender, EventArgs e)
        {
            
        }
    }
}
