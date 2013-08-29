using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace EmpireUpdate
{
    public class EmpireUpdater
    {
        private IEmpireUpdate appInfo;
        private BackgroundWorker bgWorker;

        public EmpireUpdater(IEmpireUpdate appInfo)
        {
            this.appInfo = appInfo;

            this.bgWorker = new BackgroundWorker();
            this.bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
            this.bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);
        }

        public void DoUpdate()
        {
            if (!this.bgWorker.IsBusy)
                this.bgWorker.RunWorkerAsync(this.appInfo);
        }

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            IEmpireUpdate app = (IEmpireUpdate)e.Argument;

            if (!EmpireUpdateXml.ExistsOnServer(app.UpdateXmlLoc))
                e.Cancel = true;
            else
                e.Result = EmpireUpdateXml.Parse(app.UpdateXmlLoc, app.AppID);
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                EmpireUpdateXml update = (EmpireUpdateXml)e.Result;

                if (update != null && update.isNewerThan(this.appInfo.AppAssembly.GetName().Version))
                {
                    if (new EmpireUpdateAcceptForm(this.appInfo, update).ShowDialog(this.appInfo.Context) == DialogResult.Yes)
                        this.DownloadUpdate(update);
                }
            }
        }

        private void DownloadUpdate(EmpireUpdateXml update)
        {
            EmpireUpdateDownloadForm form = new EmpireUpdateDownloadForm(update.Uri, update.MD5, this.appInfo.AppIcon);
            DialogResult result = form.ShowDialog(this.appInfo.Context);

            if (result == DialogResult.OK)
            {
                string currentPath = this.appInfo.AppAssembly.Location;
                string newPath = Path.GetDirectoryName(currentPath) + "\\" + update.FileName;

                UpdateApplication(form.TempFilePath, currentPath, newPath, update.LaunchArgs);

                Application.Exit();
            }
            else if (result == DialogResult.Abort)
            {
                MessageBox.Show("The update download was cancelled.\nTHisProgram has not been modified.", "Update Download Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("There was an error with the update, Please try again later.", "Update Download Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void UpdateApplication(string tempFilePath, string currentPath, string newPath, string launchArgs)
        {
            string argument = "/C choice /C Y /N /D Y /T 4 & Del /F /Q \"{0}\" & choice /C Y /N /D Y /T 2 & Move /Y \"{1}\" \"{2}\" & Start \"\" /D \"{3}\" \"{4}\" {5}";

            ProcessStartInfo Info = new ProcessStartInfo();
            Info.Arguments = String.Format(argument, currentPath, tempFilePath, newPath, Path.GetDirectoryName(newPath), Path.GetFileName(newPath), launchArgs);
            Info.WindowStyle = ProcessWindowStyle.Hidden;
            Info.CreateNoWindow = true;
            Info.FileName = "cmd.exe";
            Process.Start(Info);
        }
    }
}
