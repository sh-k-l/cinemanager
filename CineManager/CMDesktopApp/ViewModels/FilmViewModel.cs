using AutoMapper;
using Caliburn.Micro;
using CMDesktopApp.Events;
using CMDesktopApp.Library.Api;
using CMDesktopApp.Library.Models;
using CMDesktopApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CMDesktopApp.ViewModels
{
    public class FilmViewModel : Screen
    {
        private readonly IEventAggregator _events;
        private readonly IFilmEndpoint _filmEndpoint;
        private readonly IMapper _mapper;
        private BindingList<FilmDisplayModel> _films;
        private FilmDisplayModel _selectedFilm;

        private string _filmSearchText;

        private string _filmTitle; 
        private string _filmDescription; 
        private string _filmImage; 
        private string _filmTrailer; 
        private DateTime _filmReleaseDate;
        private string _filmRuntime;
        private string _filmLanguage;

        public FilmViewModel(IEventAggregator events, IFilmEndpoint filmEndpoint, IMapper mapper)
        {
            _events = events;
            _filmEndpoint = filmEndpoint;
            _mapper = mapper;
        }
        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            LoadFilms();
        }


        public BindingList<FilmDisplayModel> Films
        {
            get 
            {
                if (string.IsNullOrWhiteSpace(_filmSearchText))
                {
                    return _films; 
                }
                else
                {
                    string search = _filmSearchText.Trim().ToLower();
                    List<FilmDisplayModel> films = _films.Where(film => film.Title.ToLower().Contains(search)).ToList();
                    return new BindingList<FilmDisplayModel>(films);
                }

            }
            set 
            { 
                _films = value;
                NotifyOfPropertyChange(() => Films);
            }
        }

        public FilmDisplayModel SelectedFilm
        {
            get { return _selectedFilm; }
            set
            {
                _selectedFilm = value;
                NotifyOfPropertyChange(() => SelectedFilm);
                NotifyOfPropertyChange(() => HaveSelectedFilm);
                NotifyOfPropertyChange(() => CanSaveFilm);
                NotifyOfPropertyChange(() => CanDeleteFilm);

                if(SelectedFilm != null)
                {
                    SetInputs(SelectedFilm);
                }
                else
                {
                    ClearInputs();
                }
            }
        }

        public string FilmSearchText
        {
            get { return _filmSearchText; }
            set 
            {
                _filmSearchText = value;
                NotifyOfPropertyChange(() => FilmSearchText);
                NotifyOfPropertyChange(() => Films);
            }
        }


        public string FilmTitle
        {
            get { return _filmTitle; }
            set
            {
                _filmTitle = value;
                NotifyOfPropertyChange(() => FilmTitle);
                NotifyOfPropertyChange(() => CanSaveFilm);
            }
        }
        public string FilmDescription
        {
            get { return _filmDescription; }
            set
            {
                _filmDescription = value;
                NotifyOfPropertyChange(() => FilmDescription);
            }
        }

        public string FilmImage
        {
            get { return _filmImage; }
            set
            {
                _filmImage = value;
                NotifyOfPropertyChange(() => FilmImage);
            }
        }

        public string FilmTrailer
        {
            get { return _filmTrailer; }
            set
            {
                _filmTrailer = value;
                NotifyOfPropertyChange(() => FilmTrailer);
            }
        }

        public DateTime FilmReleaseDate
        {
            get { return _filmReleaseDate; }
            set
            {
                _filmReleaseDate = value;
                NotifyOfPropertyChange(() => FilmReleaseDate);
            }
        }
        public string FilmRuntime
        {
            get { return _filmRuntime; }
            set
            {
                _filmRuntime = value;
                NotifyOfPropertyChange(() => FilmRuntime);
            }
        }
        public string FilmLanguage
        {
            get { return _filmLanguage; }
            set
            {
                _filmLanguage = value;
                NotifyOfPropertyChange(() => FilmLanguage);
            }
        }

        public bool HaveSelectedFilm
        {
            get
            {
                return SelectedFilm != null;
            }
        }

        public void AddNewFilm()
        {
            SelectedFilm = new FilmDisplayModel
            {
                ReleaseDate = DateTime.Today
            };
        }

        public void DeleteFilm()
        {
            string message = $"Are you sure you want to delete {SelectedFilm.Title}?";
            MessageBoxResult messageBoxResult = MessageBox.Show(message, "Delete Confirmation", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                HandleDeleteFilm(SelectedFilm);
            }
        }

        public bool CanDeleteFilm
        {
            get
            {
                return SelectedFilm != null && SelectedFilm.Id != null;
            }
        }

        public void SaveFilm()
        {
            if (SelectedFilm.Id == null)
            {
                HandleAddFilm(SelectedFilm);
            }
            else
            {
                HandleEditFilm(SelectedFilm);
            }
        }

        public bool CanSaveFilm
        {
            get 
            {
                return SelectedFilm != null
                && string.IsNullOrWhiteSpace(FilmTitle) == false;
            }
        }

        private async Task LoadFilms()
        {
            try
            {
                await _events.PublishOnUIThreadAsync(new LoadingOnEvent());
                var films = await _filmEndpoint.GetAllFilms();
                Films = _mapper.Map<BindingList<FilmDisplayModel>>(films);
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

        private void SetInputs(FilmDisplayModel film)
        {
            FilmTitle = film.Title;
            FilmDescription = film.Description;
            FilmImage = film.ImageLink;
            FilmTrailer = film.TrailerLink;
            FilmReleaseDate = film.ReleaseDate;
            FilmRuntime = film.Runtime;
            FilmLanguage = film.Language;
        }

        private void GetInputs(FilmDisplayModel film)
        {
            film.Title = FilmTitle;
            film.Description = FilmDescription;
            film.ImageLink = FilmImage;
            film.TrailerLink = FilmTrailer;
            film.ReleaseDate = FilmReleaseDate;
            film.Runtime = FilmRuntime;
            film.Language = FilmLanguage;
        }

        private void ClearInputs()
        {
            FilmTitle = "";
            FilmDescription = "";
            FilmImage = "";
            FilmTrailer = "";
            FilmReleaseDate = DateTime.Today;
            FilmRuntime = "";
            FilmLanguage = "";
        }

        private async Task HandleAddFilm(FilmDisplayModel film)
        {
            try
            {
                await _events.PublishOnUIThreadAsync(new LoadingOnEvent());
                GetInputs(film);
                film.Id = Guid.NewGuid().ToString();
                await _filmEndpoint.AddFilm(FilmDisplayModelToModel(film));

                Films.Add(film);
                SelectedFilm = null;
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

        private async Task HandleEditFilm(FilmDisplayModel film)
        {
            try
            {
                await _events.PublishOnUIThreadAsync(new LoadingOnEvent());
                GetInputs(film);
                await _filmEndpoint.EditFilm(FilmDisplayModelToModel(film));
                SelectedFilm = null;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                await _events.PublishOnUIThreadAsync(new LoadingOffEvent());
            }            
        }

        private async Task HandleDeleteFilm(FilmDisplayModel film)
        {
            try
            {
                await _events.PublishOnUIThreadAsync(new LoadingOnEvent());     
                await _filmEndpoint.DeleteFilm(film.Id);

                Films.Remove(film);               
                SelectedFilm = null;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                await _events.PublishOnUIThreadAsync(new LoadingOffEvent());
            }
        }

        private FilmModel FilmDisplayModelToModel(FilmDisplayModel film)
        {
            return _mapper.Map<FilmModel>(film);
        }
    }
}
