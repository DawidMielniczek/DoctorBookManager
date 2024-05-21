using AutoMapper;
using DoctorBookManager.Data.Entities;
using DoctorBookManager.Data.Repositories;
using DoctorBookManager.Models.Doctor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace DoctorBookManager.Controllers
{
    public class DoctorController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IRepository<DoctorModel> _doctorRepository;

        public DoctorController(ILogger<DoctorController> logger, UserManager<IdentityUser> userManager, IMapper mapper, IRepository<DoctorModel> doctorRepository, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _doctorRepository = doctorRepository;
            _roleManager = roleManager;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string serachString, string city)
        {
            var doctors =  _doctorRepository.GetAll();
            if (!string.IsNullOrEmpty(serachString))
                doctors = doctors.Where(x => x.Specialization.ToLower().Contains(serachString.ToLower())
                    || x.FullName.ToLower().Contains(serachString.ToLower()));           
            if (!string.IsNullOrEmpty(city))
                doctors = doctors.Where(x => x.City.ToLower().Contains(city.ToLower()));
            
            return View(doctors.ToList());
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> AddDoctor()
        {
            return View(new CreateDoctorModel() { Users=await GetUsers()});
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddDoctor(CreateDoctorModel doctorModel)
        {


            //var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);


           // doctorModel.UserId = 1.ToString();   //current id 
           
            ModelState["UserId"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
            if (ModelState.IsValid)
            {
                try
                {
                    var doctorUser= await _userManager.FindByIdAsync(doctorModel.UserId);
                    await _userManager.RemoveFromRoleAsync(doctorUser, DoctorBookManageConsts.UserRoleName);
                    await _userManager.AddToRoleAsync(doctorUser, DoctorBookManageConsts.DoctorRoleName);
                    var entity = _mapper.Map<DoctorModel>(doctorModel);
                    await _doctorRepository.InsertAsync(entity);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Wystąpił błąd podczas dodawania lekarza: " + ex.Message;
                    return View(doctorModel);
                }

            }
            else
            {
                return View();
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var concreteDoctor = _doctorRepository.GetAll().FirstOrDefault(x => x.Id == id);

            return View(concreteDoctor);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteDoctor(int id, DoctorModel doctorModel)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);
            if (doctorModel.UserId == currentUserId || role == "Admin")
                await _doctorRepository.DeleteAsync(doctorModel.Id);
            else
                throw new Exception("You don't have permission to do this action");
            return RedirectToAction(nameof(Index));
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UpdateDoctor(int id)
        {
            var concreteDoctor = await _doctorRepository.GetByIdAsync(id);

            var model = _mapper.Map<UpdateDoctorModel>(concreteDoctor);

            return View(model);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateDoctor(UpdateDoctorModel model)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);
            var concreteDoctor = await _doctorRepository.GetByIdAsync(model.Id);
            if (concreteDoctor.UserId == currentUserId || role == "Admin")
            {
                concreteDoctor = _mapper.Map<UpdateDoctorModel, DoctorModel>(model, concreteDoctor);
                await _doctorRepository.UpdateAsync(concreteDoctor);
            }
            else
                throw new Exception("You don't have permission to do this action");

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int id)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);
            var viewModel = _mapper.Map<GetDoctorModel>(doctor);
            var doctorUser = await _userManager.FindByIdAsync(doctor.UserId);
            viewModel.Email = await _userManager.GetEmailAsync(doctorUser);
            viewModel.PhoneNumber = await _userManager.GetPhoneNumberAsync(doctorUser);
            return View(viewModel);
        }
        
        private async Task<List<GetUserModel>> GetUsers()
        {
            var users = await _userManager.GetUsersInRoleAsync(DoctorBookManageConsts.UserRoleName);
            var doctors = await _doctorRepository.GetAllListAsync();

            var result = _mapper.Map<List<GetUserModel>>(users);
            result.RemoveAll(x => doctors.Any(d => d.UserId == x.Id));
            return result;

        }

        public IActionResult Review()
        {
            return RedirectToAction("Index", "Review");
        }
    }
}
