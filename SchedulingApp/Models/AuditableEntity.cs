using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace SchedulingApp.Models
{
    [ExcludeFromCodeCoverage]
    public class AuditableEntity : BaseEntity
    {
        [Column(TypeName = "datetime2")]
        public DateTime CreationDate { get; set; }
    }
}
