using AutoMapper;
using PWA_API.Application.DTOs.Users;
using PWA_API.Application.Interfaces.Repositories;
using PWA_API.Application.Interfaces.Services;
using PWA_API.Domain.Common;
using PWA_API.Domain.Entities;
using PWA_API.Domain.Enums;

namespace PWA_API.Application.Services;

public class UserService(
    IUserRepository userRepository,
    INewsQueryService newsQueryService,
    IMapper mapper) : IUserService
{
    public async Task<Result<IEnumerable<UserDto>>> GetAllAsync()
    {
        var users = await newsQueryService.GetAllUsersAsync();
        return Result<IEnumerable<UserDto>>.Success(users);
    }

    public async Task<Result<UserDto>> GetByIdAsync(int id)
    {
        var user = await newsQueryService.GetUserByIdAsync(id);
        if (user is null)
            return Result<UserDto>.NotFound($"User with id {id} not found.");
        return Result<UserDto>.Success(user);
    }

    public async Task<Result<UserDto>> CreateAsync(CreateUserDto dto)
    {
        if (await userRepository.ExistsByEmailAsync(dto.Email))
            return Result<UserDto>.Failure("Email is already in use.");

        if (await userRepository.ExistsByUsernameAsync(dto.Username))
            return Result<UserDto>.Failure("Username is already taken.");

        var user = new User
        {
            FullName = dto.FullName,
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = (UserRole)dto.Role,
            MustChangePassword = dto.MustChangePassword
        };

        await userRepository.AddAsync(user);
        return Result<UserDto>.Success(mapper.Map<UserDto>(user), 201);
    }

    public async Task<Result<UserDto>> UpdateAsync(int id, UpdateUserDto dto)
    {
        var user = await userRepository.GetByIdAsync(id);
        if (user is null)
            return Result<UserDto>.NotFound($"User with id {id} not found.");

        if (dto.Email is not null && dto.Email != user.Email && await userRepository.ExistsByEmailAsync(dto.Email))
            return Result<UserDto>.Failure("Email is already in use.");

        if (dto.Username is not null && dto.Username != user.Username && await userRepository.ExistsByUsernameAsync(dto.Username))
            return Result<UserDto>.Failure("Username is already taken.");

        if (dto.FullName is not null) user.FullName = dto.FullName;
        if (dto.Username is not null) user.Username = dto.Username;
        if (dto.Email is not null) user.Email = dto.Email;
        if (dto.Password is not null) user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        if (dto.Role.HasValue) user.Role = (UserRole)dto.Role.Value;
        if (dto.MustChangePassword.HasValue) user.MustChangePassword = dto.MustChangePassword.Value;

        await userRepository.UpdateAsync(user);
        return Result<UserDto>.Success(mapper.Map<UserDto>(user));
    }

    public async Task<Result<UserDto>> SetActiveAsync(int id, bool isActive, int actorUserId)
    {
        if (id == actorUserId && !isActive)
            return Result<UserDto>.Failure("You cannot deactivate your own account.");

        var user = await userRepository.GetByIdAsync(id);
        if (user is null)
            return Result<UserDto>.NotFound($"User with id {id} not found.");

        if (!isActive && user.Role == UserRole.Admin && user.IsActive &&
            await userRepository.CountActiveAdminsAsync() <= 1)
            return Result<UserDto>.Failure("At least one active administrator is required.");

        user.IsActive = isActive;
        user.DeactivatedAt = isActive ? null : DateTime.UtcNow;
        await userRepository.UpdateAsync(user);
        return Result<UserDto>.Success(mapper.Map<UserDto>(user));
    }

    public async Task<Result<bool>> DeleteAsync(int id, int actorUserId)
    {
        var user = await userRepository.GetByIdAsync(id);
        if (user is null)
            return Result<bool>.NotFound($"User with id {id} not found.");

        var result = await SetActiveAsync(id, false, actorUserId);
        return result.IsSuccess
            ? Result<bool>.Success(true)
            : Result<bool>.Failure(result.Error!, result.StatusCode);
    }
}
