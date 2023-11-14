using Microsoft.EntityFrameworkCore;
using checkinItTwice_BE.Models;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.Xml;

public class CheckingItTwiceDbContext : DbContext
{
    public DbSet<ChristmasList> ChristmasLists { get; set; }
    public DbSet<ChristmasYear> ChristmasYears { get; set; }
    public DbSet<Gift> Gifts { get; set; }
    public DbSet<Giftee> Giftees { get; set; }
    public DbSet<User> Users { get; set; }



    public CheckingItTwiceDbContext(DbContextOptions<CheckingItTwiceDbContext> context) : base(context)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }
}