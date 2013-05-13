using System;

namespace Sitecore.Modules.EventManager.Entities
{
    public class PlanData
    {
        public Guid Id { get; set; }

        [StateName("Signed up")]
        public int NumberOfSignups { get; set; }
    }

    public class StateNameAttribute : Attribute
    {
        public StateNameAttribute(string stateName)
        {
            this.StateName = stateName;
        }

        public string StateName { get; private set; }
    }
}