using System;
using Microsoft.EntityFrameworkCore;
using OAuthMvcSample.Models;

namespace OAuthMvcSample.Data;

public class AppDbContext : DbContext
{
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Position> Positions => Set<Position>();
    public DbSet<Employee> Employees => Set<Employee>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed Departments
        modelBuilder.Entity<Department>().HasData(
            new Department { Id = 1, Name = "IT", Description = "Information Technology" },
            new Department { Id = 2, Name = "HR", Description = "Human Resources" },
            new Department { Id = 3, Name = "Finance", Description = "Financial Department" }
        );

        // Seed Positions
        modelBuilder.Entity<Position>().HasData(
            new Position { Id = 1, Name = "Junior Developer", Description = "Entry-level developer" },
            new Position { Id = 2, Name = "Senior Developer", Description = "Experienced developer" },
            new Position { Id = 3, Name = "HR Manager", Description = "Manages HR processes" },
            new Position { Id = 4, Name = "Accountant", Description = "Handles company finances" }
        );

        // Seed Employees
        modelBuilder.Entity<Employee>().HasData(
            new Employee
            {
                Id = 1,
                FullName = "John Smith",
                Email = "john.smith@example.com",
                HireDate = new DateTime(2024, 10, 1, 9, 0, 0),
                DepartmentId = 1,
                PositionId = 1
            },
            new Employee
            {
                Id = 2,
                FullName = "Anna Johnson",
                Email = "anna.johnson@example.com",
                HireDate = new DateTime(2023, 5, 15, 10, 30, 0),
                DepartmentId = 2,
                PositionId = 3
            },
            new Employee
            {
                Id = 3,
                FullName = "Michael Brown",
                Email = "michael.brown@example.com",
                HireDate = new DateTime(2022, 3, 20, 8, 45, 0),
                DepartmentId = 3,
                PositionId = 4
            },
            new Employee
            {
                Id = 4,
                FullName = "Emily Davis",
                Email = "emily.davis@example.com",
                HireDate = new DateTime(2024, 1, 10, 11, 15, 0),
                DepartmentId = 1,
                PositionId = 2
            }
        );
    }
}
