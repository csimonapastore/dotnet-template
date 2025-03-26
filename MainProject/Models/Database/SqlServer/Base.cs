using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace BasicDotnetTemplate.MainProject.Models.Database.SqlServer;

public class Base
{
    public int Id { get; set; }
    [MaxLength(45)]
    public string Guid { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreationTime { get; set; }
#nullable enable
    public int? CreationUserId { get; set; }
    public DateTime? UpdateTime { get; set; }
    public int? UpdateUserId { get; set; }
    public DateTime? DeletionTime { get; set; }
    public int? DeletionUserId { get; set; }
#nullable disable
}




