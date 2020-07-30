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
        private readonly IShowingEndpoint _showingEndpoint;

        // Tickets
        private double _studentPrice = 5.99;
        private double _standardPrice = 6.99;
        private double _permierPrice = 8.99;
        private int _studentQuantity = 0;
        private int _standardQuantity = 0;
        private int _premierQuantity = 0;

        //Showings
        private BindingList<ShowingModel> _showings;
        private ShowingModel _selectedShowing;

        // Films
        private BindingList<FilmModel> _films;
        private FilmModel _selectedFilm;

        // Dates
        private BindingList<ComboBoxDatePairModel> _dates;
        private ComboBoxDatePairModel _selectedDate;

        public SalesViewModel(IEventAggregator events, IFilmEndpoint filmEndpoint, IShowingEndpoint showingEndpoint)
        {
            _events = events;
            _filmEndpoint = filmEndpoint;
            _showingEndpoint = showingEndpoint;
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            //LoadAllFilms();
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
                var res = await _filmEndpoint.GetFilmsByDate(SelectedDate.Value.ToString("yyyy-MM-dd"));
                Films = new BindingList<FilmModel>(res);
            }
            catch (Exception ex)
            {
                // TODO
                var date = SelectedDate;
                throw;
            }
            finally
            {
                await _events.PublishOnUIThreadAsync(new LoadingOffEvent());
            }
        }

        private async Task LoadShowingByIdAndDate()
        {
            if(SelectedDate == null || SelectedFilm == null)
            {
                Showings.Clear();
                return;
            }

            try
            {
                await _events.PublishOnUIThreadAsync(new LoadingOnEvent());
                var res = await _showingEndpoint.GetShowingsByIdAndDate(SelectedFilm.Id, SelectedDate.Value);
                Showings = new BindingList<ShowingModel>(res);
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

        public BindingList<ShowingModel> Showings
        {
            get { return _showings; }
            set
            {
                _showings = value;
                NotifyOfPropertyChange(() => Showings);
            }
        }

        public ShowingModel SelectedShowing
        {
            get { return _selectedShowing; }
            set
            {
                _selectedShowing = value;
                NotifyOfPropertyChange(() => SelectedShowing);
                NotifyOfPropertyChange(() => CanAddTicket);
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
                LoadShowingByIdAndDate();
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
                _studentQuantity = value;
                NotifyOfPropertyChange(() => StudentQuantity);
                NotifyOfPropertyChange(() => Total);
            }
        }

        public int StandardQuantity
        {
            get { return _standardQuantity; }
            set
            {
                _standardQuantity = value;
                NotifyOfPropertyChange(() => StandardQuantity);
                NotifyOfPropertyChange(() => Total);
            }
        }

        public int PremierQuantity
        {
            get { return _premierQuantity; }
            set
            {
                _premierQuantity = value;
                NotifyOfPropertyChange(() => PremierQuantity);
                NotifyOfPropertyChange(() => Total);
            }
        }

        public bool CanAddTicket
        {
            get
            {
                return SelectedShowing != null;
            }
        }

        public string Total
        {
            get
            {
                double total = (StudentPrice * StudentQuantity) + (StandardPrice * StandardQuantity) + (PremierPrice * PremierQuantity);
                return $"Total: {total:C}";
            }
        }

        private void HandleTicketChange(string type, int increment)
        {
            switch (type)
            {
                case "Student":
                    StudentQuantity = StudentQuantity + increment < 0 ? 0 : StudentQuantity + increment;
                    break;
                case "Standard":
                    StandardQuantity = StandardQuantity + increment < 0 ? 0 : StandardQuantity + increment;
                    break;
                case "Premier":
                    PremierQuantity = PremierQuantity + increment < 0 ? 0 : PremierQuantity + increment;
                    break;
                default:
                    break;
            }
            NotifyOfPropertyChange(() => CanBookTickets);
        }

        public void TestLeftClick(string type)
        {
            HandleTicketChange(type, 1);
        }

        public void TestRightClick(string type)
        {
            HandleTicketChange(type, -1);
        }

        public void BookTickets()
        {

        }

        public bool CanBookTickets
        {
            get
            {
                return StudentQuantity > 0 || StandardQuantity > 0 || PremierQuantity > 0;
            }
        }

    }
}
