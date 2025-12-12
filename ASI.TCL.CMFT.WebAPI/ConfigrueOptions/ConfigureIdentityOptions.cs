using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace ASI.TCL.CMFT.WebAPI.ConfigrueOptions
{
    public class ConfigureIdentityOptions : IConfigureOptions<IdentityOptions>
    {
        //這邊是在設定UserManager/RoleManager/SignInManager的使用規則
        private readonly IHostEnvironment _hostEnvironment;

        public ConfigureIdentityOptions(IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        public void Configure(IdentityOptions options)
        {
            // User
            options.User.RequireUniqueEmail = false;

            // Password
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 5;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
           
            // SignIn
            options.SignIn.RequireConfirmedAccount = false;
            options.SignIn.RequireConfirmedEmail = false;

            if (!_hostEnvironment.IsDevelopment())
            {
               
            }

        }
    }
}