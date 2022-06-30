using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Common.Models.TRANSMISION
{
    public class AdministradoresVM
    {
        public string UsuarioId { get; set; }
        public string Nombres { get; set; }
        public string UsuarioIds { get; set; }
    }
    public class ListaSemanasAnualVM
    {
        public string Semana { get; set; }
        public string Fecha { get; set; }
    }

    public class TicketsVM
    {
        public string Agente { get; set; }
        public string Base { get; set; }
        public string Billable { get; set; }
        public string Cliente { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime Date { get; set; }
        public string Estado { get; set; }
        public string Grupo { get; set; }
        public string Horas { get; set; }
        public string? Nota { get; set; }
        public string Prioridad { get; set; }
        public string RequesterName { get; set; }
        public string Ticket { get; set; }
        public int T { get; set; }
        public string TipodeTicket { get; set; }
        public string Semana { get; set; }
        public string Administrador { get; set; }
        public  DateTime Fecha { get; set; }
    }
    public class GetTicketsVM : TicketsVM
    {
        public string Fecha { get; set; }
    }

    public class GetTicketsTableVM
    {
        public string Administrador { get; set; }
        public string TipodeTicket { get; set; }
        public int Cantidad { get; set; }
        public string Cliente { get; set; }
    }
}
