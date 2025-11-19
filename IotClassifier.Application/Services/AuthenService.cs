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
    public class AuthenService : IAuthenService
    {
        private readonly ILogger<AuthenService> _logger;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ICurrentUserService _currentUserService;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Role> _roleRepository;

        public AuthenService(ILogger<AuthenService> logger,
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
        public async Task<User> LoginAsync(LoginDto request)
        {
            var user = await _userRepository.FirstOrDefaultAsync(x => x.Username == request.Username && x.Status == (int)CommonStatus.Active);
            if (user == null)
            {
                throw new Exception("User was not found.");
            }

            var verifyResult = _passwordHasher.VerifyHashedPassword(user, user.Password, request.Password);
            if (verifyResult == PasswordVerificationResult.Failed)
            {
                throw new Exception("User was not found.");
            }

            var utcNow = DateTime.UtcNow;

            // update user
            user.UpdatedDate = utcNow;
            user.UpdatedBy = user.IdUser.ToString();

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            return user;
        }
        public async Task<string> GetRoleByIdUser(Guid idUser)
        {
            var user = await _userRepository.FirstOrDefaultAsync(x => x.IdUser == idUser && x.Status == (int)CommonStatus.Active,
                                                                 x => x.Include(u => u.IdRoleNavigation));
            if (user == null || user.IdRoleNavigation == null)
            {
                throw new Exception("User was not found.");
            }

            return user.IdRoleNavigation.Name;
        }
    }
}
