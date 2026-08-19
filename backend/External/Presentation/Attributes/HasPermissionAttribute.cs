using Microsoft.AspNetCore.Authorization;

namespace Presentation.Attributes
{
    public sealed class HasPermissionAttribute : AuthorizeAttribute
    {
        // TODO: Add Permission entity and use it here instead of string
        public HasPermissionAttribute()
            : base(policy: string.Empty)
        {
        }
    }
}
