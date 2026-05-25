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
            Role = (UserRole)dto.Role
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

        await userRepository.UpdateAsync(user);
        return Result<UserDto>.Success(mapper.Map<UserDto>(user));
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        var user = await userRepository.GetByIdAsync(id);
        if (user is null)
            return Result<bool>.NotFound($"User with id {id} not found.");

        await userRepository.DeleteAsync(user);
        return Result<bool>.Success(true);
    }
}
