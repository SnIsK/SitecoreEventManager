using Sitecore.Data.Items;

namespace Sitecore.Modules.EventManager.Entities
{
    public class EventRootItem : CustomItem
    {
        public EventRootItem(Item innerItem)
            : base(innerItem)
        {
        }

        public Item EngangementPlanItem
        {
            get
            {
                var engagementPlanItem = (Sitecore.Data.Fields.LookupField) this.InnerItem.Fields["Standard Message Plan"];

                return engagementPlanItem.TargetItem;
            }
        }
    }
}