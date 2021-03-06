﻿using AuthSharp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace AuthSharp
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // 在此处插入电子邮件服务可发送电子邮件。
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // 在此处插入 SMS 服务可发送短信。
            return Task.FromResult(0);
        }
    }

    // 配置此应用程序中使用的应用程序用户管理器。UserManager 在 ASP.NET Identity 中定义，并由此应用程序使用。
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // 配置用户名的验证逻辑
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // 配置密码的验证逻辑
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // 配置用户锁定默认值
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // 注册双重身份验证提供程序。此应用程序使用手机和电子邮件作为接收用于验证用户的代码的一个步骤
            // 你可以编写自己的提供程序并将其插入到此处。
            manager.RegisterTwoFactorProvider("电话代码", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "你的安全代码是 {0}"
            });
            manager.RegisterTwoFactorProvider("电子邮件代码", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "安全代码",
                BodyFormat = "你的安全代码是 {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    public class ApplicationRoleManager : RoleManager<IdentityRole>
    {
        private RoleStore<IdentityRole> roleStore;

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            var manager = new ApplicationRoleManager(new RoleStore<IdentityRole>(context.Get<ApplicationDbContext>()));
            return manager;
        }
        public ApplicationRoleManager(RoleStore<IdentityRole> roleStore)
            : base(roleStore)
        {
            // TODO: Complete member initialization
            this.roleStore = roleStore;
        }
    }

    // 配置要在此应用程序中使用的应用程序登录管理器。
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }

    public class ApplicationDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {

        protected override void Seed(ApplicationDbContext context)
        {
            InitializeIdentityForEF(context);
            base.Seed(context);
        }

        public static void InitializeIdentityForEF(ApplicationDbContext db)
        {
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            //new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
            //new ApplicationRoleManager(new RoleStore<IdentityRole>(new ApplicationDbContext())); 

            const string adminRoleName = "Administrators";
            const string userRoleName = "Users";

            var originRoles = new[] {
                new { Name = adminRoleName, DisplayName = "系统管理员组" } ,
                new { Name = userRoleName, DisplayName = "用户组" } 
            };
            foreach (var item in originRoles)
            {
                if (!roleManager.RoleExists(item.Name))
                {
                    IdentityRole administrators = new IdentityRole(item.Name);
                    IdentityResult result = roleManager.Create(administrators);
                    if (!result.Succeeded) throw new HttpException(string.Concat(result.Errors));
                }
            }
            //const string adminName = "tianyh2000@163.com";
            //const string password = "$AuthSharp$";
            var testUsers = new[]{
                new { Name = "admin", Email = "tianyh2000@163.com", Password = "$AuthSharp$", RoleName = adminRoleName } ,
                new { Name = "test", Email = "t@t.t", Password = "$Test$", RoleName = userRoleName } 
            };
            foreach (var user in testUsers)
            {
                if (userManager.FindByName(user.Name) == null)
                {
                    ApplicationUser newUser = new ApplicationUser() { UserName = user.Name, Email = user.Email, TrafficRemaining = 3 * 1024 * 1024 };
                    var result = userManager.Create(newUser, user.Password);
                    if (!result.Succeeded) throw new HttpException(string.Concat(result.Errors));
                    //添加角色
                    result = userManager.AddToRole(newUser.Id, user.RoleName);
                    //设置剩余流量
                    //newUser.TrafficRemaining = 3 * 1024 * 1024;
                }
            }

        }
    }




}
