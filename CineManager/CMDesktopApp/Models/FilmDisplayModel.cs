using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CMDesktopApp.Models
{
    public class FilmDisplayModel : INotifyPropertyChanged
    {
        private string _id;
        private string _title;
        private string _description;
        private string _imageLink;
        private string _trailerLink;
        private DateTime _releaseDate;
        private string _runtime;
        private string _language;

        public string Id { get => _id; set => _id = value; }
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }
        public string ImageLink
        {
            get => _imageLink;
            set
            {
                _imageLink = value;
                OnPropertyChanged();
            }
        }
        public string TrailerLink
        {
            get => _trailerLink;
            set
            {
                _trailerLink = value;
                OnPropertyChanged();
            }
        }
        public DateTime ReleaseDate
        {
            get => _releaseDate;
            set
            {
                _releaseDate = value;
                OnPropertyChanged();
            }
        }
        public string Runtime
        {
            get => _runtime;
            set
            {
                _runtime = value;
                OnPropertyChanged();
            }
        }
        public string Language
        {
            get => _language;
            set
            {
                _language = value;
                OnPropertyChanged();
            }
        }

        public string DateOnly
        {
            get
            {
                return ReleaseDate.ToString("yyyy-MM-dd");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
