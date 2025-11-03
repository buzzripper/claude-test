using Dyvenix.App1.Common.DTOs;

namespace Dyvenix.App1.Common.Interfaces;

public interface IProductService
{
    Task<ApiResponse<List<ProductDto>>> GetAllProductsAsync(Guid organizationId);
    Task<ApiResponse<ProductDto>> GetProductAsync(int id, Guid organizationId);
}
