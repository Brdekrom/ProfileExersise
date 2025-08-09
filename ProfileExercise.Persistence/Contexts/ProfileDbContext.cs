using Microsoft.EntityFrameworkCore;
using ProfileExercise.Domain.Entities;

namespace ProfileExercise.Persistence.Contexts;

public class ProfileDbContext(DbContextOptions<ProfileDbContext> options) : DbContext(options)
{
    public DbSet<Profile> Profiles => Set<Profile>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var profile = modelBuilder.Entity<Profile>();

        profile.ToContainer("profiles");
        profile.HasKey(p => p.Id);
        profile.HasPartitionKey(p => p.Id);
        profile.Property(p => p.Id).ToJsonProperty("id");

        profile.OwnsOne(p => p.FirstName, n => { n.Property(x => x.Value).ToJsonProperty("firstName"); });
        profile.OwnsOne(p => p.LastName, n => { n.Property(x => x.Value).ToJsonProperty("lastName"); });

        profile.OwnsMany(p => p.SocialSkills, sb =>
        {
            sb.ToJsonProperty("socialSkills");
            sb.WithOwner().HasForeignKey("ProfileId");

            sb.Property<int>("Id");
            sb.HasKey("ProfileId", "Id");

            sb.Property(s => s.Value).ToJsonProperty("value");
        });

        profile.Navigation(p => p.SocialSkills)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        profile.OwnsMany(p => p.SocialAccounts, ab =>
        {
            ab.ToJsonProperty("socialAccounts");
            ab.WithOwner().HasForeignKey("ProfileId");

            ab.Property<int>("Id");
            ab.HasKey("ProfileId", "Id");

            ab.Property(a => a.Address).ToJsonProperty("address");
            ab.Property(a => a.SocialMediaType).ToJsonProperty("type");
        });

        profile.Navigation(p => p.SocialAccounts)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}