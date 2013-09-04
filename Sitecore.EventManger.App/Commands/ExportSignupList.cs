using System;
using System.Linq;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Modules.EventManager.App.Entities;
using Sitecore.Modules.EventManager.Entities;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web;
using Sitecore.Web.UI.Sheer;
using Sitecore.Web.UI.WebControls;
using Sitecore.Web.UI.XamlSharp.Continuations;

namespace Sitecore.Modules.EventManager.App.Commands
{
    public class ExportSignupList : Command, ISupportsContinuation
    {
        public override void Execute(CommandContext context)
        {
            string eventId = context.Parameters["ids"];

            //Item item = ItemUtil.GetContentItem(Guid.Parse(eventId));

            //var eventItem = new EventItem(item);

            //var registered = eventItem.GetRegistered().Select(t => new RegisteredUser()
            //    {

            //        Email = t,
            //        Name = "TODO"
            //    });

            //SheerResponse.Download("Web.config");
            //// TODO: Make this a command instead - and make the sheerresponse work (Could be done in a DialogPage)
            //// SEE : Sitecore.Modules.EmailCampaign.Speak.Web.Commands.ExportUsers

            ClientPipelineArgs args = new ClientPipelineArgs();
            args.Parameters["itemID"] = eventId;


            ContinuationManager current = ContinuationManager.Current;
            ISupportsContinuation thisObject = this as ISupportsContinuation;
            if (current != null && thisObject != null)
                current.Start(thisObject, "Run", args);
            else
                Context.ClientPage.Start(thisObject, "Run", args);
        }


        protected virtual void Run(ClientPipelineArgs args)
        {
            var eventId = args.Parameters["itemID"];
            if (!args.IsPostBack)
            {
                Context.SetActiveSite("shell");
                SheerResponse.ShowModalDialog("/", "540px", "590px", string.Empty, true);
                args.WaitForPostBack();
            }
        }
    }
}