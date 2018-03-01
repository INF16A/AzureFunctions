using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace AzureFunctionsDHBW
{
    public static class DHBWAzureFunction
    {
        [FunctionName("DHBWPlan")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            
            string name = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
                .Value;

            string course = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "course", true) == 0)
                .Value;

            string time = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "time", true) == 0)
                .Value;
            
            dynamic data = await req.Content.ReadAsAsync<object>();
            
            name = name ?? data?.name;
            course = course ?? data?.course;
            time = time ?? data?.time;


            if (name == null || course == null || time == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body");
            }
            else
            {
                DHBWAPIRequester requester = new DHBWAPIRequester("http://stuv-mosbach.de/survival/api.php?action=getLectures&course=" + course);
                PlanParser p = new PlanParser(requester.GetResponse());
                if (p.getLectures(time))
                {
                    return req.CreateResponse(HttpStatusCode.OK, "Hallo " + name + ", Du hast den Kurs: " + course + " gewählt. Vorlesungen am "+ time + ": " + p.getResult());
                }
                else return req.CreateResponse(HttpStatusCode.OK, "Hallo" + name + ", es gibt keine Vorlesungen an diesem Tag.");

            }
        }
    }
}
