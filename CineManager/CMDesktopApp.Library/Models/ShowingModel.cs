using System;
using System.Collections.Generic;
using System.Text;

namespace CMDesktopApp.Library.Models
{
    public class ShowingModel
    {
        public int Id { get; set; }
        public string Screen { get; set; }
        public string Title { get; set; }
        public DateTime DateTime { get; set; }

        public string Time
        {
            get
            {
                return DateTime.ToString("HH:mm");
            }
        }
    }
}
