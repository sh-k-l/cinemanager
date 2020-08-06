using System;
using System.Collections.Generic;
using System.Text;

namespace CMDesktopApp.Library.Models
{
    public class FilmModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageLink { get; set; }
        public string TrailerLink { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Runtime { get; set; }
        public string Language { get; set; }
    }
}
