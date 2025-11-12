using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dtos
{
    public class TicketEditarDto
    {
        public int Id { get; set; }
        public string? Usuario { get; set; }
        public bool Estatus { get; set; }

    }
}
