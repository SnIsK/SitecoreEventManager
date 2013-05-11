using System;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace Sitecore.Modules.EventManager.App.Entities
{
    public class EventListData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Going { get; set; }
        public DateTime To { get; set; }
        public DateTime From { get; set; }

        public static EventListData Converter(Item input)
        {
            var data = new EventListData();
            data.Id = input.ID.Guid.ToString("D");
            data.Name = input.DisplayName;
            data.Going = 10;
            var toField = (Sitecore.Data.Fields.DateField) input.Fields["To"];
            data.To = toField.DateTime;
            var fromField = (Sitecore.Data.Fields.DateField)input.Fields["From"];
            data.From = fromField.DateTime;

            return data;
        }
    }
}