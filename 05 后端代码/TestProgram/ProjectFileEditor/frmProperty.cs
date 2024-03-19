using System;
using System.Drawing;
using System.Windows.Forms;

namespace Test.ProjectFileEditor
{
    public partial class frmProperty : Form
    {
        public frmProperty()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Global.Prj.PartNumber = txtPCBNumber.Text;
            Global.Prj.Version = txtVersion.Text;
            Global.Prj.mImage = picBackground.Image;
            Global.Prj.Description = txtDescription.Text;
            Global.Prj.InitFileName = txtInitialFile.Text;
            Global.Prj.IsAnalyse = chkIsAnalyse.Checked;
            Global.Prj.IsStartTest = chkIsPowerTest.Checked;
            Global.Prj.IsErrorBreak = chkErrorIsBreak.Checked;
            this.Close();
        }

        private void frmProperty_Load(object sender, EventArgs e)
        {
            txtPCBNumber.Text = Global.Prj.PartNumber;
            txtVersion.Text = Global.Prj.Version;
            picBackground.Image = Global.Prj.mImage;
            txtDescription.Text = Global.Prj.Description;
            txtInitialFile.Text = Global.Prj.InitFileName;
            chkIsAnalyse.Checked = Global.Prj.IsAnalyse;
            chkIsPowerTest.Checked = Global.Prj.IsStartTest;
            chkErrorIsBreak.Checked = Global.Prj.IsErrorBreak;
        }

     
        private void picBrowse_Click(object sender, EventArgs e)
        {
            fileOpen.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF";
            fileOpen.Multiselect = false;
            fileOpen.FileName = "";

            DialogResult res = fileOpen.ShowDialog(this);
            if (res == DialogResult.OK)
            {
                picBackground.Image = Image.FromFile(fileOpen.FileName);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmPowerOnTestConfigure fc = new frmPowerOnTestConfigure(Global.Prj, "");
            fc.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (txtInitialFile.Text.Trim() == "")
            {
                MessageBox.Show("请选择配置文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                //frmConfigure fc = new frmConfigure(txtInitialFile.Text.Trim());
                //fc.ShowDialog();
                frmPowerOnTestConfigure fc = new frmPowerOnTestConfigure(Global.Prj, txtInitialFile.Text);
                fc.ShowDialog();
            }
        }

        private void btnBrowsPath_Click(object sender, EventArgs e)
        {
            fileOpen.Multiselect = false;
            fileOpen.FileName = "";

            DialogResult res = fileOpen.ShowDialog(this);
            if (res == DialogResult.OK)
            {
                txtInitialFile.Text = fileOpen.FileName;
            }
        }

        private void chkIsPowerTest_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
