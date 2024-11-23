using System;
using System.Collections.Generic;
using System.IO;

namespace NPILib
{
    public static class FolderScanner
    {
        private static readonly List<string> SkippedFiles = new List<string>();

        // Non-recursive folder scanning
        public static List<File> ScanFolder(string folderPath)
        {
            if (string.IsNullOrWhiteSpace(folderPath))
                throw new ArgumentException("Folder path cannot be null or empty.", nameof(folderPath));

            if (!Directory.Exists(folderPath))
                throw new DirectoryNotFoundException($"The directory '{folderPath}' does not exist.");

            var fileList = new List<File>();
            var files = Directory.GetFiles(folderPath);

            foreach (var filePath in files)
            {
                var file = new File(filePath);

                if (!file.IsValid)
                {
                    SkippedFiles.Add(file.FileName);
                    continue;
                }

                fileList.Add(file);
            }

            LogSkippedFiles();
            return fileList;
        }

        // Recursive folder scanning
        public static List<File> ScanFolderRecursively(string folderPath)
        {
            if (string.IsNullOrWhiteSpace(folderPath))
                throw new ArgumentException("Folder path cannot be null or empty.", nameof(folderPath));

            if (!Directory.Exists(folderPath))
                throw new DirectoryNotFoundException($"The directory '{folderPath}' does not exist.");

            var fileList = new List<File>();
            fileList.AddRange(ScanFolder(folderPath));

            var subdirectories = Directory.GetDirectories(folderPath);
            foreach (var subdirectory in subdirectories)
            {
                fileList.AddRange(ScanFolderRecursively(subdirectory));
            }

            return fileList;
        }

        private static void LogSkippedFiles()
        {
            Console.WriteLine("\nSkipped Files:");
            if (SkippedFiles.Count > 0)
            {
                foreach (var file in SkippedFiles)
                {
                    Console.WriteLine($"- {file}");
                }
            }
            else
            {
                Console.WriteLine("No files were skipped.");
            }
        }
    }
}
