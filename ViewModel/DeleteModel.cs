using MercyProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MercyProject.ViewModel
{
    public class DeleteModel:Gallery
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DeletePhoto { get; set; }
    }
}
