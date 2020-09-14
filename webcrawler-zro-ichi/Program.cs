using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace webcrawler_zro_ichi
{
    public class Program
    {
        //website that will be visited
        static string url = "https://www.anitube.site/";

        //buffer of how many episodes will be catch
        static int num_ep = 20; 

        //find all txt where has an code
        static string find_all(string bffr, string cod)
        {
            string res;
            int i, f;

            i = bffr.IndexOf(cod);
            f = bffr.LastIndexOf(cod);
            
            res = bffr.Substring(i, f - i);

            return res;
        }
        public async void execute()
        {
            await Main(null);
        }

        //get values where cod and num are parameters for the search
        static string get_by_pref(string bffr, string cod,int count,int num)
        {
            string res = "";
            int i,j;
            
            i = bffr.IndexOf(cod);

            if(num == count)
            {
                
                for (j = i+cod.Length+2; bffr[j] != '\"'; j++) ;

                res = bffr.Substring(i + cod.Length + 2, j -( i + cod.Length+2));

                return res;
            }
            else
            {
                res = bffr.Substring(i + cod.Length + 2);
                count++;
                res = get_by_pref(res,cod,count,num);
            }

            return res;
        }

        static void main_thread()
        {

            Console.WriteLine("Starting up ...");
            string html_aux = "";
            while (true)
            {
                string html = "";

                try
                {
                    HttpWebRequest rqst = WebRequest.Create(url) as HttpWebRequest;
                    rqst.Method = "GET";
                    HttpWebResponse resp = rqst.GetResponse() as HttpWebResponse;

                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        Stream inStream = resp.GetResponseStream();
                        StreamReader strmrdr = null;

                        if (String.IsNullOrEmpty(resp.CharacterSet))
                            strmrdr = new StreamReader(inStream);
                        else
                            strmrdr = new StreamReader(inStream, Encoding.GetEncoding(resp.CharacterSet));

                        html = strmrdr.ReadToEnd();

                        //  Console.WriteLine(html);

                        resp.Close();
                        strmrdr.Close();
                    }
                }
                catch (Exception err)
                {                        
                    Console.WriteLine("Erro : " + err.Message);
                    break;
                }
                if (!html_aux.Equals(html))
                {
                    html_aux = html;
                    string bf = find_all(html, "epiItem"); ///episode item

                    //Console.WriteLine($"\n\n\n\n\n\n\n\n\n\n{bf}");

                    //Fill DB with current data

                    AnimeContext ac = new AnimeContext();

                    Anime anime;

                    for (int i = 0; i < num_ep; i++)
                    {
                        string link = get_by_pref(bf, "href", 0, i);
                        string tit = get_by_pref(bf, "title", 0, 2 * i); /// there has 2 title values
                        string img = get_by_pref(bf, "img src", 0, i);

                        anime = new Anime(tit, img, link, i + 1);

                        ac.animes.Add(anime);

                        Console.WriteLine("link : " + link + "\ntitle : " + tit + "\nImg_src : " + img + "\n\n");
                    }
                    ac.SaveChanges();
                }
            }
            Console.WriteLine("Ending ...");
        }


        //main
        public static async Task Main(string[] args)
        {
            
          await Task.Run(() =>
                {
                    main_thread();
                });
        }
    }
}
