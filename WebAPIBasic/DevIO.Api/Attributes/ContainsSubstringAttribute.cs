using System.ComponentModel.DataAnnotations;

namespace DevIO.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class ContainsSubstringAttribute: ValidationAttribute
    {
        private readonly string _substring;

        public ContainsSubstringAttribute(string substring)
        {
            _substring = substring;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            return value.ToString()!.Contains(_substring);
        }
    }
}
