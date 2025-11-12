using BLL.Dtos;
using BLL.Interface;
using BLL.RN;
using DataLayer.Interface;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;

namespace TicketsServicesTests
{
    public class TicketsServicesTests
    {
        private readonly Mock<IEjecutarRepository> _mockRepositorio;
        private readonly Mock<IRedisCacheService> _mockRedis;
        private readonly ITicketsServices _servicio;

        public TicketsServicesTests()
        {
            _mockRepositorio = new Mock<IEjecutarRepository>();
            _mockRedis = new Mock<IRedisCacheService>();

           
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    
                    { "ConnectionStrings:connection", "Host=localhost;Port=5432;Database=PruebaDoubleV;Username=postgres;Password=123456" }
                })
                .Build();

          
            _servicio = new TicketsServices(
                configuration,
                _mockRepositorio.Object,
                _mockRedis.Object
            );
        }

        [Fact]
        public async Task EditarTicketAsync_DebeDevolverIdMayorQueCero_SiExisteElRegistro()
        {

            TicketEditarDto dto = new TicketEditarDto { Id = 1, Usuario = "julian", Estatus = true };

            _mockRepositorio
                .Setup(repo => repo.ExecuteScalarAsync<int?>(
                    "editarTicket",
                    It.IsAny<string>(),
                    It.Is<object>(p => p != null)))
                .ReturnsAsync(1);

       
            int resultado = await _servicio.EditarTicketAsync(dto);

       
            resultado.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task EditarTicketAsync_DevuelveCero_SiNoExisteElId()
        {

            TicketEditarDto dto = new TicketEditarDto { Id = 999, Usuario = "inexistente", Estatus = false };

            _mockRepositorio
                .Setup(r => r.ExecuteScalarAsync<int?>("editarTicket", It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync((int?)null);

          
            int resultado = await _servicio.EditarTicketAsync(dto);

           
            resultado.Should().Be(0);
        }

        [Fact]
        public async Task EditarTicketAsync_LanzaError_SiConexionEsNull()
        {
           
            var dto = new TicketEditarDto { Id = 1, Usuario = "test" };

            var configSinConexion = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    
                })
                .Build();

            
            var servicioConError = new TicketsServices(
                configSinConexion,
                _mockRepositorio.Object,
                _mockRedis.Object
            );

            
            await servicioConError
                .Invoking(s => s.EditarTicketAsync(dto))
                .Should().ThrowAsync<Exception>()
                .WithMessage("No se encontro la cadena de conexión");
        }
    }
}