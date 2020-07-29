using Caliburn.Micro;
using CMDesktopApp.Events;
using CMDesktopApp.Library.Api;
using CMDesktopApp.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace CMDesktopApp.ViewModels
{
    public class SalesViewModel : Screen
    {
        private readonly IEventAggregator _events;
        private readonly IFilmEndpoint _filmEndpoint;

        // Tickets
        private double _studentPrice = 5.99;
        private double _standardPrice = 6.99;
        private double _permierPrice = 8.99;
        private int _studentQuantity = 0;
        private int _standardQuantity = 0;
        private int _premierQuantity = 0;

        // Films
        private BindingList<FilmModel> _films;
        private FilmModel _selectedFilm;

        // Dates
        private BindingList<ComboBoxDatePairModel> _dates;
        private ComboBoxDatePairModel _selectedDate;



        public SalesViewModel(IEventAggregator events, IFilmEndpoint filmEndpoint)
        {
            _events = events;
            _filmEndpoint = filmEndpoint;

            //_dates = new BindingList<string>();
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            LoadAllFilms();
            InitialiseDates();
            
        }

        private void InitialiseDates()
        {
            var currentDate = DateTime.Today;

            BindingList<ComboBoxDatePairModel> dates = new BindingList<ComboBoxDatePairModel>();

            for (int i = 0; i < 14; i++)
            {
                dates.Add(new ComboBoxDatePairModel
                {
                    Display = $"{currentDate.AddDays(i):dd/MM} ({currentDate.AddDays(i).DayOfWeek})",
                    Value = currentDate.AddDays(i)
                });  
            }
            Dates = dates;
        }

        private async Task LoadAllFilms()
        {
            try
            {
                await _events.PublishOnUIThreadAsync(new LoadingOnEvent());
                var res = await _filmEndpoint.GetAllFilms();
                Films = new BindingList<FilmModel>(res);
            }
            catch (Exception ex)
            {
                // TODO
                throw;
            }
            finally
            {
                await _events.PublishOnUIThreadAsync(new LoadingOffEvent());
            }
        }

        private async Task LoadFilmsByDate()
        {
            try
            {
                await _events.PublishOnUIThreadAsync(new LoadingOnEvent());
                var res = await _filmEndpoint.GetFilmsByDate(SelectedDate.Value);
                Films = new BindingList<FilmModel>(res);
            }
            catch (Exception ex)
            {
                // TODO
                throw;
            }
            finally
            {
                await _events.PublishOnUIThreadAsync(new LoadingOffEvent());
            }
        }

        public BindingList<FilmModel> Films
        {
            get { return _films; }
            set
            {
                _films = value;
                NotifyOfPropertyChange(() => Films);
            }
        }

        public FilmModel SelectedFilm
        {
            get { return _selectedFilm; }
            set
            {
                _selectedFilm = value;
                NotifyOfPropertyChange(() => SelectedFilm);
            }
        }

        public BindingList<ComboBoxDatePairModel> Dates
        {
            get { return _dates; }
            set
            {
                _dates = value;
                NotifyOfPropertyChange(() => Dates);                
            }
        }

        public ComboBoxDatePairModel SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;                
                NotifyOfPropertyChange(() => SelectedDate);
                LoadFilmsByDate();
            }
        }

        public double StudentPrice
        {
            get { return _studentPrice; }
            set 
            { 
                _studentPrice = value;
                NotifyOfPropertyChange(() => StudentPrice);
            }
        }

        public double StandardPrice
        {
            get { return _standardPrice; }
            set
            {
                _standardPrice = value;
                NotifyOfPropertyChange(() => StandardPrice);
            }
        }

        public double PremierPrice
        {
            get { return _permierPrice; }
            set
            {
                _permierPrice = value;
                NotifyOfPropertyChange(() => PremierPrice);
            }
        }

        public int StudentQuantity
        {
            get { return _studentQuantity; }
            set
            {
                _studentPrice = value;
                NotifyOfPropertyChange(() => StudentQuantity);
            }
        }

        public int StandardQuantity
        {
            get { return _standardQuantity; }
            set
            {
                _standardPrice = value;
                NotifyOfPropertyChange(() => StandardQuantity);
            }
        }

        public int PremierQuantity
        {
            get { return _premierQuantity; }
            set
            {
                _premierQuantity = value;
                NotifyOfPropertyChange(() => PremierQuantity);
            }
        }

    }
}
