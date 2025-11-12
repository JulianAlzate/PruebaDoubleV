using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dtos
{
    public class TicketFiltroDto
    {
        public string? Usuario { get; set; }
        public int Pagina { get; set; } = 1;
        public int TamanoPagina { get; set; } = 10;
    }
}
