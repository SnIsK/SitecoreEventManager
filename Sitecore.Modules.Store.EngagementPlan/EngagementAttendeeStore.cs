using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Analytics.Automation.Data;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Events;
using Sitecore.Modules.EventManager;
using Sitecore.Modules.EventManager.Entities;
using Sitecore.Modules.EventManager.Events.Args;
using Sitecore.Modules.EventManager.Interfaces;
using Sitecore.Security.Accounts;

namespace Sitecore.Modules.Store.EngagementPlan
{
    public class EngagementAttendeeStore : IAttendeesStore
    {
        public bool AddUser(EventItem eventItem, User user)
        {
            var removedState = AnalyticsHelper.GetState("Deregistered", Sitecore.Data.ID.Parse(eventItem.PlanId));
            var signupState = AnalyticsHelper.GetState("Registered", Sitecore.Data.ID.Parse(eventItem.PlanId));
            var stateVisistors = AutomationManager.Provider.GetStateVisitors(removedState.Guid);

            if (stateVisistors.Any(t => t == user.Profile.UserName))
            {
                AutomationManager.Provider.ChangeUserState(user.Profile.UserName, removedState.Guid, signupState.Guid);
            }

            AutomationManager.Provider.CreateAutomationState(user.Profile.UserName, eventItem.PlanId, signupState.ToGuid());
            return true;
        }

        public bool RemoveUser(EventItem eventItem, User user)
        {
            var signupStateId = AnalyticsHelper.GetState("Registered", Sitecore.Data.ID.Parse(eventItem.PlanId));
            var removedStateId = AnalyticsHelper.GetState("Deregistered", Sitecore.Data.ID.Parse(eventItem.PlanId));
            AutomationManager.Provider.ChangeUserState(user.Profile.UserName, signupStateId.Guid, removedStateId.Guid);

            return true;
        }

        public List<string> GetRegistered(EventItem eventItem)
        {
            var stateVisitors =
                AutomationManager.Provider.GetStateVisitors(
                    AnalyticsHelper.GetState("Registered", Sitecore.Data.ID.Parse(eventItem.PlanId)).Guid);

            return stateVisitors;


        }
    }
}