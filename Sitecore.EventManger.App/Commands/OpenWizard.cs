using Sitecore.Diagnostics;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Text;
using Sitecore.Web.UI.Sheer;
using Sitecore.Web.UI.WebControls;
using Sitecore.Web.UI.XamlSharp.Continuations;

namespace Sitecore.Modules.EventManager.App.Commands
{
    public abstract class OpenWizard : Command, ISupportsContinuation
    {
        public override void Execute(CommandContext context)
        {
            var obj = context.Items[0];
            ClientPipelineArgs args = new ClientPipelineArgs();
            args.Parameters["itemID"] = obj.ID.ToString();
            if (context.Parameters["type"] != null)
                args.Parameters["type"] = context.Parameters["type"];
            StartClientPipeline((object)this, "Run", args);
        }

        protected virtual void Run(ClientPipelineArgs args)
        {
            if (!args.IsPostBack)
            {
                Context.SetActiveSite("shell");
                UrlString urlString = new UrlString(UIUtil.GetUri(this.GetWizardUri()));
                foreach (string key in args.Parameters.AllKeys)
                    urlString.Add(key, args.Parameters[key]);
                SheerResponse.ShowModalDialog(urlString.ToString(), "540px", "590px", string.Empty, true);
                args.WaitForPostBack();
            }
            //else
            //    this.HandlePostBack(args.Parameters["type"] ?? string.Empty);
        }

        public abstract string GetWizardUri();
        

        private void StartClientPipeline(object obj, string methodName, ClientPipelineArgs args)
        {
            Assert.ArgumentNotNull(obj, "obj");
            Assert.ArgumentNotNullOrEmpty(methodName, "methodName");
            Assert.ArgumentNotNull((object)args, "args");
            ContinuationManager current = ContinuationManager.Current;
            ISupportsContinuation @object = obj as ISupportsContinuation;
            if (current != null && @object != null)
                current.Start(@object, methodName, args);
            else
                Context.ClientPage.Start(obj, methodName, args);
        }
    }


    
}