using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication1.Models.Interfaces
{
    public interface IFileManager
    {
        FileTreeNodeModel SearchElementInFileTree(FileTreeNodeModel fileTree, string selectedFilePath);
        void CheckAndUpdateParent(FileTreeNodeModel fileTree, FileTreeNodeModel asjdasj);
        FileTreeNodeModel GetFileTree(string rootDirectory);
        void UpdateElement(FileTreeNodeModel fileTree, FileTreeNodeModel selectedFile);
        FileTreeNodeModel GoToFolder(FileTreeNodeModel fileTree, FileTreeNodeModel selectedFile);
        FileTreeNodeModel GoBackFolder(FileTreeNodeModel fileTree, FileTreeNodeModel currentFolder);
        void UpdateChildrens(ObservableCollection<FileTreeNodeModel> children, bool statusChangedFile);
    }
}
