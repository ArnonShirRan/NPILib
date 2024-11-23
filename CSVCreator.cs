using System.IO;

namespace NPILib
{
    public static class CSVCreator
    {
        public static void CreateFilesCSV(string outputPath, Files filesInstance)
        {
            if (filesInstance == null)
                throw new System.ArgumentNullException(nameof(filesInstance));

            if (string.IsNullOrWhiteSpace(outputPath))
                throw new System.ArgumentException("Output path cannot be null or empty.", nameof(outputPath));

            if (!Directory.Exists(Path.GetDirectoryName(outputPath)))
                throw new DirectoryNotFoundException($"The directory '{Path.GetDirectoryName(outputPath)}' does not exist.");

            using (var writer = new StreamWriter(outputPath))
            {
                writer.WriteLine("FileName,FullPath,Type,PartNumber,Revision,IsProductionFile");

                foreach (var file in filesInstance.FileList)
                {
                    if (!file.IsValid) continue; // Skip invalid files

                    writer.WriteLine($"{EscapeCsvField(file.FileName)}," +
                                     $"{EscapeCsvField(file.Path)}," +
                                     $"{EscapeCsvField(file.Type)}," +
                                     $"{EscapeCsvField(file.PartNumber)}," +
                                     $"{EscapeCsvField(file.Rev)}," +
                                     $"{EscapeCsvField(file.IsProductionFile.ToString())}");
                }
            }

            System.Console.WriteLine($"CSV file successfully created at: {outputPath}");
        }

        private static string EscapeCsvField(string field)
        {
            if (string.IsNullOrWhiteSpace(field)) return string.Empty;

            if (field.Contains(",") || field.Contains("\"") || field.Contains(" "))
            {
                field = $"\"{field.Replace("\"", "\"\"")}\"";
            }

            return field;
        }
    }
}
