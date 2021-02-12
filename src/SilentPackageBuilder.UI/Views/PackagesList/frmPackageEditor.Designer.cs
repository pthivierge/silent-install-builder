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
            this.packageEditorControl1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.packageEditorControl1.Location = new System.Drawing.Point(18, 14);
            this.packageEditorControl1.Margin = new System.Windows.Forms.Padding(4);
            this.packageEditorControl1.Name = "packageEditorControl1";
            this.packageEditorControl1.Size = new System.Drawing.Size(898, 500);
            this.packageEditorControl1.TabIndex = 2;
            // 
            // frmPackageEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(935, 528);
            this.Controls.Add(this.packageEditorControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmPackageEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Package Editor";
            this.Load += new System.EventHandler(this.frmPackageEditor_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Views.PackageEditorControl packageEditorControl1;
    }
}