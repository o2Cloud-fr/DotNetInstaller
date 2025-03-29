using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Linq;

namespace Install.NETFramework
{
    internal class Program
    {
        // Structure pour stocker les informations sur les packages
        public class Package
        {
            public string Name { get; set; }
            public string Url { get; set; }
            public string FileName { get; set; }
            public string Arguments { get; set; }
            public bool CompatibleWin10 { get; set; }
            public bool CompatibleWin11 { get; set; }
            public bool IsInstalled { get; set; }
        }

        // Dossier temporaire pour les téléchargements
        private static string tempDir = Path.Combine(Path.GetTempPath(), "DotNetInstaller");

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Title = "Installateur .NET Framework et C++ Redistributable";

            // Créer le dossier temporaire s'il n'existe pas
            if (!Directory.Exists(tempDir))
            {
                Directory.CreateDirectory(tempDir);
            }

            // Liste des packages .NET Framework et C++ Redistributable
            var packages = InitializePackages();

            // Vérifier les packages déjà installés
            CheckInstalledPackages(packages);

            while (true)
            {
                Console.Clear();
                DisplayHeader();
                DisplayPackages(packages);

                Console.WriteLine("\nOptions:");
                Console.WriteLine("1. Installer tous les packages compatibles");
                Console.WriteLine("2. Installer un package spécifique");
                Console.WriteLine("3. Installer tous les C++ Redistributable");
                Console.WriteLine("4. Installer tous les .NET Framework");
                Console.WriteLine("5. Quitter");
                Console.Write("\nChoisissez une option (1-5): ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await InstallCompatiblePackages(packages);
                        break;
                    case "2":
                        await InstallSpecificPackage(packages);
                        break;
                    case "3":
                        await InstallCppRedistPackages(packages);
                        break;
                    case "4":
                        await InstallDotNetPackages(packages);
                        break;
                    case "5":
                        CleanupAndExit();
                        return;
                    default:
                        Console.WriteLine("Option invalide. Appuyez sur une touche pour continuer.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static void DisplayHeader()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔═════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║            INSTALLATEUR .NET FRAMEWORK ET C++ REDIST            ║");
            Console.WriteLine("╚═════════════════════════════════════════════════════════════════╝");
            Console.ResetColor();
            Console.WriteLine("\nCet outil installe les versions de .NET Framework et C++ Redistributable compatibles avec Windows 10/11");
            Console.WriteLine("Les packages déjà installés sont marqués [INSTALLÉ]\n");
        }

        private static void DisplayPackages(List<Package> packages)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("╔═══════╦════════════════════════════════════════╦═══════════╦═══════════╦═══════════╗");
            Console.WriteLine("║ INDEX ║ NOM DU PACKAGE                         ║ WINDOWS 10 ║ WINDOWS 11 ║   STATUT   ║");
            Console.WriteLine("╠═══════╬════════════════════════════════════════╬═══════════╬═══════════╬═══════════╣");
            Console.ResetColor();

            for (int i = 0; i < packages.Count; i++)
            {
                var package = packages[i];
                string status = package.IsInstalled ? "INSTALLÉ" : "NON INSTALLÉ";
                string win10Status = package.CompatibleWin10 ? "   ✓   " : "   ✗   ";
                string win11Status = package.CompatibleWin11 ? "   ✓   " : "   ✗   ";

                Console.Write($"║ {i + 1,5} ║ {package.Name,-38} ║ {win10Status} ║ {win11Status} ║");

                if (package.IsInstalled)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($" {status,10} ║");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($" {status,10} ║");
                    Console.ResetColor();
                }
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("╚═══════╩════════════════════════════════════════╩═══════════╩═══════════╩═══════════╝");
            Console.ResetColor();
        }

