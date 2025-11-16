using System;
using System.ComponentModel.DataAnnotations;

namespace OAuthMvcSample.Models;

public class Employee
{
    public int Id { get; set; }

    [Required, MaxLength(150)]
    public string FullName { get; set; } = default!;

    [MaxLength(150)]
    public string? Email { get; set; }

    public DateTime HireDate { get; set; }

    // FK на Department
    public int DepartmentId { get; set; }
    public Department? Department { get; set; }

    // FK на Position
    public int PositionId { get; set; }
    public Position? Position { get; set; }
}