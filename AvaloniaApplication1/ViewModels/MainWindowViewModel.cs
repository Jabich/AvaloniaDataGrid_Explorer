using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using AvaloniaApplication1.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;

namespace AvaloniaApplication1.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region FIELDS
        private static string rootFolder = "C:\\Program Files (x86)";
        private static IconConverter? s_iconConverter;
        private FileTreeNodeModel? _fileTree;
        private string _pathSelectedFolder;
        private ObservableCollection<FileTreeNodeModel>? _filesView;
        private MainModel _mainModel = new();
        #endregion

        #region PROPERTIES
        public FileTreeNodeModel FileTree { get => _fileTree; set => this.RaiseAndSetIfChanged(ref _fileTree, value); }
        public ObservableCollection<FileTreeNodeModel>? FilesView { get => _filesView; set => this.RaiseAndSetIfChanged(ref _filesView, value); }
        public string PathSelectedFolder { get => _pathSelectedFolder; set => this.RaiseAndSetIfChanged(ref _pathSelectedFolder, value); }
        public static IMultiValueConverter FileIconConverter
        {
            get
            {
                if (s_iconConverter is null)
                {
                    var assetLoader = AvaloniaLocator.Current.GetRequiredService<IAssetLoader>();

                    using (var fileStream = assetLoader.Open(new Uri("avares://AvaloniaApplication1/Assets/file.png")))
                    using (var folderStream = assetLoader.Open(new Uri("avares://AvaloniaApplication1/Assets/folder.png")))
                    using (var folderOpenStream = assetLoader.Open(new Uri("avares://AvaloniaApplication1/Assets/folder.png")))
                    {
                        var fileIcon = new Bitmap(fileStream);
                        var folderIcon = new Bitmap(folderStream);
                        var folderOpenIcon = new Bitmap(folderOpenStream);

                        s_iconConverter = new IconConverter(fileIcon, folderOpenIcon, folderIcon);
                    }
                }

                return s_iconConverter;
            }
        }

        #endregion

        public MainWindowViewModel()
        {
            FileTree = new FileTreeNodeModel(rootFolder, Directory.Exists(rootFolder));
            PathSelectedFolder = rootFolder;
            FilesView = FileTree.Children;
        }

        #region COMMANDS
        public void GoToFolder(FileTreeNodeModel selectedFile)
        {
            if (Directory.Exists(selectedFile.Path))
            {
                FilesView = _mainModel.GoToFolder(FilesView, selectedFile);
                PathSelectedFolder = selectedFile.Path;
            }
        }
        public void GoBackFolder(FileTreeNodeModel selectedFile)
        {
            var file = _mainModel.GoBackFolder(FileTree, PathSelectedFolder);
            FilesView = file.Children;
            PathSelectedFolder = file.Path;
        }
        public void SelectFile(FileTreeNodeModel selectedFile)
        {
            _mainModel.UpdateElement(FileTree, selectedFile);
        }
        public void CloseWindow(Window window)
        {
            window.Close();
        }
        public void GenerateAndSaveFileList(Window window)
        {
            window.Close();
        }
        #endregion

        #region CONVERTERS 
        private class IconConverter : IMultiValueConverter
        {
            private readonly Bitmap _file;
            private readonly Bitmap _folderExpanded;
            private readonly Bitmap _folderCollapsed;

            public IconConverter(Bitmap file, Bitmap folderExpanded, Bitmap folderCollapsed)
            {
                _file = file;
                _folderExpanded = folderExpanded;
                _folderCollapsed = folderCollapsed;
            }

            public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
            {
                if (values.Count == 2 &&
                values[0] is bool isDirectory &&
                values[1] is bool isExpanded)
                {
                    if (!isDirectory)
                        return _file;
                    else
                        return isExpanded ? _folderExpanded : _folderCollapsed;
                }
                return null;
            }
        }
        #endregion
    }
}
