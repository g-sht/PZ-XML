using Auth.Persistance;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Organization>().ToTable("organization");
        modelBuilder.Entity<ContactPerson>().ToTable("contact_person");
    }

    public DbSet<Organization> Organizations { get; set; }
    public DbSet<ContactPerson> Persons { get; set; }
}