using System;
using System.Linq;
using System.Net.Mail;
using System.Web;
using Sitecore.Analytics.Automation.Data;
using Sitecore.Diagnostics;
using Sitecore.Events;
using Sitecore.Modules.EventManager.Entities;
using Sitecore.Modules.EventManager.Events.Args;
using Sitecore.Security.Accounts;

namespace Sitecore.Modules.EventManager.Events
{
    public class RegisterUser
    {
        public void AttachUserToAnalyticsState(object sender, EventArgs args)
        {
            var eventArgs = Event.ExtractParameter(args, 1) as RegisterUserEventArgs;

            Assert.IsNotNull(eventArgs, "EventArgs are null!");
            var user = eventArgs.User;


            var removedState = AnalyticsHelper.GetState("Deregistered", Sitecore.Data.ID.Parse(eventArgs.EventItem.PlanId));
            var signupState = AnalyticsHelper.GetState("Registered", Sitecore.Data.ID.Parse(eventArgs.EventItem.PlanId));
            var stateVisistors = AutomationManager.Provider.GetStateVisitors(removedState.Guid);

            if (stateVisistors.Any(t => t == user.Profile.UserName))
            {
                AutomationManager.Provider.ChangeUserState(user.Profile.UserName, signupState.Guid, removedState.Guid);
            }

            AutomationManager.Provider.CreateAutomationState(user.Profile.UserName, eventArgs.EventItem.PlanId, signupState.ToGuid());
        }


        public void SendConfimationEmail(object sender, EventArgs args)
        {
            var eventArgs = Event.ExtractParameter(args, 1) as RegisterUserEventArgs;

            Assert.IsNotNull(eventArgs, "EventArgs are null!");

            using (var client = new SmtpClient())
            {
                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(eventArgs.EventItem.EmailFrom.Value, eventArgs.EventItem.EmailName.Value);
                mailMessage.To.Add(new MailAddress(eventArgs.User.Profile.Email, eventArgs.User.Profile.FullName));
                string mailContent = TransformMail(eventArgs.EventItem, eventArgs.User);
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = mailContent;
                client.Send(mailMessage);    
            }
        }

        private string TransformMail(EventItem eventItem, User user)
        {
            var content = eventItem.EmailMessage.Value.Replace(Environment.NewLine, "<br />");
            if (content.Contains("[Name]"))
            {
                content = content.Replace("[Name]", user.Profile.FullName);
            }

            if (content.Contains("[EventStart]"))
            {
                content = content.Replace("[EventStart]", eventItem.From.DateTime.ToString("HH.mm d. MMMM yyyy"));
            }

            if (content.Contains("[EventLocation"))
            {
                content = content.Replace("[EventLocation]", eventItem.Location.Value);
            }
            if (content.Contains("[EventUnregisterLink]"))
            {
                content = content.Replace("[EventUnregisterLink]", string.Format("<a href=\"{0}?unregister=true&email={1}\">{0}?unregister=true&email={1}</a>",
                    eventItem.FullUrl, user.Profile.Email));
            }
            return content;
        }
    }
}