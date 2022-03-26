using System.Diagnostics;
using System.Runtime.InteropServices;

namespace FileExplorer
{
    public partial class Form1 : Form
    {

        FileSystemVisitor fileSystemVisitor = new FileSystemVisitor();
        private OutputGenerator _output;

        public Form1()
        {
            InitializeComponent();
        }

        #region Methods
        public void StartProgram()
        {
            _output = new OutputGenerator();
            Func<string, bool> filter1 = str => str[0] == 'a';
            Func<string, bool> filter2 = str => str[0] == 'b';
            Func<string, bool> filter3 = str => str[0] >= 'c';
            var predicates = new List<Func<string, bool>>() { filter1, filter2, filter3 };

            // Unfiltered search.
            InitializeVisitor(fileSystemVisitor);
            _output.WriteHeader($"Unfiltered contents of {fileSystemVisitor.filePath}", false);
            foreach (var fileItem in fileSystemVisitor.GetFilesAndFolders())
            {
                _output.WriteFileItem(fileItem);
            }

            // Search with different filters.
            foreach (var predicate in predicates)
            {
                var filesVisitorWithFilter = new FileSystemVisitor(predicate, FileSystemVisitor.DefaultFolder);
                InitializeVisitor(filesVisitorWithFilter);
                _output.WriteHeader($"Contents of {FileSystemVisitor.DefaultFolder} filtered with filter{predicates.IndexOf(predicate) + 1}", true);
                foreach (var fileItem in filesVisitorWithFilter.GetFilesAndFolders())
                {
                    _output.WriteFileItem(fileItem);
                }
            }
        }

        private void LoadBtnAction()
        {
            fileSystemVisitor.filePath = filePathTxt.Text;
            fileSystemVisitor.LoadFilesAndDirectories(fileNameLabel, fileTypeLabel, listView1);
            fileSystemVisitor.isFile = false;

        }
        #endregion


        #region Events
        /// <summary>
        /// Registers methods for event handlers.
        /// </summary>
        /// <param name="visitor"></param>


        private void Form1_Load_1(object sender, EventArgs e)
        {
            AllocConsole();
            StartProgram();
            filePathTxt.Text = fileSystemVisitor.filePath;
            fileSystemVisitor.LoadFilesAndDirectories(fileNameLabel, fileTypeLabel, listView1);
        }

        private void goBtn_Click(object sender, EventArgs e)
        {
            LoadBtnAction();
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            fileSystemVisitor.currentlySelectedFileName = e.Item.Text;

            FileAttributes fileAttributes = File.GetAttributes(fileSystemVisitor.filePath + "/" + fileSystemVisitor.currentlySelectedFileName);
            if ((fileAttributes & FileAttributes.Directory) == FileAttributes.Directory)
            {
                fileSystemVisitor.isFile = false;
                filePathTxt.Text = fileSystemVisitor.filePath + "/" + fileSystemVisitor.currentlySelectedFileName;
            }
            else
            {
                fileSystemVisitor.isFile = true;
            }

        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            LoadBtnAction();
        }

        private void backBtn_Click(object sender, EventArgs e)
        {
            fileSystemVisitor.GoBack(filePathTxt);
            LoadBtnAction();
        }

        private void InitializeVisitor(FileSystemVisitor visitor)
        {
            visitor.SearchStarted += FileSystemVisitor_SearchStarted;
            visitor.SearchEnded += FileSystemVisitor_SearchEnded;
            visitor.FileFound += FileSystemVisitor_FileFound;
            visitor.DirectoryFound += FileSystemVisitor_DirectoryFound;
            visitor.FilteredFileFound += FileSystemVisitor_FilteredFileFound;
            visitor.FilteredDirectoryFound += FileSystemVisitor_FilteredDirectoryFound;
        }

        private void FileSystemVisitor_SearchStarted(object source, EventArgs e)
        {
            _output.WriteEventMessage($"Search started.");
        }

        private void FileSystemVisitor_SearchEnded(object source, EventArgs e)
        {
            _output.WriteEventMessage($"Search ended.");
        }

        private void FileSystemVisitor_FileFound(object source, FileSystemEventArgs e)
        {
            _output.WriteEventMessage($"Found unfiltered file {e.FileItemName}");
        }

        private void FileSystemVisitor_DirectoryFound(object source, FileSystemEventArgs e)
        {
            _output.WriteEventMessage($"Found unfiltered directory {e.FileItemName}");
        }

        private void FileSystemVisitor_FilteredFileFound(object source, FileSystemEventArgs e)
        {
            _output.WriteEventMessage($"Found filtered file {e.FileItemName}");
            e.AbortSearch = true;
        }

        private void FileSystemVisitor_FilteredDirectoryFound(object source, FileSystemEventArgs e)
        {
            _output.WriteEventMessage($"Found filtered directory (they are always filtered) {e.FileItemName}");
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        #endregion
    }
}