using DtoLayer.AuthDtos.Responses;

namespace WebUILayer.Areas.Admin.Services.Abstract;

public interface IUserAdminApiService
{
    Task<List<PendingUserDto>> GetPendingUsersAsync();
    Task<bool> ApproveUserAsync(string userId, string role);
    Task<bool> RejectUserAsync(string userId);
    Task<List<ApprovedUserDto>> GetAllUsersAsync();
}
