using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using Doering.OneWayMail.DataAccess;
using Doering.OneWayMail.Model;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Search;
using MailKit.Security;
using MimeKit;
using NLog;

namespace Doering.OneWayMail.Service
{
    public partial class Service1 : ServiceBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private bool _running;

        private Task _monitorTask;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Start();
        }

        protected override void OnStop()
        {
            Logger.Info("Stopping the service...");
            try
            {
                _running = false;
                _monitorTask.Wait();
                Logger.Info("The service has been stopped.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "An error occured when stopping the service.");
            }
        }

        public void Start()
        {
            Logger.Info("The service has been started.");
            _running = true;
            _monitorTask = Task.Run(MonitorCatchallMailbox);
        }

        private async Task MonitorCatchallMailbox()
        {
            while (_running)
            {
                try
                {
                    foreach (var mail in CheckForNewMails())
                    {
                        using (var db = new OneWayMailContext())
                        {
                            IList<Subscription> subscriptions = GetSubscriptionsForMail(mail, db);
                            Logger.Info($"Found {subscriptions.Count} subscriptions for the email.");
                            foreach (var subscription in subscriptions)
                            {
                                Forward(mail, subscription);
                                LogUsageInDatabase(db, subscription);
                            }
                        }
                    }
                }
                catch (Exception ex) when (!Debugger.IsAttached)
                {
                    Logger.Error(ex, "An unexpected error occured during checking for new mail.");
                }
                await Task.Delay(Settings.CheckInterval);
            }
        }

        private static IEnumerable<MimeMessage> CheckForNewMails()
        {
            Logger.Trace("Checking for new unread mails...");
            using (var client = new ImapClient())
            {
                client.Connect(Settings.ImapServer, Settings.ImapPort);
                client.Authenticate(Settings.ImapUser, Settings.ImapPassword);

                IMailFolder folder = client.Inbox;
                folder.Open(FolderAccess.ReadWrite);

                IList<UniqueId> unreadMails = folder.Search(SearchQuery.NotSeen);
                Logger.Trace($"Found {unreadMails.Count} unread mails.");
                foreach (var mailId in unreadMails)
                {
                    yield return folder.GetMessage(mailId);
                }
                if (unreadMails.Any())
                {
                    folder.SetFlags(unreadMails, MessageFlags.Seen, true);
                }
                client.Disconnect(true);
            }

        }

        private IList<Subscription> GetSubscriptionsForMail(MimeMessage mail, OneWayMailContext db)
        {
            string[] toAddresses = mail.To.OfType<MailboxAddress>().Select(x => x.Address).ToArray();
            return db.Subscriptions.Where(
                x => toAddresses.Contains(x.EmailAddress) && 
                     x.Enabled && x.ValidUntil > DateTime.Now).ToList();
        }

        private static void LogUsageInDatabase(OneWayMailContext db, Subscription subscription)
        {
            db.SubscriptionUsages.Add(new SubscriptionUsage
            {
                Id = Guid.NewGuid(),
                SubscriptionId = subscription.Id,
                Time = DateTime.Now
            });
            db.SaveChanges();
        }

        private static void Forward(MimeMessage messageToForward, Subscription subscription)
        {
            Logger.Info($"Forwarding the mail (subject: {messageToForward.Subject} to {subscription.ForwardTo}...");
            var message = new MimeMessage {Subject = $"Neue E-Mail für {subscription.EmailAddress}."};
            message.From.Add(new MailboxAddress(Settings.EmailSenderDisplayName, Settings.EmailSenderSmtpAddress));
            message.To.Add(new MailboxAddress(subscription.ForwardTo));

            var builder = new BodyBuilder
            {
                TextBody = 
                    $"{message.Subject}" +
                    "\r\n\r\n" +
                    $"Betreff:\r\n{messageToForward.Subject}" +
                    "\r\n\r\n" +
                    "Benachrichtigung deaktivieren:\r\n" +
                    $"{Settings.WebsiteUrl}/Remove.aspx?id={subscription.Id}" +
                    "\r\n\r\n" +
                    $"OneWayMail – {Settings.WebsiteUrl}"
            };
            builder.Attachments.Add(new MessagePart {Message = messageToForward});

            message.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect(Settings.SmtpServer, Settings.SmtpPort);
                client.Authenticate(Settings.SmtpUser, Settings.SmtpPassword);
                client.Send(message);
            }
        }
    }
}
