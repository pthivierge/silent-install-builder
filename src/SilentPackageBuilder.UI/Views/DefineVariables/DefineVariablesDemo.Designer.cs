namespace SilentPackagesBuilder.Views.DefineVariables
{
    partial class DefineVariablesDemo
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
            this.variablesDefinitionControl1.Location = new System.Drawing.Point(31, 30);
            this.variablesDefinitionControl1.Name = "variablesDefinitionControl1";
            this.variablesDefinitionControl1.Size = new System.Drawing.Size(603, 280);
            this.variablesDefinitionControl1.TabIndex = 0;
            // 
            // DefineVariablesDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(702, 364);
            this.Controls.Add(this.variablesDefinitionControl1);
            this.Name = "DefineVariablesDemo";
            this.Text = "DefineVariablesDemo";
            this.ResumeLayout(false);

        }

        #endregion

        private VariablesDefinitionControl variablesDefinitionControl1;
    }
}