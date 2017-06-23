namespace SilentPackagesBuilder.Views.PackagesList
{
    partial class PackagesListDemo
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
            this.packagesListControl1 = new SilentPackagesBuilder.Views.PackagesListControl();
            this.SuspendLayout();
            // 
            // packagesListControl1
            // 
            this.packagesListControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.packagesListControl1.Location = new System.Drawing.Point(-1, 1);
            this.packagesListControl1.Name = "packagesListControl1";
            this.packagesListControl1.Size = new System.Drawing.Size(903, 274);
            this.packagesListControl1.TabIndex = 0;
            // 
            // PackagesListDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 370);
            this.Controls.Add(this.packagesListControl1);
            this.Name = "PackagesListDemo";
            this.Text = "PackagesListDemo";
            this.Load += new System.EventHandler(this.PackagesListDemo_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private PackagesListControl packagesListControl1;
    }
}