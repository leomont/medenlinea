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
    
    public partial class Estadisticas
    {
        public int IdEstadistica { get; set; }
        public Nullable<int> CantBusquedas { get; set; }
        public Nullable<int> ClickEnPaginaWeb { get; set; }
        public Nullable<int> ConsultasRealizadas { get; set; }
        public Nullable<int> ClickEnCurriculum { get; set; }
        public Nullable<int> IdEspecialista { get; set; }
    
        public virtual Especialistas Especialistas { get; set; }
    }
}
