using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using Test.Common;

namespace Test.ProjectFileEditor
{
    [Serializable]
    class nodeOperatorForDIYtree
    {
        [NonSerialized]
        private MultiSelectTreeView.MultiSelectTree _treeView;

        private bool cutStatus = false;
        //private TreeNode _cutnode = null;

        public TreeNode _node;
        public TestStep _TS;
        public SubTestStep _SubTS;

        public TreeNode _cutNode;
        public TestStep _cutTS;
        public SubTestStep _cutSubTS;

        public List<TreeNode> _nodes = new List<TreeNode>();
        public List<TestStep> _TSs = new List<TestStep>();
        public List<SubTestStep> _SubTSs = new List<SubTestStep>();

        public List<TreeNode> _cutNodes = new List<TreeNode>();
        public List<TestStep> _cutTSs = new List<TestStep>();
        public List<SubTestStep> _cutSubTSs = new List<SubTestStep>();



        public nodeOperatorForDIYtree(MultiSelectTreeView.MultiSelectTree treeview)
        {
            _treeView = treeview;

        }

        private string getNewNodeText(string prefix, TreeNode node, int pos)
        {
            string stxt = "";
            int cnt = pos;
            bool exist = false;
            do
            {
                stxt = prefix + cnt.ToString();
                cnt++;
                exist = false;
                foreach (TreeNode n in node.Nodes)
                {
                    if (n.Text == stxt)
                    {
                        exist = true;
                        break;
                    }
                }
            } while (exist);
            return stxt;
        }

        public void AddNode()
        {
            TreeNode node = _treeView.SelectedNode;
            TreeNode t;
            int insertPOS = node.Nodes.Count + 1;
            string skey = "";
            string stxt = "";
            if (node.Name == "TestSteps")
            {
                skey = "TS" + insertPOS.ToString();
                stxt = getNewNodeText("TS", node, insertPOS);
                t = node.Nodes.Add(skey, stxt);
                TestStep ts = new TestStep();
                ts.Name = node.LastNode.Text;
                ts.Enable = true;
                ts.Description = ts.Name;

                Global.Prj.TestSteps.Add(ts);
                t.Checked = true;
            }

            else if (node.Name.StartsWith("TS"))
            {
                skey = "SubTS" + insertPOS.ToString();
                stxt = getNewNodeText("SubTS", node, insertPOS);
                t = node.Nodes.Add(skey, stxt);

                SubTestStep subts = new SubTestStep();
                subts.Name = node.LastNode.Text;
                subts.Enable = true;
                subts.Description = subts.Name;
                subts.ErrorCode = "";
                subts.SaveResult = false;
                subts.RetryCount = 0;
                subts.MeasureType = enumMeasureType.AutoTest;
                subts.Timeout = 0;
                Global.Prj.TestSteps[node.Index].SubTest.Add(subts);
                t.Checked = true;
            }
        }

        public void DeleteNode()
        {
            TreeNode node = _treeView.SelectedNode;
            if (node.Name.StartsWith("TS"))
            {
                Global.Prj.TestSteps.RemoveAt(node.Index);
                node.Remove();
            }
            else if (node.Name.StartsWith("SubTS"))
            {
                Global.Prj.TestSteps[node.Parent.Index].SubTest.RemoveAt(node.Index);
                node.Remove();
            }
        }

        public void DeleteNodes()
        {
           List<TreeNode> Nodes = _treeView.SelectedNodes;
           for (int i = 0; i < Nodes.Count; i++ )
           {
               if (Nodes[i].Name.StartsWith("TS"))
               {
                   Global.Prj.TestSteps.RemoveAt(Nodes[i].Index);
                   Nodes[i].Remove();
               }
               else if (Nodes[i].Name.StartsWith("SubTS"))
               {
                   Global.Prj.TestSteps[Nodes[i].Parent.Index].SubTest.RemoveAt(Nodes[i].Index);
                   Nodes[i].Remove();
               }
           }
          
        }

