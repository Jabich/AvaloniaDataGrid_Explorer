using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics;
using AvaloniaApplication1.Models.Interfaces;
using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;

namespace AvaloniaApplication1.Models
{
    internal class FileManagerWindows : IFileManager
    {
        public void CheckAndUpdateParent(FileTreeNodeModel fileTree, FileTreeNodeModel selectedFile)
        {
            var parentFolder = SearchElementInFileTree(fileTree, Path.GetDirectoryName(selectedFile.Path));
        }

        public FileTreeNodeModel GetFileTree(string rootDirectory)
        {
            return new FileTreeNodeModel(rootDirectory, Directory.Exists(rootDirectory));
        }

        public FileTreeNodeModel SearchElementInFileTree(FileTreeNodeModel fileTree, string selectedFilePath)
        {
            if (fileTree.Path == selectedFilePath)
            {
                return fileTree;
            }
            else
            {
                return SearchElementInFileTree(fileTree.Children.OrderByDescending(f => f.Path.Split('\\').Intersect(selectedFilePath.Split('\\')).Count()).FirstOrDefault(), selectedFilePath);
            }
        }

        public void UpdateElement(FileTreeNodeModel fileTree, FileTreeNodeModel selectedFile)
        {
            var updateFile = SearchElementInFileTree(fileTree, selectedFile.Path);
            updateFile.IsChecked = selectedFile.IsChecked;
            UpdateChildrens(updateFile.Children, updateFile.IsChecked);
        }

        public FileTreeNodeModel GoToFolder(FileTreeNodeModel fileTree, FileTreeNodeModel selectedFile)
        {
            return SearchElementInFileTree(fileTree, selectedFile.Path);
        }

        public FileTreeNodeModel GoBackFolder(FileTreeNodeModel fileTree, FileTreeNodeModel currentFolder)
        {
            string pastPath = Path.GetDirectoryName(currentFolder.Path);
            return fileTree.Path == pastPath || fileTree.Path.Length >= pastPath.Length ? fileTree : SearchElementInFileTree(fileTree, pastPath);
        }

        public void UpdateChildrens(ObservableCollection<FileTreeNodeModel> children, bool statusChangedFile)
        {
            foreach (var child in children)
            {
                child.IsChecked = statusChangedFile;
                if (Directory.Exists(child.Path))
                    UpdateChildrens(child.Children, statusChangedFile);
            }
        }
    }
}
