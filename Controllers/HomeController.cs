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

        // ���� ������
        public async Task<IActionResult> Index()
        {
            var generalViews = await this._httpClient.GetFromJsonAsync<GeneralView>(
                "http://localhost:5176/GeneralView"
            );

            return View(generalViews);
        }

        // ���� �� ������
        public async Task<IActionResult> Targets()
        {
            List<TargetView>? targets = await this._httpClient.GetFromJsonAsync<List<TargetView>>(
                "http://localhost:5176/Targets"
            );
            return View(targets);
        }

        // ���� �� ������� ������
        public async Task<IActionResult> GetMissions()
        {
            List<TaskManagementForView>? missions = await this._httpClient.GetFromJsonAsync<
                List<TaskManagementForView>
            >("http://localhost:5176/Missions");
            return View(missions);
        }

        // ����� ������ ������
        public async Task<IActionResult> CommandForMission(int id)
        {
            await this._httpClient.PutAsJsonAsync(
                $"http://localhost:5176/Missions/{id}",
                new { id }
            );

            return RedirectToAction("GetMissions");
        }

        // ������� ������
        public async Task<IActionResult> Login()
        {
            return View(new User());
        }

        // ����� ����� ������� ����
        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            var response = await this._httpClient.PostAsJsonAsync(
                "http://localhost:5176/Login",
                user
            );

            TokenLogin? tokenLogin = await response.Content.ReadFromJsonAsync<TokenLogin>();

            // ����� ����� �� ��� �� ����� ����� �� ��� ����
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                tokenLogin.token
            );

            return RedirectToAction("Index");
        }
    }
}
