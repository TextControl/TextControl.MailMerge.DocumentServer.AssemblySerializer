using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using TXTextControl.DocumentServer;

namespace TestProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string sSerializedAssembly;

        private void button1_Click(object sender, EventArgs e)
        {
            sSerializedAssembly = AssemblySerializer.Serialize(tbAssemblyPath.Text);
            webBrowser1.DocumentText = sSerializedAssembly;

            try
            {
                XmlReader reader = XmlReader.Create(new StringReader(sSerializedAssembly));
                DataSet ds = new DataSet();
                ds.ReadXml(reader);
                cbTest.Checked = true;
                cbTest.Text = "DataSet load test passed";
                btnSave.Enabled = true;
            }
            catch {
                cbTest.Checked = false;
                cbTest.Text = "DataSet load test not passed";
                btnSave.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                tbAssemblyPath.Text = openFileDialog1.FileName;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog1.FileName, sSerializedAssembly);
            }
        }
    }
}
