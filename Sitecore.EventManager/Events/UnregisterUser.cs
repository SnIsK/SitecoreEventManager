using System;
using Sitecore.Analytics.Automation.Data;
using Sitecore.Events;
using Sitecore.Modules.EventManager.Events.Args;

namespace Sitecore.Modules.EventManager.Events
{
    public class UnregisterUser
    {
        public void RemoveUserFromEngagementPlan(object sender, EventArgs args)
        {
            var eventArgs = Event.ExtractParameter(args, 1) as UnregisterUserEventArgs;
            var signupStateId = AnalyticsHelper.GetState("Registered", Sitecore.Data.ID.Parse(eventArgs.EventItem.PlanId));
            var removedStateId = AnalyticsHelper.GetState("Deregistered", Sitecore.Data.ID.Parse(eventArgs.EventItem.PlanId));
            AutomationManager.Provider.ChangeUserState(eventArgs.User.Profile.UserName, signupStateId.Guid, removedStateId.Guid);
        }  
    }
}