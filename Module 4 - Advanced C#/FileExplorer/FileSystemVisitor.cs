using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer
{
    internal class FileSystemVisitor
    {
        #region Properties
        public string filePath = DefaultFolder;
        public const string DefaultFolder = "D:";
        public bool isFile = false;
        private bool _abortSearch;
        public string currentlySelectedFileName = "";
        private Func<string, bool> filter;

        public event EventHandler SearchStarted;
        public event EventHandler SearchEnded;
        public event EventHandler<FileSystemEventArgs> FileFound;
        public event EventHandler<FileSystemEventArgs> DirectoryFound;
        public event EventHandler<FileSystemEventArgs> FilteredFileFound;
        public event EventHandler<FileSystemEventArgs> FilteredDirectoryFound;
        #endregion

        #region Constructors
        public FileSystemVisitor(string? startingFolder = null)
        {
            if (!string.IsNullOrEmpty(startingFolder))
            {
                filePath = startingFolder;
            }
        }

        public FileSystemVisitor(Func<string, bool> predicate, string? startingFolder = null)  : this(startingFolder)
        {
            filter = predicate;
        }
        #endregion

        #region Events
        protected virtual void OnSearchStarted(EventArgs e)
        {
            SearchStarted?.Invoke(this, e);
        }

        protected virtual void OnSearchEnded(EventArgs e)
        {
            SearchEnded?.Invoke(this, e);
        }

        protected virtual void OnFileFound(FileSystemEventArgs e)
        {
            FileFound?.Invoke(this, e);
        }

        protected virtual void OnDirectoryFound(FileSystemEventArgs e)
        {
            DirectoryFound?.Invoke(this, e);
        }

        protected virtual void OnFilteredFileFound(FileSystemEventArgs e)
        {
            FilteredFileFound?.Invoke(this, e);
        }

        protected virtual void OnFilteredDirectoryFound(FileSystemEventArgs e)
        {
            FilteredDirectoryFound?.Invoke(this, e);
        }
        #endregion

        #region Methods
        public void LoadFilesAndDirectories(Label fileName, Label fileType, ListView listView)
        {
            DirectoryInfo fileList;
            string tempFilePath = "";
            FileAttributes fileAttributes;
            try
            {
                if (isFile)
                {
                    tempFilePath = filePath + "/" + currentlySelectedFileName;
                    FileInfo fileDetails = new FileInfo(tempFilePath);
                    fileName.Text = fileDetails.Name;
                    fileType.Text = fileDetails.Extension;
                    fileAttributes = File.GetAttributes(tempFilePath);
                    var startInfo = new ProcessStartInfo(tempFilePath)
                    {
                        UseShellExecute = true
                    };
                    Process.Start(startInfo);
                }
                else
                {
                    fileAttributes = File.GetAttributes(filePath);

                }

                if ((fileAttributes & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    fileList = new DirectoryInfo(filePath);
                    FileInfo[] files = fileList.GetFiles();
                    DirectoryInfo[] dirs = fileList.GetDirectories();

                    listView.Items.Clear();
                    foreach (var file in files)
                    {
                        OnFileFound(new FileSystemEventArgs(file.Name));
                        if (filter == null || (filter != null && filter(file.Name)))
                        {
                            if (filter != null)
                            {
                                var args = new FileSystemEventArgs(file.Name);
                                OnFilteredFileFound(args);
                            }
                        }
                        listView.Items.Add(file.Name, 2);
                    }

                    foreach (var dir in dirs)
                    {
                        OnDirectoryFound(new FileSystemEventArgs(dir.Name));
                        OnFilteredDirectoryFound(new FileSystemEventArgs(dir.Name));
                        listView.Items.Add(dir.Name, 0);
                    }

                    if (filePath == DefaultFolder)
                    {
                        OnSearchEnded(EventArgs.Empty);
                    }
                }
                else
                {
                    fileName.Text = this.currentlySelectedFileName;
                }
            }
            catch (Exception e)
            {

            }
        }

        public void RemoveBackSlash(TextBox filePathTxt)
        {
            string path = filePathTxt.Text;
            if (path.LastIndexOf("/") == path.Length - 1)
            {
                filePathTxt.Text = path.Substring(0, path.Length - 1);
            }
        }

        public void GoBack(TextBox filePathTxt)
        {
            try
            {
                RemoveBackSlash(filePathTxt);
                string path = filePathTxt.Text;
                path = path.Substring(0, path.LastIndexOf("/"));
                this.isFile = false;
                filePathTxt.Text = path;
                RemoveBackSlash(filePathTxt);
            }
            catch (Exception e)
            {

                throw;
            }
        }
        #endregion

        public IEnumerable<string> GetFilesAndFolders()
        {
            _abortSearch = false;
            OnSearchStarted(EventArgs.Empty);
            return GetFilesAndFolders(DefaultFolder);
        }

        public IEnumerable<string> GetAllFiles(string path)
        {
            try
            {
                return Directory.GetFiles(path);
            }
            catch (UnauthorizedAccessException e)
            {
                return Enumerable.Empty<string>();
            }
        }

        public IEnumerable<string> GetAllDirectories(string path)
        {
            try
            {
                return Directory.GetDirectories(path);
            }
            catch (UnauthorizedAccessException e)
            {
                return Enumerable.Empty<string>();
            }
        }

        public IEnumerable<string> GetFilesAndFolders(string parentDirectory)
        {
            // Searches for files in current folder.
            foreach (var fullFileName in GetAllFiles(parentDirectory))
            {
                var fileName = Path.GetFileName(fullFileName);
                OnFileFound(new FileSystemEventArgs(fileName));
                if (filter == null || (filter != null && filter(fileName)))
                {
                    if (filter != null)
                    {
                        var args = new FileSystemEventArgs(fileName);
                        OnFilteredFileFound(args);
                        // Handles response from event handler.
                        if (args.AbortSearch)
                        {
                            _abortSearch = true;
                        }
                    }

                    yield return fileName;

                    if (_abortSearch)
                    {
                        break;
                    }
                }
            }

            // Searches for folders in current folder.
            foreach (var directoryName in GetAllDirectories(parentDirectory))
            {
                if (_abortSearch)
                {
                    break;
                }
                OnDirectoryFound(new FileSystemEventArgs(directoryName));
                OnFilteredDirectoryFound(new FileSystemEventArgs(directoryName));
                yield return directoryName;
                foreach (var fileItem in GetFilesAndFolders(directoryName))
                {
                    yield return fileItem;
                }
            }

            // Checks if we are at the top of recursion and the method is about to finish.
            if (parentDirectory == DefaultFolder)
            {
                OnSearchEnded(EventArgs.Empty);
            }
        }
    }
}
