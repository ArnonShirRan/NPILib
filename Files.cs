using System;
using System.Collections.Generic;
using System.IO;

namespace NPILib
{
    public class Files
    {
        // List to hold File objects
        public List<File> FileList { get; private set; }

        // Constructor
        public Files()
        {
            FileList = new List<File>();
        }

        // Method to scan a folder and add files to the list
        public void ScanFolder(string folderPath)
        {
            if (string.IsNullOrWhiteSpace(folderPath))
                throw new ArgumentException("Folder path cannot be null or empty.", nameof(folderPath));

            if (!Directory.Exists(folderPath))
                throw new DirectoryNotFoundException($"The directory '{folderPath}' does not exist.");

            // Get all files in the folder (non-recursive)
            var files = Directory.GetFiles(folderPath);

            foreach (var filePath in files)
            {
                try
                {
                    // Create a File object for each file and add it to the list
                    var file = new File(filePath);
                    FileList.Add(file);
                }
                catch (Exception ex)
                {
                    // Handle any exceptions (e.g., invalid file formats) and log if necessary
                    Console.WriteLine($"Error processing file '{filePath}': {ex.Message}");
                }
            }
        }

        // NEW METHOD: Scan folder recursively
        public void ScanFolderRecursively(string folderPath)
        {
            if (string.IsNullOrWhiteSpace(folderPath))
                throw new ArgumentException("Folder path cannot be null or empty.", nameof(folderPath));

            if (!Directory.Exists(folderPath))
                throw new DirectoryNotFoundException($"The directory '{folderPath}' does not exist.");

            // Call ScanFolder for the current folder
            ScanFolder(folderPath);

            // Recursively process subdirectories
            var subdirectories = Directory.GetDirectories(folderPath);
            foreach (var subdirectory in subdirectories)
            {
                try
                {
                    ScanFolderRecursively(subdirectory); // Recursive call
                }
                catch (Exception ex)
                {
                    // Handle any exceptions and log if necessary
                    Console.WriteLine($"Error processing subdirectory '{subdirectory}': {ex.Message}");
                }
            }
        }

        // Method to create a CSV file for all file properties and save it to the given path
        public void ExportToCsv(string outputPath)
        {
            // Delegate to CSVCreator
            CSVCreator.CreateFilesCSV(outputPath, this);
        }




        // Method to display all files in the list
        public void DisplayFiles()
        {
            if (FileList.Count == 0)
            {
                Console.WriteLine("No files found.");
                return;
            }

            foreach (var file in FileList)
            {
                Console.WriteLine(file);
            }
        }
    }
}
