using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RpgGameApi.Models;

namespace RpgGameApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        //Dbye kayıt etmek için Modelleri burada tanımlamalısın!

        public DbSet<Character> Characters => Set<Character>();
        // public DbSet<Character> Characters {get;set;} // Character objesinden oluşacak Characters tablosunu oluştur. Tablo ismini genelde üretilecek modelin çoğul hali yazılır.

        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     var c1 = new Character{Id=1,Name="Altan",HitPoints=10,Strength=10,Defense=10,Intelligence=10, Class = RpgClass.Knight};
        //     var c2 = new Character{Id=2,Name="Memet",HitPoints=101,Strength=101,Defense=101,Intelligence=101, Class = RpgClass.Mage};

        //     modelBuilder.Entity<Character>().HasData(c1,c2);
        //     base.OnModelCreating(modelBuilder);
        // }
    }
}