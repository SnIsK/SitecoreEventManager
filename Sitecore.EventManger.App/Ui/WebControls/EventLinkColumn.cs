using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sitecore.Data.Items;
using Sitecore.Modules.EventManager.App.Entities;
using Sitecore.Speak.Web.UI.WebControls;
using Sitecore.Web.UI.WebControls;

namespace Sitecore.Modules.EventManager.App.Ui.WebControls
{
    public class EventLinkColumn : ColumnTemplate
    {
        protected override void DoRender(HtmlTextWriter output)
        {
           
        }

        public override void InstantiateIn(Control container)
        {
            HyperLink hyperLink = new HyperLink();
            hyperLink.DataBinding += new EventHandler(this.BindData);
            container.Controls.Add((Control)hyperLink);
        }

        private void BindData(object sender, EventArgs e)
        {
            var hyperLink = (HyperLink)sender;
            var modelContainer = (ModelContainer)hyperLink.NamingContainer;
            hyperLink.Text = modelContainer.Model.ToString();
            var eventListData = modelContainer.DataItem as EventListData;

            Item contentItem = ItemUtil.GetContentItem(Guid.Parse(eventListData.Id));

            hyperLink.NavigateUrl = TaskPageUtils.GetLinkForTaskPage(contentItem);
        }
    }
}