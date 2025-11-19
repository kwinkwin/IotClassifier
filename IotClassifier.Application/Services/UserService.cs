using IotClassifier.Application.DTOs;
using IotClassifier.Application.Interfaces;
using IotClassifier.Domain.Constants;
using IotClassifier.Domain.Entities;
using IotClassifier.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IotClassifier.Application.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ICurrentUserService _currentUserService;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Role> _roleRepository;

        public UserService(ILogger<UserService> logger,
                           IPasswordHasher<User> passwordHasher,
                           ICurrentUserService currentUserService,
                           IRepository<User> userRepository,
                           IRepository<Role> roleRepository)
        {
            _logger = logger;
            _passwordHasher = passwordHasher;
            _currentUserService = currentUserService;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }
        public async Task CreateEmployeeAsync(CreateEmployeeDto dto)
        {
            Guid idUser = Guid.Parse(_currentUserService.IdUser);
            var isExistEmployee = await _userRepository.AnyAsync(x => x.Username == dto.Username);
            if (isExistEmployee)
            {
                throw new Exception("Username has been already used. ");
            }

            var role = await _roleRepository.FirstOrDefaultAsync(x => x.Name == RoleName.Employee && x.Status == (int)CommonStatus.Active);
            if (role == null)
            {
                throw new Exception("Role is invalid.");
            }
            User user = new User();
            user.IdUser = Guid.NewGuid();
            user.IdRole = role.IdRole;
            user.FullName = dto.FullName;
            user.Username = dto.Username;
            user.Password = _passwordHasher.HashPassword(user, dto.Password);
            user.Status = (int)CommonStatus.Active;
            user.CreatedDate = DateTime.UtcNow;
            user.CreatedBy = idUser.ToString();

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync(x => x.Status == (int)CommonStatus.Active,
                                                         x => x.Include(u => u.IdRoleNavigation));
            var result = users.Select(x => new UserDto
            {
                IdUser = x.IdUser,
                IdRole = x.IdRole,
                RoleName = x.IdRoleNavigation?.Name,
                FullName = x.FullName,
                Username = x.Username,
            }).ToList();

            return result;
        }

        public async Task<List<EmployeeDto>> GetAllEmployeesAsync()
        {
            var users = await _userRepository.GetAllAsync(x => x.IdRoleNavigation.Name == RoleName.Employee,
                                                         x => x.Include(u => u.IdRoleNavigation));
            var result = users.Select(x => new EmployeeDto
            {
                IdUser = x.IdUser,
                IdRole = x.IdRole,
                RoleName = x.IdRoleNavigation?.Name,
                FullName = x.FullName,
                Username = x.Username,
                Status = x.Status
            }).ToList();

            return result;
        }

        public async Task<UserDto> GetCurrentUserInfoAsync()
        {
            if (!_currentUserService.IsAuthenticated)
                throw new UnauthorizedAccessException();

            Guid idUser = Guid.Parse(_currentUserService.IdUser);
            var currentUser = await _userRepository.FirstOrDefaultAsync(
                                    x => x.IdUser == idUser && x.Status == (int)CommonStatus.Active,
                                    x => x.Include(u => u.IdRoleNavigation));

            if (currentUser == null)
            {
                throw new Exception("Not found current user info.");
            }

            var result = new UserDto();
            result.IdUser = idUser;
            result.IdRole = currentUser.IdRole;
            result.RoleName = currentUser.IdRoleNavigation?.Name;
            result.FullName = currentUser.FullName;
            result.Username = currentUser.Username;

            return result;
        }
    }
}
