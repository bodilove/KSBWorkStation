using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MainControl
{
    public partial  class FrmListImages : Form
    {
        FrmMenuEdit _frmMenuEdit;
        public FrmListImages(FrmMenuEdit frmMenuEdit)
        {
            this._frmMenuEdit = frmMenuEdit;
            InitializeComponent();
       
        }
        public FrmListImages()
        {       
            InitializeComponent();
        }
        private void FrmListImages_Load(object sender, EventArgs e)
        {
            ReadImagesToList();
        }
        /// <summary>
        /// 读取图片到ImageList中
        /// </summary>
        /// <returns></returns>
        public void ReadImagesToList()
        {

            try
            {
                this.listView1.BeginUpdate();
                this.listView1.View = View.LargeIcon;
                listView1.LargeImageList=this.imageList1;
                for (int i = 0; i <= imageList1.Images.Count - 1; i++)
                {                  

                    ListViewItem lvi = new ListViewItem();
                    lvi.ImageIndex = i;
                    lvi.Text = i.ToString();
                    //lvi.Tag = Path.GetFileName(MyFoldersFiles[i]); 
                    this.listView1.Items.Add(lvi);
                }
                this.listView1.EndUpdate();
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
              
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.listView1.SelectedIndices.Count > 0)
            {
                int  ImageNo = int.Parse(this.listView1.SelectedItems[0].Text);
                _frmMenuEdit.pic_menu.Image = imageList1.Images[ImageNo];
                _frmMenuEdit.pic_menu.Tag = ImageNo;
                this.Close();
            }
        }

    }
}
