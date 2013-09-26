using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Modules.EventManager.Entities;
using Sitecore.Security.Accounts;
using Sitecore.Shell.Framework.Commands;

namespace Sitecore.Modules.EventManager.App.Commands
{
    public class UnregisterUser : Command
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

            eventItem.UnregistrationUser(user, true);

        }
    }

}
