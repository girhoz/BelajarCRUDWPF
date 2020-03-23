using BelajarCRUDWPF.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BelajarCRUDWPF.MyContext
{
    public class myContext : DbContext
    {
        public myContext(): base("BelajarCRUDWPF") { }
        public DbSet<Supplier> Suppliers { get; set; } //Mendaftarkan tabel yang sudah dibuat
        public DbSet<Item> Items { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
