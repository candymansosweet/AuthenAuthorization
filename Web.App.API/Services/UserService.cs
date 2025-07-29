using Common.Constants;
using Common.Models;
using Domain.Entities;
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Web.App.API.Services
{
    public interface IUserService
    {
        Guid GetUserIdOnline();
        string GetUserNameOnline();
        UserInfor GetUserInfor();
        Guid GetStaffIdOnline();

    }
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private IHttpContextAccessor _httpContextAccessor;

        //IOptions<AppSettings> appSettings,
        //IMemoryCache cache,
        // public UserService()
        // {
        //    //_cache = cache;
        //    
        // }
        protected readonly AppDbContext _context;
        public UserService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public Guid GetUserIdOnline()
        {
            if (_httpContextAccessor?.HttpContext?.Items == null)
            {
                throw new InvalidOperationException("HttpContext is not available");
            }
            return (Guid)_httpContextAccessor.HttpContext.Items[ContextItems.UserId];
        }
        public string GetUserNameOnline()
        {
            if (_httpContextAccessor?.HttpContext?.Items == null)
            {
                return string.Empty;
            }
            try
            {
                return (string)_httpContextAccessor.HttpContext.Items[ContextItems.Username] ?? string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        public string GetUserFullNameOnline()
        {
            if (_httpContextAccessor?.HttpContext?.Items == null)
            {
                return string.Empty;
            }
            try
            {
                return (string)_httpContextAccessor.HttpContext.Items[ContextItems.Username] ?? string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        public Guid GetStaffIdOnline()
        {
            if (_httpContextAccessor?.HttpContext?.Items == null)
            {
                throw new InvalidOperationException("HttpContext is not available");
            }
            return (Guid)_httpContextAccessor.HttpContext.Items[ContextItems.UserId];
        }
        public UserInfor GetUserInfor()
        {
            var contextItems_UserId = (string)_httpContextAccessor.HttpContext.Items[ContextItems.UserId];

            var accId = int.Parse(contextItems_UserId);

            Account? acc = _context.Accounts
                .Include(a => a.AccountGroupPermissions)
                    .ThenInclude(ag => ag.GroupPermission)
                    .ThenInclude(ass => ass.PermissionGroupPermissions)
                    .ThenInclude(per => per.Permission).Distinct()
                .FirstOrDefault(en => en.Id == accId);
            return new UserInfor()
            {
                UserId = acc.Id.ToString(),
                Username = acc.Name,
                Permissions = acc.AccountGroupPermissions
                    .SelectMany(ag => ag.GroupPermission.PermissionGroupPermissions)
                    .Select(ap => ap.Permission.Title)
                    .Distinct()
                    .ToList(),
            };
        }
    }
}
