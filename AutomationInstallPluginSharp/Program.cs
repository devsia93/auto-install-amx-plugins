using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationInstallPluginSharp
{
    class Program
    {
        static int CountFiles = 0;
        static DirectoryInfo RootDirectory;


        static List<string> FirstStartListPlugins = new List<string>();
        static List<string> BlackListPlugins = new List<string>();

        static void Main(string[] args)
        {
            if (args.Length >= 2)
            {
                string pathToDirectory = args[0];
                string pathToFile = args[1];

                RootDirectory = new DirectoryInfo(pathToDirectory);
                StartInitFirstListPlugins(pathToFile);

                while (true)
                {
                    List<string> files = GetNewListFiles();

                    if (CountFiles != files.Count)
                    {
                        List<string> newTextFromFiles = new List<string>();
                        string textFromFile = File.ReadAllText(pathToFile);   

                        foreach (string file in files)
                        {
                            if (BlackListPlugins.Contains(file))
                            {
                                newTextFromFiles.Add(";" + file);
                            }
                            else
                            {
                                newTextFromFiles.Add(file);
                            }
                        }
                        CountFiles = files.Count;
                        File.WriteAllLines(pathToFile, newTextFromFiles);
                    }
                    else System.Threading.Thread.Sleep(5000);
                }
            }

            Console.WriteLine("Before start program you should write 2 arguments into program.\n 1st: full path to directory addons/amxmodx/plugins\n 2nd: full path to file addons/amxmodx/configs/plugins.ini");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        //Inflate noactive plugins list
        private static void StartInitFirstListPlugins(string pathToFile)
        {
            FirstStartListPlugins.AddRange(File.ReadAllLines(pathToFile));
            List<string> currentFiles = GetNewListFiles();

            foreach (string file in currentFiles)
            {
                if (file[0].Equals(";"))
                    BlackListPlugins.Add(file.Substring(1));
                else if (!FirstStartListPlugins.Contains(file))
                {
                    BlackListPlugins.Add(file);
                }
            }
        }

        private static List<string> GetNewListFiles()
        {
            List<string> result = new List<string>();

            foreach (FileInfo f in RootDirectory.GetFiles())
            {
                result.Add(Path.GetFileName(f.FullName));
            }
            return result;
        }
    }
}
