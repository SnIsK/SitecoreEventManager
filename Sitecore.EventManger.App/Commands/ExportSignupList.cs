using System.Linq;
using Sitecore.Modules.EventManager.App.Entities;
using Sitecore.Modules.EventManager.Entities;
using Sitecore.Web;
using Sitecore.Web.UI.Sheer;
using Sitecore.Web.UI.WebControls;

namespace Sitecore.Modules.EventManager.App.Commands
{
    public class ExportSignupList : Action
    {
        public override void Execute(ActionContext context)
        {
            var queryString = WebUtil.GetQueryString("id");

            var database = Sitecore.Context.ContentDatabase ?? Sitecore.Context.Database;

            var item = database.GetItem(queryString);

            var eventItem = new EventItem(item);

            var registered = eventItem.GetRegistered().Select(t => new RegisteredUser()
                {

                    Email = t,
                    Name = "TODO"
                });

            SheerResponse.Download("Web.config");
            // TODO: Make this a command instead - and make the sheerresponse work (Could be done in a DialogPage)
            // SEE : Sitecore.Modules.EmailCampaign.Speak.Web.Commands.ExportUsers

        }
    }
}