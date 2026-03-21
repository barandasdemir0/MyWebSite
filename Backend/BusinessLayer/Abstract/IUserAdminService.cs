using DtoLayer.AuthDtos;

namespace BusinessLayer.Abstract;

public interface IUserAdminService
{
    Task<bool> AssignRoleAsync(string UserId, string role, CancellationToken cancellationToken);
    Task<List<PendingUserDto>> GetPendingUsersAsync(CancellationToken cancellationToken);
    Task<bool> ApproveUserAsync(string userId, string role, CancellationToken cancellationToken);
    Task<bool> RejectUserAsync(string userId, CancellationToken cancellationToken);
    Task<List<ApprovedUserDto>> GetAllUserAsync(CancellationToken cancellationToken);

}
