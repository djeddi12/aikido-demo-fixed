using System;
using System.Diagnostics;

namespace Infra.Deployment
{
    public class MigratePlatformRequest
    {
        public string DestServerName { get; set; } = "";
        public string DestServerSysAdminUsername { get; set; } = "";
        public string DestServerSysAdminPassword { get; set; } = "";
    }

    public interface IEnvironmentVersionPackage : IDisposable { string PackagePath { get; } }
    public interface IEnvironmentVersionPackageResolver
    {
        IEnvironmentVersionPackage ResolvePackage(string envName, string envVersion);
    }
    public class BootstrapperInfo
    {
        public string AssemblyPath { get; }
        public BootstrapperInfo(string packageLocalPath) { AssemblyPath = packageLocalPath; }
    }

    public class MigratePlatformTask
    {
        public Guid TaskID { get; private set; } = Guid.NewGuid();
        public MigratePlatformRequest MigrateRequest { get; set; } = new MigratePlatformRequest();
        private readonly IEnvironmentVersionPackageResolver packageResolver;
        private string result = "";

        public MigratePlatformTask(IEnvironmentVersionPackageResolver packageResolver)
        {
            this.packageResolver = packageResolver;
        }

        public void TriggerInstaller(string platformName, string destEnvName, string destEnvVersion)
        {
            result += "Triggering installer\r\n";
            using (var package = this.packageResolver.ResolvePackage(destEnvName, destEnvVersion))
            {
                var packageLocalPath = package.PackagePath;
                var bootstrapperInfo = new BootstrapperInfo(packageLocalPath);

                result += $"Installer path is {bootstrapperInfo.AssemblyPath}";

                var process = new Process();
                process.StartInfo.FileName = bootstrapperInfo.AssemblyPath;

                process.StartInfo.ArgumentList.Add("/i");
                process.StartInfo.ArgumentList.Add("/q=1");
                process.StartInfo.ArgumentList.Add($"/INSTANCE={platformName}");
                process.StartInfo.ArgumentList.Add($"/DATABASE_SERVER={MigrateRequest.DestServerName}");
                process.StartInfo.ArgumentList.Add($"/CONFIG_SERVER={MigrateRequest.DestServerName}");
                process.StartInfo.ArgumentList.Add($"/CONFIG_SERVER_LOGIN={MigrateRequest.DestServerSysAdminUsername}");
                process.StartInfo.ArgumentList.Add($"/CONFIG_SERVER_PASSWORD={MigrateRequest.DestServerSysAdminPassword}");
                process.StartInfo.ArgumentList.Add($"/DATABASE_ADMIN_USERNAME={MigrateRequest.DestServerSysAdminUsername}");
                process.StartInfo.ArgumentList.Add($"/DATABASE_ADMIN_PASSWORD={MigrateRequest.DestServerSysAdminPassword}");
                process.StartInfo.ArgumentList.Add($"/APPVERSION={destEnvVersion}");

                result += $"Installer args are added safely.";

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;

                process.Start();
                var stdout = process.StandardOutput.ReadToEnd();
                var stderr = process.StandardError.ReadToEnd();
                process.WaitForExit();

                result += stdout;
                if (!string.IsNullOrWhiteSpace(stderr))
                {
                    result += "\nErrors encountered:\n" + stderr;
                    throw new ApplicationException(stderr);
                }
            }
        }
    }
}
