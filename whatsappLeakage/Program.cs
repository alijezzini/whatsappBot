using System;
using System.IO;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;
using System.Diagnostics;

namespace whatsappLeakage
{
   class HttpServer
    {
        public static HttpListener listener;
        public static string url = "http://*:8808/";
        public static QueueObj obj;
        public static int requestCount = 0; 



        public static async Task HandleIncomingConnections()
        {
            bool runServer = true;
            // While a user hasn't visited the `shutdown` url, keep on handling requests
            while (runServer)
            {
                // Will wait here until we hear from a connection
                HttpListenerContext ctx = await listener.GetContextAsync();

                // Peel out the requests and response objects
                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse resp = ctx.Response;

                // Print out some info about the request
                Console.WriteLine("Request #: {0}", ++requestCount);
                Console.WriteLine(req.Url.ToString());
                Console.WriteLine(req.HttpMethod);
                Console.WriteLine(req.UserHostName);
                Console.WriteLine(req.UserAgent);
                Console.WriteLine();
                    

                // Write the response info

                if (req.Url.AbsolutePath == "/send")
                {
                    if (req.HasEntityBody)
                    {
                        string documentContents;
                        using (Stream receiveStream = req.InputStream)
                        {
                            using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                            {
                                documentContents = readStream.ReadToEnd();
                            }
                        }
                        dynamic stuff = JsonConvert.DeserializeObject(documentContents);

                        string number = stuff.number;
                        string text = stuff.text;
                        Console.WriteLine("Number: "+number);
                        Console.WriteLine("Text: "+text);
                        string uid = Guid.NewGuid().ToString("N");
                        obj.AddQ(number, text,uid);
                        String JsonResp = "{\"termID\":\"" + uid + "\"}";
                        byte[] data = Encoding.UTF8.GetBytes(JsonResp);
                        resp.ContentType = "application/json";
                        resp.ContentEncoding = Encoding.UTF8;
                        resp.ContentLength64 = data.LongLength;
                        // Write out to the response stream (asynchronously), then close it
                        await resp.OutputStream.WriteAsync(data, 0, data.Length);
                        resp.Close();
                    }
                }
                
            }
        }


        public static void Main(string[] args)
        {
            clearProfiles();
            // Create a Http server and start listening for incoming connections
            obj = new QueueObj();
            QueueClass co = new QueueClass(obj);
            Thread NewQueue = new Thread(new ThreadStart(co.Run));
            NewQueue.Start();
            listener = new HttpListener();
            listener.Prefixes.Add(url);
 
            listener.Start();
            Console.WriteLine("Listening for connections on {0}", url);

            // Handle requests
            Task listenTask = HandleIncomingConnections();
            listenTask.GetAwaiter().GetResult();

            // Close the listener
            listener.Close();
        }
        public static void clearProfiles()
        {
            foreach (var process in Process.GetProcessesByName("chromedriver"))
            {
                try
                {
                    process.Kill();
                }
                catch
                {

                }
            }

            foreach (var process in Process.GetProcessesByName("chrome"))
            {
                try
                {
                    process.Kill();
                }
                catch
                {

                }
            }
        }
    }
}