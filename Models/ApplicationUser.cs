
using Microsoft.AspNetCore.Identity;

namespace ReturnFlow.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}
