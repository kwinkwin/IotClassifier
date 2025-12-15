using IotClassifier.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IotClassifier.Application.Interfaces
{
    public interface IClassificationService
    {
        Task<List<DashboardStatDto>> GetTodayStatisticsAsync();
        Task<string> AddClassificationLogAsync(CreateClassificationLogDto request);
    }
}
