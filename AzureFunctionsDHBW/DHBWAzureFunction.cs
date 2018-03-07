using System;
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
            // Simple Function
            // Get request body
            dynamic data = await req.Content.ReadAsAsync<object>();
            log.Info($"Content={data}");
            if (data.request.type == "LaunchRequest")
            {
                // default launch request, let's just let them know what you can do
                log.Info($"Default LaunchRequest made");
                return DefaultRequest(req);
            }
            else if (data.request.type == "IntentRequest")
            {
                // Set name to query string or body data
                string intentName = data.request.intent.name;
                log.Info($"intentName={intentName}");
                switch (intentName)
                {
                    case "GetVorlesungDayXIntent":
                        string n1 = data.request.intent.slots["Day"].value;
                        DateTime dateValue = DateTime.Parse(n1);
                        string time = dateValue.ToString("dd.MM.yyyy");
                        log.Info(time);
                        DHBWAPIRequester requester = new DHBWAPIRequester("http://stuv-mosbach.de/survival/api.php?action=getLectures&course=" + "INF16A");
                        PlanParser p = new PlanParser(requester.GetResponse());
                        p.getLectures(time);
                        string result = p.getResult();

                        string subject = result.ToString();
                        return req.CreateResponse(HttpStatusCode.OK, new
                        {
                            version = "1.0",
                            sessionAttributes = new { },
                            response = new
                            {
                                outputSpeech = new
                                {
                                    type = "PlainText",
                                    text = $"Du hast: {result.ToString()}."
                                },
                                card = new
                                {
                                    type = "Simple",
                                    title = "Alexa gibt dir deinen Vorlesungsplan der DHBW Mosbach.",
                                    content = $"The result is {result.ToString()}."
                                },
                                shouldEndSession = true
                            }
                        });
                    // Add more intents and default responses
                    default:
                        return DefaultRequest(req);
                }
            }
            else
            {
                return DefaultRequest(req);
            }
        }
        private static HttpResponseMessage DefaultRequest(HttpRequestMessage req)
        {
            return req.CreateResponse(HttpStatusCode.OK, new
            {
                version = "1.0",
                sessionAttributes = new { },
                response = new
                {
                    outputSpeech = new
                    {
                        type = "PlainText",
                        text = "Willkommen zum DHBW Vorlesungsplan. Ich kann dir deine Vorlesungen mitteilen."
                    },
                    card = new
                    {
                        type = "Simple",
                        title = "Alexa DHBW Mosbach Vorlesungsplanr",
                        content = "Willkommen zum DHBW Vorlesungsplan. Ich kann dir deine Vorlesungen mitteilen. Sage: Welche Vorlesungen habe ich morgen?"
                    },
                    shouldEndSession = true
                }
            });
        }
    }
}