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
        public DbSet<User> Users => Set<User>();
        public DbSet<Weapon> Weapons => Set<Weapon>();

    }
}