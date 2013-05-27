using System;
using System.Linq;
using Sitecore.Analytics.Automation.Data;
using Sitecore.Data.Items;
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
        public EventItem() : base(Sitecore.Context.Item)
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
        /// </summary>
        /// <param name="user">The user to signup for the event</param>
        /// <returns></returns>
        public bool SignupUser(User user)
        {
            var stateId = AnalyticsHelper.GetState("Signed up", Sitecore.Data.ID.Parse(this.PlanId));
            AutomationManager.Provider.CreateAutomationState(user.Profile.UserName, this.PlanId, stateId.ToGuid());

            // add some validation that the user aint already in the plan
            return true;
        }
    }
}