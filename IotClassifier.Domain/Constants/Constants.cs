using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IotClassifier.Domain.Constants
{
    public enum CommonStatus
    {
        Inactive = 0,
        Active = 1
    }
    public static class RoleName
    {
        public const string Admin = "Admin";
        public const string Employee = "Employee";
    }
}
