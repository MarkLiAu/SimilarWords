using ApplicationCore.WordDictionary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistance;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public required DbSet<Word> Words { get; set; }
    public required DbSet<WordStudy> WordStudies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Word>( builder =>
        {
            builder.ToTable("Words");
            builder.HasKey(w => w.Name);
            builder.Property(w => w.Name).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<WordStudy>( builder =>
        {
            builder.ToTable("WordStudies");
            builder.HasKey(ws => new { ws.UserName, ws.WordName });
            builder.Property(ws => ws.UserName).IsRequired().HasMaxLength(200);
            builder.Property(ws => ws.WordName).IsRequired().HasMaxLength(100);
        });
    }
    
    
}
