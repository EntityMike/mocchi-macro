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
            this.searchTextLabel = new System.Windows.Forms.Label();
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.goButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // searchTextLabel
            //
            this.searchTextLabel.AutoSize = true;
            this.searchTextLabel.Location = new System.Drawing.Point(12, 15);
            this.searchTextLabel.Name = "searchTextLabel";
            this.searchTextLabel.Size = new System.Drawing.Size(66, 13);
            this.searchTextLabel.TabIndex = 0;
            this.searchTextLabel.Text = "Search text:";
            //
            // searchTextBox
            //
            this.searchTextBox.Location = new System.Drawing.Point(84, 12);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(120, 20);
            this.searchTextBox.TabIndex = 1;
            //
            // goButton
            //
            this.goButton.Location = new System.Drawing.Point(210, 10);
            this.goButton.Name = "goButton";
            this.goButton.Size = new System.Drawing.Size(60, 23);
            this.goButton.TabIndex = 2;
            this.goButton.Text = "Go";
            this.goButton.UseVisualStyleBackColor = true;
            this.goButton.Click += new System.EventHandler(this.GoButton_Click);
            //
            // MainWindow
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 45);
            this.Controls.Add(this.searchTextLabel);
            this.Controls.Add(this.searchTextBox);
            this.Controls.Add(this.goButton);
            this.Name = "MainWindow";
            this.Text = "Mocchi Macro";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label searchTextLabel;
        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.Button goButton;
    }
}
