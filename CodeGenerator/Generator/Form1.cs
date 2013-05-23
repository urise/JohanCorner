using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CommonClasses.Helpers;
using CommonClasses.InfoClasses;

namespace Generator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] lines = File.ReadAllLines(textBox1.Text);
            var tableInfoList = new TableInfoList(lines);
            tableInfoList.CreateResultFiles(FileHelper.GetFolderName(textBox1.Text));

            txtLog.Text = tableInfoList.GetSql();
            MessageBox.Show(@"Все готово, не забудьте обновить модель");
        }
    }
}
