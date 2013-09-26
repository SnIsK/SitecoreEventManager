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

            if (eventArgs.Error)
            {
                return;
            }

            var user = eventArgs.User;


            var removedState = AnalyticsHelper.GetState("Deregistered", Sitecore.Data.ID.Parse(eventArgs.EventItem.PlanId));
            var signupState = AnalyticsHelper.GetState("Registered", Sitecore.Data.ID.Parse(eventArgs.EventItem.PlanId));
            var stateVisistors = AutomationManager.Provider.GetStateVisitors(removedState.Guid);

            if (stateVisistors.Any(t => t == user.Profile.UserName))
            {
                AutomationManager.Provider.ChangeUserState(user.Profile.UserName, removedState.Guid, signupState.Guid);
            }

            AutomationManager.Provider.CreateAutomationState(user.Profile.UserName, eventArgs.EventItem.PlanId, signupState.ToGuid());
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