using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace 英汉词典
{

    public partial class Form1 : Form
    {
        string Input = "";
        Dictionary<string, string> dic = new Dictionary<string, string>();
        Dictionary<string, string> dicEnglish = new Dictionary<string, string>();
        Dictionary<string, string> dicHuoxing = new Dictionary<string, string>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            createEnDic("英汉词典TXT格式.txt");
            createHxwDic("火星文.txt");
            cbSelect.Items.Add("英汉词典");
            cbSelect.Items.Add("火星文词典");
            cbSelect.SelectedItem = cbSelect.Items[0];
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            Input = txtInput.Text.Trim();
            if (dic.ContainsKey(Input))
            {
                txtResult.Text = dic[Input];
            }
            else
            {
                txtResult.Text = "很遗憾，没有查询到";
            }
        }

        private void createEnDic(string path)//生成英汉字典
        {
            string[] str = File.ReadAllLines(path, Encoding.Default);
            for (int i = 0; i < str.Length; i++)
            {
                string[] str1 = str[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (str1.Length > 1)
                {
                    string s = "";
                    for (int j = 1; j < str1.Length; j++)
                    {
                        s = s + str1[j] + "\r\n";
                    }
                    if (!dicEnglish.ContainsKey(str1[0]))
                    {

                        dicEnglish.Add(str1[0], s);
                    }
                    else
                    {
                        dicEnglish[str1[0]] += s;
                    }

                }
            }
        }

        private void createHxwDic(string path)//生成火星文字典
        {
            string str = File.ReadAllText(path, Encoding.Default);
            string[] s = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < s.Length; i++)
            {
                if (!dicHuoxing.ContainsKey(s[i][0].ToString()))
                {
                    dicHuoxing.Add(s[i][0].ToString(), s[i].Substring(1, s[i].Length - 1));
                }

            }
        }

        private void cbSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            Input = "";
            txtResult.Text = "";
            lbResult.Items.Clear();
            txtInput.Text = "";

            if (cbSelect.SelectedItem.ToString() == "英汉词典")
            {
                dic = dicEnglish;
            }
            else
            {
                dic = dicHuoxing;
            }
            txtInput.Text = "\"回车\"查询";
            txtInput.ForeColor = Color.Gray;

        }

        private void txtInput_MouseClick(object sender, MouseEventArgs e)
        {
            txtInput.Focus();
            txtInput.SelectAll();
        }

        private void lbResult_SelectedIndexChanged(object sender, EventArgs e)
        {

            txtResult.Text = dic[lbResult.SelectedItem.ToString()];
        }

        private void txtInput_TextChanged(object sender, EventArgs e)
        {
            txtInput.ForeColor = Color.Black;
            if (txtInput.Text == "")
            {
                Input = "";
                txtResult.Text = "";
                lbResult.Items.Clear();
                return;
            }
            lbResult.Items.Clear();
            Input = txtInput.Text;
            var v = from d in dic where d.Key.StartsWith(Input) select d;//进行模糊查询
            foreach (var k in v)
            {
                lbResult.Items.Add(k.Key);
            }
        }
    }
}