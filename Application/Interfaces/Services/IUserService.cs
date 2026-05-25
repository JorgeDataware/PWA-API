using PWA_API.Application.DTOs.Users;
using PWA_API.Domain.Common;

namespace PWA_API.Application.Interfaces.Services;

public interface IUserService
{
    Task<Result<IEnumerable<UserDto>>> GetAllAsync();
    Task<Result<UserDto>> GetByIdAsync(int id);
    Task<Result<UserDto>> CreateAsync(CreateUserDto dto);
    Task<Result<UserDto>> UpdateAsync(int id, UpdateUserDto dto);
    Task<Result<bool>> DeleteAsync(int id);
}
