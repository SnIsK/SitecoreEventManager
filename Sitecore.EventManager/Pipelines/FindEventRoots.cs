using Sitecore.Data;
using Sitecore.Data.Items;

namespace Sitecore.Modules.EventManager.Pipelines
{
    public class FindEventRoots
    {
        public void Process(FindEventRootsArgs args)
        {
            //TODO: Make this work!
            Database database = Sitecore.Context.ContentDatabase ?? Sitecore.Context.Database;
            Item item = database.GetItem(Sitecore.Configuration.Settings.GetSetting("EventManager.EventRootId"));

            args.EventRootItems.Add(item);
        }
    }
}