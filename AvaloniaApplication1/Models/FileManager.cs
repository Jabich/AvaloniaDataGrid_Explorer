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
        public static ObservableCollection<FileTreeNodeModel> GetFiles(string path,
        Dictionary<string, ObservableCollection<FileTreeNodeModel>> ChangedPages = null)
        {
            if (ChangedPages != null)
            {
                foreach (var item in ChangedPages)
                {
                    if (item.Key == path)
                    {
                        return ChangedPages[path];
                    }

                }
            }
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
        /// Возвращает True если коллекция файлов была изменена, False если НЕ была изменена
        /// </summary>
        /// <param name="Files"></param>
        /// <returns></returns>
        public static bool CheckChangeFiles(ObservableCollection<FileTreeNodeModel> Files)
        {
            foreach (var file in Files)
            {
                if (file.IsChecked != false)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Добавляет в словарь измененные страницы 
        /// </summary>
        /// <param name="ChangedPages"></param>
        public static void AddChangePages(Dictionary<string, ObservableCollection<FileTreeNodeModel>> ChangedPages,
            ObservableCollection<FileTreeNodeModel> Files, FileTreeNodeModel CurrentFile)
        {
            if (!ChangedPages.ContainsKey(CurrentFile.Path))
            {
                ChangedPages.Remove(CurrentFile.Path);
                ChangedPages.Add(CurrentFile.Path, Files);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Files"></param>
        public static Dictionary<string, ObservableCollection<FileTreeNodeModel>> ChangeChildren(ObservableCollection<FileTreeNodeModel> Files)
        {
            var changeChildrensDictionary = new Dictionary<string, ObservableCollection<FileTreeNodeModel>>();
            foreach (var file in Files)
            {
                if (file.IsChecked == true && file.IsDirectory)
                {
                    foreach (var children in file.Children)
                    {
                        children.IsChecked = true;
                        if (children.IsDirectory)
                        {
                            UpdatePropIsChecked(children.Children);
                            changeChildrensDictionary.Add(children.Path, children.Children);
                        }
                    }
                }
            }
            return changeChildrensDictionary;
        }

        static void UpdatePropIsChecked(ObservableCollection<FileTreeNodeModel> Files)
        {
            foreach (var file in Files)
            {
                file.IsChecked = true;
            }
        }
        static void AddModifPagesToDictionary()
        {

        }

    }
}

