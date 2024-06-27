namespace express_patt
{
    partial class Form2
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
            this.resulTextBox = new System.Windows.Forms.RichTextBox();
            this.loaddbutton = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.button6 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // resulTextBox
            // 
            this.resulTextBox.Location = new System.Drawing.Point(36, 12);
            this.resulTextBox.Name = "resulTextBox";
            this.resulTextBox.Size = new System.Drawing.Size(277, 500);
            this.resulTextBox.TabIndex = 2;
            this.resulTextBox.Text = "";
            // 
            // loaddbutton
            // 
            this.loaddbutton.Location = new System.Drawing.Point(354, 27);
            this.loaddbutton.Name = "loaddbutton";
            this.loaddbutton.Size = new System.Drawing.Size(75, 23);
            this.loaddbutton.TabIndex = 6;
            this.loaddbutton.Text = "load data";
            this.loaddbutton.UseVisualStyleBackColor = true;
            this.loaddbutton.Click += new System.EventHandler(this.loaddbutton_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog1";
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(332, 109);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(139, 23);
            this.button6.TabIndex = 20;
            this.button6.Text = "mine";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(758, 622);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.loaddbutton);
            this.Controls.Add(this.resulTextBox);
            this.Name = "Form2";
            this.Text = "DatapreForm";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.RichTextBox resulTextBox;
        private System.Windows.Forms.Button loaddbutton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.Button button6;
    }
}