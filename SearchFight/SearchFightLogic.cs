using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SearchFight.Logic
{
    public class SearchFightLogic
    {
        public SearchFightLogic() { }
        
        public async Task<long> FindWithGoogle(string query) 
        {
            long result = 0;
            string api_key = ConfigurationManager.AppSettings["GOOGLE_API_KEY"];
            string cx = ConfigurationManager.AppSettings["GOOGLE_CX"];

            string urlBase = "https://www.googleapis.com/customsearch/v1?key={0}&cx={1}&q={2}";

            string path = string.Format(urlBase, api_key, cx, query);

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(path);

            string responseStr;

            if (response.IsSuccessStatusCode)
            {
                responseStr = await response.Content.ReadAsStringAsync();
                var responseObj = JsonConvert.DeserializeObject<dynamic>(responseStr);
                var googleResult = responseObj["queries"]["request"][0]["totalResults"];
                result = googleResult;                
            }

            return result;

        }
        
        public async Task<long> FindWithBing(string query)
        {
            long result = 0;
            string api_key = ConfigurationManager.AppSettings["BING_API_KEY"];

            string urlBase = "https://api.cognitive.microsoft.com/bing/v7.0/search?q={0}";

            string path = string.Format(urlBase, query);

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", api_key);
            HttpResponseMessage response = await client.GetAsync(path);

            string responseStr;

            if (response.IsSuccessStatusCode)
            {
                responseStr = await response.Content.ReadAsStringAsync();
                var responseObj = JsonConvert.DeserializeObject<dynamic>(responseStr);
                var bingResult = responseObj["webPages"]["totalEstimatedMatches"];
                result = bingResult;
            }

            return result;

        }

        public string GetWinner(List<string> summaryResults) 
        {
            string result = "";

            long maxResult = 0;
            long totalRow = 0;
            
            string[] rowItems;

            foreach (var item in summaryResults)
            {
                rowItems = item.Split(',');
                totalRow = Convert.ToInt64(rowItems[1]) + Convert.ToInt64(rowItems[2]);

                if (totalRow > maxResult) 
                {
                    maxResult = totalRow;
                    result = rowItems[0];
                }

            }

            return result;
        }

    }


}
