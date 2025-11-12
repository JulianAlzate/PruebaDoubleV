using BLL.Interface;
using Castle.Core.Configuration;
using DataLayer.Interface;
using Moq;

namespace TestProject1
{
    public class UnitTest1
    {
        private readonly Mock<IEjecutarRepository> _mockRepositorio;
        private readonly Mock<IConfiguration> _mockConfiguracion;
        private readonly ITicketsServices _servicio;

        [Fact]
        public void Test1()
        {

        }
    }
}