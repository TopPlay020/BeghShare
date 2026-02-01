namespace BeghShare
{
    partial class MainWindow
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
            MouseY = new TextBox();
            label1 = new Label();
            label2 = new Label();
            flowLayoutPanel1 = new FlowLayoutPanel();
            MouseX = new TextBox();
            KeyDownText = new TextBox();
            label3 = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            flowLayoutPanel2 = new FlowLayoutPanel();
            OnlineText = new TextBox();
            label6 = new Label();
            tableLayoutPanel2 = new TableLayoutPanel();
            SideMenu = new TreeView();
            MainFrame = new Panel();
            flowLayoutPanel1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            flowLayoutPanel2.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // MouseY
            // 
            MouseY.Enabled = false;
            MouseY.Location = new Point(298, 3);
            MouseY.Name = "MouseY";
            MouseY.Size = new Size(50, 27);
            MouseY.TabIndex = 0;
            MouseY.Text = "0";
            MouseY.TextAlign = HorizontalAlignment.Center;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            label1.AutoSize = true;
            label1.Location = new Point(198, 0);
            label1.Margin = new Padding(0);
            label1.Name = "label1";
            label1.Size = new Size(21, 33);
            label1.TabIndex = 2;
            label1.Text = "X:";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            label2.AutoSize = true;
            label2.Location = new Point(275, 0);
            label2.Margin = new Padding(0);
            label2.Name = "label2";
            label2.Size = new Size(20, 33);
            label2.TabIndex = 3;
            label2.Text = "Y:";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoSize = true;
            flowLayoutPanel1.Controls.Add(MouseY);
            flowLayoutPanel1.Controls.Add(label2);
            flowLayoutPanel1.Controls.Add(MouseX);
            flowLayoutPanel1.Controls.Add(label1);
            flowLayoutPanel1.Controls.Add(KeyDownText);
            flowLayoutPanel1.Controls.Add(label3);
            flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
            flowLayoutPanel1.Location = new Point(264, 3);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(351, 33);
            flowLayoutPanel1.TabIndex = 4;
            flowLayoutPanel1.WrapContents = false;
            // 
            // MouseX
            // 
            MouseX.Enabled = false;
            MouseX.Location = new Point(222, 3);
            MouseX.Name = "MouseX";
            MouseX.Size = new Size(50, 27);
            MouseX.TabIndex = 1;
            MouseX.Text = "0";
            MouseX.TextAlign = HorizontalAlignment.Center;
            // 
            // KeyDownText
            // 
            KeyDownText.Enabled = false;
            KeyDownText.Location = new Point(78, 3);
            KeyDownText.Name = "KeyDownText";
            KeyDownText.Size = new Size(117, 27);
            KeyDownText.TabIndex = 5;
            KeyDownText.Text = "A";
            KeyDownText.TextAlign = HorizontalAlignment.Center;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            label3.AutoSize = true;
            label3.Location = new Point(0, 0);
            label3.Margin = new Padding(0);
            label3.Name = "label3";
            label3.Size = new Size(75, 33);
            label3.TabIndex = 4;
            label3.Text = "KeyDown:";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.Controls.Add(flowLayoutPanel2, 0, 0);
            tableLayoutPanel1.Controls.Add(flowLayoutPanel1, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Bottom;
            tableLayoutPanel1.Location = new Point(0, 407);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(618, 39);
            tableLayoutPanel1.TabIndex = 5;
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.AutoSize = true;
            flowLayoutPanel2.Controls.Add(OnlineText);
            flowLayoutPanel2.Controls.Add(label6);
            flowLayoutPanel2.FlowDirection = FlowDirection.RightToLeft;
            flowLayoutPanel2.Location = new Point(3, 3);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(108, 33);
            flowLayoutPanel2.TabIndex = 5;
            flowLayoutPanel2.WrapContents = false;
            // 
            // OnlineText
            // 
            OnlineText.Enabled = false;
            OnlineText.Location = new Point(55, 3);
            OnlineText.Name = "OnlineText";
            OnlineText.Size = new Size(50, 27);
            OnlineText.TabIndex = 5;
            OnlineText.Text = "0";
            OnlineText.TextAlign = HorizontalAlignment.Center;
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            label6.AutoSize = true;
            label6.Location = new Point(0, 0);
            label6.Margin = new Padding(0);
            label6.Name = "label6";
            label6.Size = new Size(52, 33);
            label6.TabIndex = 4;
            label6.Text = "Online";
            label6.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 37.5F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62.5F));
            tableLayoutPanel2.Controls.Add(SideMenu, 0, 0);
            tableLayoutPanel2.Controls.Add(MainFrame, 1, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(618, 407);
            tableLayoutPanel2.TabIndex = 6;
            // 
            // SideMenu
            // 
            SideMenu.Dock = DockStyle.Fill;
            SideMenu.Location = new Point(3, 3);
            SideMenu.Margin = new Padding(3, 3, 3, 6);
            SideMenu.Name = "SideMenu";
            SideMenu.Size = new Size(225, 398);
            SideMenu.TabIndex = 0;
            SideMenu.AfterSelect += SideMenu_AfterSelect;
            // 
            // MainFrame
            // 
            MainFrame.Dock = DockStyle.Fill;
            MainFrame.Location = new Point(234, 3);
            MainFrame.Name = "MainFrame";
            MainFrame.Size = new Size(381, 401);
            MainFrame.TabIndex = 1;
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(618, 446);
            Controls.Add(tableLayoutPanel2);
            Controls.Add(tableLayoutPanel1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            Name = "MainWindow";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "MainWindow";
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            flowLayoutPanel2.ResumeLayout(false);
            flowLayoutPanel2.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private TextBox MouseY;
        private Label label1;
        private Label label2;
        private FlowLayoutPanel flowLayoutPanel1;
        private TextBox MouseX;
        private TextBox KeyDownText;
        private Label label3;
        private TableLayoutPanel tableLayoutPanel1;
        private FlowLayoutPanel flowLayoutPanel2;
        private TextBox OnlineText;
        private Label label6;
        private TableLayoutPanel tableLayoutPanel2;
        private TreeView SideMenu;
        private Panel MainFrame;
    }
}

