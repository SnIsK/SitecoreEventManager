using System;
using System.Linq;
using Sitecore.Data.Items;

namespace Sitecore.Modules.EventManager.Entities
{
    public class EventItem : CustomItem
    {
        public EventItem(Item innerItem)
            : base(innerItem)
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
    }
}