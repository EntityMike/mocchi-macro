namespace MocchiMacro
{
    partial class AboutBox
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
            this.mascotBox = new System.Windows.Forms.PictureBox();
            this.textColumn = new System.Windows.Forms.FlowLayoutPanel();
            this.nameAndVersionLabel = new System.Windows.Forms.Label();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.copyrightLabel = new System.Windows.Forms.Label();
            this.helpLink = new System.Windows.Forms.LinkLabel();
            this.supportLink = new System.Windows.Forms.LinkLabel();
            this.steamLink = new System.Windows.Forms.LinkLabel();
            this.thanksLabel = new System.Windows.Forms.Label();
            this.notAffiliatedLabel = new System.Windows.Forms.Label();
            this.enjoyLabel = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.mascotBox)).BeginInit();
            this.textColumn.SuspendLayout();
            this.SuspendLayout();
            //
            // mascotBox
            //
            this.mascotBox.Location = new System.Drawing.Point(12, 12);
            this.mascotBox.Name = "mascotBox";
            this.mascotBox.Size = new System.Drawing.Size(96, 96);
            this.mascotBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.mascotBox.TabIndex = 0;
            this.mascotBox.TabStop = false;
            //
            // textColumn
            //
            this.textColumn.AutoSize = true;
            this.textColumn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.textColumn.Controls.Add(this.nameAndVersionLabel);
            this.textColumn.Controls.Add(this.descriptionLabel);
            this.textColumn.Controls.Add(this.copyrightLabel);
            this.textColumn.Controls.Add(this.helpLink);
            this.textColumn.Controls.Add(this.supportLink);
            this.textColumn.Controls.Add(this.steamLink);
            this.textColumn.Controls.Add(this.thanksLabel);
            this.textColumn.Controls.Add(this.notAffiliatedLabel);
            this.textColumn.Controls.Add(this.enjoyLabel);
            this.textColumn.Controls.Add(this.okButton);
            this.textColumn.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.textColumn.Location = new System.Drawing.Point(120, 12);
            this.textColumn.Name = "textColumn";
            this.textColumn.Size = new System.Drawing.Size(300, 260);
            this.textColumn.TabIndex = 1;
            this.textColumn.WrapContents = false;
            //
            // nameAndVersionLabel
            //
            this.nameAndVersionLabel.AutoSize = true;
            this.nameAndVersionLabel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.nameAndVersionLabel.Name = "nameAndVersionLabel";
            this.nameAndVersionLabel.TabIndex = 0;
            //
            // descriptionLabel
            //
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.TabIndex = 1;
            //
            // copyrightLabel
            //
            this.copyrightLabel.AutoSize = true;
            this.copyrightLabel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.copyrightLabel.Name = "copyrightLabel";
            this.copyrightLabel.TabIndex = 2;
            //
            // helpLink
            //
            this.helpLink.AutoSize = true;
            this.helpLink.Margin = new System.Windows.Forms.Padding(0, 10, 0, 2);
            this.helpLink.Name = "helpLink";
            this.helpLink.TabIndex = 3;
            this.helpLink.TabStop = true;
            this.helpLink.Text = "Help and changelog";
            this.helpLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Link_LinkClicked);
            //
            // supportLink
            //
            this.supportLink.AutoSize = true;
            this.supportLink.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.supportLink.Name = "supportLink";
            this.supportLink.TabIndex = 4;
            this.supportLink.TabStop = true;
            this.supportLink.Text = "Support";
            this.supportLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Link_LinkClicked);
            //
            // steamLink
            //
            this.steamLink.AutoSize = true;
            this.steamLink.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.steamLink.Name = "steamLink";
            this.steamLink.TabIndex = 5;
            this.steamLink.TabStop = true;
            this.steamLink.Text = "Get Monster Rancher 1 & 2 DX on Steam";
            this.steamLink.UseMnemonic = false;
            this.steamLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Link_LinkClicked);
            //
            // thanksLabel
            //
            this.thanksLabel.AutoSize = true;
            this.thanksLabel.Margin = new System.Windows.Forms.Padding(0, 10, 0, 2);
            this.thanksLabel.Name = "thanksLabel";
            this.thanksLabel.TabIndex = 6;
            this.thanksLabel.Text = "Thanks to Koei Tecmo, for releasing Monster Rancher 1 & 2 DX,\r\nand to the Monster Rancher community.";
            this.thanksLabel.UseMnemonic = false;
            //
            // notAffiliatedLabel
            //
            this.notAffiliatedLabel.AutoSize = true;
            this.notAffiliatedLabel.Margin = new System.Windows.Forms.Padding(0, 10, 0, 2);
            this.notAffiliatedLabel.Name = "notAffiliatedLabel";
            this.notAffiliatedLabel.TabIndex = 7;
            this.notAffiliatedLabel.Text = "Not affiliated with or endorsed by Koei Tecmo.\r\nMonster Rancher and Mocchi are theirs.";
            //
            // enjoyLabel
            //
            this.enjoyLabel.AutoSize = true;
            this.enjoyLabel.Margin = new System.Windows.Forms.Padding(0, 10, 0, 2);
            this.enjoyLabel.Name = "enjoyLabel";
            this.enjoyLabel.TabIndex = 8;
            this.enjoyLabel.Text = "Enjoy!";
            //
            // okButton
            //
            this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 9;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            //
            // AboutBox
            //
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.okButton;
            this.ClientSize = new System.Drawing.Size(432, 284);
            this.Controls.Add(this.mascotBox);
            this.Controls.Add(this.textColumn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutBox";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            ((System.ComponentModel.ISupportInitialize)(this.mascotBox)).EndInit();
            this.textColumn.ResumeLayout(false);
            this.textColumn.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.PictureBox mascotBox;
        private System.Windows.Forms.FlowLayoutPanel textColumn;
        private System.Windows.Forms.Label nameAndVersionLabel;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.Label copyrightLabel;
        private System.Windows.Forms.LinkLabel helpLink;
        private System.Windows.Forms.LinkLabel supportLink;
        private System.Windows.Forms.LinkLabel steamLink;
        private System.Windows.Forms.Label thanksLabel;
        private System.Windows.Forms.Label notAffiliatedLabel;
        private System.Windows.Forms.Label enjoyLabel;
        private System.Windows.Forms.Button okButton;
    }
}
