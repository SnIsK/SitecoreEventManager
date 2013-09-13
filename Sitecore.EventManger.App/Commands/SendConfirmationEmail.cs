using System;
using Sitecore.Modules.EventManager.Entities;
using Sitecore.Security.Accounts;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.WebControls;
using Action = Sitecore.Web.UI.WebControls.Action;

namespace Sitecore.Modules.EventManager.App.Commands
{
    public class SendConfirmationEmail : Command
    {
        public override void Execute(CommandContext context)
        {
            string itemId = System.Web.HttpContext.Current.Request.QueryString["id"];

            var eventItem = new EventItem(ItemUtil.GetContentItem(Guid.Parse(itemId)));
            string userName = context.Parameters["ids"];

            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new Exception("There was no one selected");
            }

            User user = User.FromName(string.Format("extranet\\{0}", userName.TrimStart('{').TrimEnd('}')), false);

            eventItem.SendConfirmationEmail(user);

        }
    }
}