using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Globalization;
using WebApi.Dapper.Controller.Models;

namespace WebApi.Dapper.Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public ProductosController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<List<Productos>>> GetAllProducts()
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            const string sql = """
                Select * from Productos
                """;
            var productos = await connection.QueryAsync<Productos>(sql);

            return Ok(productos);
        }

        [HttpGet("{ProductoId}")]
        public async Task<ActionResult<List<Productos>>> GetProducto(int ProductoId)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            const string sql = """
                Select * from Productos
                where id = @Id
                """;
            var producto = await connection.QueryFirstOrDefaultAsync<Productos>(sql, new { Id = ProductoId });

            return producto is not null ? Ok(producto) : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<List<Productos>>> PostProducto(Productos productos)
        {

            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            const string sql = """
                Insert Into Productos (nombre, descripcion, precio, Activo, FechaAlta)
                Values (@nombre, @descripcion, @precio, @activo, getdate())
                """;

            await connection.ExecuteAsync(sql, productos);
            return Ok(productos);
        }

        [HttpPut]
        public async Task<ActionResult<List<Productos>>> PutProductos(Productos productos)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            const string sql = """
                Update Productos
                Set nombre = @Nombre, descripcion = @Descripcion, precio = @Precio, Activo = @Activo, fechaAlta = getdate()
                where id = @id
                """;

            await connection.ExecuteAsync(sql, productos);
            return Ok(productos);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Productos>>> DeleteProducto(int id)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            const string sql = """
                Delete from Productos where id = @Id
                """;
            await connection.ExecuteAsync(sql, new { Id = id });
            return Ok();
        }
    }
}
