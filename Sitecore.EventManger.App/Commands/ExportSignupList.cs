using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Modules.EventManager.App.Entities;
using Sitecore.Modules.EventManager.Entities;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Text;
using Sitecore.Web;
using Sitecore.Web.UI.Sheer;
using Sitecore.Web.UI.WebControls;
using Sitecore.Web.UI.XamlSharp.Continuations;
using Action = Sitecore.Web.UI.WebControls.Action;

namespace Sitecore.Modules.EventManager.App.Commands
{
    public class ExportSignupList : Action
    {

        public string SettingsId { get; set; }
        public override void Execute(ActionContext context)
        {
            Page page = context.Owner.Page;

            UrlString urlString = new UrlString("/sitecore modules/Shell/EventManager/Handlers/DownloadCsv.ashx");

            urlString.Add("eventId", HttpContext.Current.Request.QueryString["id"]);
            urlString.Add("settingsId", "{C7B40285-CF41-49BB-B9C0-BF721733F755}");
            page.Response.Redirect(urlString.ToString());
        }
    }
}