using System;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using System.Web;
using System.Threading.Tasks;
using System.IO;

namespace PaperSearch
{
    public class ResearchController
    {

        public static async Task<Stream> MakeRequest(String query)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "3e2cb50858c44fd9aa7c2b9b8beb4c4c");

            // Request parameters
            queryString["query"] = query;
            queryString["complete"] = "0";
            queryString["count"] = "10";
            queryString["offset"] = "0";
            queryString["timeout"] = "1000";
            queryString["model"] = "latest";
            var uri = "https://api.projectoxford.ai/academic/v1.0/interpret?" + queryString;

            return await client.GetStreamAsync(uri);

        }

    }
}
