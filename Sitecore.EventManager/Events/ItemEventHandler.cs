using System;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Events;
using Sitecore.Workflows;

namespace Sitecore.Modules.EventManager.Events
{
    public class ItemEventHandler
    {
        public void OnItemAdded(object sender, EventArgs args)
        {
            var item = Event.ExtractParameter(args, 0) as Item;

            if (item == null)
                return;

            if (item.TemplateID == Configuration.Settings.EventTemplateId)
            {
                this.CopyPlan(item);
            }
        }

        private void CopyPlan(Item item)
        {
            item.Editing.BeginEdit();
            var eventItem = new Entities.EventItem(item);
            var planRoot = (Sitecore.Context.ContentDatabase ?? Sitecore.Context.Database).GetItem("{963678A4-869C-48AB-915B-D18C6D2AF357}");

            //TODO: Create more unike path
            var copiedPlanItem = eventItem.EventRoot.EngangementPlanItem.CopyTo(planRoot, item.Name);

            eventItem.PlanId = copiedPlanItem.ID.Guid;
            item.Editing.EndEdit();

            //Set the workflow state on the plan to deploy.
            // TODO: Move this to a deploy button instead!
            IWorkflow workflow = copiedPlanItem.State.GetWorkflow();

            Item workflowCommandItem = ItemUtil.GetContentItem(Guid.Parse("{4044A9C4-B583-4B57-B5FF-2791CB0351DF}"));
            if (workflow == null)
            {
                copiedPlanItem.Database.DataManager.SetWorkflowInfo(copiedPlanItem,
                    new WorkflowInfo(workflowCommandItem.Parent.Parent.ID.ToString(), workflowCommandItem.Parent.ID.ToString()));

                workflow = copiedPlanItem.State.GetWorkflow();
            }

            WorkflowInfo workflowInfo = copiedPlanItem.Database.DataManager.GetWorkflowInfo(copiedPlanItem);
            if (workflowInfo == null || workflowInfo.StateID != workflowCommandItem.Parent.ID.ToString())
            {
                return;
            }

            WorkflowResult workflowResult = workflow.Execute("{4044A9C4-B583-4B57-B5FF-2791CB0351DF}", copiedPlanItem,
                "Executed from Events save event", false, new object());
        }
    }
}