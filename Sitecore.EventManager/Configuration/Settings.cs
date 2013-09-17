using Sitecore.Data;
using Sitecore.Diagnostics;

namespace Sitecore.Modules.EventManager.Configuration
{
    public static class Settings
    {

        public static TemplateID EventBranchId
        {
            get
            {
                string eventTemplateId = Sitecore.Configuration.Settings.GetSetting("EventManager.EventBranchId");
                Assert.IsNotNullOrEmpty(eventTemplateId, "Missing EventManager.EventBranchId setting");
                return new TemplateID(ID.Parse(eventTemplateId));
            }
        }

        public static TemplateID EventTemplateId
        {
            get
            {
                string eventTemplateId = Sitecore.Configuration.Settings.GetSetting("EventManager.EventTemplateId");
                Assert.IsNotNullOrEmpty(eventTemplateId, "Missing EventManager.EventTemplateId setting");
                return new TemplateID(ID.Parse(eventTemplateId));
            }
        }

        public static TemplateID RootTemplateId
        {
            get
            {
                string rootTemplateId = Sitecore.Configuration.Settings.GetSetting("EventManager.RootTemplateId");
                Assert.IsNotNullOrEmpty(rootTemplateId, "Missing EventManager.RootTemplateId setting");
                return new TemplateID(ID.Parse(rootTemplateId));   
            }
        }
    }
}