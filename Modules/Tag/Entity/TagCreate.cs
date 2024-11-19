using System.ComponentModel.DataAnnotations;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Tag.Entity;

public class TagCreate : BaseEntity 
{ 
    public int Id { get; set; }
    [Required]
    [StringLength(255)]
    public string Name { get; set; }
    
}