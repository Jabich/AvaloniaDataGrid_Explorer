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
using static System.Net.WebRequestMethods;
using Avalonia.Controls.Shapes;
using Avalonia.Styling;
using System.Diagnostics;

namespace AvaloniaApplication1.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            Files = FileManager.GetFiles("C:\\Program Files (x86)");
            CurrentFile = new FileTreeNodeModel("C:\\Program Files (x86)", Directory.Exists("C:\\Program Files (x86)"));
            SelectPath = "C:\\Program Files (x86)";
        }

        #region FIELDS
        private static IconConverter? s_iconConverter;
        private ObservableCollection<FileTreeNodeModel>? _files;
        private FileTreeNodeModel? _currentFile;
        private string _selectPath;
        #endregion


        #region PROPERTIES
        public ObservableCollection<FileTreeNodeModel> Files { get { return _files; } set => this.RaiseAndSetIfChanged(ref _files, value); }
        public FileTreeNodeModel CurrentFile { get { return _currentFile; } set => this.RaiseAndSetIfChanged(ref _currentFile, value); }
        public string SelectPath 
        { 
            get 
            {
                return _selectPath;
            }
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
        public ICommand ForwardFileTree
        {
            get
            {
                return new ActionCommand((selectedFile) =>
                {
                    var fileElement = selectedFile as FileTreeNodeModel;
                    if (fileElement is FileTreeNodeModel && Directory.Exists(fileElement.Path))
                    {
                        CurrentFile = fileElement;
                        Files = FileManager.GetFiles(fileElement.Path);
                        SelectPath = fileElement.Path;
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
                    int countSeparators = CurrentFile.Path.Split(new string[] { "\\" }, StringSplitOptions.None).Length - 1;
                    if (countSeparators > 1)
                    {
                        string pathBackFolder = CurrentFile.Path.Substring(0, CurrentFile.Path.LastIndexOf("\\"));
                        CurrentFile = new FileTreeNodeModel(pathBackFolder, Directory.Exists(pathBackFolder));
                        Files = FileManager.GetFiles(pathBackFolder);
                        SelectPath = pathBackFolder;
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
                    var selectedFiles = FileManager.GetSelectedFiles(files);
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
