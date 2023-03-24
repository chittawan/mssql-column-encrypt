using Microsoft.EntityFrameworkCore;
using mssqlEncryption.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mssqlEncryption.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {

        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<MemberEntity> Member { get; set; }
        public DbSet<ProfileImages> ProfileImages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=EncryptionDb;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.UseEncryption(_provider);
            //modelBuilder.UseEncryption(_encryptionProvider);
            //modelBuilder.HasDefaultSchema();

            modelBuilder.Entity<MemberEntity>()
                     .Property(f => f.Id)
                     .ValueGeneratedOnAdd();

        }
    }

}
