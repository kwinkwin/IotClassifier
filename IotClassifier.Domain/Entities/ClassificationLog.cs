using System;
using System.Collections.Generic;

namespace IotClassifier.Domain.Entities;

public partial class ClassificationLog
{
    public Guid IdClassificationLog { get; set; }

    public Guid? IdComponentType { get; set; }

    public string? Score { get; set; }

    public DateTime? Timestamp { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public string? Description { get; set; }

    public int? Status { get; set; }

    public virtual ComponentType? IdComponentTypeNavigation { get; set; }
}
