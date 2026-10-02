using LegacyTestPlayer.Models;
using Microsoft.EntityFrameworkCore;

namespace LegacyTestPlayer.Data;

public class TestDbContext : DbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
    {
    }

    public DbSet<Question> Questions => Set<Question>();
    public DbSet<AnswerKey> AnswerKeys => Set<AnswerKey>();
    public DbSet<Attempt> Attempts => Set<Attempt>();
    public DbSet<Response> Responses => Set<Response>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Question>().Property(q => q.Id).ValueGeneratedNever();
        modelBuilder.Entity<AnswerKey>().HasKey(k => k.QuestionId);
        modelBuilder.Entity<AnswerKey>()
            .HasOne(k => k.Question)
            .WithOne()
            .HasForeignKey<AnswerKey>(k => k.QuestionId);

        modelBuilder.Entity<Response>()
            .HasIndex(r => new { r.AttemptId, r.QuestionId })
            .IsUnique();
    }
}
