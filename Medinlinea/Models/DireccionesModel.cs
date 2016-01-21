using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Medinlinea.Models
{
    public class DireccionesModel
    {
        public IEnumerable<Direcciones> Direcciones { get; set; }
        public PaginationModel Pagination { get; set; }
    }
}