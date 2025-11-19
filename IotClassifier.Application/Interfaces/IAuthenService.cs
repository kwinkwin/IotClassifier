using IotClassifier.Application.DTOs;
using IotClassifier.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IotClassifier.Application.Interfaces
{
    public interface IAuthenService
    {
        Task<User> LoginAsync(LoginDto request);
        Task<string> GetRoleByIdUser(Guid idUser);
    }
}
