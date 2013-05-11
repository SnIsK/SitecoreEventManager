using Sitecore.Speak.Web.UI.WebControls;

namespace Sitecore.Modules.EventManager.App.Ui.WebControls
{
    public class EventListControl : ObjectDetailList
    {
        protected override void InitializeDataSourceControlSelectMethod()
        {
            base.InitializeDataSourceControlSelectMethod();

            string sortParameterName = this.DataSourceItem["SortParameterName"];
            if (string.IsNullOrWhiteSpace(sortParameterName))
                return;

            this.dataSourceControl.SortParameterName = sortParameterName;
        }
    }
}