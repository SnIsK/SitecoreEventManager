using System.Collections.Generic;
using System.Linq;
using Sitecore.Modules.EventManager.App.Entities;
using Sitecore.Modules.EventManager.Entities;

namespace Sitecore.Modules.EventManager.App.Repositories
{
    public class EventRepository
    {
        private readonly EventRoot _eventRoot;

        public EventRepository(EventRoot eventRoot)
        {
            _eventRoot = eventRoot;
        }

        public EventRepository() : this (EventRoot.Current)
        {
        }

        public List<EventListData> GetListData(string sortColumn)
        {
            if (!string.IsNullOrWhiteSpace(sortColumn))
            {
                var strings = sortColumn.Split(' ');
                if (strings[1] == "DESC")
                {
                    return this._eventRoot.GetEventsItem().ConvertAll(EventListData.Converter).OrderByDescending(t =>
                    {
                        var propertyInfo = t.GetType().GetProperty(strings[0]);
                        var value = propertyInfo.GetValue(t);
                        return value;
                    }).ToList();    
                }
                return this._eventRoot.GetEventsItem().ConvertAll(EventListData.Converter).OrderBy(t =>
                {
                    var propertyInfo = t.GetType().GetProperty(strings[0]);
                    var value = propertyInfo.GetValue(t);
                    return value;
                }).ToList();  

            }
            return this._eventRoot.GetEventsItem().ConvertAll(EventListData.Converter);
        }
    }
}