using System.Collections.Generic;

namespace NPILib
{
    public class Files
    {
        public List<File> FileList { get; private set; }

        public Files()
        {
            FileList = new List<File>();
        }

        // Non-recursive scan
        public void ScanFolder(string folderPath)
        {
            FileList.AddRange(FolderScanner.ScanFolder(folderPath));
        }

        // Recursive scan
        public void ScanFolderRecursively(string folderPath)
        {
            FileList.AddRange(FolderScanner.ScanFolderRecursively(folderPath));
        }

        // Display files in the console
        public void DisplayFiles()
        {
            if (FileList.Count == 0)
            {
                System.Console.WriteLine("No files found.");
                return;
            }

            foreach (var file in FileList)
            {
                System.Console.WriteLine(file.GetFileInfo());
            }
        }

        // Export the file list to a CSV
        public void ExportToCsv(string outputPath)
        {
            CSVCreator.CreateFilesCSV(outputPath, this);
        }
    }
}
