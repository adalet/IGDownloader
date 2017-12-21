namespace IGDownloader
{
    partial class SettingsWindow
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsWindow));
            this.openSaveDialogueButton = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.savePath = new System.Windows.Forms.TextBox();
            this.confirmButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.templatePattern = new System.Windows.Forms.TextBox();
            this.templateToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // openSaveDialogueButton
            // 
            this.openSaveDialogueButton.Location = new System.Drawing.Point(12, 17);
            this.openSaveDialogueButton.Name = "openSaveDialogueButton";
            this.openSaveDialogueButton.Size = new System.Drawing.Size(100, 22);
            this.openSaveDialogueButton.TabIndex = 11;
            this.openSaveDialogueButton.Text = "Save Directory";
            this.openSaveDialogueButton.UseVisualStyleBackColor = true;
            this.openSaveDialogueButton.Click += new System.EventHandler(this.openSaveDialogueButton_Click);
            // 
            // savePath
            // 
            this.savePath.Location = new System.Drawing.Point(118, 17);
            this.savePath.Name = "savePath";
            this.savePath.Size = new System.Drawing.Size(287, 22);
            this.savePath.TabIndex = 12;
            // 
            // confirmButton
            // 
            this.confirmButton.Location = new System.Drawing.Point(331, 94);
            this.confirmButton.Name = "confirmButton";
            this.confirmButton.Size = new System.Drawing.Size(75, 23);
            this.confirmButton.TabIndex = 13;
            this.confirmButton.Text = "Confirm";
            this.confirmButton.UseVisualStyleBackColor = true;
            this.confirmButton.Click += new System.EventHandler(this.confirmButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Filename Template";
            // 
            // templatePattern
            // 
            this.templatePattern.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.templatePattern.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.templatePattern.Location = new System.Drawing.Point(118, 54);
            this.templatePattern.MaxLength = 100;
            this.templatePattern.Name = "templatePattern";
            this.templatePattern.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.templatePattern.Size = new System.Drawing.Size(287, 22);
            this.templatePattern.TabIndex = 15;
            this.templatePattern.Enter += new System.EventHandler(this.templatePattern_Enter);
            // 
            // SettingsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 129);
            this.Controls.Add(this.templatePattern);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.confirmButton);
            this.Controls.Add(this.savePath);
            this.Controls.Add(this.openSaveDialogueButton);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsWindow";
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button openSaveDialogueButton;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.TextBox savePath;
        private System.Windows.Forms.Button confirmButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox templatePattern;
        private System.Windows.Forms.ToolTip templateToolTip;
    }
}