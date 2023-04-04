using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace AvaloniaApplication1.Models
{
    internal class MainModel 
    {
        private FileTreeNodeModel SearchElementInFileTree(FileTreeNodeModel fileTree, string selectedFilePath)
        {
            return fileTree.Path == selectedFilePath
                ? fileTree
                : SearchElementInFileTree(fileTree.Children.OrderByDescending(f => f.Path.Split('\\').Intersect(selectedFilePath.Split('\\')).Count()).FirstOrDefault(), selectedFilePath);
        }

        internal void UpdateElement(FileTreeNodeModel fileTree, FileTreeNodeModel selectedFile)
        {
            var updateFile = SearchElementInFileTree(fileTree, selectedFile.Path);
            updateFile.IsChecked = selectedFile.IsChecked;
            SetIsCheckedChildren(updateFile.Children, updateFile.IsChecked);
        }

        internal ObservableCollection<FileTreeNodeModel> GoToFolder(ObservableCollection<FileTreeNodeModel> fileTree, FileTreeNodeModel selectedFile)
        {
            foreach (var fileTreeNode in fileTree.Where(item => item.Path == selectedFile.Path))
            {
                return fileTreeNode.Children;
            }
            return null;
            //return SearchElementInFileTree(fileTree, selectedFile.Path);
        }

        internal FileTreeNodeModel GoBackFolder(FileTreeNodeModel fileTree, string currentFolder)
        {
            string? pastPath = Path.GetDirectoryName(currentFolder);
            return fileTree.Path == pastPath || fileTree.Path.Length >= pastPath.Length ? fileTree : SearchElementInFileTree(fileTree, pastPath);
        }

        internal void SetIsCheckedChildren(ObservableCollection<FileTreeNodeModel> children, bool statusChangedFile)
        {
            foreach (var child in children)
            {
                child.IsChecked = statusChangedFile;
                if (Directory.Exists(child.Path))
                    SetIsCheckedChildren(child.Children, statusChangedFile);
            }
        }
    }
}
