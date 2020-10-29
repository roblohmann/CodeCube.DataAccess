using System;
using System.ComponentModel.DataAnnotations;

namespace CodeCube.DataAccess.EntityFrameworkCore.Attributes
{
    /// <summary>
    /// Custom attribute to require the value not to be empty or have it's default value.
    /// Nullable values are allowed since the validation is for 'NotEmpty'
    /// Eg. DateTime.MinValue and Guid.Empty are not allowed.
    /// <remarks>Code is gracefully borrowed from https://andrewlock.net/creating-an-empty-guid-validation-attribute/</remarks>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public sealed class NotEmptyAttribute : ValidationAttribute
    {
        public const string DefaultErrorMessage = "The {0} field must not be empty";
        public NotEmptyAttribute() : base(DefaultErrorMessage) { }

        public override bool IsValid(object value)
        {
            //NotDefault doesn't necessarily mean required
            if (value is null)
            {
                return true;
            }

            var type = value.GetType();
            if (type.IsValueType)
            {
                var defaultValue = Activator.CreateInstance(type);
                return !value.Equals(defaultValue);
            }

            // non-null ref type
            return true;
        }
    }
}
