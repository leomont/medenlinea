//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Medinlinea.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Especialidades
    {
        public Especialidades()
        {
            this.Especialistas = new HashSet<Especialistas>();
        }
    
        public int IdEspecialidad { get; set; }
        public string NombreEspecialidad { get; set; }
        public string ImagenEspecialidad { get; set; }
        public Nullable<bool> Estado { get; set; }
    
        public virtual ICollection<Especialistas> Especialistas { get; set; }
    }
}
