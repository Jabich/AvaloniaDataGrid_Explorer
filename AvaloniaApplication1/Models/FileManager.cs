using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication1.Models
{
    public class FileManager
    {
        public static ObservableCollection<FileTreeNodeModel> GetFiles(string path)
        {
            var outputCollectionFiles = new ObservableCollection<FileTreeNodeModel>();
            var filePaths = Directory.GetFileSystemEntries(path);
            foreach (var file in filePaths)
            {
                outputCollectionFiles.Add(new FileTreeNodeModel(file, ((File.GetAttributes(file) & FileAttributes.Directory) == FileAttributes.Directory)));
            }

            return outputCollectionFiles;
        }
        public static ObservableCollection<FileTreeNodeModel> GetSelectedFiles(ObservableCollection<FileTreeNodeModel> files)
        {
            ObservableCollection<FileTreeNodeModel> selectedFiles = new ObservableCollection<FileTreeNodeModel>(); 
            foreach (var file in files)
            {
                if(file.IsChecked == true)
                {
                    selectedFiles.Add(file);
                }
            }
            return selectedFiles;
        }
        public static void ModifyFilesState(ObservableCollection<FileTreeNodeModel> Files, 
                                            ObservableCollection<FileTreeNodeModel> FilesView,
                                            FileTreeNodeModel CurrentFile)
        {
            string rootPathLinux = "/";
            string rootpathWindows = "C:\\Program Files (x86)";

            if(CurrentFile.Path != rootPathLinux)
            {

            }
        }

    }
}
////string directoryPath = "C:\\Temp\\MyFolder";
//string[] fileNames = Directory.GetFiles(directoryPath);
//List<string> filePaths = new List<string>();
//foreach (string fileName in fileNames)
//{
//    filePaths.Add(Path.Combine(directoryPath, fileName));
//}
