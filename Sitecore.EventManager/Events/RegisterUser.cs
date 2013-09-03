using System;
using System.Linq;
using Sitecore.Analytics.Automation.Data;
using Sitecore.Diagnostics;
using Sitecore.Events;
using Sitecore.Modules.EventManager.Entities;
using Sitecore.Security.Accounts;

namespace Sitecore.Modules.EventManager.Events
{
    public class RegisterUser
    {
        public void AddToEngnagementPlan(object sender, EventArgs args)
        {
            var eventItem = Event.ExtractParameter(args, 1) as EventItem;
            var user = Event.ExtractParameter(args, 0) as User;


            var removedState = AnalyticsHelper.GetState("Deregistered", Sitecore.Data.ID.Parse(eventItem.PlanId));
            var signupState = AnalyticsHelper.GetState("Registered", Sitecore.Data.ID.Parse(eventItem.PlanId));
            var stateVisistors = AutomationManager.Provider.GetStateVisitors(removedState.Guid);

            if (stateVisistors.Any(t => t == user.Profile.UserName))
            {
                AutomationManager.Provider.ChangeUserState(user.Profile.UserName, signupState.Guid, removedState.Guid);
            }

            AutomationManager.Provider.CreateAutomationState(user.Profile.UserName, eventItem.PlanId, signupState.ToGuid());
        }
    }
}