using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Doering.OneWayMail.DataAccess;
using Doering.OneWayMail.Model;
using Doering.OneWayMail.Web.Extensions;
using System.Data.Entity;

namespace Doering.OneWayMail.Web
{
    public partial class _Default : Page
    {
        private string ForwardTo => txtForwardTo.Text.Trim();
        private string EmailAddress => $"{txtEmail.Text.Trim().Replace("@" + Settings.MailDomain, "")}@{Settings.MailDomain}";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ReloadSubscriptions();
            }
        }

        private Session _session;

        public new Session Session
        {
            get
            {
                if (_session == null)
                {
                    var cookieValue = Request.Cookies["SessionId"]?.Value;
                    if (cookieValue != null)
                    {
                        var sessionId = new Guid(cookieValue);
                        using (var db = new OneWayMailContext())
                        {
                            _session = db.Sessions
                                .Include(x => x.Subscriptions)
                                .Include(x => x.Subscriptions.Select(s => s.Usages))
                                .FirstOrDefault(x => x.Id == sessionId);
                        }
                    }
                    if (_session == null)
                    {
                        using (var db = new OneWayMailContext())
                        {
                            _session = new Session {Id = Guid.NewGuid()};
                            db.Sessions.Add(_session);
                            db.SaveChanges();
                        }
                        Response.Cookies.Add(
                            new HttpCookie("SessionId", _session.Id.ToString())
                                { Expires = DateTime.Now.AddYears(1) });
                    }
                }
                return _session;
            }
        }

        protected void btnAdd_OnClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                using (var db = new OneWayMailContext())
                {
                    if (CustomValidate(db))
                    {
                        var subscription = new Subscription
                        {
                            Id = Guid.NewGuid(),
                            EmailAddress = EmailAddress,
                            ForwardTo = ForwardTo,
                            CreatedAt = DateTime.Now,
                            ValidUntil = DateTime.Now.AddDays(1),
                            Enabled = true,
                            IpAddress = Request.GetIpAddress(),
                            UserAgent = Request.UserAgent,
                            SessionId = Session.Id
                        };
                        db.Subscriptions.Add(subscription);
                        db.SaveChanges();
                        Session.Subscriptions.Add(subscription);
                        ShowSuccess($"Die Weiterleitung ist eingerichtet. Gültig bis {subscription.ValidUntil:g} Uhr.");
                        txtEmail.Text = null;
                        txtForwardTo.Text = null;
                        ReloadSubscriptions();
                    }
                }
            }
        }

        private bool CustomValidate(OneWayMailContext db)
        {
            if (ForwardTo == Settings.OneWayServiceEmailAddress ||
                db.AllowedDomains.All(domain => !ForwardTo.EndsWith("@" + domain.Name)) &&
                db.AllowedForwardAddresses.All(email => ForwardTo != email.SmtpAddress))
            {
                ShowError("Eine Weiterleitung an diese E-Mail-Adresse ist nicht erlaubt.");
                return false;
            }
            if (db.Subscriptions.Any(
                x =>
                    x.EmailAddress == EmailAddress &&
                    x.ForwardTo == ForwardTo &&
                    x.Enabled && x.ValidUntil > DateTime.Now))
            {
                ShowError("Diese Weiterleitung existiert bereits.");
                return false;
            }
            return true;
        }

        private void ReloadSubscriptions()
        {
            List<Subscription> subscriptions =
                Session.Subscriptions
                    .Where(x => x.Enabled && x.ValidUntil > DateTime.Now)
                    .OrderByDescending(x => x.CreatedAt).ToList();
            repeaterSubscriptions.DataSource = subscriptions;
            repeaterSubscriptions.DataBind();
            panelSubscriptions.Visible = subscriptions.Any();
        }

        private void ShowError(string message)
        {
            alertSuccess.Visible = false;
            alertError.Visible = true;
            txtAlertError.Text = message;
        }

        private void ShowSuccess(string message)
        {
            alertSuccess.Visible = true;
            alertError.Visible = false;
            txtAlertSuccess.Text = message;
        }
    }
}