using FoodSpot.Business.Services;
using FoodSpot.Data;
using FoodSpot.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodSpot.UI.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IRestaurantService _service;
        private readonly ILogger<TestController> _logger;

        public TestController(
            ApplicationDbContext context,
            IRestaurantService service,
            ILogger<TestController> logger)
        {
            _context = context;
            _service = service;
            _logger = logger;
        }

        // GET: api/Test/connection
        [HttpGet("connection")]
        public IActionResult TestConnection()
        {
            var result = new
            {
                timestamp = DateTime.Now,
                tests = new List<object>()
            };

            // Teste 1: Conexão com o banco
            try
            {
                var canConnect = _context.Database.CanConnect();
                ((List<object>)result.tests).Add(new
                {
                    test = "Conexão com Oracle",
                    status = canConnect ? "✅ OK" : "❌ FALHOU",
                    message = canConnect ? "Banco conectado com sucesso!" : "Não conseguiu conectar no banco"
                });
            }
            catch (Exception ex)
            {
                ((List<object>)result.tests).Add(new
                {
                    test = "Conexão com Oracle",
                    status = "❌ ERRO",
                    message = ex.Message,
                    innerException = ex.InnerException?.Message
                });
            }

            // Teste 2: Verificar se a tabela existe
            try
            {
                var tableExists = _context.Model.FindEntityType(typeof(RestaurantModel)) != null;
                ((List<object>)result.tests).Add(new
                {
                    test = "Tabela Restaurants existe no modelo",
                    status = tableExists ? "✅ OK" : "❌ FALHOU",
                    message = tableExists ? "Tabela configurada corretamente" : "Tabela não encontrada no modelo"
                });
            }
            catch (Exception ex)
            {
                ((List<object>)result.tests).Add(new
                {
                    test = "Verificação da tabela",
                    status = "❌ ERRO",
                    message = ex.Message
                });
            }

            // Teste 3: Consultar dados
            try
            {
                var count = _context.Restaurants.Count();
                ((List<object>)result.tests).Add(new
                {
                    test = "Consulta de dados (SELECT)",
                    status = "✅ OK",
                    message = $"Encontrados {count} restaurante(s) no banco",
                    count = count
                });
            }
            catch (Exception ex)
            {
                ((List<object>)result.tests).Add(new
                {
                    test = "Consulta de dados (SELECT)",
                    status = "❌ ERRO",
                    message = ex.Message,
                    innerException = ex.InnerException?.Message
                });
            }

            // Teste 4: Testar INSERT
            try
            {
                var testRestaurant = new RestaurantModel
                {
                    Name = $"Teste API {DateTime.Now:HHmmss}",
                    Address = "Rua Teste, 123",
                    Phone = "11999999999",
                    Description = "Restaurante de teste",
                    AverageRating = 4.5
                };

                _context.Restaurants.Add(testRestaurant);
                _context.SaveChanges();

                // Remove logo em seguida
                _context.Restaurants.Remove(testRestaurant);
                _context.SaveChanges();

                ((List<object>)result.tests).Add(new
                {
                    test = "Insert e Delete (CREATE/DELETE)",
                    status = "✅ OK",
                    message = "Conseguiu inserir e remover dados com sucesso"
                });
            }
            catch (Exception ex)
            {
                ((List<object>)result.tests).Add(new
                {
                    test = "Insert e Delete (CREATE/DELETE)",
                    status = "❌ ERRO",
                    message = ex.Message,
                    innerException = ex.InnerException?.Message
                });
            }

            // Teste 5: Service está funcionando
            try
            {
                var restaurants = _service.GetAllRestaurantsAsync().Result;
                ((List<object>)result.tests).Add(new
                {
                    test = "IRestaurantService injetado",
                    status = "✅ OK",
                    message = "Service está funcionando",
                    count = restaurants.Count()
                });
            }
            catch (Exception ex)
            {
                ((List<object>)result.tests).Add(new
                {
                    test = "IRestaurantService injetado",
                    status = "❌ ERRO",
                    message = ex.Message,
                    innerException = ex.InnerException?.Message
                });
            }

            return Ok(result);
        }

        // GET: api/Test/model
        [HttpGet("model")]
        public IActionResult GetModelInfo()
        {
            var entityType = _context.Model.FindEntityType(typeof(RestaurantModel));

            if (entityType == null)
            {
                return NotFound("RestaurantModel não encontrado no DbContext");
            }

            var properties = entityType.GetProperties().Select(p => new
            {
                name = p.Name,
                type = p.ClrType.Name,
                isNullable = p.IsNullable,
                isPrimaryKey = p.IsPrimaryKey(),
                isForeignKey = p.IsForeignKey(),
                columnName = p.GetColumnName()
            });

            return Ok(new
            {
                tableName = entityType.GetTableName(),
                schema = entityType.GetSchema(),
                properties = properties
            });
        }

        // POST: api/Test/create-full
        [HttpPost("create-full")]
        public async Task<IActionResult> TestFullCreate([FromBody] RestaurantModel restaurant)
        {
            _logger.LogInformation("=== TESTE COMPLETO DE CRIAÇÃO ===");

            var response = new
            {
                timestamp = DateTime.Now,
                receivedData = restaurant,
                modelStateIsValid = ModelState.IsValid,
                errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => new {
                        message = e.ErrorMessage,
                        exception = e.Exception?.Message
                    })
                    .ToList(),
                result = (object)null
            };

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState inválido");
                return BadRequest(response);
            }

            try
            {
                var created = await _service.CreateRestaurantAsync(restaurant);
                _logger.LogInformation($"✅ Criado com sucesso! ID: {created.Id}");

                return Ok(new
                {
                    response.timestamp,
                    response.receivedData,
                    response.modelStateIsValid,
                    result = "✅ SUCESSO",
                    createdId = created.Id,
                    createdData = created
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Erro ao criar");

                return StatusCode(500, new
                {
                    response.timestamp,
                    response.receivedData,
                    response.modelStateIsValid,
                    result = "❌ ERRO",
                    error = ex.Message,
                    innerError = ex.InnerException?.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }
    }
}
