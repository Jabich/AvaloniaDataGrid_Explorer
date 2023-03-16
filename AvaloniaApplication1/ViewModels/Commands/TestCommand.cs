using AvaloniaApplication1.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AvaloniaApplication1.ViewModels.Commands
{
    public class TestCommand : ICommand
    {
        
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            new Window1().Show();
        }
    }
    //private Action<object> execute;
    //private Func<object, bool> canExecute;

    ////CommandManager - не определяется
    //public event EventHandler? CanExecuteChanged
    //{
    //    add { }
    //    remove { }
    //}
    //public TestCommand(Action<object> execute, Func<object, bool> canExecute = null)
    //{
    //    this.execute = execute;
    //    this.canExecute = canExecute;
    //}

    //public bool CanExecute(object? parameter)
    //{
    //    return this.canExecute == null || this.canExecute(parameter);
    //}

    //public void Execute(object? parameter)
    //{
    //    this.execute(parameter);
    //}
}
