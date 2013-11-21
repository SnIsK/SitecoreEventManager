using System;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Modules.EventManager.Entities;

namespace Sitecore.Modules.EventManager
{
    public static class ItemUtil
    {
        public static Database ContentDatabase
        {
            get { return Sitecore.Context.ContentDatabase ?? Sitecore.Context.Database; }
        }

        public static Item GetContentItem(ID id)
        {
            return ContentDatabase.GetItem(id, EventRoot.Current.DefaultLanguage);
        }

        public static Item GetContentItem(Guid guid)
        {
            return GetContentItem(ID.Parse(guid));
        }
    }
}