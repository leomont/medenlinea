﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Medinlinea.Models
{
    public class MembresiasModel
    {
        public IEnumerable<Membresias> Membresias { get; set; }
        public PaginationModel Pagination { get; set; }
    }
}