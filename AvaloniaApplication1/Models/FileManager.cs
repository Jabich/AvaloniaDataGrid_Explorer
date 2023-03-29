using DynamicData;
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
        /// <summary>
        /// Метод для поиска и возврата папок в файловом дереве. 
        /// </summary>
        /// <param name="Files"></param>
        /// <param name="Path"></param>
        /// <param name="PathRootDirectory"></param>
        /// <returns></returns>
        public static ObservableCollection<FileTreeNodeModel> SearchElementsInFileTree(ObservableCollection<FileTreeNodeModel> Files, string Path, string PathRootDirectory)
        {
            string[] partsPath = Path.Split("\\");
            string path = "";
            int indexPart = 0;
            if(Path == PathRootDirectory)
            {
                return Files;
            }
            foreach (var part in partsPath)
            {
                if (PathRootDirectory + "\\" != path)
                {
                    path += part + "\\";
                    indexPart++;
                }
                else
                {
                    return Iterate(Files, path, partsPath, indexPart);
                }

            }
           
            return new ObservableCollection<FileTreeNodeModel>();
        }
        /// <summary>
        /// Рекурсивный метод, проходит по коллекции файлов. Возвращает вложенную коллекцию.
        /// </summary>
        /// <param name="TargetFolder"></param>
        /// <param name="TargetPath"></param>
        /// <param name="PartsPath"></param>
        /// <param name="IndexPartPath"></param>
        /// <returns></returns>
        static ObservableCollection<FileTreeNodeModel> Iterate(ObservableCollection<FileTreeNodeModel> TargetFolder, string TargetPath, string[] PartsPath, int IndexPartPath)
        {
            if (IndexPartPath == PartsPath.Length-1)
            {
                TargetPath += PartsPath[IndexPartPath]+"\\";
                foreach (var file in TargetFolder)
                {
                    if (file.Path + "\\" == TargetPath)
                        return file.Children;
                }
                return new ObservableCollection<FileTreeNodeModel>();
            }
            else
            {
                TargetPath += PartsPath[IndexPartPath] + "\\";
                IndexPartPath++;
                foreach (var file in TargetFolder)
                {
                    if (TargetPath == file.Path + "\\")
                    {
                        //Path = PartsPath[IndexPartPath] + "\\";
                        return Iterate(file.Children, TargetPath, PartsPath, IndexPartPath);
                    }
                }
                return new ObservableCollection<FileTreeNodeModel>();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static void ChangeFileSource(ObservableCollection<FileTreeNodeModel> Files, ObservableCollection<FileTreeNodeModel> FilesView, FileTreeNodeModel CurrentFile)
        {
            var folderToChange =  SearchElementsInFileTree(Files, CurrentFile.Path, "C:\\Program Files (x86)");
            folderToChange = FilesView;
        }
    }
}

