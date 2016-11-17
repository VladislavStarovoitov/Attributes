using System;

namespace Attributes
{
    // Should be applied to assembly only.
    [AttributeUsage(AttributeTargets.Assembly)]
    public class InstantiateAdvancedUserAttribute : InstantiateUserAttribute
    {
        public Nullable<int> ExternalId { get; set; }

        public InstantiateAdvancedUserAttribute(string firstName, string lastName) : base(firstName, lastName)
        {
            ExternalId = null;
        }

        public InstantiateAdvancedUserAttribute(int id, string firstName, string lastName) : base(id, firstName, lastName)
        {
            ExternalId = null;
        }

        public InstantiateAdvancedUserAttribute(string firstName, string lastName, int externalId) : base(firstName, lastName)
        {
            ExternalId = externalId;
        }

        public InstantiateAdvancedUserAttribute(int id, string firstName, string lastName, int externalId): base(id, firstName, lastName)
        {
            ExternalId = externalId;
        }
    }
}
