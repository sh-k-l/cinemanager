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
        private BindingList<string> _userRoles;
        private bool _showNoUserFound = false;
        private List<string> _allRoles { get; set; } = new List<string>();
        private string _selectedRoleToRemove;
        private string _selectedRoleToAdd;


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
                    UserRoles = new BindingList<string>(value.Roles.Select(x => x.Value).ToList());
                }
                NotifyOfPropertyChange(() => SelectedUser);
                NotifyOfPropertyChange(() => UserRoles);
                NotifyOfPropertyChange(() => AddableRoles);
            }
        }

        public BindingList<string> UserRoles
        {
            get
            {
                return _userRoles;
            }
            set
            {
                _userRoles = value;
                NotifyOfPropertyChange(() => UserRoles);
            }
        }

        public BindingList<string> AddableRoles
        {
            get
            {
                BindingList<string> roleList = new BindingList<string>();
                if (SelectedUser != null) {
                    var roles = _allRoles.Where(x => UserRoles.Contains(x) == false).ToList();
                    roleList = new BindingList<string>(roles);
                } 
                
                return roleList;
            }           
        }

        public string SelectedUserRole
        {
            get
            {
                return _selectedRoleToRemove;
            }
            set
            {
                _selectedRoleToRemove = value;
                NotifyOfPropertyChange(() => SelectedUserRole);
                NotifyOfPropertyChange(() => CanRemoveRole);
                NotifyOfPropertyChange(() => AddableRoles);
                NotifyOfPropertyChange(() => UserRoles);
            }
        }

        public string SelectedAddableRole
        {
            get
            {
                return _selectedRoleToAdd;
            }
            set
            {
                _selectedRoleToAdd = value;
                NotifyOfPropertyChange(() => SelectedAddableRole);
                NotifyOfPropertyChange(() => CanAddRole);
                NotifyOfPropertyChange(() => UserRoles);

            }
        }

        public bool CanAddRole
        {
            get 
            {
                return string.IsNullOrWhiteSpace(SelectedAddableRole) == false;
            }
        }

        public async Task AddRole()
        {
            await _events.PublishOnUIThreadAsync(new LoadingOnEvent());
            await _userEndpoint.AddUserToRole(SelectedUser.Id, SelectedAddableRole);
            UserRoles.Add(SelectedAddableRole);
            AddableRoles.Remove(SelectedAddableRole);
            SelectedAddableRole = null;
            SelectedUserRole = null;
            await _events.PublishOnUIThreadAsync(new LoadingOffEvent());
        }

        public bool CanRemoveRole
        {
            get
            {
                return string.IsNullOrWhiteSpace(SelectedUserRole) == false;
            }
        }

        public async Task RemoveRole()
        {
            await _events.PublishOnUIThreadAsync(new LoadingOnEvent());
            await _userEndpoint.RemoveUserFromRole(SelectedUser.Id, SelectedUserRole);
            AddableRoles.Add(SelectedUserRole);
            UserRoles.Remove(SelectedUserRole);
            SelectedUserRole = null;
            SelectedAddableRole = null;

            await _events.PublishOnUIThreadAsync(new LoadingOffEvent());
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
                UserRoles = null;
                Users = null;
                NotifyOfPropertyChange(() => AddableRoles);

                FindUsers();                
            }
        }

        public bool ShowNoUserFound
        {
            get 
            {
                return _showNoUserFound;
            }

            set
            {
                _showNoUserFound = value;
                NotifyOfPropertyChange(() => ShowNoUserFound);
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
                ShowNoUserFound = false;
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
                if(user != null)
                {
                    Users.Add(user);
                    ShowNoUserFound = false;
                }
                else
                {
                    ShowNoUserFound = true;
                }

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
