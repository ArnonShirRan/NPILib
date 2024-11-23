using System;

namespace NPILib
{
    public class File
    {
        public string Path { get; private set; }
        public string FileName { get; private set; }
        public string Type { get; private set; }
        public bool IsProductionFile { get; private set; }
        public string PartNumber { get; private set; }
        public string Rev { get; private set; }
        public bool IsValid { get; private set; } // New property

        public File(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Path cannot be null or empty.", nameof(path));

            Path = path;
            FileName = System.IO.Path.GetFileName(path);
            IsValid = FileNameValidator.IsValidFileName(FileName); // Validate file name
            Type = GetFileType(path);
            SetPartNumberAndRev(path);
            SetIsProductionFile();
        }

        private string GetFileType(string path)
        {
            string extension = System.IO.Path.GetExtension(path);
            return string.IsNullOrWhiteSpace(extension) ? "unknown" : extension.TrimStart('.').ToLower();
        }

        private void SetPartNumberAndRev(string path)
        {
            string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(path);

            var match = System.Text.RegularExpressions.Regex.Match(fileNameWithoutExtension, @"_Rev([A-Z])");
            if (match.Success)
            {
                PartNumber = fileNameWithoutExtension.Substring(0, match.Index);
                Rev = match.Groups[1].Value;
            }
            else
            {
                PartNumber = fileNameWithoutExtension;
                Rev = "N/A";
            }
        }

        private void SetIsProductionFile()
        {
            IsProductionFile = (Type == "pdf" || Type == "x_t") && !string.IsNullOrEmpty(Rev) && System.Text.RegularExpressions.Regex.IsMatch(Rev, "^[A-Z]$");
        }

        public string GetFileInfo()
        {
            return $"File: {FileName}, Type: {Type}, PartNumber: {PartNumber}, Revision: {Rev}, IsProductionFile: {IsProductionFile}, IsValid: {IsValid}";
        }

        public override string ToString()
        {
            return GetFileInfo();
        }
    }
}
