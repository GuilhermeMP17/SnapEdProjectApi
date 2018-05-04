using SnapEd.Infra.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapEd.Infra.DataContexts
{
    public class SnapEdDataContext : DbContext
    {
        //Codesp-PC
        //public SnapEdDataContext() : base(@"Data Source=CODESP2389\SQLEXPRESS;Initial Catalog=DB_SnapEd_Local;User ID=sa;Password=123Mudar;")
        //Notebook
        public SnapEdDataContext() : base(@"Data Source=.\SQLEXPRESS;Initial Catalog=DB_SnapEd_Local;User ID=sa;Password=123Mudar;") 
        {
            Database.SetInitializer<SnapEdDataContext>(new SnapEdDataContextInitializer());
        }

        internal class SnapEdDataContextInitializer : CreateDatabaseIfNotExists<SnapEdDataContext>
        {

        }

        public DbSet<ClassRoom> ClassRoom { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<StudentClass> StudantClass { get; set; }
        public DbSet<Posting> Posting { get; set; }
    }    
}
