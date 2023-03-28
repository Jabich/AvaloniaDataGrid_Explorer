using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication1.Models
{
    public class FileManager
    {
        /// <summary>
        /// Возвращает коллекцию файлов 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
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

        public static List<FileTreeNodeModel> GetSelectedFiles(Dictionary<string, ObservableCollection<FileTreeNodeModel>> ChangedPages)
        {
            var SelectedFiles = new List<FileTreeNodeModel>();
            foreach (var file in ChangedPages.Values)
            {

            }
            return SelectedFiles;
        }

        public static ObservableCollection<FileTreeNodeModel> SearchElementsInFileTree(ObservableCollection<FileTreeNodeModel> Files, string Path)
        {
            foreach(var file in Files)
            {
                if(file.Path == Path)
                {
                    return file.Children;
                }
                else if (file.IsDirectory)
                {
                    SearchElementsInFileTree(file.Children, Path);
                }

            }
            return null;
        }
    }
}

