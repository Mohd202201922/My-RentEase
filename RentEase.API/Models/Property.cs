using PropertyLeasing.API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentEase.API.Models;

[Table("Properties")]
public partial class Property
{
    [Key]
    [Column("PropertyId")]
    public Guid PropertyId { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(250)]
    public string? Description { get; set; }

    [Required]
    [StringLength(200)]
    public string Address { get; set; } = null!;

    [StringLength(50)]
    public string? City { get; set; }

    [StringLength(50)]
    public string? PropertyType { get; set; }

    [StringLength(100)]
    public string? ImgPath { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual ICollection<Unit> Units { get; set; } = new List<Unit>();
}