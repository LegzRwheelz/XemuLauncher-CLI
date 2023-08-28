using System.Diagnostics;
using System.IO.Compression;
using System.Net;

namespace XemuLauncher_CLI
{
    public class Helper
    {
        // The location the XemuUpdater application is stored
        //string _currentFullPath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName.Replace(System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName, "");

        //Creates the expected folder structure for a build of Xemu which backs up the zip of the previously downloaded build
        public void CreateFolderStructure(XemuBuild build)
        {
            Console.WriteLine($"Creating {build.FolderName}");
            Directory.CreateDirectory(build.FolderName); // Xemu build extracts here
        }
        public void UninstallBuild(XemuBuild build, bool skipMsg = false)
        {
            string[] filesToDelete = { $"{build.FolderName}\\{build.ExecutableName}.exe", $"{build.FolderName}\\LICENSE", $"{build.FolderName}\\Xemu.pdb" };
            List<string> existingFiles = filesToDelete.Where(File.Exists).ToList();

            if (existingFiles.Count == 0) return;

            var validFiles = string.Join("\n", existingFiles);

            if (!skipMsg)
            {
                Console.WriteLine($"Are you sure you want to delete\n{validFiles}");
                string answer = "";
                while (answer != "yes")
                {
                    Console.WriteLine($"Type \"yes\" to continue...");
                    answer = Console.ReadLine();
                }
                return;
            }

            if (!Directory.Exists(build.FolderName)) return;

            // Kill Xemu if it's running
            try
            {
                StopProcess(build);
                foreach (var file in existingFiles)
                {
                    if (File.Exists(file))
                        File.Delete(file);
                }
            }
            catch
            {
                UninstallBuild(build, true);
            }

        }

        // Extracts build of Xemu to expected folder
        public void ExtractBuild(XemuBuild build)
        {
            //Deletes LICENSE file because it isn't needed and also causes issues for some reason
            try
            {
                File.Delete($"{build.FolderName}/LICENSE.txt");
                ZipFile.ExtractToDirectory($"{build.FolderName}/{build.ZipName}.zip", build.FolderName);
                File.Delete($"{build.FolderName}/LICENSE.txt");

            }
            catch
            {
                File.Delete($"{build.FolderName}/LICENSE"); // ignored
            }
        }

        // Starts a build of Xemu
        public void OpenLocation(XemuBuild build)
        {
            try
            {
                Process.Start($"{build.FolderName}");
            }
            catch
            {
                Console.WriteLine($"\"{build.ExecutableName}.exe\" could not be found.", "Error");
            }
        }

        // Starts a build of Xemu
        public void StartProcess(XemuBuild build)
        {
            try
            {
                Process.Start($"{build.FolderName}\\{build.ExecutableName}");
            }
            catch
            {
                Console.WriteLine($"\"{build.ExecutableName}.exe\" could not be started.\nThe file must be present and executable.", "Error");
            }
        }

        // Kills the Xemu process
        public void StopProcess(XemuBuild build)
        {
            try
            {
                var proc = Process.GetProcessesByName(build.ExecutableName);
                proc[0].Kill();
            }
            catch
            {
                // ignored
            }
        }


        public bool InternetAvailable()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://github.com"))
                    return true;
            }
            catch
            {
                return false;
            }
        }
    }

}
