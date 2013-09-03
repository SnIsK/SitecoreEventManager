using Sitecore.Analytics.Automation.Data;

namespace Sitecore.Modules.EventManager.Events
{
    public class DeRegisterUser
    {
        public void RemoveUserFromEngagementPlan(object sender, Args.RegisterUserEventArgs args)
        {
            var signupStateId = AnalyticsHelper.GetState("Registered", Sitecore.Data.ID.Parse(args.EventItem.PlanId));
            var removedStateId = AnalyticsHelper.GetState("Deregistered", Sitecore.Data.ID.Parse(args.EventItem.PlanId));
            AutomationManager.Provider.ChangeUserState(args.User.Profile.UserName, signupStateId.Guid, removedStateId.Guid);
        } 
    }
}