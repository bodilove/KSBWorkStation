using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.Process.Model;
using Common.StationProduct.Model;

namespace MainControl
{
    public partial class StationUI : UserControl
    {
        public StationUI()
        {
            InitializeComponent();
        }

        int RichBoxCount = 100;
        //List<P_BomPart> bomparts = new List<P_BomPart>();
        //List<P_TestItemConfig> TestItemConfiglst = new List<P_TestItemConfig>();
        public void InitTable(List<P_BomPart> bomparts, List<P_TestItemConfig> TestItemConfiglst)
        {
            dgvBom.Rows.Clear();
            dgvTest.Rows.Clear();
            foreach (P_BomPart pbom in bomparts)
            {
                dgvBom.Rows.Add(new object[] {pbom.ItemName, pbom.PartNo,"" });
            }

            foreach (P_TestItemConfig pTest in TestItemConfiglst)
            {
                dgvTest.Rows.Add(new object[] { pTest.TestNum, pTest.TestName, DesignedValueString(pTest.ComPareMode,pTest.Llimit,pTest.Ulimit,pTest.Nom), "", "NotTest",pTest.Description});
            }
            txtLog.Text = "";
        }

        public void Dispose()
        {
            dgvBom.Rows.Clear();
            dgvTest.Rows.Clear();
            txtLog.Text = "";
        }

        public void ResetTable(string SN ,string UUTid)
        {
            this.Invoke(new Action(()=>{
            for (int i = 0; i < dgvBom.Rows.Count; i++)
            {
                dgvBom.Rows[i].DefaultCellStyle.BackColor = Color.White;
                dgvBom.Rows[i].Cells["ColScannerCode"].Value = "";
            }
            for (int i = 0; i < dgvTest.Rows.Count; i++)
            {
                dgvTest.Rows[i].DefaultCellStyle.BackColor = Color.White;
                dgvTest.Rows[i].Cells["ColTestValue"].Value = "";
                dgvTest.Rows[i].Cells["ColResult"].Value = "";
            }

            lblUUTId_SN.Text = "SN: " + SN + "      " + "UUTId: " + UUTid;}));
        }

        private string DesignedValueString(string cmpMode, string LLimit, string ULimit, string NomValue)
        {
        //      Between = 1,
        //Equal = 2,
        //HexStringBetween = 3,
        //CaseEqual = 4,
        //Contains = 5,
        //HexStringMatch = 6,
        //NotBetween = 7,
        //More = 8,
        //Show = 9,
        //NotEqual = 10


            CompareMode CmpMode= CompareMode.Equal;
            if (!Enum.TryParse(cmpMode, out CmpMode))
            {
                throw new Exception("枚举" + cmpMode + "输入字符串不合法。");
            }


            string sRet = "";

            object obj_Nom = null;
            object obj_LL = null;
            object obj_UL = null;


            switch (CmpMode)
            {

                case CompareMode.Between:
                case CompareMode.NotBetween:
                case CompareMode.HexStringBetween:

                    sRet = "{" + LLimit.ToString() + "," + ULimit.ToString() + "}";
                    break;

                case CompareMode.Equal:
                    sRet = "= " + NomValue.ToString();
                    break;
                case CompareMode.More:
                    sRet = "> " + NomValue.ToString();
                    break;
                case CompareMode.CaseEqual:
                    string[] switchs = NomValue.ToString().Split(',');
                    for (int i = 0; i < switchs.Length; i++)
                    {
                        sRet = sRet + "Cs(" + i.ToString() + ") = " + switchs[i] + " ";
                    }
                    //sRet = "= " + NomValue_OBJ.ToString();
                    break;
                case CompareMode.Contains:
                    sRet = "⊇" + NomValue.ToString();
                    break;
                case CompareMode.HexStringMatch:

                    sRet = "Match " + NomValue.ToString();

                    break;


                case CompareMode.Show:

                    sRet = "Show " + NomValue.ToString();

                    break;
                case CompareMode.NotEqual:

                    sRet = "≠ " + NomValue.ToString();

                    break;
            }


            return sRet;
        }

        public void UpAssembleRecord(AssembleRecord assembleRecord,int dataRowIndex)
        {
        this.Invoke(new Action(()=>{      dgvBom.Rows[dataRowIndex].Cells["ColScannerCode"].Value = assembleRecord.ScanCode;
            if (assembleRecord.Result==(int)Result.Pass)
            {
                dgvBom.Rows[dataRowIndex].DefaultCellStyle.BackColor = Color.PaleGreen;
            }
            else
            {
                dgvBom.Rows[dataRowIndex].DefaultCellStyle.BackColor = Color.Red;
            }}));
        }

        public void UpTestRecord(TestRecord testRecord, int dataRowIndex)
        {
            this.Invoke(new Action(() =>
            {
                dgvTest.Rows[dataRowIndex].Cells["ColTestValue"].Value = testRecord.TestValue;
                dgvTest.Rows[dataRowIndex].Cells["ColResult"].Value = testRecord.Result;
                if (testRecord.Result == (int)Result.Pass)
                {
                    dgvTest.Rows[dataRowIndex].DefaultCellStyle.BackColor = Color.PaleGreen;
                }
                else
                {
                    dgvTest.Rows[dataRowIndex].DefaultCellStyle.BackColor = Color.Red;
                }
            }));
        }

        public void AddtextLog(string s)
        {
            this.Invoke(new Action(() =>
          {
              if (txtLog.Lines.Count() > RichBoxCount)
              {
                  DeleteLine(0);
              }
              txtLog.Text += s + '\r';
          }));
    
        }

        private void DeleteLine(int a_line)
        {
            int start_index = txtLog.GetFirstCharIndexFromLine(a_line);
            int count = txtLog.Lines[a_line].Length;

            // Eat new line chars
            if (a_line < txtLog.Lines.Length - 1)
            {
                count += txtLog.GetFirstCharIndexFromLine(a_line + 1) -
                    ((start_index + count - 1) + 1);
            }

            txtLog.Text = txtLog.Text.Remove(start_index, count);
        }
    }
}
