namespace IGDownloader
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.listBox = new System.Windows.Forms.CheckedListBox();
            this.loginButton = new System.Windows.Forms.Button();
            this.usernameBox = new System.Windows.Forms.TextBox();
            this.passwordBox = new System.Windows.Forms.TextBox();
            this.fetchButton = new System.Windows.Forms.Button();
            this.downloadButton = new System.Windows.Forms.Button();
            this.consoleBox = new IGDownloader.ExRichTextBox();
            this.fetchFollowingCheckBox = new System.Windows.Forms.CheckBox();
            this.fetchLocalCheckBox = new System.Windows.Forms.CheckBox();
            this.loggedInStatus = new System.Windows.Forms.Label();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.settingsButton = new System.Windows.Forms.Button();
            this.downloadThread = new System.ComponentModel.BackgroundWorker();
            this.selectAllCheck = new System.Windows.Forms.CheckBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SuspendLayout();
            // 
            // listBox
            // 
            this.listBox.CheckOnClick = true;
            this.listBox.FormattingEnabled = true;
            this.listBox.Location = new System.Drawing.Point(7, 45);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(210, 208);
            this.listBox.TabIndex = 0;
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(261, 17);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(100, 23);
            this.loginButton.TabIndex = 1;
            this.loginButton.Text = "Login";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // usernameBox
            // 
            this.usernameBox.Location = new System.Drawing.Point(7, 19);
            this.usernameBox.Name = "usernameBox";
            this.usernameBox.Size = new System.Drawing.Size(121, 22);
            this.usernameBox.TabIndex = 2;
            this.usernameBox.Text = "Username";
            // 
            // passwordBox
            // 
            this.passwordBox.Location = new System.Drawing.Point(134, 19);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.Size = new System.Drawing.Size(121, 22);
            this.passwordBox.TabIndex = 3;
            this.passwordBox.Text = "Password";
            this.passwordBox.UseSystemPasswordChar = true;
            // 
            // fetchButton
            // 
            this.fetchButton.Location = new System.Drawing.Point(7, 283);
            this.fetchButton.Name = "fetchButton";
            this.fetchButton.Size = new System.Drawing.Size(100, 23);
            this.fetchButton.TabIndex = 4;
            this.fetchButton.Text = "Fetch";
            this.fetchButton.UseVisualStyleBackColor = true;
            this.fetchButton.Click += new System.EventHandler(this.fetchButton_Click);
            // 
            // downloadButton
            // 
            this.downloadButton.Location = new System.Drawing.Point(117, 283);
            this.downloadButton.Name = "downloadButton";
            this.downloadButton.Size = new System.Drawing.Size(100, 23);
            this.downloadButton.TabIndex = 5;
            this.downloadButton.Text = "Download";
            this.downloadButton.UseVisualStyleBackColor = true;
            this.downloadButton.Click += new System.EventHandler(this.downloadButton_Click);
            // 
            // consoleBox
            // 
            this.consoleBox.Location = new System.Drawing.Point(223, 45);
            this.consoleBox.Name = "consoleBox";
            this.consoleBox.ReadOnly = true;
            this.consoleBox.Size = new System.Drawing.Size(469, 309);
            this.consoleBox.TabIndex = 6;
            this.consoleBox.TabStop = false;
            this.consoleBox.Text = "";
            // 
            // fetchFollowingCheckBox
            // 
            this.fetchFollowingCheckBox.AutoSize = true;
            this.fetchFollowingCheckBox.Location = new System.Drawing.Point(7, 312);
            this.fetchFollowingCheckBox.Name = "fetchFollowingCheckBox";
            this.fetchFollowingCheckBox.Size = new System.Drawing.Size(156, 17);
            this.fetchFollowingCheckBox.TabIndex = 7;
            this.fetchFollowingCheckBox.Text = "Fetch account followings";
            this.fetchFollowingCheckBox.UseVisualStyleBackColor = true;
            // 
            // fetchLocalCheckBox
            // 
            this.fetchLocalCheckBox.AutoSize = true;
            this.fetchLocalCheckBox.Location = new System.Drawing.Point(7, 335);
            this.fetchLocalCheckBox.Name = "fetchLocalCheckBox";
            this.fetchLocalCheckBox.Size = new System.Drawing.Size(127, 17);
            this.fetchLocalCheckBox.TabIndex = 8;
            this.fetchLocalCheckBox.Text = "Fetch from local file";
            this.fetchLocalCheckBox.UseVisualStyleBackColor = true;
            // 
            // loggedInStatus
            // 
            this.loggedInStatus.AutoSize = true;
            this.loggedInStatus.Location = new System.Drawing.Point(367, 22);
            this.loggedInStatus.Name = "loggedInStatus";
            this.loggedInStatus.Size = new System.Drawing.Size(117, 13);
            this.loggedInStatus.TabIndex = 9;
            this.loggedInStatus.Text = "Status: Not logged in";
            // 
            // settingsButton
            // 
            this.settingsButton.Location = new System.Drawing.Point(617, 17);
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new System.Drawing.Size(75, 24);
            this.settingsButton.TabIndex = 13;
            this.settingsButton.Text = "Settings";
            this.settingsButton.UseVisualStyleBackColor = true;
            this.settingsButton.Click += new System.EventHandler(this.settingsButton_Click);
            // 
            // downloadThread
            // 
            this.downloadThread.DoWork += new System.ComponentModel.DoWorkEventHandler(this.downloadThread_DoWork);
            this.downloadThread.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.downloadThread_RunWorkerCompleted);
            // 
            // selectAllCheck
            // 
            this.selectAllCheck.AutoSize = true;
            this.selectAllCheck.Location = new System.Drawing.Point(7, 260);
            this.selectAllCheck.Name = "selectAllCheck";
            this.selectAllCheck.Size = new System.Drawing.Size(72, 17);
            this.selectAllCheck.TabIndex = 14;
            this.selectAllCheck.Text = "Select All";
            this.selectAllCheck.UseVisualStyleBackColor = true;
            this.selectAllCheck.Visible = false;
            this.selectAllCheck.CheckStateChanged += new System.EventHandler(this.selectAllCheck_CheckStateChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 366);
            this.Controls.Add(this.consoleBox);
            this.Controls.Add(this.selectAllCheck);
            this.Controls.Add(this.settingsButton);
            this.Controls.Add(this.loggedInStatus);
            this.Controls.Add(this.fetchLocalCheckBox);
            this.Controls.Add(this.fetchFollowingCheckBox);
            this.Controls.Add(this.downloadButton);
            this.Controls.Add(this.fetchButton);
            this.Controls.Add(this.passwordBox);
            this.Controls.Add(this.usernameBox);
            this.Controls.Add(this.loginButton);
            this.Controls.Add(this.listBox);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Main";
            this.Text = "Instagram Image Downloader 0.2.1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox listBox;
        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.TextBox usernameBox;
        private System.Windows.Forms.TextBox passwordBox;
        private System.Windows.Forms.Button fetchButton;
        private System.Windows.Forms.Button downloadButton;
        private System.Windows.Forms.CheckBox fetchFollowingCheckBox;
        private System.Windows.Forms.CheckBox fetchLocalCheckBox;
        private System.Windows.Forms.Label loggedInStatus;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Button settingsButton;
        private System.ComponentModel.BackgroundWorker downloadThread;
        private System.Windows.Forms.CheckBox selectAllCheck;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private ExRichTextBox consoleBox;
    }
}