        public void MoveUp()
        {
            TreeNode node = _treeView.SelectedNode;
            TreeNode newnode = (TreeNode)node.Clone();

            if (node.Name.StartsWith("TS"))
            {
                if (node.Index == 0) return;//防止在TS第一行时在上移报错。
                TestStep ts = (TestStep)Global.Prj.TestSteps[node.Index].Clone();

                Global.Prj.TestSteps.Insert(node.PrevNode.Index, ts);
                Global.Prj.TestSteps.RemoveAt(node.Index + 1);
            }
            else if (node.Name.StartsWith("SubTS"))
            {
                if (node.Index == 0) return;//防止在SubTS第一行时在上移报错。
                TestStep ts = Global.Prj.TestSteps[node.Parent.Index];
                SubTestStep subts = (SubTestStep)ts.SubTest[node.Index].Clone();
                ts.SubTest.Insert(node.PrevNode.Index, subts);
                ts.SubTest.RemoveAt(node.Index + 1);
            }
            else
            {
                return;
            }
            node.Parent.Nodes.Insert(node.PrevNode.Index, newnode);
            node.Remove();
            _treeView.SelectedNode = newnode;
        }

        public void MoveDown()
        {
            TreeNode node = _treeView.SelectedNode;
            TreeNode newnode = (TreeNode)node.Clone();

            if (node.Name.StartsWith("TS"))
            {
                if (node.Index == node.Parent.Nodes.Count - 1) return;//ts中防止下移报错。
                TestStep ts = (TestStep)Global.Prj.TestSteps[node.Index].Clone();
                Global.Prj.TestSteps.Insert(node.NextNode.Index + 1, ts);
                Global.Prj.TestSteps.RemoveAt(node.Index);
            }
            else if (node.Name.StartsWith("SubTS"))
            {
                if (node.Index == node.Parent.Nodes.Count - 1) return;//subts中防止下移报错。
                TestStep ts = Global.Prj.TestSteps[node.Parent.Index];
                SubTestStep subts = (SubTestStep)ts.SubTest[node.Index].Clone();
                ts.SubTest.Insert(node.NextNode.Index + 1, subts);
                ts.SubTest.RemoveAt(node.Index);
            }
            else
            {
                return;  //防止在大步时上下移出错。
            }
            node.Parent.Nodes.Insert(node.NextNode.Index + 1, newnode);
            node.Remove();
            _treeView.SelectedNode = newnode;
        }

        public void InsertSameParent( TreeNode  Node, TreeNode DropNode)
        {
            TreeNode node = Node;
            TreeNode newnode = (TreeNode)node.Clone();

            if (node.Name.StartsWith("TS"))
            {
               // if (node.Index == node.Parent.Nodes.Count - 1) return;//ts中防止下移报错。
                TestStep Oldts = (TestStep)Global.Prj.TestSteps[node.Index];
                TestStep ts = (TestStep)Global.Prj.TestSteps[node.Index].Clone();
                Global.Prj.TestSteps.Insert(DropNode.Index, ts);
                Global.Prj.TestSteps.Remove(Oldts);
            }
            else if (node.Name.StartsWith("SubTS"))
            {
               // if (node.Index == node.Parent.Nodes.Count - 1) return;//subts中防止下移报错。
                TestStep ts = Global.Prj.TestSteps[node.Parent.Index];
                SubTestStep Oldsubts = (SubTestStep)ts.SubTest[node.Index];
                SubTestStep subts = (SubTestStep)ts.SubTest[node.Index].Clone();

                ts.SubTest.Insert(DropNode.Index, subts);
                ts.SubTest.Remove(Oldsubts);
            }
            else
            {
                return;  //防止在大步时上下移出错。
            }
            //node.Parent.Nodes.Insert(node.Index, newnode);
            //node.Remove();
            //_treeView.SelectedNode = newnode;
        }

