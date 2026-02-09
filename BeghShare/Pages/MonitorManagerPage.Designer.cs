using System.Windows.Forms;

namespace BeghShare.View.Pages
{
    partial class MonitorManagerPage
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            mainmonitor = new Panel();
            label1 = new Label();
            mainmonitor.SuspendLayout();
            SuspendLayout();
            // 
            // mainmonitor
            // 
            mainmonitor.BackColor = SystemColors.ActiveCaption;
            mainmonitor.BorderStyle = BorderStyle.FixedSingle;
            mainmonitor.Controls.Add(label1);
            mainmonitor.Size = new Size(200, 90);
            mainmonitor.Left = (Width - mainmonitor.Size.Width) / 2;
            mainmonitor.Top = (Height - mainmonitor.Size.Height) / 2;
            mainmonitor.Name = "mainmonitor";
            mainmonitor.TabIndex = 0;
            // 
            // label1
            // 
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI", 30F);
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(198, 88);
            label1.TabIndex = 0;
            label1.Text = "1";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // MonitorManagerPage
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(mainmonitor);
            Name = "MonitorManagerPage";
            Size = new Size(365, 400);
            mainmonitor.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel mainmonitor;
        private Label label1;
    }
}
