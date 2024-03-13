using ChaosMod.Activator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ChaosMod.WebServer
{
    // This class is mostly made to make sure that oauth tokens aren't stored in the config
    // There should be changes made to make this more error proof but this is something that will happen in the future
    internal class HttpServer
    {
        public bool IsEnabled { get; }
        public bool IsSupported { get; }
        private HttpListener listener;

        public HttpServer()
        {
            if (!HttpListener.IsSupported)
            {
                ChaosMod.getInstance().logsource.LogInfo("HttpListener is not supported");
                return;
            }
            ChaosMod.getInstance().logsource.LogInfo("Attempting to start httpserver");
            IsSupported = true;
            listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8000/");
            listener.Start();
            listener.BeginGetContext(new AsyncCallback(HandleContext), null);
            ChaosMod.getInstance().logsource.LogInfo("Httpserver running");
        }

        public void HandleContext(IAsyncResult iftAr)
        {
            ChaosMod.getInstance().logsource.LogInfo("Received request");
            HttpListenerContext context = listener.EndGetContext(iftAr);
            listener.BeginGetContext(new AsyncCallback(HandleContext), null);
            HttpListenerResponse response = context.Response;
            HttpListenerRequest request = context.Request;
            switch (request.Url.AbsolutePath)
            {
                case "/":
                    HandleMainPage(context, request, response);
                    break;
                case "/oauth":
                    HandleOauthPage(context, request, response);
                    break;
                case "/finished":
                    HandleFinishedPage(context, request, response);
                    break;
                case "/api":
                    HandleAPI(context, request, response);
                    break;
                default:
                    HandleNotFound(context, request, response);
                    break;
            }
            ChaosMod.getInstance().logsource.LogInfo(request.Url.AbsolutePath);
        }

        private void HandleMainPage(HttpListenerContext context, HttpListenerRequest request, HttpListenerResponse response)
        {
            WriteResponse(response, "<html><body><script>window.location.href='https://id.twitch.tv/oauth2/authorize?client_id=sq2intgw715qqv89nwhf12o8fl4o3u&redirect_uri=http://localhost:8000/oauth&response_type=token&scope=chat:read+chat:edit';</script></body></html>");
        }

        private void HandleOauthPage(HttpListenerContext context, HttpListenerRequest request, HttpListenerResponse response)
        {
            WriteResponse(response, "<html><body><p>Authenticating!</p><script>fetch(\"https://api.twitch.tv/helix/users\", {method: 'get',headers: new Headers({'Authorization': 'Bearer ' + document.location.hash.split('=')[1].split('&')[0],'Client-Id': 'sq2intgw715qqv89nwhf12o8fl4o3u'})}).then(res => {res.json().then(json => {fetch('http://localhost:8000/api?token=' + document.location.hash.split('=')[1].split('&')[0] + '&username=' + json.data[0].login).then(() => {document.location.href='http://localhost:8000/finished'});});});</script></body></html>");
        }

        private void HandleFinishedPage(HttpListenerContext context, HttpListenerRequest request, HttpListenerResponse response)
        {
            WriteResponse(response, "<html><body><p>Authenticated with Twitch, you can close this page!</p></body></html>");
        }

        private void HandleAPI(HttpListenerContext context, HttpListenerRequest request, HttpListenerResponse response)
        {
            ChaosMod.getInstance().twitchOauthToken = "oauth:" + request.QueryString.Get("token");
            ChaosMod.getInstance().twitchOauthUsername = request.QueryString.Get("username");
            ChaosMod.getInstance().twitchIRCClient?.Disconnect();
            if (TimerSystem.GetEnabled())
            {
                ChaosMod.getInstance().twitchIRCClient = new Twitch.TwitchIRCClient();
                ChaosMod.getInstance().twitchIRCClient.ConnectToTwitch();
            }
            WriteResponse(response, "<html><body><p>Authenticated with Twitch, you can close this page! If the mod doesn't start please press Ctrl + P</p></body></html>");
        }

        private void HandleNotFound(HttpListenerContext context, HttpListenerRequest request, HttpListenerResponse response)
        {
            WriteResponse(response, "<html><body><p>Page wasn't found!</p></body></html>");
        }

        private void WriteResponse(HttpListenerResponse response, string responseString)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
        }
    }
}