        public void InsertdiffrentParent(TreeNode Node, TreeNode DropNode)
        {
            TreeNode node = Node;
            TreeNode newnode = (TreeNode)node.Clone();

            if (node.Name.StartsWith("TS"))
            {
                // if (node.Index == node.Parent.Nodes.Count - 1) return;//ts中防止下移报错。
                TestStep Oldts = (TestStep)Global.Prj.TestSteps[node.Index];
                TestStep ts = (TestStep)Global.Prj.TestSteps[node.Index].Clone();
                Global.Prj.TestSteps.Insert(DropNode.Index, ts);
                Global.Prj.TestSteps.Remove(Oldts);
            }
            else if (node.Name.StartsWith("SubTS"))
            {
                // if (node.Index == node.Parent.Nodes.Count - 1) return;//subts中防止下移报错。
                TestStep ts = Global.Prj.TestSteps[node.Parent.Index];
                SubTestStep Oldsubts = (SubTestStep)ts.SubTest[node.Index];
                SubTestStep subts = (SubTestStep)ts.SubTest[node.Index].Clone();
                TestStep DropNodeTs = Global.Prj.TestSteps[DropNode.Parent.Index];
                DropNodeTs.SubTest.Insert(DropNode.Index, subts);
                ts.SubTest.Remove(Oldsubts);
            }
            else
            {
                return;  //防止在大步时上下移出错。
            }
        }

        public void CopyNode(bool cut)
        {
             cutStatus = cut;
            _cutNode = _treeView.SelectedNode;
            _node = (TreeNode)_cutNode.Clone();

            Clipboard.Clear();
            if (_node.Name.StartsWith("TS"))
            {
                _cutTS = Global.Prj.TestSteps[_cutNode.Index];
                _TS = (TestStep)_cutTS.Clone();

                Clipboard.SetData("TSNode", this);
            }
            else if (_node.Name.StartsWith("SubTS"))
            {
                _cutSubTS = Global.Prj.TestSteps[_cutNode.Parent.Index].SubTest[_cutNode.Index];
                _SubTS = (SubTestStep)_cutSubTS.Clone();
                Clipboard.SetData("SubTSNode", this);
            }

        }

        public void PasteNode()
        {
            TreeNode rNode = _treeView.SelectedNode;

            int insertPOS = rNode.Nodes.Count + 1;

            if (rNode.Name.StartsWith("TestSteps"))
            {
                nodeOperatorForDIYtree nodeOpe = (nodeOperatorForDIYtree)Clipboard.GetData("TSNode");
                if (nodeOpe == null) { MessageBox.Show("焦点错误"); return; };//防止粘贴错误
                nodeOpe._node.Name = "TS" + insertPOS.ToString();
                nodeOpe._node.Text = nodeOpe._node.Text + " 副本";
                nodeOpe._TS.Name = nodeOpe._node.Text;
                rNode.Nodes.Add(nodeOpe._node);
                Global.Prj.TestSteps.Add(nodeOpe._TS);
            }
            else if (rNode.Name.StartsWith("TS"))
            {
                nodeOperatorForDIYtree nodeOpe = (nodeOperatorForDIYtree)Clipboard.GetData("SubTSNode");
                if (nodeOpe == null) { MessageBox.Show("焦点错误"); return; };//防止粘贴错误
                nodeOpe._node.Name = "SubTS" + insertPOS.ToString();
                nodeOpe._node.Text = nodeOpe._node.Text + " 副本";
                nodeOpe._SubTS.Name = nodeOpe._node.Text;
                rNode.Nodes.Add(nodeOpe._node);
                Global.Prj.TestSteps[rNode.Index].SubTest.Add(nodeOpe._SubTS);
            }

            if (cutStatus)
            {
                //_cutnode.Parent.Nodes.Remove(_cutnode);
                _cutNode.Parent.Nodes.Remove(_cutNode);
                if (rNode.Name.StartsWith("TestSteps"))
                {
                    Global.Prj.TestSteps.Remove(_cutTS);
                }
                else if (rNode.Name.StartsWith("TS"))
                {
                    Global.Prj.TestSteps[rNode.Index].SubTest.Remove(_cutSubTS);
                }
                cutStatus = false;
            }
        }

