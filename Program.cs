using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.IO;
using RestSharp;
using System.Net;
using System.Configuration;
using SuperfeedrManager.Models;

namespace SuperfeedrManager.Subscriptions
{
    class Program
    {
        static IRestClient client = new RestClient(ConfigurationManager.AppSettings["SuperfeedrApiEndpoint"]);
        static IRestRequest request = new RestRequest();

        static void Main(string[] args)
        {
            client.Authenticator = new HttpBasicAuthenticator(ConfigurationManager.AppSettings["SuperfeedrUsername"], ConfigurationManager.AppSettings["SuperfeedrPassword"]);

            HttpStatusCode httpStatusCode;
            string response = string.Empty;

            switch (args[0])
            {
                case "list": // SuperfeedrManager.exe list
                    ListSubscriptions();
                    break;

                case "export":
                    ExportSubscriptions();
                    break;

                case "replay":
                    httpStatusCode = ReplayNotification(args[1], args[2], out response);
                    Console.WriteLine(httpStatusCode.ToString() + " Response (if any): " + response);
                    break;

                case "replayall":
                    ReplayAllNotifications();
                    break;

                case "unsubscribe": // SuperfeedrManager.exe delete hub.topic hub.callback
                    httpStatusCode = DeleteSubscription(args[1], out response, args[2]);
                    Console.WriteLine(httpStatusCode.ToString() + " Response (if any): " + response);
                    break;

                case "unsubscribeall": // SuperfeedrManager.exe delete hub.topic
                    DeleteAllSubscriptions();
                    break;

                default:
                    break;
            }
        }

        public static HttpStatusCode ReplayNotification(string hubTopic, string hubCallback, out string response)
        {
            request.Parameters.Clear();
            request.Method = Method.GET;
            request.AddParameter("hub.mode", "replay");
            request.AddParameter("hub.topic", hubTopic);
            request.AddParameter("hub.callback", hubCallback);
            return CallApi(out response);
        }

        public static void ExportSubscriptions()
        {
            var subs = GetSubscriptionList();
            int count = 0;
            using (StreamWriter sr = new StreamWriter("SuperfeedrSubscriptionsExport.csv"))
            {
                foreach (SuperfeedrSubscription sub in subs)
                {
                    sr.WriteLine(sub.subscription.feed.url + "," + sub.subscription.endpoint);
                    count++;
                }
            }
            Console.WriteLine(count.ToString() + " subscriptions exported");
        }

        public static void ListSubscriptions()
        {
            var subs = GetSubscriptionList();
            int count = 1;

            foreach (SuperfeedrSubscription sub in subs)
            {
                Console.WriteLine("[" + count.ToString() + "] Title: " + sub.subscription.feed.title + ", Subscription: " + sub.subscription.feed.url + ", Callback: " + sub.subscription.endpoint + ", Format: " + sub.subscription.format);
                count++;
            }
        }

        public static HttpStatusCode DeleteSubscription(string hubTopic, out string response, string hubCallback = "")
        {
            request.Parameters.Clear();
            request.Method = Method.POST;
            request.AddParameter("hub.mode", "unsubscribe");
            request.AddParameter("hub.topic", hubTopic);
            request.AddParameter("hub.callback", hubCallback);
            return CallApi(out response);
        }

        public static void DeleteAllSubscriptions()
        {
            var subs = GetSubscriptionList();
            int count = 1;

            foreach (SuperfeedrSubscription sub in subs)
            {
                HttpStatusCode statusCode = DeleteSubscription(sub.subscription.feed.url, out string response, sub.subscription.endpoint);
                Console.WriteLine("[" + count.ToString() + "] " + statusCode.ToString() + sub.subscription.feed.url + " | " + sub.subscription.endpoint + " Response body: " + response);
                count++;
            }
        }

        public static void ReplayAllNotifications()
        {
            var subs = GetSubscriptionList();
            int count = 1;

            foreach (SuperfeedrSubscription sub in subs)
            {
                HttpStatusCode statusCode = ReplayNotification(sub.subscription.feed.url, sub.subscription.endpoint, out string response);
                Console.WriteLine("[" + count.ToString() + "] " + statusCode.ToString() + sub.subscription.feed.url + " | " + sub.subscription.endpoint + " Response body: " + response);
                count++;
            }
        }

        public static List<SuperfeedrSubscription> GetSubscriptionList()
        {
            List<SuperfeedrSubscription> subs = new List<SuperfeedrSubscription>();
            List<SuperfeedrSubscription> substemp;

            const int REQUEST_PAGE_SIZE = 500;
            int page = 1;

            do
            {
                request.Method = Method.GET;
                request.Parameters.Clear();
                request.AddParameter("hub.mode", "list");
                request.AddParameter("by_page", REQUEST_PAGE_SIZE);  // 500 is max page size
                request.AddParameter("page", page); // To get the 2nd page if more than the max specified above
                // The number of feeds is part of the response you should be getting from us.  Responses do include something like the following:
                // "meta": { "total": 68886, "page": 1, "by_page": 5, "search": {} }
                HttpStatusCode statusCode = CallApi(out string response);
                JavaScriptSerializer ser = new JavaScriptSerializer();
                substemp = ser.Deserialize<List<SuperfeedrSubscription>>(response);
                subs.AddRange(substemp);
                page++;
            } while (substemp.Count > 0);
            
            return subs;
        }

        public static HttpStatusCode CallApi(out string Response)
        {
            IRestResponse response = client.Execute(request);
            int statusCode = (int)response.StatusCode;
            Response = string.Empty;

            switch (statusCode)
            {
                case 204: // Successfully created
                    break;

                case 202: // Should not apply unless I provide 'hub.verify' parameter
                    break;

                case 200: // Should only apply if I used the 'retrieve' parameter.  The content of the feed is in the body of the response.
                    Response = response.Content;
                    break;

                case 403: // Failed.  Check body for failure reason.
                    Response = response.Content;
                    break;

                case 422: // Failed.  Check body for failure reason.
                    Response = response.Content;
                    break;
                // case 4xx // TODO - Other codes are possible like 403
            }

            return (HttpStatusCode)statusCode;
        }
    }
}