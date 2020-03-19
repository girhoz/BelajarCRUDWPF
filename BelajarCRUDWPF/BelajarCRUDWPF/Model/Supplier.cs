using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BelajarCRUDWPF.Model
{
    [Table("TB_M_Supplier")]//Definisi Nama Tabel
    public class Supplier
    {
        [Key] //Definisi primary key (yang dibawah key menjadi primary key)
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public Supplier()
        {

        }
        public Supplier(String name, String address)
        {
            this.Name = name;
            this.Address = address;
        }
    }
}
