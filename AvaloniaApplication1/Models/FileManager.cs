using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;

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
        #region Методы для поиска и изменения коллекции источника файлов
        ///// <summary>
        ///// Метод для поиска и возврата папок в файловом дереве. 
        ///// </summary>
        ///// <param name="Files"></param>
        ///// <param name="Path"></param>
        ///// <param name="PathRootDirectory"></param>
        ///// <returns></returns>
        //public static ObservableCollection<FileTreeNodeModel> SearchElementsInFileTree(ObservableCollection<FileTreeNodeModel> Files, string Path, string PathRootDirectory)
        //{
        //    string[] partsPath = Path.Split("\\");
        //    string path = "";
        //    int indexPart = 0;
        //    if(Path == PathRootDirectory)
        //    {
        //        return Files;
        //    }
        //    foreach (var part in partsPath)
        //    {
        //        if (PathRootDirectory + "\\" != path)
        //        {
        //            path += part + "\\";
        //            indexPart++;
        //        }
        //        else
        //        {
        //            return Iterate(Files, path, partsPath, indexPart);
        //        }

        //    }

        //    return new ObservableCollection<FileTreeNodeModel>();
        //}
        ///// <summary>
        ///// Рекурсивный метод, проходит по коллекции файлов. Возвращает вложенную коллекцию.
        ///// </summary>
        ///// <param name="TargetFolder"></param>
        ///// <param name="TargetPath"></param>
        ///// <param name="PartsPath"></param>
        ///// <param name="IndexPartPath"></param>
        ///// <returns></returns>
        //static ObservableCollection<FileTreeNodeModel> Iterate(ObservableCollection<FileTreeNodeModel> TargetFolder, string TargetPath, string[] PartsPath, int IndexPartPath)
        //{
        //    if (IndexPartPath == PartsPath.Length-1)
        //    {
        //        TargetPath += PartsPath[IndexPartPath]+"\\";
        //        foreach (var file in TargetFolder)
        //        {
        //            if (file.Path + "\\" == TargetPath)
        //                return file.Children;
        //        }
        //        return new ObservableCollection<FileTreeNodeModel>();
        //    }
        //    else
        //    {
        //        TargetPath += PartsPath[IndexPartPath] + "\\";
        //        IndexPartPath++;
        //        foreach (var file in TargetFolder)
        //        {
        //            if (TargetPath == file.Path + "\\")
        //            {
        //                //Path = PartsPath[IndexPartPath] + "\\";
        //                return Iterate(file.Children, TargetPath, PartsPath, IndexPartPath);
        //            }
        //        }
        //        return new ObservableCollection<FileTreeNodeModel>();
        //    }
        //}
        #endregion



        /// <summary>
        /// Метод для поиска и возврата папок в файловом дереве. 
        /// </summary>
        /// <param name="Files"></param>
        /// <param name="Path"></param>
        /// <param name="PathRootDirectory"></param>
        /// <returns></returns>
        public static T SearchElementsInFileTree<T>(ObservableCollection<FileTreeNodeModel> files, string Path, string PathRootDirectory)
        {
            string[] partsPath = Path.Split("\\");
            string path = "";
            int indexPart = 0;
            if (Path == PathRootDirectory)
            {
                if (typeof(T) == typeof(ObservableCollection<FileTreeNodeModel>))
                {
                    return (T)(object)files;
                }
            }
            foreach (var part in partsPath)
            {
                if (PathRootDirectory + "\\" != path)
                {
                    path += part + "\\";
                    indexPart++;
                }
                else if (typeof(T) == typeof(ObservableCollection<FileTreeNodeModel>))
                {
                    return (T)(object)Iterate(files, path, partsPath, indexPart).Children;
                }
                else

                {
                    return (T)(object)Iterate(files, path, partsPath, indexPart);
                }

            }
            return (T)(object)Iterate(files, path, partsPath, indexPart);
        }
        /// <summary>
        /// Рекурсивный метод, проходит по коллекции файлов. Возвращает вложенную коллекцию.
        /// </summary>
        /// <param name="TargetFolder"></param>
        /// <param name="TargetPath"></param>
        /// <param name="PartsPath"></param>
        /// <param name="IndexPartPath"></param>
        /// <returns></returns>
        static FileTreeNodeModel Iterate(ObservableCollection<FileTreeNodeModel> TargetFolder, string TargetPath, string[] PartsPath, int IndexPartPath)
        {
            if (IndexPartPath == PartsPath.Length - 1)
            {
                TargetPath += PartsPath[IndexPartPath] + "\\";
                foreach (var file in TargetFolder)
                {
                    if (file.Path + "\\" == TargetPath)
                        return file;
                }
                return null;
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
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SelectedElement"></param>
        /// <param name="Files"></param>
        /// <param name="SelectedFiles"></param>
        public static void ChangeFileSource(FileTreeNodeModel SelectedElement, ObservableCollection<FileTreeNodeModel> Files,List<string> SelectedFiles = null)
        {
            CheckAndChangedTreeParentChecked(Files, SelectedElement);
            if (Directory.Exists(SelectedElement.Path))
            {
                var SourceFileElement = SearchElementsInFileTree<FileTreeNodeModel>(Files, SelectedElement.Path, "C:\\Program Files (x86)");
                SourceFileElement.IsChecked = SelectedElement.IsChecked;
                RecursiveChangePropIsChecked(SourceFileElement.Children, SelectedElement.IsChecked);
            }
            else
            {
                var targetTreeFilesChanges = SearchElementsInFileTree<FileTreeNodeModel>(Files, SelectedElement.Path, "C:\\Program Files (x86)");
                targetTreeFilesChanges.IsChecked = SelectedElement.IsChecked;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FilesAndChildren"></param>
        /// <param name="IsChecked"></param>
        static void RecursiveChangePropIsChecked(ObservableCollection<FileTreeNodeModel> FilesAndChildren, bool IsChecked, List<string> SelectedFiles = null)
        {
            foreach (var file in FilesAndChildren)
            {
                file.IsChecked = IsChecked;
                if (Directory.Exists(file.Path))
                {
                    RecursiveChangePropIsChecked(file.Children, IsChecked);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Files"></param>
        /// <param name="SelectedFile"></param>
        static void CheckAndChangedTreeParentChecked(ObservableCollection<FileTreeNodeModel> Files, FileTreeNodeModel SelectedFile, List<string> SelectedFiles = null)
        {
            if (SelectedFile.IsChecked == false)
            {
                string awdaw = "";
                string pathParent = SelectedFile.Path;
                int quantityPartsOfPath = SelectedFile.Path.Count(n => n == '\\');
                for (int i = 0; i < quantityPartsOfPath - 2; i++)
                {
                    pathParent = pathParent.Substring(0, pathParent.LastIndexOf("\\"));
                    //var a  = SearchElementsInFileTree<FileTreeNodeModel>(Files, pathParent, "C:\\Program Files (x86)").IsChecked = false;
                    var targetTreeFilesChanges = SearchElementsInFileTree<FileTreeNodeModel>(Files, pathParent, "C:\\Program Files (x86)");
                    targetTreeFilesChanges.IsChecked = false;
                }
            }
            else
            {
                CheckAndChangeParentFolder(Files, SelectedFile);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Files"></param>
        /// <param name="SelectFile"></param>
        static void CheckAndChangeParentFolder(ObservableCollection<FileTreeNodeModel> Files, FileTreeNodeModel SelectFile, List<string> SelectedFiles = null)
        {
            var pathParent = SelectFile.Path.Substring(0, SelectFile.Path.LastIndexOf("\\"));
            var targetTreeFilesChanges = SearchElementsInFileTree<ObservableCollection<FileTreeNodeModel>>(Files, pathParent, "C:\\Program Files (x86)");
            int countCheckedFiles = 0;
            foreach (var file in targetTreeFilesChanges)
            {
                countCheckedFiles = file.IsChecked ==false ? +1 : countCheckedFiles;
            }
            if(countCheckedFiles == 0)
            {
                var parentFile = SearchElementsInFileTree<FileTreeNodeModel>(Files, pathParent, "C:\\Program Files (x86)");
                parentFile.IsChecked = true;
            }
        }
        static void FillingListSelectedElement(FileTreeNodeModel SelectedFile, List<string> SelectedFiles = null)
        {
             //SelectedFile.IsChecked == true ? SelectedFiles.Add(SelectedFile.IsChecked)
        }

    }
}

