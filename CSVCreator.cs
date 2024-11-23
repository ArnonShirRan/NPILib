using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NPILib
{
    public class CSVCreator
    {
        // Path where the CSV will be saved
        public string FilePath { get; private set; }

        // Constructor to set the file path
        public CSVCreator(string filePath)
        {
            FilePath = filePath;
        }

        // Method to create a CSV file from a list of data
        public void CreateCSV<T>(IEnumerable<T> data, string[] headers)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (headers == null || headers.Length == 0) throw new ArgumentException("Headers cannot be null or empty", nameof(headers));

            using (var writer = new StreamWriter(FilePath))
            {
                // Write headers
                writer.WriteLine(string.Join(",", headers));

                // Write data
                foreach (var item in data)
                {
                    var row = string.Join(",", item.GetType().GetProperties().Select(p => p.GetValue(item, null)?.ToString() ?? ""));
                    writer.WriteLine(row);
                }
            }
        }

        // Static method to create a CSV file for a Files object
        public static void CreateFilesCSV(string outputPath, Files filesInstance)
        {
            if (filesInstance == null)
                throw new ArgumentNullException(nameof(filesInstance));

            if (string.IsNullOrWhiteSpace(outputPath))
                throw new ArgumentException("Output path cannot be null or empty.", nameof(outputPath));

            if (!Directory.Exists(Path.GetDirectoryName(outputPath)))
                throw new DirectoryNotFoundException($"The directory '{Path.GetDirectoryName(outputPath)}' does not exist.");

            try
            {
                using (var writer = new StreamWriter(outputPath))
                {
                    // Write CSV header
                    writer.WriteLine("FileName,Type,PartNumber,Revision,IsProductionFile");

                    // Write each file's properties as a CSV row
                    foreach (var file in filesInstance.FileList)
                    {
                        writer.WriteLine($"{EscapeCsvField(Path.GetFileName(file.Path))}," +
                                         $"{EscapeCsvField(file.Type)}," +
                                         $"{EscapeCsvField(file.PartNumber)}," +
                                         $"{EscapeCsvField(file.Rev)}," +
                                         $"{EscapeCsvField(file.IsProductionFile.ToString())}");
                    }
                }

                Console.WriteLine($"CSV file successfully created at: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating CSV file: {ex.Message}");
            }
        }

        // Private helper method to escape and sanitize CSV fields
        private static string EscapeCsvField(string field)
        {
            if (string.IsNullOrWhiteSpace(field))
                return string.Empty;

            // Replace newlines, carriage returns, and tabs with spaces
            field = field.Replace("\r", " ").Replace("\n", " ").Replace("\t", " ");

            // If the field contains commas, double quotes, or spaces, wrap it in double quotes
            if (field.Contains(",") || field.Contains("\"") || field.Contains(" "))
            {
                // Escape double quotes by doubling them
                field = $"\"{field.Replace("\"", "\"\"")}\"";
            }

            return field;
        }
    }
}
