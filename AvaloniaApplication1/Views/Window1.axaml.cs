using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace AvaloniaApplication1.Views
{
    public partial class Window1 : Window
    {
        public static string FileName;
        public Window1()
        {
            InitializeComponent();
            //var label = new Label
            //{
            //    Content = FileName,
            //};

            //// Добавление Label в Grid
            //var grid = new Grid();
            //grid.Children.Add(label);
            //Content = grid;
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            
        }
        private void doubleTab(object sender, RoutedEventArgs e)
        {
            new Window1().Show();
        }
    }
}
