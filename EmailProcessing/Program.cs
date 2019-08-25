using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Pop3;
using MimeKit;

namespace EmailProcessing
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                EmailExample.ShowPop3Subjects();
            }
            catch (Exception exception)
            {

                Console.WriteLine(exception.StackTrace);
            }
        }
    }
    public class EmailExample
    {
        private const string cPopUserName = "test@popserver.com";
        private const string cPopPwd = "testPwd123";
        private const string cPopMailServer = "mail.popserver.com";
        private const int cPopPort = 110;
        public static void ShowPop3Subjects()
        {
            using (EmailParser ep =new EmailParser(cPopUserName,cPopPwd, cPopMailServer, cPopPort))
            {
                ep.OpenPop3();
                ep.DisplayPop3Subjects();
                ep.ClosePop3();
            }
        }
    }
}

public class EmailParser : IDisposable
    {
        protected string User { get; set; }
        protected string Password { get; set; }
        protected string MailServer { get; set; }
        protected int Port { get; set; }
        public Pop3Client Pop3 { get; set; }

        public EmailParser(string user, string password, string mailServer, int port)
        {
            User = user;
            Password = password;
            MailServer = mailServer;
            Port = port;
            Pop3 = null;
        }

    

    public void OpenPop3()
        {
            if (Pop3==null)
            {
                Pop3=new Pop3Client();
                Pop3.Connect(this.MailServer,this.Port,false);
                Pop3.AuthenticationMechanisms.Remove("XOAUTH2");
                Pop3.Authenticate(this.User,this.Password);
            }
        }

        public void ClosePop3()
        {
            if (Pop3!=null)
            {
                Pop3.Disconnect(true);
                Pop3.Dispose();
                Pop3 = null;
            }
        }

        public void DisplayPop3Subjects()
        {
            for (int i = 0; i < Pop3?.Count; i++)
            {
                MimeMessage message = Pop3.GetMessage(i);
                Console.WriteLine($"Subject: {message.Subject}");
            }
        }


        public void Dispose()
        {
            
        }
    }

