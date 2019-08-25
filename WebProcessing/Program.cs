using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

namespace WebProcessing
{
    class Program
    {
        static void Main(string[] args)
        {
            var bs=WebExample.GetNytBestSellersListNames();
            for (int i = 0; i < bs.num_results; i++)
            {
                Console.WriteLine(bs.results[i].display_name);
            }
            Console.ReadLine();
        }
    }
    public class BestSellersListNames
    {
        public string status;
        public string copyright;
        public int num_results;
        public BestSellerListNamesItem[] results;
    }
    public class BestSellerListNamesItem
    {
        public string list_name;
        public string display_name;
        public string list_name_encoded;
        public DateTime oldest_published_date;
        public DateTime newest_published_date;
        public string updated;
    }
    public class WebExample
    {
        
        public const string cStrNYTBooksBaseUrl ="http://api.nytimes.com/svc/books/";
        public const string cStrNYTBooksResource = "v3/lists/names.json";
        public const string cStrNYTApiKeyStr = "api-key";
        public const string cStrNYTApiKeyVal = "9188ea453d814d979d832ef840b07037";
        public static BestSellersListNames GetNytBestSellersListNames()
        {
            BestSellersListNames bs = null;
            string res = String.Empty;
            using (WebParser wp = new WebParser(cStrNYTBooksBaseUrl))
            {
                res = wp.Request(cStrNYTBooksResource, Method.GET,cStrNYTApiKeyStr, cStrNYTApiKeyVal);
                bs = wp.DeserializeJson<BestSellersListNames>(res);
               }
            return bs;
        }
       
       
    }
    public class WebParser:IDisposable
    {

        protected RestClient WebClient = null;
        protected string response=String.Empty;
        public string Response { get { return response; } }

        public string Request(string resource,Method type,string param,string value)
        {
            string res = string.Empty;
            if (WebClient!=null)
            {var request=new RestRequest(resource,type);
                request.AddParameter(param, value);
                IRestResponse htmlRes = WebClient.Execute(request);
                res = htmlRes.Content;
                if (res != null)
                    response = res;
            }
            return res;
        }
        public T DeserializeJson<T>(string res)
        {
            return JsonConvert.DeserializeObject<T>(res);
        }
        protected bool disposed;

        public WebParser(string baseUrl)
        {
            WebClient=new RestClient(baseUrl);
        }

        ~WebParser() { this.Dispose(false);}
        
        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                    WebClient = null;
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
