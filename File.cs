using System;
using System.IO;
using System.Text.RegularExpressions;

namespace NPILib
{
    public class File
    {
        // Properties
        public string Path { get; private set; }              // Full file path
        public string FileName { get; private set; }          // File name (e.g., file.txt)
        public string Type { get; private set; }              // File type (e.g., pdf, x_t, etc.)
        public bool IsProductionFile { get; private set; }    // Flag indicating if it's a production file
        public string PartNumber { get; private set; }        // Part number associated with the file
        public string Rev { get; private set; }               // Revision identifier

        // Constructor that receives only the path
        public File(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Path cannot be null or empty.", nameof(path));

            Path = path;
            FileName = System.IO.Path.GetFileName(path); // Set the file name
            Type = GetFileType(path);
            SetPartNumberAndRev(path);
            SetIsProductionFile();
        }

        // Method to get the file type (extension) from the path
        private string GetFileType(string path)
        {
            string extension = System.IO.Path.GetExtension(path);
            return string.IsNullOrWhiteSpace(extension) ? "unknown" : extension.TrimStart('.').ToLower();
        }

        // Method to set PartNumber and Rev from the file name
        private void SetPartNumberAndRev(string path)
        {
            string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(path);

            // Look for patterns with "_Rev" followed by a letter
            var match = Regex.Match(fileNameWithoutExtension, @"_Rev([A-Z])");
            if (match.Success)
            {
                // Extract PartNumber (everything before "_Rev")
                PartNumber = fileNameWithoutExtension.Substring(0, match.Index);

                // Extract Rev (the letter after "_Rev")
                Rev = match.Groups[1].Value;
            }
            else
            {
                // If no "_Rev" pattern is found, set defaults
                PartNumber = fileNameWithoutExtension;
                Rev = "N/A";
            }
        }

        // Method to determine if the file is a production file
        private void SetIsProductionFile()
        {
            // A file is a production file if:
            // - The type is "pdf" or "x_t"
            // - The name contains "_Rev" followed by a letter
            IsProductionFile = (Type == "pdf" || Type == "x_t") && !string.IsNullOrEmpty(Rev) && Regex.IsMatch(Rev, "^[A-Z]$");
        }

        // Method to return file information as a formatted string
        public string GetFileInfo()
        {
            return $"File: {FileName}, Type: {Type}, PartNumber: {PartNumber}, Revision: {Rev}, IsProductionFile: {IsProductionFile}";
        }

        // Override ToString for debugging purposes
        public override string ToString()
        {
            return GetFileInfo();
        }
    }
}
