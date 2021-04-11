using MercyProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MercyProject.Data
{
    public class MercyContext:DbContext
    {
        public MercyContext(DbContextOptions options):base(options)
        {

        }

        public DbSet<HowCanWeHelpYou> howCanWeHelpYou { get; set; }

        public DbSet<MercyProject.Models.Galleries> Galleries { get; set; }
        public DbSet<ViewModel.ViewDetails> ViewDetails { get; set; }
        //public DbSet<OurGallerry> ourGallerries { get; set; }
    }
}
