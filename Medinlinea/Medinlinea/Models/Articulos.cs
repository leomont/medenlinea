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
    
    public partial class Articulos
    {
        public int IdArticulo { get; set; }
        public string Titulo { get; set; }
        public string Resumen { get; set; }
        public string Cuerpo { get; set; }
        public string Etiquetas { get; set; }
        public string ImagenArt { get; set; }
        public Nullable<int> IdEspecialista { get; set; }
    
        public virtual Especialistas Especialistas { get; set; }
    }
}
