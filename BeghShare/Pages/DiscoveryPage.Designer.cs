namespace BeghShare.UI.Pages
{
    partial class DiscoveryPage
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
            tableLayoutPanel1 = new TableLayoutPanel();
            label1 = new Label();
            SearchText = new TextBox();
            button1 = new Button();
            DiscoveryButton = new Button();
            PeersList = new ListView();
            ComputerName = new ColumnHeader();
            IPAddress = new ColumnHeader();
            Online = new ColumnHeader();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Controls.Add(SearchText, 1, 0);
            tableLayoutPanel1.Controls.Add(button1, 2, 0);
            tableLayoutPanel1.Controls.Add(DiscoveryButton, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Top;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(365, 70);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(72, 35);
            label1.TabIndex = 0;
            label1.Text = "IP Adress:";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SearchText
            // 
            SearchText.Dock = DockStyle.Fill;
            SearchText.Location = new Point(81, 3);
            SearchText.Name = "SearchText";
            SearchText.Size = new Size(181, 27);
            SearchText.TabIndex = 1;
            // 
            // button1
            // 
            button1.Location = new Point(268, 3);
            button1.Name = "button1";
            button1.Size = new Size(94, 29);
            button1.TabIndex = 2;
            button1.Text = "Search";
            button1.UseVisualStyleBackColor = true;
            button1.Click += SearchClick;
            // 
            // DiscoveryButton
            // 
            tableLayoutPanel1.SetColumnSpan(DiscoveryButton, 3);
            DiscoveryButton.Dock = DockStyle.Fill;
            DiscoveryButton.Location = new Point(3, 38);
            DiscoveryButton.Name = "DiscoveryButton";
            DiscoveryButton.Size = new Size(359, 29);
            DiscoveryButton.TabIndex = 3;
            DiscoveryButton.Text = "Discovery";
            DiscoveryButton.UseVisualStyleBackColor = true;
            DiscoveryButton.Click += DiscoveryClick;
            // 
            // PeersList
            // 
            PeersList.Columns.AddRange(new ColumnHeader[] { ComputerName, IPAddress, Online });
            PeersList.Dock = DockStyle.Fill;
            PeersList.FullRowSelect = true;
            PeersList.Location = new Point(0, 70);
            PeersList.MultiSelect = false;
            PeersList.Name = "PeersList";
            PeersList.Size = new Size(365, 330);
            PeersList.TabIndex = 2;
            PeersList.UseCompatibleStateImageBehavior = false;
            PeersList.View = System.Windows.Forms.View.Details;
            PeersList.MouseDoubleClick += PeerItemDoubleClick;
            // 
            // ComputerName
            // 
            ComputerName.Text = "Name";
            ComputerName.Width = 100;
            // 
            // IPAddress
            // 
            IPAddress.Text = "IPAddress";
            IPAddress.Width = 150;
            // 
            // Online
            // 
            Online.Text = "Online";
            // 
            // DiscoveryPage
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(PeersList);
            Controls.Add(tableLayoutPanel1);
            Name = "DiscoveryPage";
            Size = new Size(365, 400);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TableLayoutPanel tableLayoutPanel1;
        private Label label1;
        private TextBox SearchText;
        private Button button1;
        private Button DiscoveryButton;
        private ListView PeersList;
        private ColumnHeader ComputerName;
        private ColumnHeader IPAddress;
        private ColumnHeader Online;
    }
}
