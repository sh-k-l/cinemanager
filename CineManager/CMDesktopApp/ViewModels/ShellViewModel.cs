using Caliburn.Micro;
using CMDesktopApp.Events;
using CMDesktopApp.Library.Api;
using CMDesktopApp.Library.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CMDesktopApp.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LoadingOffEvent>, IHandle<LoadingOnEvent>, IHandle<LogInEvent>
    {
        private readonly IEventAggregator _events;
        private readonly IAuthenticatedUser _user;
        private readonly IApiHelper _apiHelper;

        public ShellViewModel(IEventAggregator events, IAuthenticatedUser authenticatedUser, IApiHelper apiHelper)
        {
            _events = events;
            _user = authenticatedUser;
            _apiHelper = apiHelper;
            _events.SubscribeOnUIThread(this);
            ActivateItemAsync(IoC.Get<LoginViewModel>());            
        }

        // Menu handlers
        public void UserManagement()
        {
            ActivateItemAsync(IoC.Get<UserManagementViewModel>());
        }

        public void Sales()
        {
            ActivateItemAsync(IoC.Get<SalesViewModel>());
        }

        private void SyncMenu()
        {
            NotifyOfPropertyChange(() => IsLoggedIn);
            NotifyOfPropertyChange(() => IsAdmin);
        }

        public bool IsLoggedIn
        {
            get
            {
                return string.IsNullOrWhiteSpace(_user.Access_Token) == false;
            }
        }

        public bool IsAdmin
        {
            get
            {
                return IsLoggedIn && _user.Roles.Contains("Admin");
            }
        }


        // Loading Bar
        private bool _isLoading = false;       
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                NotifyOfPropertyChange(() => IsLoading);
            }
        }
        
        public Task HandleAsync(LoadingOffEvent message, CancellationToken cancellationToken)
        {
            IsLoading = false;
            return Task.CompletedTask;
        }

        public Task HandleAsync(LoadingOnEvent message, CancellationToken cancellationToken)
        {
            IsLoading = true;
            return Task.CompletedTask;
        }

        // Log In Event
        public async Task HandleAsync(LogInEvent message, CancellationToken cancellationToken)
        {            
            await ActivateItemAsync(IoC.Get<SalesViewModel>());
            SyncMenu();
        }

        public void Logout()
        {
            _user.Reset();
            _apiHelper.ClearDefaultHeaders();
            SyncMenu();
            ActivateItemAsync(IoC.Get<LoginViewModel>());
        }


    }
}
