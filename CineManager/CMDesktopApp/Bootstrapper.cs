using Caliburn.Micro;
using CMDesktopApp.Helpers;
using CMDesktopApp.Library.Api;
using CMDesktopApp.Library.Models;
using CMDesktopApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace CMDesktopApp
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer _container = new SimpleContainer();
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            // From: https://stackoverflow.com/questions/30631522/caliburn-micro-support-for-passwordbox
            ConventionManager.AddElementConvention<PasswordBox>(
                PasswordBoxHelper.BoundPasswordProperty,
                "Password",
                "PasswordChanged");


            // Dependency Injection
            _container.Instance(_container);

            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>()
                .Singleton<IAuthenticatedUser, AuthenticatedUser>()
                .Singleton<IApiHelper, ApiHelper>()
                .Singleton<IAuthEndpoint, AuthEndpoint>()
                .Singleton<IUserEndpoint, UserEndpoint>();

            // New ViewModels on each request
            GetType().Assembly.GetTypes()
                .Where(type => type.IsClass)
                .Where(type => type.Name.EndsWith("ViewModel"))
                .ToList()
                .ForEach(viewModelType => _container.RegisterPerRequest(viewModelType, viewModelType.ToString(), viewModelType));
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }
        protected override object GetInstance(Type serviceType, string key)
        {
            return _container.GetInstance(serviceType, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return _container.GetAllInstances(serviceType);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }
    }
}
