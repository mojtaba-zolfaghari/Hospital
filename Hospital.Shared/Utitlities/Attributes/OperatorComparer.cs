using System.Reflection;

namespace Hospital.Shared.Utitlities.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class OperatorComparer : Attribute
    {
        public readonly Utitlities.PublicEnumes.OperatorComparer _operatorName;
        public OperatorComparer(Utitlities.PublicEnumes.OperatorComparer operatorCmp)
        {
            _operatorName = operatorCmp;
        }
        // MyMethod will have MyAttribute but not YourAttribute.
    }
}
