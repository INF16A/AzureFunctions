using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureFunctionsDHBW
{
    class PlanParser
    {
        private string raw = string.Empty;
        private List<Plan> lectures;
        private string result;

        public PlanParser(string json)
        {
            raw = json;
            planToLectures();
        }

        private void planToLectures()
        {
            lectures = JsonConvert.DeserializeObject<List<Plan>>(raw);
        }


        public bool getLectures(string datestr)
        {
            var date = DateTime.Now;
            result = string.Empty;
            switch (datestr)
            {
                case "today":
                    datestr = date.ToString("dd.MM.yyyy");
                    break;
                case "tomorrow":
                    date = date.AddDays(1);
                    datestr = date.ToString("dd.MM.yyyy");
                    break;
            }

            filterLecturesByDate(datestr);
            return result != string.Empty;
        }
      
        private void filterLecturesByDate(string datestr)
        {
            foreach (Plan lecture in lectures)
            {
                if (lecture.Start_date == datestr)
                {
                    result += lecture.Name + ", Beginnt um: " + lecture.Start_time + " Uhr und endet um: " + lecture.End_time + " Uhr. ";
                }
            }
        }

        public string getResult()
        {
            return result;
        }
    }
}

