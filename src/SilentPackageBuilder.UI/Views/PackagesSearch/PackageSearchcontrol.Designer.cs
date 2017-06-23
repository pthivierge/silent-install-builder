

namespace SilentPackagesBuilder.Views
{
    partial class PackageSearchControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gridPackagesSearch = new System.Windows.Forms.DataGridView();
            this.pictureBox7 = new System.Windows.Forms.PictureBox();
            this.btnScanPackages = new System.Windows.Forms.Button();
            this.btnOpenSearchDirDialog = new System.Windows.Forms.Button();
            this.btnCancelSearch = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPackagesDir = new System.Windows.Forms.TextBox();
            this.txtfileNameFilter = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusFilesCount = new SilentPackagesBuilder.Components.BindableToolStripStatusLabel();
            this.statusSearchInProgress = new SilentPackagesBuilder.Components.BindableToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.gridPackagesSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridPackagesSearch
            // 
            this.gridPackagesSearch.AllowUserToAddRows = false;
            this.gridPackagesSearch.AllowUserToDeleteRows = false;
            this.gridPackagesSearch.AllowUserToOrderColumns = true;
            this.gridPackagesSearch.AllowUserToResizeRows = false;
            this.gridPackagesSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridPackagesSearch.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridPackagesSearch.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.gridPackagesSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridPackagesSearch.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.gridPackagesSearch.Location = new System.Drawing.Point(3, 97);
            this.gridPackagesSearch.Margin = new System.Windows.Forms.Padding(4);
            this.gridPackagesSearch.MultiSelect = false;
            this.gridPackagesSearch.Name = "gridPackagesSearch";
            this.gridPackagesSearch.ReadOnly = true;
            this.gridPackagesSearch.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridPackagesSearch.Size = new System.Drawing.Size(609, 184);
            this.gridPackagesSearch.TabIndex = 59;
            // 
            // pictureBox7
            // 
            this.pictureBox7.Image = global::SilentPackagesBuilder.Properties.Resources.help;
            this.pictureBox7.InitialImage = global::SilentPackagesBuilder.Properties.Resources.help;
            this.pictureBox7.Location = new System.Drawing.Point(587, 27);
            this.pictureBox7.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox7.Name = "pictureBox7";
            this.pictureBox7.Size = new System.Drawing.Size(21, 20);
            this.pictureBox7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox7.TabIndex = 58;
            this.pictureBox7.TabStop = false;
            // 
            // btnScanPackages
            // 
            this.btnScanPackages.Location = new System.Drawing.Point(392, 29);
            this.btnScanPackages.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnScanPackages.Name = "btnScanPackages";
            this.btnScanPackages.Size = new System.Drawing.Size(91, 30);
            this.btnScanPackages.TabIndex = 56;
            this.btnScanPackages.Text = "Search";
            this.btnScanPackages.UseVisualStyleBackColor = true;
            // 
            // btnOpenSearchDirDialog
            // 
            this.btnOpenSearchDirDialog.Location = new System.Drawing.Point(350, 29);
            this.btnOpenSearchDirDialog.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnOpenSearchDirDialog.Name = "btnOpenSearchDirDialog";
            this.btnOpenSearchDirDialog.Size = new System.Drawing.Size(36, 30);
            this.btnOpenSearchDirDialog.TabIndex = 55;
            this.btnOpenSearchDirDialog.Text = "...";
            this.btnOpenSearchDirDialog.UseVisualStyleBackColor = true;
            this.btnOpenSearchDirDialog.Click += new System.EventHandler(this.btnOpenSearchDirDialog_Click);
            // 
            // btnCancelSearch
            // 
            this.btnCancelSearch.Location = new System.Drawing.Point(489, 29);
            this.btnCancelSearch.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCancelSearch.Name = "btnCancelSearch";
            this.btnCancelSearch.Size = new System.Drawing.Size(91, 30);
            this.btnCancelSearch.TabIndex = 57;
            this.btnCancelSearch.Text = "Cancel";
            this.btnCancelSearch.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(2, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 53;
            this.label1.Text = "Directory to search";
            // 
            // txtPackagesDir
            // 
            this.txtPackagesDir.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPackagesDir.Location = new System.Drawing.Point(5, 35);
            this.txtPackagesDir.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtPackagesDir.Name = "txtPackagesDir";
            this.txtPackagesDir.Size = new System.Drawing.Size(339, 19);
            this.txtPackagesDir.TabIndex = 54;
            this.txtPackagesDir.Text = "F:\\02 Programmes\\OSIsoft";
            // 
            // txtfileNameFilter
            // 
            this.txtfileNameFilter.Location = new System.Drawing.Point(5, 74);
            this.txtfileNameFilter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtfileNameFilter.Name = "txtfileNameFilter";
            this.txtfileNameFilter.Size = new System.Drawing.Size(339, 19);
            this.txtfileNameFilter.TabIndex = 51;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 50;
            this.label2.Text = "Filter by Name";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel2,
            this.statusFilesCount,
            this.statusSearchInProgress});
            this.statusStrip1.Location = new System.Drawing.Point(0, 281);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(617, 22);
            this.statusStrip1.TabIndex = 60;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(70, 17);
            this.toolStripStatusLabel2.Text = "Files Found:";
            // 
            // statusFilesCount
            // 
            this.statusFilesCount.Name = "statusFilesCount";
            this.statusFilesCount.Size = new System.Drawing.Size(13, 17);
            this.statusFilesCount.Text = "0";
            // 
            // statusSearchInProgress
            // 
            this.statusSearchInProgress.Name = "statusSearchInProgress";
            this.statusSearchInProgress.Size = new System.Drawing.Size(112, 17);
            this.statusSearchInProgress.Text = "Search in progress...";
            this.statusSearchInProgress.Visible = false;
            // 
            // PackageSearchControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.gridPackagesSearch);
            this.Controls.Add(this.pictureBox7);
            this.Controls.Add(this.btnScanPackages);
            this.Controls.Add(this.btnOpenSearchDirDialog);
            this.Controls.Add(this.btnCancelSearch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPackagesDir);
            this.Controls.Add(this.txtfileNameFilter);
            this.Controls.Add(this.label2);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PackageSearchControl";
            this.Size = new System.Drawing.Size(617, 303);
            ((System.ComponentModel.ISupportInitialize)(this.gridPackagesSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView gridPackagesSearch;
        private System.Windows.Forms.PictureBox pictureBox7;
        private System.Windows.Forms.Button btnScanPackages;
        private System.Windows.Forms.Button btnOpenSearchDirDialog;
        private System.Windows.Forms.Button btnCancelSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPackagesDir;
        private System.Windows.Forms.TextBox txtfileNameFilter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private Components.BindableToolStripStatusLabel statusFilesCount;
        private Components.BindableToolStripStatusLabel statusSearchInProgress;
    }
}
