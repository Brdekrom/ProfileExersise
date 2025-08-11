using Microsoft.EntityFrameworkCore;
using ProfileExercise.Domain.Entities;

namespace ProfileExercise.Application.Tests.Helpers;

internal sealed class TestDbContext(DbContextOptions<TestDbContext> options) : DbContext(options)
{
    public DbSet<Profile> Profiles => Set<Profile>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var profile = modelBuilder.Entity<Profile>();

        profile.HasKey(p => p.Id);

        profile.OwnsOne(p => p.FirstName, nb => { nb.Property(n => n.Value).IsRequired(); });

        profile.OwnsOne(p => p.LastName, nb => { nb.Property(n => n.Value).IsRequired(); });

        profile.OwnsMany(p => p.SocialSkills, sb =>
        {
            sb.WithOwner().HasForeignKey("ProfileId");
            sb.Property<int>("Id");
            sb.HasKey("Id");
            sb.Property(s => s.Value).IsRequired();
        });

        profile.Navigation(p => p.SocialSkills)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        profile.OwnsMany(p => p.SocialAccounts, ab =>
        {
            ab.WithOwner().HasForeignKey("ProfileId");
            ab.Property<int>("Id");
            ab.HasKey("Id");
            ab.Property(a => a.Address).IsRequired();
            ab.Property(a => a.SocialMediaType).HasConversion<int>().IsRequired();
        });

        profile.Navigation(p => p.SocialAccounts)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}