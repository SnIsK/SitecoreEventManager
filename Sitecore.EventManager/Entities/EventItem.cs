using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using Sitecore.Analytics.Automation.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Events;
using Sitecore.Links;
using Sitecore.Modules.EventManager.Events.Args;
using Sitecore.Security.Accounts;

namespace Sitecore.Modules.EventManager.Entities
{
    public class EventItem : CustomItem
    {
        public EventItem(Item innerItem)
            : base(innerItem)
        {
        }

        /// <summary>
        /// Uses Sitecore.Context.Item to create the EventItem
        /// </summary>
        public EventItem()
            : base(Sitecore.Context.Item)
        {
        }

        public DateField To
        {
            get { return this.InnerItem.Fields["To"]; }
        }

        public DateField From
        {
            get { return this.InnerItem.Fields["From"]; }
        }

        public TextField Title
        {
            get { return this.InnerItem.Fields["Title"]; }
        }

        public TextField Description
        {
            get { return this.InnerItem.Fields["Description"]; }
        }

        public TextField Location
        {
            get { return this.InnerItem.Fields["Location"]; }
        }

        public String Url
        {
            get { return LinkManager.GetItemUrl(this.InnerItem); }
        }

        public String FullUrl
        {
            get
            {
                UrlOptions urlOptions = UrlOptions.DefaultOptions;

                urlOptions.AlwaysIncludeServerUrl = true;
                return LinkManager.GetItemUrl(this.InnerItem, urlOptions);
            }
        }

        public TextField EmailMessage
        {
            get
            {
                return this.InnerItem.Fields["EmailMessage"];
            }
        }

        public TextField EmailSubject
        {
            get
            {
                return this.InnerItem.Fields["EmailSubject"];
            }
        }

        public TextField EmailName
        {
            get
            {
                return this.InnerItem.Fields["EmailName"];
            }
        }

        public TextField EmailFrom
        {
            get
            {
                return this.InnerItem.Fields["FromEmail"];
            }
        }

        public EventRoot EventRoot
        {
            get
            {
                var eventRoot =
                    this.InnerItem.Axes.GetAncestors()
                        .FirstOrDefault(t => t.TemplateID == Configuration.Settings.RootTemplateId);

                if (eventRoot == null)
                    return null;

                return new EventRoot(eventRoot);
            }
        }

        public Guid PlanId
        {
            get { return Guid.Parse(this.InnerItem.Fields["{AB68A816-1091-4729-84E2-4A2DBF82F277}"].Value); }
            set { this.InnerItem.Fields["{AB68A816-1091-4729-84E2-4A2DBF82F277}"].Value = value.ToString("D"); }
        }

        /// <summary>
        /// Adds a user in the signup stage
        /// If the user is in the removed state, move the user back to the signed up state
        /// </summary>
        /// <param name="user">The user to signup for the event</param>
        /// <returns></returns>
        public bool RegisterUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user", "user cant be null");
            }
            var eventArgs = new RegisterUserEventArgs()
            {
                EventItem = this,
                User = user
            };

            Event.RaiseEvent("eventmanager:registeruser", this, eventArgs);

            return !eventArgs.Error;
        }

        /// <summary>
        /// Changes the user to the removed state
        /// </summary>
        /// <param name="user">The user to remove</param>
        /// <param name="fromCommand">Is it run from a command</param>
        /// <returns></returns>
        public bool UnregistrationUser(User user, bool fromCommand = false)
        {
            var eventArgs = new UnregisterUserEventArgs()
            {
                EventItem = this,
                User = user,
                FromCommand = fromCommand
            };

            Event.RaiseEvent("eventmanager:unregisteruser", this, eventArgs);


            return !eventArgs.Error;
        }


        public List<string> GetRegistered()
        {
            var stateVisitors =
                AutomationManager.Provider.GetStateVisitors(
                    AnalyticsHelper.GetState("Registered", Sitecore.Data.ID.Parse(this.PlanId)).Guid);

            return stateVisitors;
        }

        public bool SendConfirmationEmail(User user)
        {
            try
            {
                using (var client = new SmtpClient())
                {
                    var mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress(this.EmailFrom.Value, this.EmailName.Value);
                    mailMessage.To.Add(new MailAddress(user.Profile.Email, user.Profile.FullName));
                    string mailContent = TransformMail(user);
                    mailMessage.IsBodyHtml = true;
                    mailMessage.Body = mailContent;
                    mailMessage.Subject = this.EmailSubject.Value.Replace("[Title]", this.Title.Value);
                    client.Send(mailMessage);
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("Error sending mail to {0}", user.Profile.Email), ex, this);
                return false;
            }
            return true;
        }

        private string TransformMail(User user)
        {
            var content = this.EmailMessage.Value.Replace(Environment.NewLine, "<br />");
            if (content.Contains("[Title]"))
            {
                content = content.Replace("[Title]", this.Title.Value);
            }

            if (content.Contains("[Name]"))
            {
                content = content.Replace("[Name]", user.Profile.FullName);
            }

            if (content.Contains("[EventStart]"))
            {
                if (this.From.DateTime.Date != this.To.DateTime.Date)
                {
                    content = content.Replace("[EventStart]", this.From.DateTime.ToString("d. MMMM yyyy"));
                }
                else
                {
                    content = content.Replace("[EventStart]", this.From.DateTime.ToString("H.mm d. MMMM yyyy"));
                }
            }

            if (content.Contains("[EventEnd]"))
            {
                if (this.From.DateTime.Date != this.To.DateTime.Date)
                {
                    content = content.Replace("[EventEnd]", this.To.DateTime.ToString("d. MMMM yyyy"));
                }
                else
                {
                    content = content.Replace("[EventEnd]", this.To.DateTime.ToString("H.mm d. MMMM yyyy"));
                }
            }

            if (content.Contains("[EventLocation]"))
            {
                content = content.Replace("[EventLocation]", this.Location.Value);
            }
            if (content.Contains("[#EventUnregisterLink]") && content.Contains("[/EventUnregisterLink]"))
            {
                content = content.Replace("[#EventUnregisterLink]",
                    string.Format("<a href=\"{0}/Unregister?email={1}\">",
                        this.FullUrl, user.Profile.Email));

                content = content.Replace("[/EventUnregisterLink]", "</a>");
            }
            else
            {
                content = content.Replace("[#EventUnregisterLink]", string.Empty);
                content = content.Replace("[/EventUnregisterLink]", string.Empty);
            }
            return content;
        }

        public Item EngangementPlanItem
        {
            get
            {
                var engagementPlanItem =
                    (Sitecore.Data.Fields.LookupField)this.InnerItem.Fields["Standard Message Plan"];

                return engagementPlanItem.TargetItem;
            }
        }
    }
}