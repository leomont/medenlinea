﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Medinlinea.Models
{
    public class UsuarioModel
    {
        public IEnumerable<Usuarios> Usuarios { get; set; }
        public PaginationModel Pagination { get; set; }
    }
}