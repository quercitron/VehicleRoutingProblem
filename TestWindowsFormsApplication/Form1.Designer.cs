namespace TestWindowsFormsApplication
{
    partial class TestForm
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
            this.bitmapPanel = new System.Windows.Forms.Panel();
            this.buttonGetPath = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.buttonReset = new System.Windows.Forms.Button();
            this.buttonAddPoints = new System.Windows.Forms.Button();
            this.numericPoints = new System.Windows.Forms.NumericUpDown();
            this.solversPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonUpload = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericPoints)).BeginInit();
            this.SuspendLayout();
            // 
            // bitmapPanel
            // 
            this.bitmapPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.bitmapPanel.Location = new System.Drawing.Point(12, 12);
            this.bitmapPanel.Name = "bitmapPanel";
            this.bitmapPanel.Size = new System.Drawing.Size(200, 100);
            this.bitmapPanel.TabIndex = 0;
            this.bitmapPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.bitmapPanel_Paint);
            this.bitmapPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.bitmapPanel_MouseUp);
            // 
            // buttonGetPath
            // 
            this.buttonGetPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonGetPath.Location = new System.Drawing.Point(1272, 38);
            this.buttonGetPath.Name = "buttonGetPath";
            this.buttonGetPath.Size = new System.Drawing.Size(100, 23);
            this.buttonGetPath.TabIndex = 1;
            this.buttonGetPath.Text = "Get Path";
            this.buttonGetPath.UseVisualStyleBackColor = true;
            this.buttonGetPath.Click += new System.EventHandler(this.buttonGetPath_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(1272, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 2;
            // 
            // buttonReset
            // 
            this.buttonReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonReset.Location = new System.Drawing.Point(1272, 67);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(100, 23);
            this.buttonReset.TabIndex = 3;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // buttonAddPoints
            // 
            this.buttonAddPoints.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddPoints.Location = new System.Drawing.Point(1272, 122);
            this.buttonAddPoints.Name = "buttonAddPoints";
            this.buttonAddPoints.Size = new System.Drawing.Size(100, 23);
            this.buttonAddPoints.TabIndex = 4;
            this.buttonAddPoints.Text = "Add Points";
            this.buttonAddPoints.UseVisualStyleBackColor = true;
            this.buttonAddPoints.Click += new System.EventHandler(this.buttonAddPoints_Click);
            // 
            // numericPoints
            // 
            this.numericPoints.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericPoints.Location = new System.Drawing.Point(1272, 96);
            this.numericPoints.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericPoints.Name = "numericPoints";
            this.numericPoints.Size = new System.Drawing.Size(100, 20);
            this.numericPoints.TabIndex = 5;
            this.numericPoints.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // solversPanel
            // 
            this.solversPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.solversPanel.Location = new System.Drawing.Point(1238, 151);
            this.solversPanel.Name = "solversPanel";
            this.solversPanel.Size = new System.Drawing.Size(134, 450);
            this.solversPanel.TabIndex = 6;
            // 
            // buttonUpload
            // 
            this.buttonUpload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUpload.Location = new System.Drawing.Point(1272, 727);
            this.buttonUpload.Name = "buttonUpload";
            this.buttonUpload.Size = new System.Drawing.Size(100, 23);
            this.buttonUpload.TabIndex = 7;
            this.buttonUpload.Text = "Upload";
            this.buttonUpload.UseVisualStyleBackColor = true;
            this.buttonUpload.Click += new System.EventHandler(this.buttonUpload_Click);
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1384, 962);
            this.Controls.Add(this.buttonUpload);
            this.Controls.Add(this.solversPanel);
            this.Controls.Add(this.numericPoints);
            this.Controls.Add(this.buttonAddPoints);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.buttonGetPath);
            this.Controls.Add(this.bitmapPanel);
            this.Name = "TestForm";
            this.Text = "Test App";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.numericPoints)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel bitmapPanel;
        private System.Windows.Forms.Button buttonGetPath;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Button buttonAddPoints;
        private System.Windows.Forms.NumericUpDown numericPoints;
        private System.Windows.Forms.FlowLayoutPanel solversPanel;
        private System.Windows.Forms.Button buttonUpload;
    }
}

