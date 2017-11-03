namespace AccountDemo
{
    partial class FormCloese
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCloese));
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.ISExit = new System.Windows.Forms.RadioButton();
            this.ISNOExit = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(139, 89);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "取消";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(45, 89);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "确定";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ISExit
            // 
            this.ISExit.AutoSize = true;
            this.ISExit.Checked = true;
            this.ISExit.Location = new System.Drawing.Point(60, 60);
            this.ISExit.Name = "ISExit";
            this.ISExit.Size = new System.Drawing.Size(71, 16);
            this.ISExit.TabIndex = 7;
            this.ISExit.TabStop = true;
            this.ISExit.Text = "退出程序";
            this.ISExit.UseVisualStyleBackColor = true;
            // 
            // ISNOExit
            // 
            this.ISNOExit.AutoSize = true;
            this.ISNOExit.Location = new System.Drawing.Point(60, 37);
            this.ISNOExit.Name = "ISNOExit";
            this.ISNOExit.Size = new System.Drawing.Size(191, 16);
            this.ISNOExit.TabIndex = 6;
            this.ISNOExit.TabStop = true;
            this.ISNOExit.Text = "最小化到系统托盘，不退出程序";
            this.ISNOExit.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "您点击了关闭按钮，您是想：";
            // 
            // FormCloese
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(279, 121);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ISExit);
            this.Controls.Add(this.ISNOExit);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormCloese";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "关闭窗体";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton ISExit;
        private System.Windows.Forms.RadioButton ISNOExit;
        private System.Windows.Forms.Label label1;
    }
}