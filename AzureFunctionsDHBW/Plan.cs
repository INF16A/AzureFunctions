using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureFunctionsDHBW
{
    class Plan
    {
        private string ID, course, name, start_mysql, start_date, start_time, end_mysql, end_date, end_time, lecturer, location, lastModified_mysql;
        private int duration;
        private bool today, allDayEvent, over, multipleDayEvent;

        public int Duration { get; set; }
        public string Id { get; set; }
        public string Course { get; set; }
        public string Name { get; set; }
        public string Start_date { get; set; }
        public string End_date { get; set; }
        public string Lecturer { get; set; }
        public string Location { get; set; }
        public string Start_time { get; set; }
        public string End_time { get; set; }
        public bool Today { get; set; }
        public bool Over { get; set; }
        public bool AllDayEvent { get; set; }
    }
}
