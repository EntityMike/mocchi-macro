namespace MocchiMacro
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
            this.components = new System.ComponentModel.Container();
            this.controlsColumn = new System.Windows.Forms.Panel();
            this.mascotBox = new System.Windows.Forms.PictureBox();
            this.searchTextLabel = new System.Windows.Forms.Label();
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.goButton = new System.Windows.Forms.Button();
            this.droppedNoteLabel = new System.Windows.Forms.Label();
            this.typingRateLabel = new System.Windows.Forms.Label();
            this.typingRateBox = new System.Windows.Forms.NumericUpDown();
            this.typingRateUnitLabel = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.gameBox = new System.Windows.Forms.ComboBox();
            this.searchTextToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.hotkeyLabel = new System.Windows.Forms.Label();
            this.mascotTimer = new System.Windows.Forms.Timer(this.components);
            this.footerRow = new System.Windows.Forms.Panel();
            this.footerBand = new System.Windows.Forms.PictureBox();
            this.updateNoticeLink = new System.Windows.Forms.LinkLabel();
            this.aboutButton = new System.Windows.Forms.Button();
            this.aboutButtonToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.controlsColumn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mascotBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.typingRateBox)).BeginInit();
            this.footerRow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.footerBand)).BeginInit();
            this.SuspendLayout();
            //
            // controlsColumn
            //
            this.controlsColumn.Controls.Add(this.searchTextLabel);
            this.controlsColumn.Controls.Add(this.searchTextBox);
            this.controlsColumn.Controls.Add(this.goButton);
            this.controlsColumn.Controls.Add(this.droppedNoteLabel);
            this.controlsColumn.Controls.Add(this.typingRateLabel);
            this.controlsColumn.Controls.Add(this.typingRateBox);
            this.controlsColumn.Controls.Add(this.typingRateUnitLabel);
            this.controlsColumn.Controls.Add(this.statusLabel);
            this.controlsColumn.Controls.Add(this.gameBox);
            this.controlsColumn.Controls.Add(this.hotkeyLabel);
            this.controlsColumn.Location = new System.Drawing.Point(120, 12);
            this.controlsColumn.Name = "controlsColumn";
            this.controlsColumn.Size = new System.Drawing.Size(288, 146);
            this.controlsColumn.TabIndex = 0;
            //
            // mascotBox
            //
            this.mascotBox.Location = new System.Drawing.Point(12, 37);
            this.mascotBox.Name = "mascotBox";
            this.mascotBox.Size = new System.Drawing.Size(96, 96);
            this.mascotBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.mascotBox.TabIndex = 1;
            this.mascotBox.TabStop = false;
            //
            // searchTextLabel
            //
            this.searchTextLabel.AutoSize = true;
            this.searchTextLabel.Location = new System.Drawing.Point(0, 0);
            this.searchTextLabel.Name = "searchTextLabel";
            this.searchTextLabel.Size = new System.Drawing.Size(67, 15);
            this.searchTextLabel.TabIndex = 0;
            this.searchTextLabel.Text = "Search text:";
            //
            // searchTextBox
            //
            this.searchTextBox.Location = new System.Drawing.Point(0, 18);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(206, 23);
            this.searchTextBox.TabIndex = 1;
            this.searchTextBox.TextChanged += new System.EventHandler(this.SearchTextBox_TextChanged);
            this.searchTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SearchTextBox_KeyPress);
            //
            // goButton
            //
            this.goButton.Location = new System.Drawing.Point(214, 0);
            this.goButton.Name = "goButton";
            this.goButton.Size = new System.Drawing.Size(74, 41);
            this.goButton.TabIndex = 2;
            this.goButton.Text = "Go";
            this.goButton.UseVisualStyleBackColor = true;
            this.goButton.Click += new System.EventHandler(this.GoButton_Click);
            //
            // droppedNoteLabel
            //
            this.droppedNoteLabel.AutoEllipsis = true;
            this.droppedNoteLabel.ForeColor = System.Drawing.Color.Firebrick;
            this.droppedNoteLabel.Location = new System.Drawing.Point(0, 45);
            this.droppedNoteLabel.Name = "droppedNoteLabel";
            this.droppedNoteLabel.Size = new System.Drawing.Size(288, 17);
            this.droppedNoteLabel.TabIndex = 3;
            this.droppedNoteLabel.UseMnemonic = false;
            //
            // typingRateLabel
            //
            this.typingRateLabel.AutoSize = true;
            this.typingRateLabel.Location = new System.Drawing.Point(0, 104);
            this.typingRateLabel.Name = "typingRateLabel";
            this.typingRateLabel.Size = new System.Drawing.Size(69, 15);
            this.typingRateLabel.TabIndex = 4;
            this.typingRateLabel.Text = "Typing rate:";
            //
            // typingRateBox
            //
            this.typingRateBox.Location = new System.Drawing.Point(72, 100);
            this.typingRateBox.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.typingRateBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.typingRateBox.Name = "typingRateBox";
            this.typingRateBox.Size = new System.Drawing.Size(42, 23);
            this.typingRateBox.TabIndex = 5;
            this.typingRateBox.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.typingRateBox.ValueChanged += new System.EventHandler(this.TypingRateBox_ValueChanged);
            //
            // typingRateUnitLabel
            //
            this.typingRateUnitLabel.AutoSize = true;
            this.typingRateUnitLabel.Location = new System.Drawing.Point(116, 104);
            this.typingRateUnitLabel.Name = "typingRateUnitLabel";
            this.typingRateUnitLabel.Size = new System.Drawing.Size(91, 15);
            this.typingRateUnitLabel.TabIndex = 6;
            this.typingRateUnitLabel.Text = "keys per second";
            //
            // statusLabel
            //
            this.statusLabel.Location = new System.Drawing.Point(0, 63);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(288, 34);
            this.statusLabel.TabIndex = 7;
            //
            // gameBox
            //
            this.gameBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.gameBox.Location = new System.Drawing.Point(214, 100);
            this.gameBox.Name = "gameBox";
            this.gameBox.Size = new System.Drawing.Size(74, 23);
            this.gameBox.TabIndex = 8;
            this.gameBox.SelectedIndexChanged += new System.EventHandler(this.GameBox_SelectedIndexChanged);
            //
            // hotkeyLabel
            //
            this.hotkeyLabel.AutoSize = true;
            this.hotkeyLabel.Location = new System.Drawing.Point(0, 128);
            this.hotkeyLabel.Name = "hotkeyLabel";
            this.hotkeyLabel.Size = new System.Drawing.Size(0, 15);
            this.hotkeyLabel.TabIndex = 9;
            //
            // mascotTimer
            //
            this.mascotTimer.Enabled = true;
            this.mascotTimer.Interval = 30;
            this.mascotTimer.Tick += new System.EventHandler(this.MascotTimer_Tick);
            //
            // footerRow
            //
            this.footerRow.Controls.Add(this.footerBand);
            this.footerRow.Controls.Add(this.updateNoticeLink);
            this.footerRow.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.footerRow.Location = new System.Drawing.Point(0, 170);
            this.footerRow.Name = "footerRow";
            this.footerRow.Size = new System.Drawing.Size(420, 18);
            this.footerRow.TabIndex = 2;
            //
            // footerBand
            //
            this.footerBand.Location = new System.Drawing.Point(0, 0);
            this.footerBand.Name = "footerBand";
            this.footerBand.Size = new System.Drawing.Size(420, 18);
            this.footerBand.TabIndex = 0;
            this.footerBand.TabStop = false;
            //
            // updateNoticeLink
            //
            this.updateNoticeLink.Dock = System.Windows.Forms.DockStyle.Fill;
            this.updateNoticeLink.Location = new System.Drawing.Point(0, 0);
            this.updateNoticeLink.Name = "updateNoticeLink";
            this.updateNoticeLink.Size = new System.Drawing.Size(420, 18);
            this.updateNoticeLink.TabIndex = 1;
            this.updateNoticeLink.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.updateNoticeLink.Visible = false;
            this.updateNoticeLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.UpdateNoticeLink_LinkClicked);
            //
            // aboutButton
            //
            this.aboutButton.Location = new System.Drawing.Point(12, 10);
            this.aboutButton.Name = "aboutButton";
            this.aboutButton.Size = new System.Drawing.Size(22, 22);
            this.aboutButton.TabIndex = 3;
            this.aboutButton.Text = "?";
            this.aboutButtonToolTip.SetToolTip(this.aboutButton, "About");
            this.aboutButton.UseVisualStyleBackColor = true;
            this.aboutButton.Click += new System.EventHandler(this.AboutButton_Click);
            //
            // MainWindow
            //
            this.AcceptButton = this.goButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(420, 188);
            this.Controls.Add(this.controlsColumn);
            this.Controls.Add(this.mascotBox);
            this.Controls.Add(this.footerRow);
            this.Controls.Add(this.aboutButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.Text = "Mocchi Macro";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainWindow_FormClosed);
            this.controlsColumn.ResumeLayout(false);
            this.controlsColumn.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mascotBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.typingRateBox)).EndInit();
            this.footerRow.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.footerBand)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel controlsColumn;
        private System.Windows.Forms.PictureBox mascotBox;
        private System.Windows.Forms.Label searchTextLabel;
        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.Button goButton;
        private System.Windows.Forms.Label droppedNoteLabel;
        private System.Windows.Forms.Label typingRateLabel;
        private System.Windows.Forms.NumericUpDown typingRateBox;
        private System.Windows.Forms.Label typingRateUnitLabel;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.ToolTip searchTextToolTip;
        private System.Windows.Forms.ComboBox gameBox;
        private System.Windows.Forms.Label hotkeyLabel;
        private System.Windows.Forms.Timer mascotTimer;
        private System.Windows.Forms.Panel footerRow;
        private System.Windows.Forms.PictureBox footerBand;
        private System.Windows.Forms.LinkLabel updateNoticeLink;
        private System.Windows.Forms.Button aboutButton;
        private System.Windows.Forms.ToolTip aboutButtonToolTip;
    }
}
