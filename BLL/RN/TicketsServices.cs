using BLL.Dtos;
using BLL.Interface;
using DataLayer.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace BLL.RN
{
    public class TicketsServices : ITicketsServices
    {
        private readonly IConfiguration _configuracion;
        private readonly IEjecutarRepository _ejecutarRepository;
        private readonly IRedisCacheService _cache;

        public TicketsServices(IConfiguration configuracion, IEjecutarRepository ejecutarRepository, IRedisCacheService cache)
        {
            _configuracion = configuracion;
            _ejecutarRepository = ejecutarRepository;
            _cache = cache;
        }

        public async Task<int> CrearTicketAsync(CrearTicketDto crearTicketDto)
        {
            string? conexion = _configuracion.GetConnectionString("connection");
            try
            {
                if (string.IsNullOrEmpty(conexion))
                {
                    throw new Exception("No se encontro la cadena de conexión");
                }
                object? parametros = null;
                parametros = new
                {
                    p_usuario = crearTicketDto.Usuario

                };
                return await _ejecutarRepository.ExecuteScalarAsync<int>("CrearTicket", conexion, parametros); ;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<int> EditarTicketAsync(TicketEditarDto ticketEditarDto)
        {
            string? conexion = _configuracion.GetConnectionString("connection");
            try
            {
                if (string.IsNullOrEmpty(conexion))
                {
                    throw new Exception("No se encontro la cadena de conexión");
                }
                object? parametros = null;
                parametros = new
                {
                    p_id = ticketEditarDto.Id,
                    p_usuario = ticketEditarDto.Usuario,
                    p_estatus = ticketEditarDto.Estatus

                };
                
                return await _ejecutarRepository.ExecuteScalarAsync<int>("editarTicket", conexion, parametros); // Devuelve 0 si fue NULL (no se encontró)
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> EliminarTicketAsync(int id)
        {
            string? conexion = _configuracion.GetConnectionString("connection");
            try
            {
                if (string.IsNullOrEmpty(conexion))
                {
                    throw new Exception("No se encontro la cadena de conexión");
                }
                object? parametros = null;
                parametros = new
                {
                    p_id = id

                };
                return await _ejecutarRepository.ExecuteScalarAsync<int>("eliminarTicket", conexion, parametros); ;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<TicketDto>> ObtenerTicketsAsync(TicketFiltroDto filtro)
        {
            string cacheKey = $"tickets_{filtro.Usuario}_{filtro.Pagina}_{filtro.TamanoPagina}";
            var cached = await _cache.GetAsync<IEnumerable<TicketDto>>(cacheKey);
            if (cached != null)
                return cached;

            string? conexion = _configuracion.GetConnectionString("connection");

            var parametros = new
            {
                p_usuario = filtro.Usuario,
                p_page = filtro.Pagina,
                p_page_size = filtro.TamanoPagina
            };

            var tickets = await _ejecutarRepository.ExecuteFunctionAsync<TicketDto>(
                "ObtenerTickets", conexion, parametros);

            await _cache.SetAsync(cacheKey, tickets, TimeSpan.FromMinutes(30));

            return tickets;
        }
    }
}
