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
    
    public partial class Usuarios
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string ApellidoUsuario { get; set; }
        public string EmailUsuario { get; set; }
        public string CelularUsuario { get; set; }
        public string Direccion { get; set; }
        public Nullable<bool> Activo { get; set; }
        public string CedulaUsuario { get; set; }
        public Nullable<int> IdLogin { get; set; }
    
        public virtual Login Login { get; set; }
    }
}