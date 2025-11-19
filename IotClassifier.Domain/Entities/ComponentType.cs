using System;
using System.Collections.Generic;

namespace IotClassifier.Domain.Entities;

public partial class ComponentType
{
    public Guid IdComponentType { get; set; }

    public string? Name { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public string? Description { get; set; }

    public int? Status { get; set; }

    public virtual ICollection<ClassificationLog> ClassificationLogs { get; set; } = new List<ClassificationLog>();
}
