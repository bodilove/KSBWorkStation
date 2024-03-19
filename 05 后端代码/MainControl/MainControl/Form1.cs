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


    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //从数据库获取生产变种
            //获取生产信息，工艺，BOM
            //
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            c1 c= new c1("2222");
            c = null;
          c = new c1("1111");
        }
        int i = 0;
        private void button23_Click(object sender, EventArgs e)
        {
            //txtLog.Text+="要加入的文字\r";
            i++;
            AddtextLog("要加入的文字" + i);
            button23.Text =":::"+ txtLog.Lines.Count();
        }

        public void AddtextLog(string s)
        {
            if (txtLog.Lines.Count() > 10)
            {
                DeleteLine(0);
            }
            txtLog.Text += s + '\r';

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


    public class c1 : IDisposable
    {
        string a = "";
        public c1(string b)
        {
            a = b;
        }

        public void Dispose()
        {
            a =
                null;
        }
    }
}