        private static List<Package> InitializePackages()
        {
            var packages = new List<Package>
            {
                // .NET Framework packages
                // Les versions 1.0 et 1.1 ne sont pas compatibles avec Windows 10/11
                new Package
                {
                    Name = ".NET Framework 1.0",
                    Url = "https://download.microsoft.com/download/2/0/6/206bdb83-90ea-4ec0-bfba-853e40137648/NDP1.0sp3-KB930494-X86-Ocm-Enu.exe",
                    FileName = "NDP1.0sp3-KB930494-X86-Ocm-Enu.exe",
                    Arguments = "/q:a /c:\"install /qb /l\"",
                    CompatibleWin10 = false,
                    CompatibleWin11 = false
                },
                new Package
                {
                    Name = ".NET Framework 1.1",
                    Url = "https://download.microsoft.com/download/e/d/a/eda9d4ea-8ec9-4431-8efa-75391fb91421/dotnetfx.exe",
                    FileName = "dotnetfx.exe",
                    Arguments = "/q:a /c:\"install /qb /l\"",
                    CompatibleWin10 = false,
                    CompatibleWin11 = false
                },
                new Package
                {
                    Name = ".NET Framework 2.0",
                    Url = "https://download.microsoft.com/download/9/8/6/98610406-c2b7-45a4-bdc3-9db1b1c5f7e2/NetFx20SP1_x64.exe",
                    FileName = "NetFx20SP1_x64.exe",
                    Arguments = "/q:a /c:\"install /qb /l\"",
                    CompatibleWin10 = false,
                    CompatibleWin11 = false
                },
                new Package
                {
                    Name = ".NET Framework 3.0",
                    Url = "https://download.microsoft.com/download/4/9/0/49001df1-af88-4a4d-b10f-2d5e3a8ea5f3/dotnetfx30SP1setup.exe",
                    FileName = "dotnetfx30SP1setup.exe",
                    Arguments = "/q:a /c:\"install /qb /l\"",
                    CompatibleWin10 = false,
                    CompatibleWin11 = false
                },
                new Package
                {
                    Name = ".NET Framework 3.5 SP1",
                    Url = "https://download.microsoft.com/download/2/0/e/20e90413-712f-438c-988e-fdaa79a8ac3d/dotnetfx35.exe",
                    FileName = "dotnetfx35.exe",
                    Arguments = "/q /norestart",
                    CompatibleWin10 = true,
                    CompatibleWin11 = true
                },
                new Package
                {
                    Name = ".NET Framework 4.0",
                    Url = "https://download.microsoft.com/download/9/5/A/95A9616B-7A37-4AF6-BC36-D6EA96C8DAAE/dotNetFx40_Full_x86_x64.exe",
                    FileName = "dotNetFx40_Full_x86_x64.exe",
                    Arguments = "/q /norestart",
                    CompatibleWin10 = false,
                    CompatibleWin11 = false
                },
                new Package
                {
                    Name = ".NET Framework 4.5",
                    Url = "https://download.microsoft.com/download/b/a/4/ba4a7e71-2906-4b2d-a0e1-80cf16844f5f/dotnetfx45_full_x86_x64.exe",
                    FileName = "dotnetfx45_full_x86_x64.exe",
                    Arguments = "/q /norestart",
                    CompatibleWin10 = false,
                    CompatibleWin11 = false
                },
                new Package
                {
                    Name = ".NET Framework 4.5.1",
                    Url = "https://download.microsoft.com/download/1/6/7/167F0D79-9317-48AE-AEDB-17120579F8E2/NDP451-KB2858728-x86-x64-AllOS-ENU.exe",
                    FileName = "NDP451-KB2858728-x86-x64-AllOS-ENU.exe",
                    Arguments = "/q /norestart",
                    CompatibleWin10 = false,
                    CompatibleWin11 = false
                },
                new Package
                {
                    Name = ".NET Framework 4.5.2",
                    Url = "https://download.microsoft.com/download/E/2/1/E21644B5-2DF2-47C2-91BD-63C560427900/NDP452-KB2901907-x86-x64-AllOS-ENU.exe",
                    FileName = "NDP452-KB2901907-x86-x64-AllOS-ENU.exe",
                    Arguments = "/q /norestart",
                    CompatibleWin10 = true,
                    CompatibleWin11 = false
                },
                new Package
                {
                    Name = ".NET Framework 4.6",
                    Url = "https://download.microsoft.com/download/c/3/a/c3a5200b-d33c-47e9-9d70-2f7c65daad94/NDP46-KB3045557-x86-x64-AllOS-ENU.exe",
                    FileName = "NDP46-KB3045557-x86-x64-AllOS-ENU.exe",
                    Arguments = "/q /norestart",
                    CompatibleWin10 = true,
                    CompatibleWin11 = false
                },
                new Package
                {
                    Name = ".NET Framework 4.6.1",
                    Url = "https://download.microsoft.com/download/E/4/1/E4173890-A24A-4936-9FC9-AF930FE3FA40/NDP461-KB3102436-x86-x64-AllOS-ENU.exe",
                    FileName = "NDP461-KB3102436-x86-x64-AllOS-ENU.exe",
                    Arguments = "/q /norestart",
                    CompatibleWin10 = true,
                    CompatibleWin11 = false
                },
                new Package
                {
                    Name = ".NET Framework 4.6.2",
                    Url = "https://download.visualstudio.microsoft.com/download/pr/8e396c75-4d0d-41d3-aea8-848babc2736a/80b431456d8866ebe053eb8b81a168b3/ndp462-kb3151800-x86-x64-allos-enu.exe",
                    FileName = "ndp462-kb3151800-x86-x64-allos-enu.exe",
                    Arguments = "/q /norestart",
                    CompatibleWin10 = true,
                    CompatibleWin11 = true
                },
                new Package
                {
                    Name = ".NET Framework 4.7",
                    Url = "https://download.visualstudio.microsoft.com/download/pr/2dfcc711-bb60-421a-a17b-76c63f8d1907/4473f6b1cda5012d1e2aac0fa16dee62/ndp47-kb3186497-x86-x64-allos-fra.exe",
                    FileName = "ndp47-kb3186497-x86-x64-allos-fra.exe",
                    Arguments = "/q /norestart",
                    CompatibleWin10 = true,
                    CompatibleWin11 = true
                },
                new Package
                {
                    Name = ".NET Framework 4.7.1",
                    Url = "https://download.visualstudio.microsoft.com/download/pr/4312fa21-59b0-4451-9482-a1376f7f3ba4/042d300912e9366b404f9b01e044ba44/ndp471-kb4033342-x86-x64-allos-fra.exe",
                    FileName = "ndp471-kb4033342-x86-x64-allos-fra.exe",
                    Arguments = "/q /norestart",
                    CompatibleWin10 = true,
                    CompatibleWin11 = true
                },
                new Package
                {
                    Name = ".NET Framework 4.7.2",
                    Url = "https://download.microsoft.com/download/f/3/a/f3a6af84-da23-40a5-8d1c-49cc10c8e76f/NDP472-KB4054530-x86-x64-AllOS-ENU.exe",
                    FileName = "NDP472-KB4054530-x86-x64-AllOS-ENU.exe",
                    Arguments = "/q /norestart",
                    CompatibleWin10 = true,
                    CompatibleWin11 = true
                },
                new Package
                {
                    Name = ".NET Framework 4.8",
                    Url = "https://download.visualstudio.microsoft.com/download/pr/c2ad65ab-bab3-4d24-ada4-aaf2ff0c1266/d05b8a935181f07ff446770261472ad2/ndp48-x86-x64-allos-fra.exe",
                    FileName = "ndp48-x86-x64-allos-fra.exe",
                    Arguments = "/q /norestart",
                    CompatibleWin10 = true,
                    CompatibleWin11 = true
                },
                new Package
                {
                    Name = ".NET Framework 4.8.1",
                    Url = "https://download.microsoft.com/download/a/f/9/af9410a7-31c7-4a77-a7e0-1cd2e3320b37/ndp481-x86-x64-allos-fra.exe",
                    FileName = "ndp481-x86-x64-allos-fra.exe",
                    Arguments = "/q /norestart",
                    CompatibleWin10 = true,
                    CompatibleWin11 = true
                },

                // Visual C++ Redistributable packages
                new Package
                {
                    Name = "Visual C++ 2005 Redistributable (x86)",
                    Url = "https://download.microsoft.com/download/8/B/4/8B42259F-5D70-43F4-AC2E-4B208FD8D66A/vcredist_x86.exe",
                    FileName = "vcredist_2005_x86.exe",
                    Arguments = "/q",
                    CompatibleWin10 = true,
                    CompatibleWin11 = true
                },
                new Package
                {
                    Name = "Visual C++ 2005 Redistributable (x64)",
                    Url = "https://download.microsoft.com/download/8/B/4/8B42259F-5D70-43F4-AC2E-4B208FD8D66A/vcredist_x64.exe",
                    FileName = "vcredist_2005_x64.exe",
                    Arguments = "/q",
                    CompatibleWin10 = true,
                    CompatibleWin11 = true
                },
                new Package
                {
                    Name = "Visual C++ 2008 Redistributable (x86)",
                    Url = "https://download.microsoft.com/download/5/D/8/5D8C65CB-C849-4025-8E95-C3966CAFD8AE/vcredist_x86.exe",
                    FileName = "vcredist_2008_x86.exe",
                    Arguments = "/q",
                    CompatibleWin10 = true,
                    CompatibleWin11 = true
                },
                new Package
                {
                    Name = "Visual C++ 2008 Redistributable (x64)",
                    Url = "https://download.microsoft.com/download/5/D/8/5D8C65CB-C849-4025-8E95-C3966CAFD8AE/vcredist_x64.exe",
                    FileName = "vcredist_2008_x64.exe",
                    Arguments = "/q",
                    CompatibleWin10 = true,
                    CompatibleWin11 = true
                },
                new Package
                {
                    Name = "Visual C++ 2010 Redistributable (x86)",
                    Url = "https://download.microsoft.com/download/C/6/D/C6D0FD4E-9E53-4897-9B91-836EBA2AACD3/vcredist_x86.exe",
                    FileName = "vcredist_2010_x86.exe",
                    Arguments = "/passive /norestart",
                    CompatibleWin10 = true,
                    CompatibleWin11 = true
                },
                new Package
                {
                    Name = "Visual C++ 2010 Redistributable (x64)",
                    Url = "https://download.microsoft.com/download/A/8/0/A80747C3-41BD-45DF-B505-E9710D2744E0/vcredist_x64.exe",
                    FileName = "vcredist_2010_x64.exe",
                    Arguments = "/passive /norestart",
                    CompatibleWin10 = true,
                    CompatibleWin11 = true
                },
                new Package
                {
                    Name = "Visual C++ 2012 Redistributable (x86)",
                    Url = "https://download.microsoft.com/download/1/6/B/16B06F60-3B20-4FF2-B699-5E9B7962F9AE/VSU_4/vcredist_x86.exe",
                    FileName = "vcredist_2012_x86.exe",
                    Arguments = "/passive /norestart",
                    CompatibleWin10 = true,
                    CompatibleWin11 = true
                },
                new Package
                {
                    Name = "Visual C++ 2012 Redistributable (x64)",
                    Url = "https://download.microsoft.com/download/1/6/B/16B06F60-3B20-4FF2-B699-5E9B7962F9AE/VSU_4/vcredist_x64.exe",
                    FileName = "vcredist_2012_x64.exe",
                    Arguments = "/passive /norestart",
                    CompatibleWin10 = true,
                    CompatibleWin11 = true
                },
                new Package
                {
                    Name = "Visual C++ 2013 Redistributable (x86)",
                    Url = "https://download.microsoft.com/download/2/E/6/2E61CFA4-993B-4DD4-91DA-3737CD5CD6E3/vcredist_x86.exe",
                    FileName = "vcredist_2013_x86.exe",
                    Arguments = "/passive /norestart",
                    CompatibleWin10 = true,
                    CompatibleWin11 = true
                },
                new Package
                {
                    Name = "Visual C++ 2013 Redistributable (x64)",
                    Url = "https://download.microsoft.com/download/2/E/6/2E61CFA4-993B-4DD4-91DA-3737CD5CD6E3/vcredist_x64.exe",
                    FileName = "vcredist_2013_x64.exe",
                    Arguments = "/passive /norestart",
                    CompatibleWin10 = true,
                    CompatibleWin11 = true
                },
                new Package
                {
                    Name = "Visual C++ 2015-2022 Redistributable (x86)",
                    Url = "https://aka.ms/vs/17/release/vc_redist.x86.exe",
                    FileName = "vc_redist.x86.exe",
                    Arguments = "/passive /norestart",
                    CompatibleWin10 = true,
                    CompatibleWin11 = true
                },
                new Package
                {
                    Name = "Visual C++ 2015-2022 Redistributable (x64)",
                    Url = "https://aka.ms/vs/17/release/vc_redist.x64.exe",
                    FileName = "vc_redist.x64.exe",
                    Arguments = "/passive /norestart",
                    CompatibleWin10 = true,
                    CompatibleWin11 = true
                }
            };

            return packages;
        }

