using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using AvaloniaApplication1.Models;
using AvaloniaApplication1.ViewModels.Commands;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Windows.Input;

namespace AvaloniaApplication1.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            Files = FileManager.GetFiles("C:\\Program Files (x86)");
            CurrentFile = new FileTreeNodeModel("C:\\Program Files (x86)", Directory.Exists("C:\\Program Files (x86)"));
            SelectPath = "C:\\Program Files (x86)";
            FilesView = FileManager.GetFiles("C:\\Program Files (x86)");
        }

        #region FIELDS
        private static IconConverter? s_iconConverter;
        private ObservableCollection<FileTreeNodeModel>? _files;
        private FileTreeNodeModel? _currentFile;
        private string _selectPath;
        private ObservableCollection<FileTreeNodeModel>? _filesView;
        #endregion


        #region PROPERTIES
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<FileTreeNodeModel> Files { get { return _files; } set => this.RaiseAndSetIfChanged(ref _files, value); }
        public ObservableCollection<FileTreeNodeModel> FilesView { get { return _filesView; } set => this.RaiseAndSetIfChanged(ref _filesView, value); }
        public FileTreeNodeModel CurrentFile { get { return _currentFile; } set => this.RaiseAndSetIfChanged(ref _currentFile, value); }
        public string SelectPath
        {
            get => _selectPath;
            set => this.RaiseAndSetIfChanged(ref _selectPath, value);
        }
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


        #region COMMANDS
        public void ForwardFileTree(FileTreeNodeModel selectedFile)
        {
            if (Directory.Exists(selectedFile.Path))
            {
                try 
                {
                    FilesView = FileManager.SearchElementsInFileTree<ObservableCollection<FileTreeNodeModel>>(Files, selectedFile.Path, "C:\\Program Files (x86)");
                }
                catch
                {
                    FilesView = new ObservableCollection<FileTreeNodeModel>() { };
                }

                CurrentFile = selectedFile;
                SelectPath = selectedFile.Path;
            }
        }
        public ICommand BackFileTree
        {
            get
            {
                return new ActionCommand((object a) =>
                {
                    int countSeparators = CurrentFile.Path.Split(new string[] { "\\" }, StringSplitOptions.None).Length - 1;
                    if (countSeparators > 1)
                    { 
                        string pathBackFolder = CurrentFile.Path.Substring(0, CurrentFile.Path.LastIndexOf("\\"));
                        CurrentFile = new FileTreeNodeModel(pathBackFolder, Directory.Exists(pathBackFolder));
                        try
                        {
                            FilesView = FileManager.SearchElementsInFileTree<ObservableCollection<FileTreeNodeModel>>(Files, pathBackFolder, "C:\\Program Files (x86)");
                        }
                        catch
                        {
                            FilesView = new ObservableCollection<FileTreeNodeModel>(){ };
                        }
                        SelectPath = pathBackFolder;
                    }
                });
            }
        }

        public ICommand ClickCheckBoxCommand
        {
            get
            {
                return new ActionCommand((obj) =>
                {
                    var file = obj as FileTreeNodeModel;
                    try
                    {
                        FileManager.ChangeFileSource(file, Files);
                    }
                    catch
                    {

                    }
              
                });
            }
        }
        public ICommand FileSelection
        {
            get
            {
                return new ActionCommand((obj) =>
                {
                    var files = (obj as MainWindowViewModel).Files;
                });
            }
        }
        public ICommand CloseWindow
        {
            get
            {
                return new ActionCommand((obj) =>
                {
                    (obj as Window).Close();
                });
            }
        }
        #endregion


        #region METHODS

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
