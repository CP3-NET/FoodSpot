using FoodSpot.Business.Services;
using FoodSpot.Model;
using Microsoft.AspNetCore.Mvc;

namespace FoodSpot.UI.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly IRestaurantService _restaurantService;
        private readonly ILogger<RestaurantController> _logger;

        public RestaurantController(IRestaurantService restaurantService, ILogger<RestaurantController> logger)
        {
            _restaurantService = restaurantService;
            _logger = logger;
        }

        // GET: Restaurant
        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation("Buscando todos os restaurantes...");
                var restaurants = await _restaurantService.GetAllRestaurantsAsync();
                _logger.LogInformation($"Encontrados {restaurants.Count()} restaurantes");
                return View(restaurants);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar restaurantes");
                return View("Error");
            }
        }

        // GET: Restaurant/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                _logger.LogInformation($"Buscando restaurante com ID: {id}");
                var restaurant = await _restaurantService.GetRestaurantByIdAsync(id);

                if (restaurant == null)
                {
                    _logger.LogWarning($"Restaurante com ID {id} não encontrado");
                    return NotFound();
                }

                return View(restaurant);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar restaurante ID: {id}");
                return View("Error");
            }
        }

        // GET: Restaurant/Create
        public IActionResult Create()
        {
            _logger.LogInformation("Abrindo página de criação");
            return View();
        }

        // POST: Restaurant/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RestaurantModel restaurant)
        {
            _logger.LogInformation("=== TENTANDO CRIAR RESTAURANTE ===");
            _logger.LogInformation($"Nome recebido: {restaurant?.Name ?? "NULL"}");

            // LOG DE TODOS OS ERROS DE VALIDAÇÃO
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState INVÁLIDO! Erros encontrados:");

                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        _logger.LogError($"❌ ERRO DE VALIDAÇÃO: {error.ErrorMessage}");
                        if (error.Exception != null)
                        {
                            _logger.LogError($"   Exception: {error.Exception.Message}");
                        }
                    }
                }

                // Adiciona os erros para exibir na view
                ViewBag.ValidationErrors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return View(restaurant);
            }

            try
            {
                _logger.LogInformation("ModelState VÁLIDO! Tentando salvar no banco...");
                var created = await _restaurantService.CreateRestaurantAsync(restaurant);
                _logger.LogInformation($"✅ Restaurante criado com sucesso! ID: {created.Id}");

                TempData["SuccessMessage"] = $"Restaurante '{created.Name}' criado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ ERRO ao salvar no banco de dados");
                ModelState.AddModelError("", $"Erro ao criar restaurante: {ex.Message}");

                if (ex.InnerException != null)
                {
                    _logger.LogError($"Inner Exception: {ex.InnerException.Message}");
                    ModelState.AddModelError("", $"Detalhes: {ex.InnerException.Message}");
                }

                return View(restaurant);
            }
        }

        // GET: Restaurant/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                _logger.LogInformation($"Abrindo página de edição para ID: {id}");
                var restaurant = await _restaurantService.GetRestaurantByIdAsync(id);

                if (restaurant == null)
                {
                    _logger.LogWarning($"Restaurante com ID {id} não encontrado para edição");
                    return NotFound();
                }

                return View(restaurant);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar restaurante para edição ID: {id}");
                return View("Error");
            }
        }

        // POST: Restaurant/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RestaurantModel restaurant)
        {
            _logger.LogInformation($"=== TENTANDO EDITAR RESTAURANTE ID: {id} ===");

            if (id != restaurant.Id)
            {
                _logger.LogError($"ID da URL ({id}) diferente do ID do modelo ({restaurant.Id})");
                return NotFound();
            }

            // LOG DE TODOS OS ERROS DE VALIDAÇÃO
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState INVÁLIDO na edição! Erros encontrados:");

                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        _logger.LogError($"❌ ERRO DE VALIDAÇÃO: {error.ErrorMessage}");
                        if (error.Exception != null)
                        {
                            _logger.LogError($"   Exception: {error.Exception.Message}");
                        }
                    }
                }

                ViewBag.ValidationErrors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return View(restaurant);
            }

            try
            {
                _logger.LogInformation("ModelState VÁLIDO! Tentando atualizar no banco...");
                await _restaurantService.UpdateRestaurantAsync(restaurant);
                _logger.LogInformation($"✅ Restaurante ID {id} atualizado com sucesso!");

                TempData["SuccessMessage"] = $"Restaurante '{restaurant.Name}' atualizado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ ERRO ao atualizar restaurante ID: {id}");
                ModelState.AddModelError("", $"Erro ao atualizar restaurante: {ex.Message}");

                if (ex.InnerException != null)
                {
                    _logger.LogError($"Inner Exception: {ex.InnerException.Message}");
                    ModelState.AddModelError("", $"Detalhes: {ex.InnerException.Message}");
                }

                return View(restaurant);
            }
        }

        // GET: Restaurant/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation($"Abrindo página de confirmação de exclusão para ID: {id}");
                var restaurant = await _restaurantService.GetRestaurantByIdAsync(id);

                if (restaurant == null)
                {
                    _logger.LogWarning($"Restaurante com ID {id} não encontrado para exclusão");
                    return NotFound();
                }

                return View(restaurant);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar restaurante para exclusão ID: {id}");
                return View("Error");
            }
        }

        // POST: Restaurant/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                _logger.LogInformation($"=== TENTANDO EXCLUIR RESTAURANTE ID: {id} ===");
                var result = await _restaurantService.DeleteRestaurantAsync(id);

                if (result)
                {
                    _logger.LogInformation($"✅ Restaurante ID {id} excluído com sucesso!");
                    TempData["SuccessMessage"] = "Restaurante excluído com sucesso!";
                }
                else
                {
                    _logger.LogWarning($"⚠️ Restaurante ID {id} não foi encontrado para exclusão");
                    TempData["ErrorMessage"] = "Restaurante não encontrado.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ ERRO ao excluir restaurante ID: {id}");
                TempData["ErrorMessage"] = $"Erro ao excluir restaurante: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
