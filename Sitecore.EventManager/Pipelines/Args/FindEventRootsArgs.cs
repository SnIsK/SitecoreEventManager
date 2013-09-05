using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Pipelines;

namespace Sitecore.Modules.EventManager.Pipelines.Args
{
    public class FindEventRootsArgs : PipelineArgs
    {
        public FindEventRootsArgs()
        {
            this.EventRootItems = new List<Item>();
        }

        public List<Item> EventRootItems { get; private set; }
    }
}