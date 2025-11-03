using Dyvenix.Auth.Common.DTOs;

namespace Dyvenix.Auth.Common.Interfaces;

public interface IUserManagementService
{
    Task<ApiResponse<UserDto>> GetUserAsync(string userId, Guid organizationId);
    Task<ApiResponse<List<UserDto>>> GetAllUsersAsync(Guid organizationId);
}
