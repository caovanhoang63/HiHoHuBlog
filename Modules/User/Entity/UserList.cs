﻿using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.User.Entity;

public class UserList
{
    public int Id { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Phone { get; set; }
    public string UserName { get; set; }

    public string? Role { get; set; }
    public Image? Avatar { get; set; }   // JSON type mapped to JsonDocument in C#

    public int Status { get; set; } = 1;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}