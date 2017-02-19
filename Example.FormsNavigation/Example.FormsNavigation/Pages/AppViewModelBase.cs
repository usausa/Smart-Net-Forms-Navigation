namespace Example.FormsNavigation.Pages
{
    using System;
    using System.Threading.Tasks;

    using Smart.Forms.Input;
    using Smart.Forms.ViewModels;

    public class AppViewModelBase : DisposableViewModelBase
    {
        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        // ------------------------------------------------------------
        // Command helper
        // ------------------------------------------------------------

        protected AsyncCommand MakeBusyCommand(Func<Task> execute)
        {
            return MakeBusyCommand(execute, () => true);
        }

        protected AsyncCommand MakeBusyCommand(Func<Task> execute, Func<bool> canExecute)
        {
            return new AsyncCommand(
                async () =>
                {
                    IsBusy = true;
                    try
                    {
                        await execute();
                    }
                    finally
                    {
                        IsBusy = false;
                    }
                }, () => !IsBusy && canExecute())
                .Observe(this, nameof(IsBusy))
                .RemoveObserverBy(Disposables);
        }

        protected AsyncCommand<TParameter> MakeBusyCommand<TParameter>(Func<TParameter, Task> execute)
        {
            return MakeBusyCommand(execute, x => true);
        }

        protected AsyncCommand<TParameter> MakeBusyCommand<TParameter>(Func<TParameter, Task> execute, Func<TParameter, bool> canExecute)
        {
            return new AsyncCommand<TParameter>(
                async parameter =>
                {
                    IsBusy = true;
                    try
                    {
                        await execute(parameter);
                    }
                    finally
                    {
                        IsBusy = false;
                    }
                }, parameter => !IsBusy && canExecute(parameter))
                .Observe(this, nameof(IsBusy))
                .RemoveObserverBy(Disposables);
        }

        protected AsyncCommand MakeBusyCommand(Action execute)
        {
            return MakeBusyCommand(execute, () => true);
        }

        protected AsyncCommand MakeBusyCommand(Action execute, Func<bool> canExecute)
        {
            return new AsyncCommand(
                () =>
                {
                    IsBusy = true;
                    try
                    {
                        execute();
                    }
                    finally
                    {
                        IsBusy = false;
                    }
                }, () => !IsBusy && canExecute())
                .Observe(this, nameof(IsBusy))
                .RemoveObserverBy(Disposables);
        }

        protected AsyncCommand<TParameter> MakeBusyCommand<TParameter>(Action<TParameter> execute)
        {
            return MakeBusyCommand(execute, x => true);
        }

        protected AsyncCommand<TParameter> MakeBusyCommand<TParameter>(Action<TParameter> execute, Func<TParameter, bool> canExecute)
        {
            return new AsyncCommand<TParameter>(
                parameter =>
                {
                    IsBusy = true;
                    try
                    {
                        execute(parameter);
                    }
                    finally
                    {
                        IsBusy = false;
                    }
                }, parameter => !IsBusy && canExecute(parameter))
                .Observe(this, nameof(IsBusy))
                .RemoveObserverBy(Disposables);
        }

        // ------------------------------------------------------------
        // Execute helper
        // ------------------------------------------------------------

        public void ExecuteBusy(Action execute)
        {
            try
            {
                IsBusy = true;

                execute();
            }
            finally
            {
                IsBusy = false;
            }
        }

        public TResult ExecuteBusy<TResult>(Func<TResult> execute)
        {
            try
            {
                IsBusy = true;

                return execute();
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task ExecuteBusyAsync(Func<Task> execute)
        {
            try
            {
                IsBusy = true;

                await execute();
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task<TResult> ExecuteBusyAsync<TResult>(Func<Task<TResult>> execute)
        {
            try
            {
                IsBusy = true;

                return await execute();
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
