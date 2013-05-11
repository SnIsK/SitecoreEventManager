using System;
using Sitecore.Analytics;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Data.Query;

namespace Sitecore.Modules.EventManager.App.Entities
{
    public class EventListData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Going
        {
            get
            {
                //TODO: Query the engangementPlan for this
                return 10;
            }
        }
        public DateTime To { get; set; }
        public DateTime From { get; set; }

        public int PageViews
        {
            get
            {
                // TODO: QUERY THE DATABASE for the pageview :( 
                return 10;
            }
        }

        public static EventListData Converter(Item input)
        {
            var data = new EventListData();
            data.Id = input.ID.Guid.ToString("D");
            data.Name = input.DisplayName;
            var toField = (Sitecore.Data.Fields.DateField)input.Fields["To"];
            data.To = toField.DateTime;
            var fromField = (Sitecore.Data.Fields.DateField)input.Fields["From"];
            data.From = fromField.DateTime;

            return data;
        }
    }
}