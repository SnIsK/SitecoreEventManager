using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Sitecore.Analytics.Automation;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Exceptions;
using Sitecore.Modules.EventManager.Entities;

namespace Sitecore.Modules.EventManager
{
    public class AnalyticsHelper
    {
        /// <summary>
        /// Gets engangement plan data
        /// </summary>
        /// <param name="engagementId">The id of the engangement plan</param>
        /// <returns></returns>
        public static PlanData GetPlanData(Guid engagementId)
        {
            // this is the only thing there is needed right now, should be changed to a sql statement, if there are properties
            //See Sitecore.Modules.EmailCampaign.Core.Analytics.GetPlanData and Sitecore.Modules.EmailCampaign.Core.Analytics.GetPlanData

            var properties = typeof(PlanData).GetProperties().Where(t => Attribute.IsDefined(t, typeof(StateNameAttribute)));

            var database = Sitecore.Context.ContentDatabase ?? Sitecore.Context.Database;

            var engagementPlan = database.GetItem(ID.Parse(engagementId));

            var childList = engagementPlan.Children;

            var planData = new PlanData();

            foreach (var propertyInfo in properties)
            {
                var attribute =
                    propertyInfo.GetCustomAttributes(typeof(StateNameAttribute), true).First() as StateNameAttribute;

                var stateItem = childList.FirstOrDefault(t => t.Name == attribute.StateName);

                var numberStateVisitors = VisitorManager.GetNumberStateVisitors(stateItem.ID);

                propertyInfo.SetValue(planData, numberStateVisitors);
            }

            return planData;
        }

        public static ID GetState(string statename, ID engagementId)
        {
            var database = Sitecore.Context.ContentDatabase ?? Sitecore.Context.Database;

            var engagementPlan = database.GetItem(engagementId);

            Assert.IsNotNull(engagementPlan, string.Format("Could not find engangement plan with id {0}", engagementId.Guid.ToString("D")));

            var stateItem = engagementPlan.Children.FirstOrDefault(t => t.Name == statename);
            Assert.IsNotNull(stateItem, string.Format(string.Format("State {0} not found under {1}", statename, engagementId.Guid.ToString("D"))));

            return stateItem.ID;
        }
    }
}