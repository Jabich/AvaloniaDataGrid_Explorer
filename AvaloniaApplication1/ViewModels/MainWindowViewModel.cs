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

namespace AvaloniaApplication1.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private static IconConverter? s_iconConverter;
        private ObservableCollection<Human> _humans;
        public ObservableCollection<FileTreeNodeModel> _files;
        public MainWindowViewModel()
        {
            Files = FileManager.GetFiles("C:\\Program Files (x86)");
            
            //MyCommand = new TestCommand1(ExecuteMyCommand);
            Humans = new ObservableCollection<Human>()
            {
                new Human("Alexey",22),
                 new Human("Roman",22),
                  new Human("Mikhail",22),
                  new Human("Pavel",22),
                  new Human("Alexander",22),
                  new Human("Sergey",22),

            };
        }
        public ObservableCollection<FileTreeNodeModel> Files { get { return _files; } set => this.RaiseAndSetIfChanged(ref _files, value); }
        public ObservableCollection<Human> Humans { get { return _humans; } set => this.RaiseAndSetIfChanged(ref _humans, value); }
        public string Greeting => "Welcome to Avalonia!";

        //public ICommand TestCommandMVVM
        //{
        //    get
        //    {
        //        return new TestCommand();
        //    }
            
        //}
        //public ICommand MyCommand { get; set; }
        //public ICommand TestCommandMVVM2
        //{
        //    get
        //    {
        //        return new TestCommand1((object a) =>
        //        {
        //            new Window1().Show();
        //        });
        //    }

        //}
        public ICommand TestDataGridCommand
        {
            get
            {
                return new TestCommand1((object a) =>
                {
                    Humans = null;
                });
            }
        }

        public string TextBoxText { get => "TEEEEST TEXT BOX"; }
        public void ExecuteMyCommand(object a)
        {
            new Window1().Show();
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

    }
}
