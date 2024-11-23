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

        // Add scanned files to the list
        public void AddFiles(IEnumerable<File> files)
        {
            FileList.AddRange(files);
        }
    }
}
