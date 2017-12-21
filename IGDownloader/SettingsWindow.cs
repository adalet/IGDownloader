using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace IGDownloader
{
    public partial class SettingsWindow : Form
    {
        public SettingsWindow()
        {
            InitializeComponent();
            savePath.Text = (string)Properties.Settings.Default["SaveLocation"];
            templatePattern.Text = (string)Properties.Settings.Default["FilenameTemplate"];
        }

        private void templatePattern_Enter(object sender, EventArgs e)
        {
            string toolTipText = "Template Codes:" +
                "\n{id}" +
                "\n{date}";

            templateToolTip.ShowAlways = true;
            templateToolTip.Show(toolTipText, templatePattern, 0, 26, 5000);
        }

        private void openSaveDialogueButton_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {     
                savePath.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private bool ParseFilenameTemplate()
        {
            string temp = templatePattern.Text;
            if (string.IsNullOrWhiteSpace(temp))
            {
                MessageBox.Show("Error, the filename template you used is invalid.");
            }
            else
            {
                Properties.Settings.Default["FilenameTemplate"] = templatePattern.Text;
                return true;
            }
            return false;
        }

        private void confirmButton_Click(object sender, EventArgs e)
        {
            
            if(!Directory.Exists(savePath.Text))
            {
                MessageBox.Show("Error, the path you selected does not exist.");
            }
            else if(!ParseFilenameTemplate())
            {

            }
            else
            {
                Properties.Settings.Default["SaveLocation"] = savePath.Text;
                Properties.Settings.Default.Save();
                this.Close();
            }
        }
    }
}
