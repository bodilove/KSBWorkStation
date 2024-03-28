﻿using Common;
using MainControl.BLL;
using MainControl.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace MainControl
{
    public partial class WindowParent : DockContent
    {
       
        public WindowParent()
        {

            #region 关闭窗口
            System.Windows.Forms.ContextMenuStrip cms = new System.Windows.Forms.ContextMenuStrip();
            // 
            // tsmiClose
            // 
            System.Windows.Forms.ToolStripMenuItem tsmiClose = new System.Windows.Forms.ToolStripMenuItem();
            tsmiClose.Name = "cms";
            tsmiClose.Size = new System.Drawing.Size(98, 22);
            tsmiClose.Text = "关闭";
            tsmiClose.Click += new System.EventHandler(this.tsmiClose_Click);
            // 
            // tsmiALLClose
            // 
            System.Windows.Forms.ToolStripMenuItem tsmiALLClose = new System.Windows.Forms.ToolStripMenuItem();
            tsmiALLClose.Name = "cms";
            tsmiALLClose.Size = new System.Drawing.Size(98, 22);
            tsmiALLClose.Text = "全部关闭";
            tsmiALLClose.Click += new System.EventHandler(this.tsmiALLClose_Click);
            // 
            // tsmiApartFromClose
            // 
            System.Windows.Forms.ToolStripMenuItem tsmiApartFromClose = new System.Windows.Forms.ToolStripMenuItem();
            tsmiApartFromClose.Name = "cms";
            tsmiApartFromClose.Size = new System.Drawing.Size(98, 22);
            tsmiApartFromClose.Text = "除此之外全部关闭";
            tsmiApartFromClose.Click += new System.EventHandler(this.tsmiApartFromClose_Click);
            // 
            // tsmiClose
            // 
            cms.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            tsmiClose,tsmiApartFromClose,tsmiALLClose
                             });
            cms.Name = "tsmiClose";
            cms.Size = new System.Drawing.Size(99, 26);
            this.TabPageContextMenuStrip = cms;

            #endregion


        }

       
        protected void InitView(Form form)
        {
            /*
             * 一、固定窗体大小
                方法一：选中窗体--属性--FormBorderStyle--FixedSingle
                方法二：将Form中，MaximumSize和MinmunSize的值设置为与当前的Form的Size值一样。如均设为688,631（作者采用的第一种做法，因此这里显示的不一样）
             * 
             * 二、隐藏窗体相关按钮
             * 
            屏蔽最大化 / 最小化：设置MaximizeBox为False，设置MinimizeBox为False；此外，还可以设置FormBorder格式为FixedToolWindow，直接隐藏上边框
            屏蔽右上角三个按钮：设置属性Control为False
            */
            //form.MaximizeBox = false;
            form.MinimizeBox=false;

        }



        #region 按钮权限
        protected void  SetButton(int ParentId,ToolStrip toolStrip1)
        {
            if (GlobalUserHandle.UserNum == "SuperAdmin")
            {
                return;
            }
            SysMenuService menu_bll = new SysMenuService();
            List<SysMenuModel> list = menu_bll.GetButtonList(ParentId, GlobalUserHandle.RoleID);
            foreach (var control in toolStrip1.Items)
            {
                if (control is ToolStripButton)
                {
                    ToolStripButton t = (ToolStripButton)control;
                    if (list != null)
                    {
                        var name = list.Where(o => o.Menu_Tag == t.Name.Trim()).FirstOrDefault();
                        if (name != null)
                        {
                            if (t.Name == name.Menu_Tag)
                            {
                                t.Enabled = true;
                            }
                            else
                            {
                                t.Enabled = false;
                            }
                        }
                        else
                        {
                            t.Enabled = false;
                        }

                    }
                    else
                    {
                        t.Enabled = false;
                    }

                }
            }
        }
        #endregion

        #region 关闭窗口事件
        private void tsmiClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void tsmiApartFromClose_Click(object sender, EventArgs e)
        {
            DockContentCollection contents = DockPanel.Contents;
            int num = 0;
            while (num < contents.Count)
            {
                if (contents[num].DockHandler.DockState == DockState.Document && DockPanel.ActiveContent != contents[num])
                {
                    contents[num].DockHandler.Hide();
                }
                else
                {
                    num++;
                }
            }
        }
        private void tsmiALLClose_Click(object sender, EventArgs e)
        {
            DockContentCollection contents = DockPanel.Contents;
            int num = 0;
            while (num < contents.Count)
            {
                if (contents[num].DockHandler.DockState == DockState.Document)
                {
                    contents[num].DockHandler.Hide();
                }
                else
                {
                    num++;
                }
            }
        }
        #endregion

    }
}
