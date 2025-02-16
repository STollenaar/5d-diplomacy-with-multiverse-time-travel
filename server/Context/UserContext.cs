using Microsoft.EntityFrameworkCore;
using Models;

namespace Context;

public abstract class UserContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
};