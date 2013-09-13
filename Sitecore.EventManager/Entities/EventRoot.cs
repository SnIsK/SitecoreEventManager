using System;
using System.Collections.Generic;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Exceptions;
using Sitecore.Modules.EventManager.Configuration;
using Sitecore.Modules.EventManager.Pipelines.Args;
using System.Linq;

namespace Sitecore.Modules.EventManager.Entities
{
    public class EventRoot : CustomItem
    {
        private EventRoot(Item rootItem) : base(rootItem)
        {
        }

        public static EventRoot Current
        {
            get
            {
                var eventRoot = System.Web.HttpContext.Current.Items["EventRoot"] as EventRoot;

                if (eventRoot == null)
                {
                    var args = new FindEventRootsArgs();

                    Sitecore.Pipelines.CorePipeline.Run("FindEventRoots", args);

                    if (args.EventRootItems.Count == 0)
                    {
                        throw new Exception("Could not find any event roots");
                    }
                    eventRoot = new EventRoot(args.EventRootItems.First());

                    System.Web.HttpContext.Current.Items["EventRoot"] = eventRoot;
                }

                return eventRoot;
            }
        }

        /// <summary>
        /// Creates event item under the root
        /// </summary>
        /// <param name="eventName">The name of the event</param>
        public Item CreateEvent(string eventName)
        {
            string proposeValidItemName = Sitecore.Data.Items.ItemUtil.ProposeValidItemName(eventName);
            Item item = this.InnerItem.Add(proposeValidItemName, Settings.EventTemplateId);

            if (item == null)
            {
                throw new Exception("Event not created");
            }

            return item;
        }

        public List<Item> GetEvents()
        {
            return this.InnerItem.Children.Select(t => t).ToList();
        }

        public List<Item> GetEvents(DateTime afterDate)
        {
            var eventsItem = this.GetEvents().Select(t => new EventItem(t)).Where(t => t.From.DateTime.Date >= afterDate.Date);
            return eventsItem.Select(t => t.InnerItem).ToList();
        }

        public EventItem GetEvent(Guid id)
        {
            return new EventItem(this.InnerItem.Axes.GetChild(ID.Parse(id)));
        }
    }
}