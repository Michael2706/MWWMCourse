using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MWWMCourse
{
    public class ViewModel : ViewModelBase
    {
        private string _text;
        private ICommand hiButtonCommand;

        private ICommand toggleExecuteCommand { get; set; }

        private bool canExecute = true;
        public string Text { get => _text; set { _text = value; OnPropertyChanged(); } }

        public ViewModel()
        {
            Text = "Hallo Welt";
           // HiButtonCommand = new RelayCommand(ShowMessage, param => this.canExecute);
            HiButtonCommand = new RelayCommand(ChangeText, param => this.canExecute);

        }

        public string HiButtonContent
        {
            get
            {
                return "click to hi";
            }
        }

        public void ShowMessage(object obj)
        {
            MessageBox.Show(obj.ToString());
        }

        public ICommand HiButtonCommand
        {
            get
            {
                return hiButtonCommand;
            }
            set
            {
                hiButtonCommand = value;
                
            }
        }

        public void ChangeText(object tt)
        {
            Text = "Bye bye";
        }

        public string ButtonContent
        {
            get
            {
                return "Click Me";
            }
        }

      


    }

  

    public class ViewModelBase: INotifyPropertyChanged, INotifyPropertyChanging
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }


   

    public class RelayCommand : ICommand
    {
        private Action<object> execute;

        private Predicate<object> canExecute;

        private event EventHandler CanExecuteChangedInternal;

        public RelayCommand(Action<object> execute)
            : this(execute, DefaultCanExecute)
        {
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            if (canExecute == null)
            {
                throw new ArgumentNullException("canExecute");
            }

            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                this.CanExecuteChangedInternal += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
                this.CanExecuteChangedInternal -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute != null && this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }

        public void OnCanExecuteChanged()
        {
            EventHandler handler = this.CanExecuteChangedInternal;
            if (handler != null)
            {
                //DispatcherHelper.BeginInvokeOnUIThread(() => handler.Invoke(this, EventArgs.Empty));
                handler.Invoke(this, EventArgs.Empty);
            }
        }

        public void Destroy()
        {
            this.canExecute = _ => false;
            this.execute = _ => { return; };
        }

        private static bool DefaultCanExecute(object parameter)
        {
            return true;
        }
    }

}
