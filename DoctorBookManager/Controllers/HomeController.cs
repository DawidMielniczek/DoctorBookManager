using AutoMapper;
using DoctorBookManager.Data.Entities;
using DoctorBookManager.Data.Repositories;
using DoctorBookManager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace DoctorBookManager.Controllers
{
    public class HomeController : Controller
    {
        //Przykładowe wstrzyknięcia zależności
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IRepository<DoctorModel> _doctorRepository;
        public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager, IMapper mapper, IRepository<DoctorModel> doctorRepository)
        {
            _logger = logger;
            _userManager = userManager;
            _mapper = mapper;
            _doctorRepository = doctorRepository;
        }

        public async  Task<IActionResult> Index()
        {
            //Przykładowe pobranie aktualne id użytkownika z sesji
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //Przykładowe użycie repozytorium
            var doctors = await _doctorRepository.GetAll().ToListAsync();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}