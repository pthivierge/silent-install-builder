namespace SilentPackagesBuilder.Views.PackageEditor.Editors
{
    partial class PackageEditorDemo
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
            this.packageEditorControl1 = new SilentPackagesBuilder.Views.PackageEditorControl();
            this.SuspendLayout();
            // 
            // packagesListControl1
            // 
            this.packagesListControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.packagesListControl1.Location = new System.Drawing.Point(12, 391);
            this.packagesListControl1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.packagesListControl1.Name = "packagesListControl1";
            this.packagesListControl1.Size = new System.Drawing.Size(1175, 219);
            this.packagesListControl1.TabIndex = 1;
            // 
            // packageEditorControl1
            // 
            this.packageEditorControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.packageEditorControl1.BackColor = System.Drawing.SystemColors.Control;
            this.packageEditorControl1.Location = new System.Drawing.Point(16, 15);
            this.packageEditorControl1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.packageEditorControl1.Name = "packageEditorControl1";
            this.packageEditorControl1.Size = new System.Drawing.Size(1171, 363);
            this.packageEditorControl1.TabIndex = 0;
            // 
            // PackageEditorDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1203, 618);
            this.Controls.Add(this.packagesListControl1);
            this.Controls.Add(this.packageEditorControl1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "PackageEditorDemo";
            this.Text = "PackageEditorDemo";
            this.Load += new System.EventHandler(this.PackageEditorDemo_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private PackageEditorControl packageEditorControl1;
        private PackagesListControl packagesListControl1;
    }
}