        private static void CheckInstalledPackages(List<Package> packages)
        {
            // Cette vérification est simplifiée - en réalité, vous devriez vérifier le registre ou autre
            // Pour les .NET Framework
            foreach (var package in packages.Where(p => p.Name.Contains(".NET Framework")))
            {
                string version = package.Name.Replace(".NET Framework ", "");

                // Vérification simplifiée basée sur la présence des dossiers
                if (version == "3.5 SP1")
                {
                    package.IsInstalled = Directory.Exists(@"C:\Windows\Microsoft.NET\Framework\v3.5");
                }
                else if (version.StartsWith("4."))
                {
                    package.IsInstalled = Directory.Exists(@"C:\Windows\Microsoft.NET\Framework\v4.0.30319");
                }
                else
                {
                    package.IsInstalled = false;
                }
            }

            // Pour les C++ Redistributable
            foreach (var package in packages.Where(p => p.Name.Contains("Visual C++")))
            {
                // Vérification simplifiée
                if (package.Name.Contains("2015-2022"))
                {
                    // Les plus récents VC++ Redist
                    string registryPath = package.Name.Contains("x64")
                        ? @"SOFTWARE\Microsoft\VisualStudio\14.0\VC\Runtimes\x64"
                        : @"SOFTWARE\Microsoft\VisualStudio\14.0\VC\Runtimes\x86";

                    // Simuler la vérification pour l'exemple
                    package.IsInstalled = false;
                }
                else
                {
                    // Simuler la vérification pour l'exemple
                    package.IsInstalled = false;
                }
            }
        }

