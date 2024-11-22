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
        public void ExportToCsv(string outputFolderPath)
        {
            if (string.IsNullOrWhiteSpace(outputFolderPath))
                throw new ArgumentException("Output folder path cannot be null or empty.", nameof(outputFolderPath));

            if (!Directory.Exists(outputFolderPath))
                throw new DirectoryNotFoundException($"The directory '{outputFolderPath}' does not exist.");

            // Define the CSV file path
            string csvFilePath = Path.Combine(outputFolderPath, "FileList.csv");

            try
            {
                using (var writer = new StreamWriter(csvFilePath))
                {
                    // Write CSV header
                    writer.WriteLine("FileName,Type,PartNumber,Revision,IsProductionFile");

                    // Write each file's properties as a CSV row
                    foreach (var file in FileList)
                    {
                        writer.WriteLine($"{EscapeCsvField(Path.GetFileName(file.Path))}," +
                                         $"{EscapeCsvField(file.Type)}," +
                                         $"{EscapeCsvField(file.PartNumber)}," +
                                         $"{EscapeCsvField(file.Rev)}," +
                                         $"{EscapeCsvField(file.IsProductionFile.ToString())}");
                    }
                }

                Console.WriteLine($"CSV file successfully created at: {csvFilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating CSV file: {ex.Message}");
            }
        }

        // Helper method to escape and sanitize CSV fields
        private string EscapeCsvField(string field)
        {
            if (string.IsNullOrWhiteSpace(field))
                return string.Empty;

            // Replace newlines and carriage returns with spaces
            field = field.Replace("\r", " ").Replace("\n", " ");

            // Escape double quotes by doubling them and enclose the field in double quotes
            return field.Contains(",") || field.Contains("\"") || field.Contains(" ") || field.Contains("\t")
                ? $"\"{field.Replace("\"", "\"\"")}\""
                : field;
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
