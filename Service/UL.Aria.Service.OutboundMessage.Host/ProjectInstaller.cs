using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace UL.Aria.Service.OutboundMessage.Host
{
    /// <summary>
    /// Installer for host components.
    /// </summary>
    [RunInstaller(true)]
    [ExcludeFromCodeCoverage]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectInstaller"/> class.
        /// </summary>
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        private void serviceProcessInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {

        }
    }
}
