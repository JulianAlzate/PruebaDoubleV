using BLL.Dtos;
using BLL.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ApiPruebaDoubleV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketsServices _ticketsServices;
        public TicketsController(ITicketsServices ticketsServices)
        {

            _ticketsServices = ticketsServices;
        }


        [HttpPost]
        public async Task<IActionResult> GuardarTicket(CrearTicketDto dto)
        {
            try
            {

                //resultadoTicket = await _ticketsServices.CrearTicketAsync(dto);
                int idTicket = await _ticketsServices.CrearTicketAsync(dto);
                if (idTicket <= 0)
                    return BadRequest(new { mensaje = "No se pudo crear el ticket." });


                return CreatedAtAction(nameof(GuardarTicket), new { id = idTicket }, new { idTicket });


            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error interno al procesar la solicitud.", detalle = ex.Message });

            }

        }
        [HttpGet("RecuperarTickets")]
        public async Task<IActionResult> ObtenerTickets([FromQuery] TicketFiltroDto filtro)
        {
            try
            {
                IEnumerable<TicketDto> resultado = await _ticketsServices.ObtenerTicketsAsync(filtro);
                if (resultado != null)
                    return Ok(resultado);

                return NotFound("No se encontraron registros");

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al obtener los tickets", detalle = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] TicketEditarDto dto)
        {
            try
            {


                await _ticketsServices.EditarTicketAsync(dto);
                return Ok(new { message = "Ticket actualizado correctamente" });



            }
            catch (Exception ex)
            {

                return StatusCode(500, new { mensaje = "Error interno al editar los tickets", detalle = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                await _ticketsServices.EliminarTicketAsync(id);
                return Ok(new { message = "Ticket eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al eliminar los tickets", detalle = ex.Message });

            }
        }
    }
}
