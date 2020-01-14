using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ToolGood.CodeCounts
{
    class Program
    {
        static List<string> exts = new List<string>() {
            ".cs", ".html", ".htm", ".js", ".css",".less",".sess",
            ".aspx",".ashx",".txt",".ts",
            ".cshtml", ".csproj", ".json", ".xml", ".config" };
        static List<string> excludeExts = new List<string>() {
            ".deps.json",".dev.json",".runtimeconfig.json","CodeCounts.txt",
            ".designer.cs" };

        static List<string> excludeFolder = new List<string>() { "bin", "obj", ".git", ".svn", ".vs", "packages" };

        static void Main(string[] args)
        {
            var folder = Directory.GetCurrentDirectory();
            var files = GetAllFiles(folder);
            files = files.Distinct().ToList();
            var sum = 0;
            long bytes = 0;
            var changeFiles = 0;
            foreach (var file in files) {
                var fi = new FileInfo(file);
                bytes += fi.Length;
                if (fi.LastWriteTime.Date == DateTime.Now.Date) {
                    changeFiles++;
                }

                var lines = File.ReadAllLines(file);
                Console.WriteLine($"{file}|{lines.Length}");
                sum += lines.Length;

            }
            Console.WriteLine($"总行数：{sum}");

            File.AppendAllText("CodeCounts.txt", $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}]代码总行数：{sum}\t容量体积：{bytes}\t文件数：{files.Count}\t今天修改文件数：{changeFiles} \r\n");
        }

        static List<string> GetAllFiles(string folder)
        {
            List<string> list = new List<string>();
            GetAllFiles(folder, list);
            return list;
        }

        static void GetAllFiles(string folder, List<string> list)
        {
            var files = GetFiles(folder);
            list.AddRange(files);

            var folders = GetFolders(folder);
            foreach (var item in folders) {
                GetAllFiles(item, list);
            }
        }

        static List<string> GetFolders(string folder)
        {
            List<string> list = new List<string>();
            var files = Directory.GetDirectories(folder);

            foreach (var file in files) {
                var ext = Path.GetFileName(file);
                if (excludeFolder.Contains(ext) == false) {
                    list.Add(file);
                }
            }
            return list;
        }

        static List<string> GetFiles(string folder)
        {
            List<string> list = new List<string>();
            var files = Directory.GetFiles(folder, "*.*", SearchOption.TopDirectoryOnly);

            foreach (var file in files) {
                var ext = Path.GetExtension(file).ToLower();
                var b = false;
                foreach (var item in excludeExts) {
                    if (file.EndsWith(item)) {
                        b = true;
                        break;
                    }
                }
                if (b) {
                    continue;
                }

                foreach (var item in exts) {
                    if (ext.EndsWith(item)) {

                        list.Add(file);
                        break;
                    }
                }
            }
            return list;
        }

        static int GetLineCount(string file)
        {
            var lines = File.ReadAllLines(file);
            Console.WriteLine($"{file}|{lines.Length}");
            return lines.Length;
        }

    }
}
