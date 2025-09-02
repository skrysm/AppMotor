using AppMotor.HttpServerKit.Samples.WebApi.Models;

using JetBrains.Annotations;

using Microsoft.EntityFrameworkCore;

namespace AppMotor.HttpServerKit.Samples.WebApi.Db;

public class TodoContext : DbContext
{
    public TodoContext(DbContextOptions<TodoContext> options)
        : base(options)
    {
    }

    [UsedImplicitly(ImplicitUseKindFlags.Assign)]
    public DbSet<TodoItem> TodoItems { get; set; } = null!;
}
