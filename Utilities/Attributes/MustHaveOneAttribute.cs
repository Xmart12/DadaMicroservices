using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Utilities.Attributes
{
    public class MustHaveOneAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is IList list)
            {
                return list.Count > 0;
            }
            return false;
        }
    }
}
