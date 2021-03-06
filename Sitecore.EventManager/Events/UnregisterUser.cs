﻿using System;
using Sitecore.Analytics.Automation.Data;
using Sitecore.Diagnostics;
using Sitecore.Events;
using Sitecore.Modules.EventManager.Events.Args;

namespace Sitecore.Modules.EventManager.Events
{
    public class UnregisterUser
    {

        public void RemoveUserToAttendeesStore(object sender, EventArgs args)
        {
            var eventArgs = Event.ExtractParameter(args, 1) as UnregisterUserEventArgs;

            if (!Configuration.Settings.AttendeesStore.RemoveUser(eventArgs.EventItem, eventArgs.User))
            {
                eventArgs.Error = true;
            }

        }

        public void RemoveUserFromEngagementPlan(object sender, EventArgs args)
        {
            var eventArgs = Event.ExtractParameter(args, 1) as UnregisterUserEventArgs;
            var signupStateId = AnalyticsHelper.GetState("Registered", Sitecore.Data.ID.Parse(eventArgs.EventItem.PlanId));
            var removedStateId = AnalyticsHelper.GetState("Deregistered", Sitecore.Data.ID.Parse(eventArgs.EventItem.PlanId));
            AutomationManager.Provider.ChangeUserState(eventArgs.User.Profile.UserName, signupStateId.Guid, removedStateId.Guid);
        }

        public void SendUserConfirmationEmail(object sender, EventArgs args)
        {
            var eventArgs = Event.ExtractParameter(args, 1) as UnregisterUserEventArgs;

            Assert.IsNotNull(eventArgs, "EventArgs are null!");

            if (!eventArgs.EventItem.SendConfirmationEmail(eventArgs.User))
            {
                eventArgs.Error = true;
            }
        } 
    }
}