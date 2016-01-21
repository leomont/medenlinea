using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Medinlinea.Models
{
    public class EspecialidadesModel
    {
        public IEnumerable<Especialidades> Especialidades { get; set; }
        public PaginationModel Pagination { get; set; }
    }
}