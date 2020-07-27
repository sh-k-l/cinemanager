using Caliburn.Micro;
using CMDesktopApp.Events;
using CMDesktopApp.Library;
using CMDesktopApp.Library.Api;
using CMDesktopApp.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CMDesktopApp.ViewModels
{
    public class UserManagementViewModel : Screen
    {
        private readonly IEventAggregator _events;
        private readonly IUserEndpoint _userEndpoint;
        private string _email = ""; 
        private BindingList<UserManagementSearchType> _searchTypes;
        private UserManagementSearchType _selectedSearchTypes;
        private BindingList<UserModel> _users;
        private UserModel _selectedUser;
        private BindingList<string> _rolesOnUser;
        private List<string> _allRoles { get; set; } = new List<string>();


        public UserManagementViewModel(IEventAggregator events, IUserEndpoint userEndpoint)
        {
            _events = events;
            _userEndpoint = userEndpoint;
            _users = new BindingList<UserModel>();            
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            SearchTypes = new BindingList<UserManagementSearchType>(Enum.GetValues(typeof(UserManagementSearchType)) as IList<UserManagementSearchType>);
            LoadRoles();
        }

        public BindingList<UserModel> Users
        {
            get
            {
                return _users;
            }
            set
            {
                _users = value;
                NotifyOfPropertyChange(() => Users);
            }
        }

        public UserModel SelectedUser
        {
            get
            {
                return _selectedUser;
            }
            set
            {
                _selectedUser = value;
                if(value != null)
                {
                    RolesOnUser = new BindingList<string>(value.Roles.Select(x => x.Value).ToList());
                }
                NotifyOfPropertyChange(() => SelectedUser);
                NotifyOfPropertyChange(() => RolesOnUser);
                NotifyOfPropertyChange(() => RolesToAdd);
            }
        }

        public BindingList<string> RolesOnUser
        {
            get
            {
                return _rolesOnUser;
            }
            set
            {
                _rolesOnUser = value;
                NotifyOfPropertyChange(() => RolesOnUser);
            }
        }


        public BindingList<string> RolesToAdd
        {
            get
            {
                BindingList<string> roleList = new BindingList<string>();
                if (SelectedUser != null) {
                    var roles = _allRoles.Where(x => RolesOnUser.Contains(x) == false).ToList();
                    roleList = new BindingList<string>(roles);
                } 
                
                return roleList;
            }           
        }

        public bool CanAddRole
        {
            get { return false; }
        }

        public void AddRole()
        {
            throw new NotImplementedException();
        }


        public bool CanRemoveRole
        {
            get { return false; }
        }

        public void RemoveRole()
        {
            throw new NotImplementedException();
        }

        public BindingList<UserManagementSearchType> SearchTypes
        { 
            get
            {
                return _searchTypes;
            }

            set
            {
                _searchTypes = value;
                NotifyOfPropertyChange(() => SearchTypes);
            }
        }

        public UserManagementSearchType SelectedSearchType
        {
            get
            {
                return _selectedSearchTypes;
            }

            set
            {
                _selectedSearchTypes = value;
                NotifyOfPropertyChange(() => SelectedSearchType);
                NotifyOfPropertyChange(() => ShowEmailSearchForm);
                SelectedUser = null;
                RolesOnUser = null;
                Users = null;
                NotifyOfPropertyChange(() => RolesToAdd);

                FindUsers();                
            }
        }

        public bool ShowEmailSearchForm
        {
            get
            {
                bool output = true;

                if (_selectedSearchTypes != UserManagementSearchType.Email)
                {
                    output = false;
                }
                return output;
            }
        }

        public string Email 
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                NotifyOfPropertyChange(() => Email);
                NotifyOfPropertyChange(() => CanSearchByEmail);


            }
        }

        public bool CanSearchByEmail
        {
            get
            {
                return Email.Length > 0;
            }
        }

        public void SearchByEmail()
        {
            FindByEmail(Email);
        }

        private async Task FindUsers()
        {
            if (SelectedSearchType == UserManagementSearchType.Email) {
                return;
            }

            await _events.PublishOnUIThreadAsync(new LoadingOnEvent());

            try
            {
                var users = await _userEndpoint.GetUsers(SelectedSearchType);
                Users = new BindingList<UserModel>(users);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                await _events.PublishOnUIThreadAsync(new LoadingOffEvent());
            }  
        }

        private async Task FindByEmail(string email)
        {  
            Users = new BindingList<UserModel>();
            await _events.PublishOnUIThreadAsync(new LoadingOnEvent());
            try
            {
                var user = await _userEndpoint.FindUserByEmail(email);
                Users.Add(user);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                await _events.PublishOnUIThreadAsync(new LoadingOffEvent());
            }
        }

        private async Task LoadRoles()
        {
            var roles = await _userEndpoint.GetAllRoles();
            _allRoles = roles.Select(x => x.Value).ToList();
        }


    }
}
