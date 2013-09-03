using System;
using Sitecore.Data.Items;
using Sitecore.Web.UI.WebControls;
using Action = Sitecore.Web.UI.WebControls.Action;

namespace Sitecore.Modules.EventManager.App.Commands
{
    public class OpenPageEditor : Action
    {
        public override void Execute(ActionContext context)
        {
            string itemId = System.Web.HttpContext.Current.Request.QueryString["id"];

            Item contentItem = EventManager.ItemUtil.GetContentItem(Guid.Parse(itemId));


            CommandUtil.OpenPageEditor(context.Owner.Page, contentItem);
            
        }
    }
}