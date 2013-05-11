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
                // Copy engagement plan from event root and assign it to the event item               
            }
        } 
    }
}