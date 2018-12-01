namespace TankWar
{
    partial class F_Main
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tm_main = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // tm_main
            // 
            this.tm_main.Interval = 50;
            this.tm_main.Tick += new System.EventHandler(this.tm_main_Tick);
            // 
            // F_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlText;
            this.ClientSize = new System.Drawing.Size(1040, 751);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "F_Main";
            this.Text = "TankWar";
            this.Load += new System.EventHandler(this.F_Main_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.F_Main_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.F_Main_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.F_Main_KeyUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer tm_main;
    }
}

