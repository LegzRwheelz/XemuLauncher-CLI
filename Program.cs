using System.Net;

namespace XemuLauncher_CLI
{
    internal class Program
    {

        static void Main(string[] args)
        {
            XemuBuild master = new XemuBuild("Xemu",
                "https://github.com/xemu-project/xemu/releases/latest/download/xemu-win-release.zip",
                 "Xemu",
                 "Xemu", "xemu-win-release");
            

            List<XemuBuild> Builds = new List<XemuBuild>
            {
                master
            };

            List<string> validVerbs = new List<string>
            {
                "update",
                "u",
                "run",
                "r",
                "help",
                "h"

            };

            XemuBuild selectedBuild = null;

            if (args.Length < 2)
            {
                insufficientArgs();
                help();
                return;
            }

            if (!validVerbs.Contains(args[0]))
            {
                Console.WriteLine($"Invalid argument: {args[0]}.");
                return;
            }

            foreach (XemuBuild XemuBuild in Builds)
            {
                if (XemuBuild.Name.ToLower().Contains(args[1]))
                {
                    selectedBuild = XemuBuild;
                    break;
                }
            }

            if (selectedBuild == null)
            {
                Console.WriteLine($"Invalid argument: {args[1]}.");
                return;
            }

            if (args[0].StartsWith('u'))
            {
                update(selectedBuild);
            }
            else if (args[0].StartsWith('r'))
            {
                run(selectedBuild);
            }
            else if (args[0].StartsWith('h'))
            {
                help();
            }
            else
            {
                insufficientArgs();
                help();
            }



            void insufficientArgs()
            {
                Console.WriteLine("Insufficient number of arguments.");
            }

            void help()
            {
                Console.WriteLine("XemuUpdater-CLI requires two arguments.\n");
                Console.WriteLine("Valid first arguments:");
                validVerbs.ForEach(x => Console.WriteLine("\t" + x));
                Console.WriteLine("\nValid second arguments:");
                Builds.ForEach(x => Console.WriteLine("\t" + x.Name.ToLower()));

                Console.WriteLine("\nExample usage:\n");
                Console.WriteLine("\tXemuUpdater-CLI.exe update Xemu\n");

            }


            void update(XemuBuild build)
            {
                var helper = new Helper();
                if (helper.InternetAvailable()) //Checks if there is a working internet connection
                {
                    helper.CreateFolderStructure(build); // Create the folder which the build will be downloaded to
                    Console.WriteLine($"Updating {build.Name}");
                    DownloadXemu(build); // Download the build
                }
                else
                    Console.WriteLine("Could not connected to server.\nPlease check you internet connection.", "Error");

            }

            void run(XemuBuild build)
            {
                Helper helper = new Helper();
                Console.WriteLine($"Running {build.Name}");
                helper.StartProcess(build);
            }

            void DownloadXemu(XemuBuild build)
            {
                var helper = new Helper();

                using (var wc = new WebClient())
                {
                    //Download from URL to location
                    wc.DownloadFile(new Uri(build.URL), $"{build.FolderName}/{build.ZipName}.zip");

                    helper.ExtractBuild(build); // Go to the next step, extracting the downloaded build
                }
            }

        }
    }
}