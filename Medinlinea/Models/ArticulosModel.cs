using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Medinlinea.Models
{
    public class ArticulosModel
    {
        public IEnumerable<Articulos> Articulos { get; set; }
        public PaginationModel Pagination { get; set; }

        public ArticulosModel()
        { 
            Pagination=new PaginationModel
            {
                CurrentPage=1,
                ItemsPerPage=1,
                TotalItems=0,
            };
        }
    }
}