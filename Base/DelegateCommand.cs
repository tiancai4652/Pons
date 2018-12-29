﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApplication2
{

    public class DelegateCommand : ICommand
    {
        Func<object, bool> canExecute;
        Action<object> executeAction;
        bool canExecuteCache;
        public DelegateCommand(Action<object> executeAction, Func<object, bool> canExecute)
        {
            this.executeAction = executeAction;
            this.canExecute = canExecute;
        }
        public DelegateCommand(Action command)
        {
            this.executeAction = delegate { command(); };
            this.canExecute = delegate { return true; };
        }
        public DelegateCommand(Action command, Func<bool> test)
        {
            this.executeAction = delegate { command(); };
            this.canExecute = delegate { return test(); }; ;
        }
        #region ICommand Members
        public bool CanExecute(object parameter)
        {
            bool temp = canExecute(parameter);

            if (canExecuteCache != temp)
            {
                canExecuteCache = temp;
                if (CanExecuteChanged != null)
                {
                    CanExecuteChanged(this, new EventArgs());
                }
            }
            return canExecuteCache;
        }

        public event EventHandler CanExecuteChanged;
        public void Execute(object parameter)
        {
            executeAction(parameter);
        }
        #endregion


       
    }
}
