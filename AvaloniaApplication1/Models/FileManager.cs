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
    }
}
////string directoryPath = "C:\\Temp\\MyFolder";
//string[] fileNames = Directory.GetFiles(directoryPath);
//List<string> filePaths = new List<string>();
//foreach (string fileName in fileNames)
//{
//    filePaths.Add(Path.Combine(directoryPath, fileName));
//}
