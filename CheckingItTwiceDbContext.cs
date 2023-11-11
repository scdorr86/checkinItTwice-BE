using Microsoft.EntityFrameworkCore;
using checkinItTwice_BE.Models;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.Xml;

public class CheckingItTwiceDbContext : DbContext
{



    public CheckingItTwiceDbContext(DbContextOptions<CheckingItTwiceDbContext> context) : base(context)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }
}