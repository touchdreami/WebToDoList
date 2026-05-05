using Microsoft.EntityFrameworkCore;
using TestWebApp.Models;

namespace TestWebApp.Data;

public class TaskDbContext : DbContext
{
    public DbSet<ToDoTask> Tasks { get; set; }
    
    // Конструктор с параметром DbContextOptions
    public TaskDbContext(DbContextOptions<TaskDbContext> options) 
        : base(options)
    {
    }
}