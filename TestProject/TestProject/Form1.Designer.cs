namespace TestProject
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSerialize = new System.Windows.Forms.Button();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.tbAssemblyPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.cbTest = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // btnSerialize
            // 
            this.btnSerialize.Location = new System.Drawing.Point(12, 66);
            this.btnSerialize.Name = "btnSerialize";
            this.btnSerialize.Size = new System.Drawing.Size(146, 36);
            this.btnSerialize.TabIndex = 0;
            this.btnSerialize.Text = "Serialize";
            this.btnSerialize.UseVisualStyleBackColor = true;
            this.btnSerialize.Click += new System.EventHandler(this.button1_Click);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser1.Location = new System.Drawing.Point(12, 120);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(862, 293);
            this.webBrowser1.TabIndex = 1;
            // 
            // tbAssemblyPath
            // 
            this.tbAssemblyPath.Location = new System.Drawing.Point(12, 40);
            this.tbAssemblyPath.Name = "tbAssemblyPath";
            this.tbAssemblyPath.Size = new System.Drawing.Size(214, 20);
            this.tbAssemblyPath.TabIndex = 2;
            this.tbAssemblyPath.Text = "myDummyClass.dll";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Assembly";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(232, 39);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 22);
            this.btnBrowse.TabIndex = 4;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.button2_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "DLL Assemblies|*.dll|EXE Assemblies|*.exe";
            // 
            // cbTest
            // 
            this.cbTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbTest.AutoCheck = false;
            this.cbTest.AutoSize = true;
            this.cbTest.Location = new System.Drawing.Point(12, 419);
            this.cbTest.Name = "cbTest";
            this.cbTest.Size = new System.Drawing.Size(145, 17);
            this.cbTest.TabIndex = 5;
            this.cbTest.Text = "DataSet load test passed";
            this.cbTest.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(11, 442);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(146, 36);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save XML";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "XML files|*.xml";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(886, 488);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.cbTest);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbAssemblyPath);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.btnSerialize);
            this.Name = "Form1";
            this.Text = "TXTextControl.DocumentServer.AssemblySerializer Test Project";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSerialize;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.TextBox tbAssemblyPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.CheckBox cbTest;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}

