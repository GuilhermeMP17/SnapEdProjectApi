﻿using SnapEd.Infra.Models;
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
        public SnapEdDataContext() : base(@"Data Source=CODESP2389\SQLEXPRESS;Initial Catalog=DB_SnapEd_Local;User ID=sa;Password=123Mudar;")
        //Notebook
        //public SnapEdContext() : base(@"Data Source=DESKTOP-NGBL7K8/SQLEXPRESS;Initial Catalog=DB_SnapEd_Local;User ID=sa;Password=123Mudar;") 
        {
            Database.SetInitializer<SnapEdDataContext>(new SnapEdDataContextInitializer());
        }

        internal class SnapEdDataContextInitializer : CreateDatabaseIfNotExists<SnapEdDataContext>
        {

        }

        public DbSet<ClassRoom> ClassRoom { get; set; }
    }    
}
