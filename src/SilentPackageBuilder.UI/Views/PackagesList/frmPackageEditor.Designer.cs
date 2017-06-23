namespace SilentPackagesBuilder
{
    partial class frmPackageEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPackageEditor));
            this.packageEditorControl1 = new SilentPackagesBuilder.Views.PackageEditorControl();
            this.SuspendLayout();
            // 
            // packageEditorControl1
            // 
            this.packageEditorControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.packageEditorControl1.Location = new System.Drawing.Point(-1, 15);
            this.packageEditorControl1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.packageEditorControl1.Name = "packageEditorControl1";
            this.packageEditorControl1.Size = new System.Drawing.Size(1031, 473);
            this.packageEditorControl1.TabIndex = 0;
            // 
            // frmPackageEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1031, 490);
            this.Controls.Add(this.packageEditorControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frmPackageEditor";
            this.Text = "Package Editor";
            this.Load += new System.EventHandler(this.frmPackageEditor_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Views.PackageEditorControl packageEditorControl1;
    }
}