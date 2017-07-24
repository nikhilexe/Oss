using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;

namespace EnzymePackageRegistration
{
    class Program
    {
        static void Main(string[] args)
        {
            var patToken = "<your_pat_token>";
            var targetDirectory = @"<OssCart_Diectory>";
            var url = "https://witnesstest.azurewebsites.net/usages?api-version=1.0-preview";

            string credidentials = "" + ":" + patToken;
            var authorization = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(credidentials));

            foreach (var filePath in Directory.GetFiles(targetDirectory))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string data = reader.ReadToEnd();
                    Console.WriteLine("Processing cart {0}", filePath);
                    GetPOSTResponse(new Uri(url), authorization, data, ParseResponse);
                }
            }
        }

        private static void GetPOSTResponse(Uri uri, string authorization, string data, Action<Response> callback)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);

            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers["Authorization"] = "Basic " + authorization;

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            byte[] bytes = encoding.GetBytes(data);

            request.ContentLength = bytes.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                // Send the data.
                requestStream.Write(bytes, 0, bytes.Length);
            }

            request.BeginGetResponse((x) =>
            {
                using (HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(x))
                {
                    if (callback != null)
                    {
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Response));
                        var respone = serializer.ReadObject(response.GetResponseStream()) as Response;
                        callback(respone);
                    }
                }
            }, null);
        }

        private static void ParseResponse(Response response)
        {
            if (response != null)
            {
                if (response.summary != null)
                {
                    if (response.summary.total == response.summary.created)
                    {
                        Console.WriteLine("Processed 100 npm packages without any issue....");
                        Console.WriteLine("");
                    }
                    else
                    {
                        logFailedPackage(response.components);
                    }
                }
            }
        }

        private static void logFailedPackage(Components[] components)
        {
            Console.WriteLine("Failure while registering npm packages");
            if (components != null && components.Length > 0)
            {
                foreach (var component in components)
                {
                    if (component != null && component.result != null && component.result.status != "created")
                    {
                        if (component.component != null && component.component.npm != null)
                        {
                            Console.WriteLine("Failed to register component {0} of version {1} with status {2}", 
                                component.component.npm.name, component.component.npm.version, component.result.status);
                            if (component.result.registration != null)
                            {
                                Console.WriteLine("Registration status of failed component {0} is {1}", component.component.npm.name, component.result.registration.status);
                            }
                        }
                    }
                }
            }

            Console.WriteLine("");
        }
    }
}