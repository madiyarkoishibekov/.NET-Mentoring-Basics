using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer
{
    class FileSystemEventArgs : EventArgs
    {
        public readonly string FileItemName;
        public bool AbortSearch;

        public FileSystemEventArgs(string fileName, bool abortSearch = false)
        {
            FileItemName = fileName;
            AbortSearch = abortSearch;
        }
    }
}
