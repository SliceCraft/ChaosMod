using ChaosMod.Activator;
using ChaosMod.Activator.Activators;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace ChaosMod.Twitch
{
    internal class TwitchIRCClient
    {
        TcpClient Twitch;
        StreamReader Reader;
        StreamWriter Writer;

        const string URL = "irc.chat.twitch.tv";
        const int PORT = 6697;

        string User = ChaosMod.getInstance().twitchOauthUsername;
        string OAuth = ChaosMod.getInstance().twitchOauthToken;
        string Channel = ChaosMod.getInstance().twitchOauthUsername;

        Timer task = null;

        public void ConnectToTwitch()
        {
            if(ChaosMod.getInstance().twitchOauthToken == null || ChaosMod.getInstance().twitchOauthUsername == null)
            {
                ChaosMod.getInstance().twitchIRCClient = null;
                HUDManager.Instance.DisplayTip("Twitch Error", "Chaos Mod has not been authenticated with Twitch. Open the webpage by pressing CTRL+I or go to http://localhost:8000", true);
                TimerSystem.Disable();
                return;
            }
            Twitch = new TcpClient(URL, PORT);
            SslStream sslStream = new SslStream(Twitch.GetStream(), false, ValidateServerCertificate, null);
            sslStream.AuthenticateAsClient(URL);
            Reader = new StreamReader(sslStream);
            Writer = new StreamWriter(sslStream);

            Writer.WriteLine("PASS " + OAuth);
            Writer.WriteLine("NICK " + User);
            Writer.WriteLine("JOIN #" + Channel);
            Writer.Flush();

            task = new Timer(SendPing, null, 30000, 30000);
        }

        public void Update()
        {
            if (Twitch == null) return;
            while (Twitch.Available > 0)
            {
                string message = Reader.ReadLine();
                ChaosMod.getInstance().logsource.LogInfo(message);
                if (message.StartsWith("PING"))
                {
                    Writer.WriteLine("PONG" + message.Substring(4));
                    Writer.Flush();
                    return;
                }
                if (TimerSystem.GetActivator() == null || !TimerSystem.GetActivator().getName().Equals("twitch") || message.Split(':').Length < 3) return;
                string username = message.Split(':')[1].Split('!')[0];
                string content = message.Split(':')[2];
                if(((TwitchActivator)TimerSystem.GetActivator()).highNumbers)
                {
                    if (content.Equals("4")) ((TwitchActivator)TimerSystem.GetActivator()).VoteEffect(username, 0);
                    else if (content.Equals("5")) ((TwitchActivator)TimerSystem.GetActivator()).VoteEffect(username, 1);
                    else if (content.Equals("6")) ((TwitchActivator)TimerSystem.GetActivator()).VoteEffect(username, 2);
                }
                else
                {
                    if (content.Equals("1")) ((TwitchActivator)TimerSystem.GetActivator()).VoteEffect(username, 0);
                    else if (content.Equals("2")) ((TwitchActivator)TimerSystem.GetActivator()).VoteEffect(username, 1);
                    else if (content.Equals("3")) ((TwitchActivator)TimerSystem.GetActivator()).VoteEffect(username, 2);
                }
            }
        }

        public void SendPing(object o)
        {
            ChaosMod.getInstance().logsource.LogInfo("Sending ping");
            Writer.WriteLine("PING");
            Writer.Flush();
        }

        public void sendMessage(string message)
        {
            Writer.WriteLine("PRIVMSG #" + Channel + " :" + message);
            Writer.Flush();
        }

        public void Disconnect()
        {
            Twitch.Close();
        }

        private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return sslPolicyErrors == SslPolicyErrors.None;
        }
    }
}
