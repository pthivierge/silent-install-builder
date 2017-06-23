namespace SilentPackagesBuilder
{
    partial class frmDefineVariables
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
            this.variablesDefinitionControl1 = new SilentPackagesBuilder.Views.DefineVariables.VariablesDefinitionControl();
            this.SuspendLayout();
            // 
            // variablesDefinitionControl1
            // 
            this.variablesDefinitionControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.variablesDefinitionControl1.Location = new System.Drawing.Point(9, 12);
            this.variablesDefinitionControl1.Name = "variablesDefinitionControl1";
            this.variablesDefinitionControl1.Size = new System.Drawing.Size(634, 335);
            this.variablesDefinitionControl1.TabIndex = 0;
            // 
            // frmDefineVariables
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(655, 371);
            this.Controls.Add(this.variablesDefinitionControl1);
            this.Name = "frmDefineVariables";
            this.Text = "Define Variables";
            this.ResumeLayout(false);

        }

        #endregion

        private Views.DefineVariables.VariablesDefinitionControl variablesDefinitionControl1;
    }
}