using System;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Events;

namespace Sitecore.Modules.EventManager.Events
{
    public class ItemEventHandler
    {
        public void OnItemAdded(object sender, EventArgs args)
        {
            var item = Event.ExtractParameter(args, 0) as Item;

            if (item == null)
                return;

            if (item.TemplateID == Configuration.Settings.EventTemplateId)
            {
                this.CopyPlan(item);
            }
        }

        private void CopyPlan(Item item)
        {
            item.Editing.BeginEdit();
            var eventItem = new Entities.EventItem(item);
            var PlanRoot = (Sitecore.Context.ContentDatabase ?? Sitecore.Context.Database).GetItem("{963678A4-869C-48AB-915B-D18C6D2AF357}");

            //TODO: Create more unike path
            var copiedPlanItem = eventItem.EventRoot.EngangementPlanItem.CopyTo(PlanRoot, item.Name);

            eventItem.PlanId = copiedPlanItem.ID.Guid;
            item.Editing.EndEdit();
        }
    }
}