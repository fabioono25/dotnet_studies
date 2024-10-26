using Microsoft.AspNetCore.Identity;

namespace AuthorizeWithIdentityAPI;

// Add a new class named AppUser that inherits from IdentityUser.
// Why: The AppUser class is used to represent a user in the application.
// By default, the IdentityUser class is used to represent a user in the application.
public class AppUser: IdentityUser
{

}
