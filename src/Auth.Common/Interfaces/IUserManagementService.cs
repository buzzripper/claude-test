using Dyvenix.Auth.Common.DTOs;

namespace Dyvenix.Auth.Common.Interfaces;

public interface IUserManagementService
{
    Task<ApiResponse<UserDto>> GetUserAsync(string userId, Guid organizationId);
    Task<ApiResponse<List<UserDto>>> GetAllUsersAsync(Guid organizationId);
    Task<ApiResponse<UserDto>> CreateUserAsync(CreateUserRequest request);
    Task<ApiResponse<bool>> AssignRoleAsync(AssignRoleRequest request);
    Task<ApiResponse<bool>> AssignPermissionAsync(AssignPermissionRequest request);
}
