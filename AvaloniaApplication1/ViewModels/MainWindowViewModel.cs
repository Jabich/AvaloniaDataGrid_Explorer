using Avalonia.Controls;
using AvaloniaApplication1.Models;
using AvaloniaApplication1.ViewModels.Commands;
using AvaloniaApplication1.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System.Drawing.Text;
using System.IO;
using System.Reactive.Linq;
using Avalonia.Data.Converters;
using Avalonia.Platform;
using Avalonia;
using System.Text.Json.Serialization;
using System.Globalization;
using Avalonia.Media.Imaging;
using Avalonia.Interactivity;
using Prism.Commands;


namespace AvaloniaApplication1.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            Files = FileManager.GetFiles("C:\\Program Files (x86)");
        }

        #region FIELDS
        private static IconConverter? s_iconConverter;
        private ObservableCollection<FileTreeNodeModel> _files;
        private FileTreeNodeModel _file;
        private object _selectedItem;
        private int _countSelectedItems;
        private List<string> _filePathsTree;
        private string _currentFolderPath;
        #endregion
       

        #region PROPERTIES
        public int CountSelectedItems { get { return _countSelectedItems; } set => this.RaiseAndSetIfChanged(ref _countSelectedItems, value); }
        public ObservableCollection<FileTreeNodeModel> Files { get { return _files; } set => this.RaiseAndSetIfChanged(ref _files, value); }

        private List<string> FilepPathsTree { get { return _filePathsTree; } set => this.RaiseAndSetIfChanged(ref _filePathsTree, value); }
        #endregion


        #region COMMANDS
        public ICommand ForwardFileTree
        {
            get
            {
                return new ActionCommand((object a) =>
                {
                    var fileElement = a as FileTreeNodeModel;
                    if (fileElement is FileTreeNodeModel && Directory.Exists(fileElement.Path))
                    {
                        Files = FileManager.GetFiles(fileElement.Path);
                        //_filePathsTree.Add(fileElement.Path);
                    }
                });
            }
        }
        public ICommand BackFileTree
        {
            get
            {
                return new ActionCommand((object a) =>
                {
                    if (_filePathsTree.Count >= 2)
                    {
                        foreach(string item in _filePathsTree)
                        {
                            //if(i)
                        }
                    }
                });
            }
        }
        public ICommand SelectFile
        {
            get
            {
                return new ActionCommand((object a) =>
                {
                    var fileElement = a as FileTreeNodeModel;
                    if (fileElement is FileTreeNodeModel && Directory.Exists(fileElement.Path))
                    {
                        Files = FileManager.GetFiles(fileElement.Path);
                        _filePathsTree.Add(fileElement.Path);
                        _currentFolderPath= fileElement.Path;
                    }
                });
            }
        }

        #endregion

        #region METHODS

        #endregion

        #region CONVERTERS 
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
