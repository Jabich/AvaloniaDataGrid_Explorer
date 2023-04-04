using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvaloniaApplication1.Models.Interfaces;

namespace AvaloniaApplication1.Models
{
    internal class FileManagerLinux : IFileManager
    {
        public void CheckAndUpdateParent(FileTreeNodeModel fileTree)
        {
            throw new NotImplementedException();
        }

        public void CheckAndUpdateParent(FileTreeNodeModel fileTree, FileTreeNodeModel selectedFile)
        {
            throw new NotImplementedException();
        }

        public FileTreeNodeModel GetFileTree(string rootDirectory)
        {
            throw new NotImplementedException();
        }

        public FileTreeNodeModel GoBackFolder(FileTreeNodeModel fileTree, FileTreeNodeModel currentFolder)
        {
            throw new NotImplementedException();
        }

        public FileTreeNodeModel GoToFolder(FileTreeNodeModel fileTree, FileTreeNodeModel selectedFile)
        {
            throw new NotImplementedException();
        }

        public FileTreeNodeModel SearchElementInFileTree(FileTreeNodeModel fileTree, string selectedFilePath)
        {
            throw new NotImplementedException();
        }

        public void UpdateChildrens(ObservableCollection<FileTreeNodeModel> children, bool statusChangedFile)
        {
            throw new NotImplementedException();
        }

        public void UpdateElement(FileTreeNodeModel fileTree, FileTreeNodeModel selectedFile)
        {
            throw new NotImplementedException();
        }

        public void UpdateElement(FileTreeNodeModel fileTree, bool statusChangedFile, FileTreeNodeModel selectedFile = null)
        {
            throw new NotImplementedException();
        }
    }
}
