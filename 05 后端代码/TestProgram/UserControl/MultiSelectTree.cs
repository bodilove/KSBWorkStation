using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace MultiSelectTreeView
{
    public partial class MultiSelectTree : TreeView
    {
        public MultiSelectTree()
        {
            InitializeComponent();
        }

        public MultiSelectTree(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        Point Position = new Point();

        public bool MultiSelect
        { get; set; }

        public List<TreeNode> SelectedNodes = new List<TreeNode>();

        bool IsPushDownCtrlKey = false;

        bool IsPushDownShiftKey = false;

        TreeNode OldNode;


        TreeNode dragDropTreeNode;
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            // e.Graphics.fi
        }

        protected override void OnAfterSelect(TreeViewEventArgs e)
        {
            if (IsPushDownCtrlKey == false)
            {
                if (SelectedNodes != null && SelectedNodes.Count > 0)
                {
                    foreach (TreeNode tn in SelectedNodes)
                    {
                        tn.BackColor = Color.White;
                    }
                }
             
                    SelectedNodes.Clear();
                
                SelectedNodes.Add(e.Node);
                e.Node.BackColor = Color.DodgerBlue;
            }

            if (IsPushDownShiftKey == true)
            {
                if (OldNode != null)
                {
                    if (OldNode.Level == 0 && e.Node.Level == 0)
                    {
                        this.SelectedNodes.Clear();
                        if (OldNode.Index < e.Node.Index)
                        {
                            for (int i = OldNode.Index; i <= e.Node.Index; i++)
                            {
                                this.SelectedNodes.Add(this.Nodes[i]);
                                Nodes[i].BackColor = Color.DodgerBlue;
                            }
                        }
                        if (OldNode.Index > e.Node.Index)
                        {
                            for (int i = e.Node.Index; i <= OldNode.Index; i++)
                            {
                                this.SelectedNodes.Add(this.Nodes[i]);
                                Nodes[i].BackColor = Color.DodgerBlue;
                            }
                        }
                        if (OldNode.Index == e.Node.Index)
                        {
                            this.SelectedNodes.Add(OldNode);
                            OldNode.BackColor = Color.DodgerBlue;
                        }
                    }
                    else
                    {
                        if (OldNode.Parent != null && e.Node.Parent != null)
                        {
                            if (OldNode.Level == e.Node.Level && OldNode.Parent.Index == e.Node.Parent.Index)
                            {
                                this.SelectedNodes.Clear();
                                if (OldNode.Index < e.Node.Index)
                                {
                                    for (int i = OldNode.Index; i <= e.Node.Index; i++)
                                    {
                                        this.SelectedNodes.Add(OldNode.Parent.Nodes[i]);
                                        OldNode.Parent.Nodes[i].BackColor = Color.DodgerBlue;
                                    }
                                }
                                if (OldNode.Index > e.Node.Index)
                                {
                                    for (int i = e.Node.Index; i <= OldNode.Index; i++)
                                    {
                                        this.SelectedNodes.Add(OldNode.Parent.Nodes[i]);
                                        OldNode.Parent.Nodes[i].BackColor = Color.DodgerBlue;
                                    }
                                }
                                if (OldNode.Index == e.Node.Index)
                                {
                                    this.SelectedNodes.Add(OldNode);
                                    OldNode.BackColor = Color.DodgerBlue;
                                }
                            }
                        }
                    }
                }
            }
            base.OnAfterSelect(e);
            OldNode = e.Node;
        }

        protected override void OnBeforeSelect(TreeViewCancelEventArgs e)
        {

            base.OnBeforeSelect(e);

            if (IsPushDownCtrlKey == true)
            {
                if (SelectedNodes[0].Parent != null && e.Node.Parent != null)
                {
                    if (SelectedNodes[0].Parent.Level == e.Node.Parent.Level)
                    {
                        if (SelectedNodes[0].Parent.Index == e.Node.Parent.Index)
                        {
                            e.Node.BackColor = Color.DodgerBlue;
                            if (!SelectedNodes.Contains(e.Node))
                            {
                                SelectedNodes.Add(e.Node);
                            }
                        }
                    }
                }
                else
                {
                    if (SelectedNodes[0].Parent == null && e.Node.Parent == null)
                    {
                        e.Node.BackColor = Color.DodgerBlue;
                        if (!SelectedNodes.Contains(e.Node))
                        {
                            SelectedNodes.Add(e.Node);
                        }
                    }
                }
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            //if (IsPushDownCtrlKey == false)
            //{
            //    if (SelectedNodes != null && SelectedNodes.Count > 0)
            //    {
            //        foreach (TreeNode tn in SelectedNodes)
            //        {
            //            tn.BackColor = Color.White;
            //        }
            //        SelectedNodes.Clear();
            //    }

            //}
            base.OnMouseClick(e);
        }
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                IsPushDownCtrlKey = true;
            }
            if (e.KeyCode == Keys.ShiftKey)
            {
                IsPushDownShiftKey = true;
            }
            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                IsPushDownCtrlKey = false;
            }
            if (e.KeyCode == Keys.ShiftKey)
            {
                IsPushDownShiftKey = false;
            }
            base.OnKeyUp(e);

        }


        public void GetNodesInRange()
        {
            //rectange.Contains(); 获取每个TreeNode的
        }

        public void DrawNode()
        {
            // e.Graphics.FillRectangle(e.Bounds) ，
        }

        public void SelectButton()
        {

        }

        protected override void OnItemDrag(ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
            base.OnItemDrag(e);
        }

        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            base.OnDragEnter(drgevent);
            if (drgevent.Data.GetDataPresent(typeof(TreeNode)))
                drgevent.Effect = DragDropEffects.Move;
            else
                drgevent.Effect = DragDropEffects.None;

        }

        protected override void OnDragOver(DragEventArgs drgevent)
        {
            base.OnDragOver(drgevent);
        }
        
        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            base.OnDragDrop(drgevent);
             TreeNode myNode = null;
            if (drgevent.Data.GetDataPresent(typeof(TreeNode)))
            {
                myNode = (TreeNode)(drgevent.Data.GetData(typeof(TreeNode)));
            }
            else
            {
                MessageBox.Show("error");
            }
            Position.X = drgevent.X;
            Position.Y = drgevent.Y;
            Position = this.PointToClient(Position);
            TreeNode DropNode = this.GetNodeAt(Position);

            if (DropNode != null && DropNode.Parent != myNode && DropNode != myNode)
            {

                TreeNode DragNode = myNode;
                if (DragNode.Level == 0 && DropNode.Level == 0)
                {
                    myNode.Remove();
                    this.Nodes.Insert(DropNode.Index, DragNode);
                    this.SelectedNode = DragNode;
                }
                else if (DropNode.Level == DragNode.Level)
                {
                    myNode.Remove();
                    DropNode.Parent.Nodes.Insert(DropNode.Index, DragNode);
                    this.SelectedNode = DragNode;

                }
            }
            // 如果目标节点不存在，即拖拽的位置不存在节点，那么就将被拖拽节点放在根节点之下
            if (DropNode == null)
            {
                TreeNode DragNode = myNode;
                myNode.Remove();
                this.Nodes.Add(DragNode);
                this.SelectedNode = DragNode;
            }

        }
    }


}
