using System;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Threading;

namespace HttpServer
{
    public class MobileHttpServer
    {
        public string Port { get; set; } = "46467";
        public string IP { get; set; }
        public bool Listening { get; protected set; } = false;


        private HttpListener httpListener;
        private Thread thread;

        public MobileHttpServer()
        {

            IP = GetLocalIP();
            httpListener = new HttpListener();

            if (IP != "" && Port != "")
            {
                httpListener.Prefixes.Add($"http://{IP}:{Port}/");
                httpListener.AuthenticationSchemes = AuthenticationSchemes.None;
                httpListener.IgnoreWriteExceptions = true;
            }
        }

        public void BeginListening()
        {
            if (!Listening)
            {
                thread = new Thread(new ThreadStart(Listen));
                thread.Start();
            }
        }
        public void StopListening()
        {
            if (Listening)
            {
                httpListener.Stop();
                thread.Abort();
                Listening = false;
            }
        }

        protected void Listen()
        {
            try
            {
                // Begin receiving requests
                httpListener.Start();
                Listening = true;

                Debug.WriteLine($"http://{IP}:{Port}/");
                // Loop to allow multiple clients.
                while (Listening)
                {
                    // Wait for someone to connect.
                    HttpListenerContext context = httpListener.GetContext();
                    HttpListenerResponse response = context.Response;

                    // Construct our HTML response into bytes
                    string htmlResponse = ConstructHTML();
                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(htmlResponse);

                    // Create an output stream to send our HTML through
                    response.ContentLength64 = buffer.Length;
                    System.IO.Stream output = response.OutputStream;

                    // Write the output stream over the network
                    output.Write(buffer, 0, buffer.Length);

                    // Close the stream
                    output.Close();

                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Something happened\n{e.Message}");
            }
        }


        /*
         * Return your entire HTML page.
         */
        protected virtual string ConstructHTML()
        {
            return "<html><body>Poop</body></html>";
        }


        protected string GetLocalIP()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                // If it's an IPv4 Address
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            return "";
        }
    }
}
