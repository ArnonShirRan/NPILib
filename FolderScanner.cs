﻿using System.Collections.Generic;
using System.IO;

namespace NPILib
{
    public static class FolderScanner
    {
        public static List<File> ScanFolder(string folderPath)
        {
            if (string.IsNullOrWhiteSpace(folderPath))
                throw new System.ArgumentException("Folder path cannot be null or empty.", nameof(folderPath));

            if (!Directory.Exists(folderPath))
                throw new DirectoryNotFoundException($"The directory '{folderPath}' does not exist.");

            var fileList = new List<File>();
            var files = Directory.GetFiles(folderPath);

            foreach (var filePath in files)
            {
                var file = new File(filePath);

                if (!file.IsValid)
                {
                    ScanLogger.LogSkippedFile(file.FileName);
                    continue;
                }

                fileList.Add(file);
            }

            return fileList;
        }

        public static List<File> ScanFolderRecursively(string folderPath)
        {
            var fileList = new List<File>();
            fileList.AddRange(ScanFolder(folderPath));

            var subdirectories = Directory.GetDirectories(folderPath);
            foreach (var subdirectory in subdirectories)
            {
                fileList.AddRange(ScanFolderRecursively(subdirectory));
            }

            return fileList;
        }
    }
}
