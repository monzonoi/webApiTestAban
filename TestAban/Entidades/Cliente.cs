using System;
using System.ComponentModel.DataAnnotations;

namespace TestAban.Entidades
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo Nombres es obligatorio.")]
        public string Nombres { get; set; }
        [Required(ErrorMessage = "El campo Apellidos es obligatorio.")]
        public string Apellidos { get; set; }
        public DateTime FechaDeNacimiento { get; set; }
        public string CUIT { get; set; }
        public string Domicilio { get; set; }
        public string Celular { get; set; }
        [EmailAddress(ErrorMessage = "El campo Email no tiene un formato de correo electrónico válido.")]
        public string Email { get; set; }
    }
}
