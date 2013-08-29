using System;
using System.Windows.Forms;

namespace EmpireUpdate
{
    internal partial class EmpireUpdateInfoForm : Form
    {
        internal EmpireUpdateInfoForm(IEmpireUpdate appInfo, EmpireUpdateXml updateInfo)
        {
            InitializeComponent();

            if (appInfo.AppIcon != null)
                this.Icon = appInfo.AppIcon;

            this.Text = appInfo.AppName + " - Update Info";
            this.lblVersions.Text = String.Format("Current Version: {0}\nUpdate Version: {1}", appInfo.AppAssembly.GetName().Version.ToString(), 
                updateInfo.Version.ToString());
            this.txtDescrition.Text = updateInfo.Description;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtDescrition_KeyDown(object sender, KeyEventArgs e)
        {
            if (!(e.Control && e.KeyCode == Keys.C))
                e.SuppressKeyPress = true;
        }
    }
}