        private static async Task InstallCompatiblePackages(List<Package> packages)
        {
            bool isWindows10 = Environment.OSVersion.Version.Major == 10;
            bool isWindows11 = Environment.OSVersion.Version.Build >= 22000; // Windows 11 build starts at 22000

            var compatiblePackages = packages.Where(p =>
                (isWindows10 && p.CompatibleWin10) ||
                (isWindows11 && p.CompatibleWin11)).ToList();

            Console.Clear();
            Console.WriteLine("Installation des packages compatibles...\n");

            foreach (var package in compatiblePackages)
            {
                if (!package.IsInstalled)
                {
                    await InstallPackage(package);
                }
                else
                {
                    Console.WriteLine($"{package.Name} est déjà installé. Skipping...");
                }
            }

            Console.WriteLine("\nInstallation terminée! Appuyez sur une touche pour continuer.");
            Console.ReadKey();
        }

        private static async Task InstallSpecificPackage(List<Package> packages)
        {
            Console.Clear();
            DisplayPackages(packages);

            Console.Write("\nEntrez le numéro du package à installer (1-" + packages.Count + "): ");
            if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= packages.Count)
            {
                var package = packages[index - 1];

                if (package.IsInstalled)
                {
                    Console.WriteLine($"\n{package.Name} est déjà installé. Voulez-vous le réinstaller? (O/N): ");
                    if (Console.ReadLine().ToUpper() != "O")
                    {
                        return;
                    }
                }

                await InstallPackage(package);

                Console.WriteLine("\nInstallation terminée! Appuyez sur une touche pour continuer.");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Numéro de package invalide. Appuyez sur une touche pour continuer.");
                Console.ReadKey();
            }
        }

