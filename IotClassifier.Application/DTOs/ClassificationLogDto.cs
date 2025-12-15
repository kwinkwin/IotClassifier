using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IotClassifier.Application.DTOs
{
    public class CreateClassificationLogDto
    {
        public string ComponentName { get; set; }
        public string Score { get; set; }
    }

    public class DashboardStatDto
    {
        public Guid ComponentTypeId { get; set; }
        public string ComponentName { get; set; }
        public int TotalCount { get; set; }
    }
}
