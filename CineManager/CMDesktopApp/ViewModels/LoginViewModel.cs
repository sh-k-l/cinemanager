using Caliburn.Micro;
using CMDesktopApp.Events;
using CMDesktopApp.Library.Api;
using CMDesktopApp.Library.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CMDesktopApp.ViewModels
{
    public class LoginViewModel : Screen
    {
        private readonly IEventAggregator _events;
        private readonly IAuthEndpoint _authEndpoint;
        private readonly IAuthenticatedUser _authenticatedUser;
        private readonly IApiHelper _apiHelper;

        public LoginViewModel(IEventAggregator events, IAuthEndpoint authEndpoint, IAuthenticatedUser authenticatedUser, IApiHelper apiHelper)
        {
            _events = events;            
            _authEndpoint = authEndpoint;
            _authenticatedUser = authenticatedUser;
            _apiHelper = apiHelper;
        }        
        
        private string _username = "shakilchyy@gmail.com";
        public string Username
        {
            get { return _username; }
            set {
                _username = value;
                NotifyOfPropertyChange(() => Username);
                NotifyOfPropertyChange(() => CanLogin);
            }
        }

        private string _password = "Pwd123.";  
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogin);
            }
        }

        public bool CanLogin
        {
            get
            {
                bool output = true;
                if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
                {
                    output = false;
                }

                return output;
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                NotifyOfPropertyChange(() => ErrorMessage);
                NotifyOfPropertyChange(() => IsErrorVisible);
            }
        }
        public bool IsErrorVisible
        {
            get
            {
                return String.IsNullOrEmpty(ErrorMessage) == false;
            }
        }

        public async Task Login()
        {
            try
            {
                await _events.PublishOnUIThreadAsync(new LoadingOnEvent());

                var user = await _authEndpoint.Authenticate(Username, Password);
                _authenticatedUser.SetFields(user);
                _apiHelper.AddAuthToken(user.Access_Token);


                await _events.PublishOnUIThreadAsync(new LogInEvent());
            }
            catch (Exception ex)
            {
                Password = "";
                ErrorMessage = ex.Message;
            }
            finally
            {
                await _events.PublishOnUIThreadAsync(new LoadingOffEvent());
            }
            
        }

    }
}
