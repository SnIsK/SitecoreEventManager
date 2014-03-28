using System;
using System.Collections.Generic;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Exceptions;
using Sitecore.Globalization;
using Sitecore.Modules.EventManager.Configuration;
using Sitecore.Modules.EventManager.Pipelines.Args;
using System.Linq;

namespace Sitecore.Modules.EventManager.Entities
{
    public class EventRoot : CustomItem
    {
        public EventRoot(Item rootItem) : base(rootItem)
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
                    var tempEventRoot = new EventRoot(args.EventRootItems.First());

                    eventRoot = new EventRoot(ItemUtil.ContentDatabase.GetItem(tempEventRoot.InnerItem.ID, tempEventRoot.DefaultLanguage));


                    System.Web.HttpContext.Current.Items["EventRoot"] = eventRoot;
                }

                return eventRoot;
            }
        }

        /// <summary>
        /// The event engangement plan
        /// It holds information about registeret, pageviews, and unregistered users
        /// </summary>
        public Item EngangementPlanItem
        {
            get
            {
                var engagementPlanItem =
                    (Sitecore.Data.Fields.LookupField) this.InnerItem.Fields["Standard Message Plan"];

                return engagementPlanItem.TargetItem;
            }
        }

        public TextField EmailMessage
        {
            get { return this.InnerItem.Fields["EmailMessage"]; }
        }

        public TextField EmailSubject
        {
            get { return this.InnerItem.Fields["EmailSubject"]; }
        }

        public TextField EmailName
        {
            get { return this.InnerItem.Fields["EmailName"]; }
        }

        public TextField EmailFrom
        {
            get { return this.InnerItem.Fields["FromEmail"]; }
        }

        public Language DefaultLanguage
        {
            get
            {
                LookupField field = this.InnerItem.Fields["DefaultLanguage"];
                Item targetItem = field.TargetItem;
                string languageName = "en";
                if (targetItem != null)
                {
                    languageName = targetItem.Name;
                }

                return LanguageManager.GetLanguage(languageName);
            }
        }

        /// <summary>
        /// Creates event item under the root
        /// </summary>
        /// <param name="eventName">The name of the event</param>
        public Item CreateEvent(string eventName)
        {
            string proposeValidItemName = Sitecore.Data.Items.ItemUtil.ProposeValidItemName(eventName);
            Item item = this.InnerItem.Add(proposeValidItemName, Settings.EventBranchId);

            if (item == null)
            {
                throw new Exception("Event not created");
            }

            return item;
        }

        public List<Item> GetEvents()
        {
            return this.InnerItem.Children.Where(t => t.TemplateID == Settings.EventTemplateId || t.Template.BaseTemplates.Any(a => a.ID.Guid == Configuration.Settings.EventTemplateId.ID.Guid)).ToList();
        }

        public List<Item> GetEvents(DateTime afterDate)
        {
            var eventsItem =
                this.GetEvents().Select(t => new EventItem(t)).Where(t => t.To.DateTime.Date >= afterDate.Date || t.From.DateTime.Date >= afterDate.Date);
            return eventsItem.Select(t => t.InnerItem).ToList();
        }

        public EventItem GetEvent(Guid id)
        {
            return new EventItem(this.InnerItem.Database.GetItem(ID.Parse(id)));
        }
    }
}