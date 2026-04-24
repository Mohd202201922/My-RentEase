using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentEase.API.Models;

[Table("Property")]
public partial class Property
{
    [Key]
    [Column("PropertyID")]
    public int PropertyId { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(250)]
    public string? Description { get; set; }

    [StringLength(50)]
    public string? PropertyType { get; set; }

    [StringLength(100)]
    public string? ImgPath { get; set; }

    // Foreign key to Location
    [Column("LocationID")]
    public int LocationId { get; set; }

    [ForeignKey("LocationId")]
    public virtual Location Location { get; set; } = null!;

    public virtual ICollection<Unit> Units { get; set; } = new List<Unit>();
}