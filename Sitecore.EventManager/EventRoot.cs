using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Exceptions;
using Sitecore.Modules.EventManager.Configuration;
using Sitecore.Pipelines;
using System.Linq;

namespace Sitecore.Modules.EventManager
{
    public class EventRoot
    {
        private readonly Item _rootItem;

        private EventRoot(Item rootItem)
        {
            _rootItem = rootItem;
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
            Item item = this._rootItem.Add(proposeValidItemName, Settings.EventTemplateId);

            if (item == null)
            {
                throw new Exception("Event not created");
            }

            return item;
        }

        public List<Item> GetEventsItem()
        {
            return this._rootItem.Children.Select(t => t).ToList();
        }
    }

    public class FindEventRootsArgs : PipelineArgs
    {
        public FindEventRootsArgs()
        {
            this.EventRootItems = new List<Item>();
        }

        public List<Item> EventRootItems { get; private set; }
    }
}