using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Modules.EventManager.Interfaces;
using Sitecore.Reflection;

namespace Sitecore.Modules.EventManager.Configuration
{
    public static class Settings
    {
        private static IAttendeesStore _attendessStore;

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
        public static IAttendeesStore AttendeesStore
        {
            get
            {
                if (_attendessStore == null)
                {
                    _attendessStore = (IAttendeesStore)Sitecore.Configuration.Settings.GetProviderObject("AttendeesStore", typeof(IAttendeesStore));
                }

                return _attendessStore;
            }
        }
    }
}