        public void CopyNodes(bool cut)
        {
            cutStatus = cut;
          //  _cutNodes = _treeView.SelectedNodes;
            _cutNodes.Clear();
            foreach (TreeNode tn in _treeView.SelectedNodes)
            {
                _cutNodes.Add(tn);
            }
            _nodes.Clear();
            foreach (TreeNode tn in _treeView.SelectedNodes)
            {
                _nodes.Add((TreeNode)tn.Clone());
            }


            Clipboard.Clear();
            if (_nodes[0].Name.StartsWith("TS"))
            {

                foreach (TreeNode tn in _cutNodes)
                {
                    _cutTSs.Add(Global.Prj.TestSteps[tn.Index]);
                    //_cutNodes.Add(tn);
                }
                //_cutTSs = Global.Prj.TestSteps[_cutNode.Index];
                //_TSs = (TestStep)_cutTS.Clone();
                foreach (TestStep ts in _cutTSs)
                {
                    _TSs.Add((TestStep)ts.Clone());
                }

                Clipboard.SetData("TSNode", this);
            }
            else if (_nodes[0].Name.StartsWith("SubTS"))
            {
                //_cutSubTS = Global.Prj.TestSteps[_cutNode.Parent.Index].SubTest[_cutNode.Index];
                //_SubTS = (SubTestStep)_cutSubTS.Clone();
                foreach (TreeNode tn in _cutNodes)
                {
                    _cutSubTSs.Add(Global.Prj.TestSteps[tn.Parent.Index].SubTest[tn.Index]);
                    //_cutNodes.Add(tn);
                }
                foreach (SubTestStep sts in _cutSubTSs)
                {
                    _SubTSs.Add((SubTestStep)sts.Clone());
                }
                Clipboard.SetData("SubTSNode", this);
            }
        }

        public void PasteNodes()
        {
            TreeNode rNode = _treeView.SelectedNode;

            nodeOperatorForDIYtree nodeOpe;
            if (rNode.Name.StartsWith("TestSteps"))
            {
                int insertPOS = rNode.Nodes.Count + 1;
                nodeOpe = (nodeOperatorForDIYtree)Clipboard.GetData("TSNode");
                if (nodeOpe == null) { MessageBox.Show("焦点错误"); return; };//防止粘贴错误
                int s = 0;
                foreach (TreeNode tn in nodeOpe._nodes)
                {
                    tn.Name = "TS" + insertPOS.ToString();
                    tn.Text = tn.Text + " 副本";
                    nodeOpe._TSs[s].Name = tn.Text;
                    rNode.Nodes.Add(tn);
                    Global.Prj.TestSteps.Add(nodeOpe._TSs[s]);
                    s++;
                    insertPOS++;
                    tn.BackColor = Color.White;
                }
            }
            else if (rNode.Name.StartsWith("TS"))
            {
                int insertPOS = rNode.Nodes.Count + 1;
                nodeOpe = (nodeOperatorForDIYtree)Clipboard.GetData("SubTSNode");
                if (nodeOpe == null) { MessageBox.Show("焦点错误"); return; };//防止粘贴错误
                int s = 0;
                foreach (TreeNode tn in nodeOpe._nodes)
                {
                    tn.Name = "SubTS" + insertPOS.ToString();
                    tn.Text = tn.Text + " 副本";
                    nodeOpe._SubTSs[s].Name = tn.Text;
                    rNode.Nodes.Add(tn);
                    Global.Prj.TestSteps[rNode.Index].SubTest.Add(nodeOpe._SubTSs[s]);
                    s++;
                    insertPOS++;
                    tn.BackColor = Color.White;
                }
            }

            if (cutStatus)
            {
                //_cutnode.Parent.Nodes.Remove(_cutnode);
                //for (int k = 0; k < _cutNodes.Count; k++)
                //{
                //    _cutNodes[k].Remove();
                //}

                foreach (TreeNode cn in _cutNodes)
                {
                    cn.Parent.Nodes.Remove(cn);
                }
                //_cutNode.Parent.Nodes.Remove(_cutNode);

                if (rNode.Name.StartsWith("TestSteps"))
                {
                    nodeOpe = (nodeOperatorForDIYtree)Clipboard.GetData("TSNode");
                    foreach (TreeNode tn in nodeOpe._nodes)
                    {
                        //tn.Remove();
                        Global.Prj.TestSteps.Remove(_cutTS);
                    }
                }

                else if (rNode.Name.StartsWith("TS"))
                {
                    nodeOpe = (nodeOperatorForDIYtree)Clipboard.GetData("SubTSNode");
                    foreach (TreeNode tn in nodeOpe._nodes)
                    {
                        //tn.Remove();
                        Global.Prj.TestSteps[tn.Index].SubTest.Remove(_cutSubTS);
                    }
                }
            }
            cutStatus = false;
        }
    }
}

