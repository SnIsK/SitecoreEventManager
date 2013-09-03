using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Analytics.Automation.Data;
using Sitecore.Data.Items;
using Sitecore.Events;
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

        public EventRootItem EventRoot
        {
            get
            {
                var eventRoot = this.InnerItem.Axes.GetAncestors().FirstOrDefault(t => t.TemplateID == Configuration.Settings.RootTemplateId);

                if (eventRoot == null)
                    return null;

                return new EventRootItem(eventRoot);
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
        public void RegisterUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user", "user cant be null");
            }
            Event.RaiseEvent("eventmanager:registeruser", this, new RegisterUserEventArgs()
            {
                EventItem = this,
                User = user
            });
        }

        /// <summary>
        /// Changes the user to the removed state
        /// </summary>
        /// <param name="user">The user to remove</param>
        /// <returns></returns>
        public void DeregistrationUser(User user)
        {
            Event.RaiseEvent("eventmanager:registeruser", user, this);
        }


        public List<string> GetRegistered()
        {
            var stateVisitors = AutomationManager.Provider.GetStateVisitors(AnalyticsHelper.GetState("Registered", Sitecore.Data.ID.Parse(this.PlanId)).Guid);

            return stateVisitors;
        }
    }
}