        private static async Task InstallCppRedistPackages(List<Package> packages)
        {
            var cppPackages = packages.Where(p => p.Name.Contains("Visual C++")).ToList();

            Console.Clear();
            Console.WriteLine("Installation des packages C++ Redistributable...\n");

            foreach (var package in cppPackages)
            {
                if (!package.IsInstalled)
                {
                    await InstallPackage(package);
                }
                else
                {
                    Console.WriteLine($"{package.Name} est déjà installé. Skipping...");
                }
            }

            Console.WriteLine("\nInstallation terminée! Appuyez sur une touche pour continuer.");
            Console.ReadKey();
        }

        private static async Task InstallDotNetPackages(List<Package> packages)
        {
            var dotNetPackages = packages.Where(p => p.Name.Contains(".NET Framework")).ToList();

            Console.Clear();
            Console.WriteLine("Installation des packages .NET Framework...\n");

            foreach (var package in dotNetPackages)
            {
                if (!package.IsInstalled)
                {
                    await InstallPackage(package);
                }
                else
                {
                    Console.WriteLine($"{package.Name} est déjà installé. Skipping...");
                }
            }

            Console.WriteLine("\nInstallation terminée! Appuyez sur une touche pour continuer.");
            Console.ReadKey();
        }

        private static async Task InstallPackage(Package package)
        {
            try
            {
                Console.WriteLine($"Installation de {package.Name}...");

                // Télécharger le package
                string filePath = Path.Combine(tempDir, package.FileName);

                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"Téléchargement de {package.Name}...");
                    using (var client = new WebClient())
                    {
                        await client.DownloadFileTaskAsync(new Uri(package.Url), filePath);
                    }
                }

                // Lancer l'installation
                Console.WriteLine($"Lancement de l'installation pour {package.Name}...");

                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = filePath,
                        Arguments = package.Arguments,
                        UseShellExecute = true,
                        CreateNoWindow = false
                    }
                };

                process.Start();
                await Task.Run(() => process.WaitForExit());

                Console.WriteLine($"Installation de {package.Name} terminée.");
                package.IsInstalled = true;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Erreur lors de l'installation de {package.Name}: {ex.Message}");
                Console.ResetColor();
            }
        }

        private static void CleanupAndExit()
        {
            try
            {
                // Nettoyer les fichiers téléchargés
                if (Directory.Exists(tempDir))
                {
                    Console.WriteLine("Nettoyage des fichiers temporaires...");
                    Directory.Delete(tempDir, true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du nettoyage: {ex.Message}");
            }

            Console.WriteLine("Merci d'avoir utilisé l'installateur .NET Framework et C++ Redistributable!");
        }
    }
}