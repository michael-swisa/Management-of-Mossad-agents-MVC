using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;
using ManagementOfMossadAgentsMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Common;

namespace ManagementOfMossadAgentsMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly HttpClient _httpClient;

        public HomeController(ILogger<HomeController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        // הצגת כמויות
        public async Task<IActionResult> Index()
        {
            var generalViews = await this._httpClient.GetFromJsonAsync<GeneralView>(
                "http://localhost:5176/GeneralView"
            );

            return View(generalViews);
        }

        // הצגת כל המטרות
        public async Task<IActionResult> Targets()
        {
            List<TargetView>? targets = await this._httpClient.GetFromJsonAsync<List<TargetView>>(
                "http://localhost:5176/Targets"
            );
            return View(targets);
        }

        // קבלת כל המשימות לציוות
        public async Task<IActionResult> GetMissions()
        {
            List<TaskManagementForView>? missions = await this._httpClient.GetFromJsonAsync<
                List<TaskManagementForView>
            >("http://localhost:5176/Missions");
            return View(missions);
        }

        // שליחת המשימה לציוות
        public async Task<IActionResult> CommandForMission(int id)
        {
            await this._httpClient.PutAsJsonAsync(
                $"http://localhost:5176/Missions/{id}",
                new { id }
            );

            return RedirectToAction("GetMissions");
        }

        // התחברות למערכת
        public async Task<IActionResult> Login()
        {
            return View(new User());
        }

        // שליחת הפרטי התחברות לשרת
        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            var response = await this._httpClient.PostAsJsonAsync(
                "http://localhost:5176/Login",
                user
            );

            TokenLogin? tokenLogin = await response.Content.ReadFromJsonAsync<TokenLogin>();

            // הגדרה שישלח כל פעם את הטוקן שחוזר לו בכל בקשה
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                tokenLogin.token
            );

            return RedirectToAction("Index");
        }
    }
}
