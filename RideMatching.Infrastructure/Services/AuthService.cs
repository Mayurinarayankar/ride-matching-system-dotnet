using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RideMatching.Application.DTOs;
using RideMatching.Application.Interfaces;
using RideMatching.Domain.Entities;
using RideMatching.Infrastructure.Data;

namespace RideMatching.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;

    public AuthService(
        UserManager<User> userManager,
        IConfiguration configuration,AppDbContext  appDbContext)
    {
        _userManager = userManager;
        _configuration = configuration;
            _context = appDbContext;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = dto.Email,
            Email = dto.Email,
            FullName = dto.FullName
        };

        var result =
            await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
            throw new Exception("Registration failed");

        await _userManager.AddToRoleAsync(user, dto.Role);
        if (dto.Role == "Rider")
        {
            _context.Riders.Add(new Rider
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Name = dto.FullName
            });
            await _context.SaveChangesAsync();
        }
        else if (dto.Role == "Driver")
        {
            _context.Drivers.Add(new Driver
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Name = dto.FullName,
                IsOnline = false,
                IsAvailable = true
            });
            await _context.SaveChangesAsync();
        }

        var token = await GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token
        };
    }
    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null)
            throw new Exception("Invalid credentials");

        var valid =
            await _userManager.CheckPasswordAsync(
                user,
                dto.Password);

        if (!valid)
            throw new Exception("Invalid credentials");

        var token = await GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token
        };
    }

    private async Task<string> GenerateToken(User user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub,
                user.Id.ToString()),
            new Claim(ClaimTypes.Email,
                user.Email!)
        };

        claims.AddRange(
            roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"]!));

        var creds =
            new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

        var token =
            new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: creds);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}