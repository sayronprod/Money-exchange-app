using Money_exchange_app.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Money_exchange_app
{
    public class WebAdapter
    {
        static string getLatestRequestString = "https://api.exchangeratesapi.io/latest";
        public static List<string> GetCurrencys()
        {
            string resultJsonString=SendRequestToApi(getLatestRequestString);
            JToken obj = JToken.Parse(resultJsonString);
            List<string> results = obj.SelectToken("$.rates").Children().OfType<JProperty>().Select(x => (x as JProperty).Name).ToList();
            string baseCurrency=obj.SelectToken("$.base").ToString();
            results.Add(baseCurrency);
            return results;
        }
        private static string SendRequestToApi(string request)
        {
            WebRequest webRequest = WebRequest.Create(request);
            Stream responseStream = webRequest.GetResponse().GetResponseStream();
            string resultJsonString;
            using (StreamReader reader = new StreamReader(responseStream))
            {
                resultJsonString = reader.ReadToEnd();
            }
            return resultJsonString;
        }
        public static string GetCostOf(string from, string to)
        {
            string request = $"{getLatestRequestString}?base={from}";
            string resultJsonString = SendRequestToApi(request);
            JToken obj = JToken.Parse(resultJsonString);
            JProperty findingCurrency = obj.SelectToken("$.rates").Children().OfType<JProperty>().First(x => (x as JProperty).Name == to);
            string result = findingCurrency.Value.ToString();
            return result;
        }
    }
}
