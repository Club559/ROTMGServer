using System;
using System.Windows.Forms;

namespace EmpireUpdate
{
    internal partial class EmpireUpdateAcceptForm : Form
    {
        private IEmpireUpdate appInfo;

        private EmpireUpdateXml updateInfo;

        private EmpireUpdateInfoForm updateInfoForm;

        internal EmpireUpdateAcceptForm(IEmpireUpdate appInfo, EmpireUpdateXml updateInfo)
        {
            InitializeComponent();

            this.appInfo = appInfo;
            this.updateInfo = updateInfo;

            this.Text = this.appInfo.AppName + " - Update Available";

            if (this.appInfo.AppIcon != null)
                this.Icon = this.appInfo.AppIcon;

            this.lblNewVersion.Text = string.Format("New Version: {0}", this.updateInfo.Version.ToString());
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void btnDetails_Click(object sender, EventArgs e)
        {
            if (this.updateInfoForm == null)
                this.updateInfoForm = new EmpireUpdateInfoForm(this.appInfo, this.updateInfo);

            this.updateInfoForm.ShowDialog(this);
        }
    }
}
