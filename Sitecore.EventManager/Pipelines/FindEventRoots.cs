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
            Item item = database.GetItem("{C76BEF46-B5C8-4C9C-9647-2788CC631EF1}");

            args.EventRootItems.Add(item);
        }
    }
}