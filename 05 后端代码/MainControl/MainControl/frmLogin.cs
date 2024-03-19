using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MainControl
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        public void Login()
        {
            if (txtUserNum.Text.Trim() != "")
            {
                MdlClass.userInfo = MdlClass.userbll.GetUserInfoByUserNum(txtUserNum.Text.Trim(), this.txtPassWord.Text.Trim());
                if (MdlClass.userInfo != null)
                {
                    if (MdlClass.userInfo.PassWord.ToUpper() != this.txtPassWord.Text.Trim().ToUpper())
                    {
                        MessageBox.Show("用户名密码错误，请重新输入！", "");
                        return;
                    }
                    else
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("用户名密码错误，请重新输入！", "");
                    txtUserNum.Focus();
                    txtUserNum.SelectAll();
                    return;
                }
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Login();
        }

        private void button1_Click(object sender, EventArgs e)
        { 
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frmLogin_KeyPress(object sender, KeyPressEventArgs e)
        {
         
        }

        private void frmLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Login();
            }
        }

        private void txtUserNum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Login();
            }
        }

        private void txtPassWord_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Login();
            }
        }
    }
}
