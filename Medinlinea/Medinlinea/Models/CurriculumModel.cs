using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Medinlinea.Models
{
    public class CurriculumModel
    {
        public IEnumerable<Curriculums> Curriculums { get; set; }
        public PaginationModel Pagination { get; set; }
    }
}