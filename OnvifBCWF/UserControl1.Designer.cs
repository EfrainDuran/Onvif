namespace OnvifBCWF
{
    partial class UserControl1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl1));
            this.panel1 = new System.Windows.Forms.Panel();
            this.listBox = new System.Windows.Forms.ListBox();
            this.vlcCamera = new Vlc.DotNet.Forms.VlcControl();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vlcCamera)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.listBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 370);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(428, 25);
            this.panel1.TabIndex = 12;
            this.panel1.Visible = false;
            // 
            // listBox
            // 
            this.listBox.FormattingEnabled = true;
            this.listBox.Location = new System.Drawing.Point(3, 3);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(120, 69);
            this.listBox.TabIndex = 1;
            // 
            // vlcCamera
            // 
            this.vlcCamera.BackColor = System.Drawing.Color.Black;
            this.vlcCamera.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vlcCamera.Location = new System.Drawing.Point(0, 0);
            this.vlcCamera.Name = "vlcCamera";
            this.vlcCamera.Size = new System.Drawing.Size(428, 395);
            this.vlcCamera.Spu = -1;
            this.vlcCamera.TabIndex = 11;
            this.vlcCamera.Text = "vlcControl1";
            this.vlcCamera.VlcLibDirectory = ((System.IO.DirectoryInfo)(resources.GetObject("vlcCamera.VlcLibDirectory")));
            this.vlcCamera.VlcMediaplayerOptions = null;
            // 
            // UserControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.vlcCamera);
            this.Name = "UserControl1";
            this.Size = new System.Drawing.Size(428, 395);
            this.Load += new System.EventHandler(this.UserControl1_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.vlcCamera)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox listBox;
        private Vlc.DotNet.Forms.VlcControl vlcCamera;
    }
}
