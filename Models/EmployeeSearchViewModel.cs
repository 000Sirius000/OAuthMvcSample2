using System;
using System.Collections.Generic;

namespace OAuthMvcSample.Models;

public class EmployeeSearchViewModel
{
    // Пошук за датою/часом
    public DateTime? HireDateFrom { get; set; }
    public DateTime? HireDateTo { get; set; }

    // Пошук за списком елементів
    public List<int> DepartmentIds { get; set; } = new();
    public List<int> PositionIds { get; set; } = new();

    // Пошук за початком/кінцем рядка
    public string? NameStartsWith { get; set; }
    public string? NameEndsWith { get; set; }

    // Для форм (випадаючі списки)
    public List<Department> AllDepartments { get; set; } = new();
    public List<Position> AllPositions { get; set; } = new();

    // Результати пошуку
    public List<Employee> Results { get; set; } = new();
}
