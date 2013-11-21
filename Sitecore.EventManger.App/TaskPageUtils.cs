using System;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Text;

namespace Sitecore.Modules.EventManager.App
{
    public class TaskPageUtils
    {
        public static string GetLinkForTaskPage(Item eventItem)
        {
            Item taskPageItem = Context.Database.GetItem(ID.Parse(UiItemIds.EventOverviewTaskPage));
            UrlString url = new UrlString(LinkManager.GetItemUrl(taskPageItem));

            url.Parameters.Add("id", eventItem.ID.ToGuid().ToString("B"));
            url.Parameters.Add("sc_speakcontentlang", eventItem.Language.Name);


            return url.ToString();
        }
    }
}