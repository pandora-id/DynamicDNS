
namespace DynamicDNS
{
    partial class ProjectInstaller
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
            this.dynamicDnsServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.dynamicDnsServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // dynamicDnsServiceProcessInstaller
            // 
            this.dynamicDnsServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.dynamicDnsServiceProcessInstaller.Password = null;
            this.dynamicDnsServiceProcessInstaller.Username = null;
            // 
            // dynamicDnsServiceInstaller
            // 
            this.dynamicDnsServiceInstaller.Description = "Trigger a scheduled DNS Record update for specified DNS Server zone.";
            this.dynamicDnsServiceInstaller.DisplayName = "Dynamic DNS Agent Service";
            this.dynamicDnsServiceInstaller.ServiceName = "DynamicDNSService";
            this.dynamicDnsServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.dynamicDnsServiceProcessInstaller,
            this.dynamicDnsServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller dynamicDnsServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller dynamicDnsServiceInstaller;
    }
}