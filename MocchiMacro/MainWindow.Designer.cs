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
            this.typingRateLabel = new System.Windows.Forms.Label();
            this.typingRateBox = new System.Windows.Forms.NumericUpDown();
            this.typingRateUnitLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.typingRateBox)).BeginInit();
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
            // typingRateLabel
            //
            this.typingRateLabel.AutoSize = true;
            this.typingRateLabel.Location = new System.Drawing.Point(12, 45);
            this.typingRateLabel.Name = "typingRateLabel";
            this.typingRateLabel.Size = new System.Drawing.Size(64, 13);
            this.typingRateLabel.TabIndex = 3;
            this.typingRateLabel.Text = "Typing rate:";
            //
            // typingRateBox
            //
            this.typingRateBox.Location = new System.Drawing.Point(84, 43);
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
            this.typingRateBox.Size = new System.Drawing.Size(45, 20);
            this.typingRateBox.TabIndex = 4;
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
            this.typingRateUnitLabel.Location = new System.Drawing.Point(135, 45);
            this.typingRateUnitLabel.Name = "typingRateUnitLabel";
            this.typingRateUnitLabel.Size = new System.Drawing.Size(82, 13);
            this.typingRateUnitLabel.TabIndex = 5;
            this.typingRateUnitLabel.Text = "keys per second";
            //
            // MainWindow
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 75);
            this.Controls.Add(this.searchTextLabel);
            this.Controls.Add(this.searchTextBox);
            this.Controls.Add(this.goButton);
            this.Controls.Add(this.typingRateLabel);
            this.Controls.Add(this.typingRateBox);
            this.Controls.Add(this.typingRateUnitLabel);
            this.Name = "MainWindow";
            this.Text = "Mocchi Macro";
            ((System.ComponentModel.ISupportInitialize)(this.typingRateBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label searchTextLabel;
        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.Button goButton;
        private System.Windows.Forms.Label typingRateLabel;
        private System.Windows.Forms.NumericUpDown typingRateBox;
        private System.Windows.Forms.Label typingRateUnitLabel;
    }
}
