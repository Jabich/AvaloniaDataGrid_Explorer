using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using AvaloniaApplication1.Models;

namespace AvaloniaApplication1.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        //private void DoubleTapped(object sender, RoutedEventArgs e)
        //{
        //    //var a = sender as FileTreeNodeModel;
        //    if (sender is DataGrid)
        //    {
        //        new Window1().Show();
        //    }
     
        //}
    }
}
