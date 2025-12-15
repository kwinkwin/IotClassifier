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
    public class ClassificationService : IClassificationService
    {
        private readonly ILogger<UserService> _logger;
        private readonly ICurrentUserService _currentUserService;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<ClassificationLog> _logRepo;
        private readonly IRepository<ComponentType> _typeRepo;

        public ClassificationService(ILogger<UserService> logger,
                           ICurrentUserService currentUserService,
                           IRepository<User> userRepository,
                           IRepository<ClassificationLog> logRepo,
                           IRepository<ComponentType> typeRepo)
        {
            _logger = logger;
            _currentUserService = currentUserService;
            _userRepository = userRepository;
            _logRepo = logRepo;
            _typeRepo = typeRepo;
        }

        public async Task<string> AddClassificationLogAsync(CreateClassificationLogDto request)
        {
            var normalizedName = request.ComponentName.Trim().ToLower();
            var existingType = await _typeRepo.FirstOrDefaultAsync(x => x.Name.ToLower() == normalizedName && x.Status == (int)CommonStatus.Active);

            if (existingType == null)
            {
                throw new Exception("Not found component type.");
            }

            var newLog = new ClassificationLog
            {
                IdComponentType = existingType.IdComponentType, 
                Score = request.Score,
                Timestamp = DateTime.Now,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = _currentUserService.IdUser.ToString(),
                Status = (int)CommonStatus.Active
            };

            await _logRepo.AddAsync(newLog);
            await _logRepo.SaveChangesAsync();

            return "Added classìication log successfully.";
        }

        public async Task<List<DashboardStatDto>> GetTodayStatisticsAsync()
        {
            var today = DateTime.Today;

            // Lấy tất cả log trong ngày hôm nay
            var logsToday = await _logRepo.GetAllAsync(x => x.Timestamp.Value.Date == today && x.Status == (int)CommonStatus.Active);

            // Lấy danh sách các loại linh kiện để đảm bảo hiển thị đủ các loại
            var allTypes = await _typeRepo.GetAllAsync(x => x.Status == (int)CommonStatus.Active);

            var stats = allTypes.Select(type => new DashboardStatDto
            {
                ComponentTypeId = type.IdComponentType,
                ComponentName = type.Name,
                TotalCount = logsToday.Count(log => log.IdComponentType == type.IdComponentType)
            }).ToList();

            return stats;
        }
    }
}
