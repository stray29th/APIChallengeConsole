using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace APIHandler
{
    public class APIHelper   
    {

        public static HttpClient APIClient { get; set; }

        public static void StartClient()
        {
            if (APIClient == null)
            {
                APIClient = new HttpClient();
                
                APIClient.DefaultRequestHeaders.Accept.Clear();
                APIClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
        }
    }
}
