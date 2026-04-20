using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentEase.API.Data;
using System.Security.Claims;

namespace RentEase.MVC.ViewComponents;

public class NotificationBadgeViewComponent : ViewComponent
{
    private readonly PropertyLeasingDbContext _db;

    public NotificationBadgeViewComponent(PropertyLeasingDbContext db)
    {
        _db = db;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var userIdClaim = UserClaimsPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return Content("");

        var user = await _db.Users.FirstOrDefaultAsync(u => u.IdentityUserId == userIdClaim);
        if (user == null)
            return Content("");

        var unreadCount = await _db.Notifications
            .Where(n => n.UserId == user.UserId && n.Status == "Unread")
            .CountAsync();

        return View(unreadCount);
    }
}