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
        public void AddUserToAttendeesStore(object sender, EventArgs args)
        {
            var eventArgs = Event.ExtractParameter(args, 1) as RegisterUserEventArgs;

            if (eventArgs == null || eventArgs.Error)
            {
                return;
            }

            if (Configuration.Settings.AttendeesStore.AddUser(eventArgs.EventItem, eventArgs.User))
            {
                eventArgs.Error = true;
            }
        }
     
        public void SendConfimationEmail(object sender, EventArgs args)
        {
            var eventArgs = Event.ExtractParameter(args, 1) as RegisterUserEventArgs;

            Assert.IsNotNull(eventArgs, "EventArgs are null!");

            if (!eventArgs.EventItem.SendConfirmationEmail(eventArgs.User))
            {
                eventArgs.Error = true;
            }
        }
    }
}