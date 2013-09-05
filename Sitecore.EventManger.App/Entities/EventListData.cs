using System;
using System.Linq;
using Sitecore.Analytics;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Data.Query;
using Sitecore.Modules.EventManager.Entities;

namespace Sitecore.Modules.EventManager.App.Entities
{
    public class EventListData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime To { get; set; }
        public DateTime From { get; set; }

        public int Registered
        {
            get { return this.Plan.NumberOfRegistred; }
        }

        public int Deregistered
        {
            get { return this.Plan.NumberOfDeregistered; }
        }

        public int PageViews
        {
            get
            {
                // TODO: QUERY THE DATABASE for the pageview :( 
                var i =
                    (int)
                    Massive.DynamicModel.Open("Analytics")
                           .Scalar("SELECT count(*) as PageViews FROM Pages where ItemId = @0", this.Id);
                return i;
            }
        }

        public static EventListData Converter(Item input)
        {
            var data = new EventListData();
            data.Id = input.ID.Guid.ToString("D");
            data.Name = input.Fields["Title"].Value;
            var toField = (Sitecore.Data.Fields.DateField)input.Fields["To"];
            data.To = toField.DateTime;
            var fromField = (Sitecore.Data.Fields.DateField)input.Fields["From"];
            data.From = fromField.DateTime;

            var eventItem = new Sitecore.Modules.EventManager.Entities.EventItem(ItemUtil.GetContentItem(ID.Parse(data.Id)));

            try
            {
                data.Plan = AnalyticsHelper.GetPlanData(eventItem.EngangementPlanItem.ID.Guid);
            }
            catch (Exception exception)
            {
                Sitecore.Diagnostics.Log.Error("EventManager: Converter failed to convert engangement plan", exception, typeof(EventListData));
            }

            return data;
        }

        protected PlanData Plan { get; set; }
    }
}