using BLL.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface ITicketsServices
    {
        Task<int> CrearTicketAsync(CrearTicketDto  crearTicketDto);
        Task<IEnumerable<TicketDto>> ObtenerTicketsAsync(TicketFiltroDto filtro);
        Task<int> EditarTicketAsync(TicketEditarDto ticketEditarDto);
        Task<int> EliminarTicketAsync(int id);
    }
}
