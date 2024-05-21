using AutoMapper;
using DoctorBookManager.Data.Entities;
using DoctorBookManager.Data.Repositories;
using DoctorBookManager.Models.Appoiment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DoctorBookManager.Controllers
{
    public class AppoimentController : Controller
    {
        private readonly IRepository<DoctorModel> _doctorRepository;
        private readonly IRepository<DoctorAppointmentModel> _appoimentRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;
        public AppoimentController(IRepository<DoctorModel> doctorRepository,
            IRepository<DoctorAppointmentModel> appoimentRepository,
            UserManager<IdentityUser> userManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
            _doctorRepository = doctorRepository;
            _appoimentRepository = appoimentRepository;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentRole = User.FindFirstValue(ClaimTypes.Role);
            var myAppoiments = await _appoimentRepository.GetAll().Include(x => x.Doctor)
                .Where(x => x.PatientId == currentUserId && x.VisitTime.Date >= DateTime.Now.Date).ToListAsync();
            var result = _mapper.Map<List<GetAppoimentModelView>>(myAppoiments);
            return View(result);
        }
        [Authorize]
        public async Task<IActionResult> AvialableVisits(int doctId, DateTime date, int hour, bool save)
        {
            if (date == DateTime.MinValue)
                date = DateTime.Now.Date;
            var doctor = await _doctorRepository.GetByIdAsync(doctId);
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                return View(new GetAvailableHours { AvaialbleHours = new List<int>(), Date = date.Date, DoctorName = doctor.FullName, DoctId = doctId });
            if (save)
              return await BookAppoiment(doctId, date, hour);
            
            var doctorHours = await GetAvaialableHours(doctId, date);
            
            return View(new GetAvailableHours { AvaialbleHours=doctorHours, Date=date.Date, DoctorName=doctor.FullName , DoctId=doctId});
        }
        [Authorize]
        public async Task<IActionResult> DeleteVisit(int id)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var visit = await _appoimentRepository.GetAll().Include(x=>x.Doctor).FirstOrDefaultAsync(x => x.Id == id);
            if (visit != null && (visit.PatientId == currentUserId || visit.Doctor.UserId== currentUserId))
            {
                var time = visit.VisitTime-DateTime.Now;
                if (time.TotalHours > 24)
                {
                    await _appoimentRepository.DeleteAsync(visit);
                    return RedirectToAction("Index");
                }
                else
                    throw new Exception("You cannot delete this reservation");

            }
            else
                throw new Exception("You cannot delete this reservation");
        }
        [Authorize]
        [HttpPost]
        private async Task<IActionResult> BookAppoiment(int doctId, DateTime date, int hour)
        {
            date= new DateTime(date.Year,date.Month, date.Day, hour, 0 ,0 );
            var avaiableHours = await GetAvaialableHours(doctId, date);
            if (avaiableHours.All(x => x != date.Hour))
                throw new Exception("This term is invalid or already taken ");
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _appoimentRepository.InsertAsync(new DoctorAppointmentModel() { DoctorId = doctId, PatientId = currentUserId, VisitTime = date });
            return RedirectToAction("Index");

        }
        [Authorize]
        public async Task<IActionResult> MyPatients()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentRole = User.FindFirstValue(ClaimTypes.Role);
            if (currentRole != DoctorBookManageConsts.DoctorRoleName)
                throw new Exception("You don't have permission to this acton");
            var patients = _appoimentRepository.GetAll().Include(x => x.Doctor).Where(x => x.Doctor.UserId == currentUserId && x.VisitTime.Date >= DateTime.Now.Date);
            var result = new List<GetPatientModel>();
            foreach (var patient in patients)
            {
                var patientModel = _mapper.Map<GetPatientModel>(patient);
                var user = await _userManager.FindByIdAsync(patient.PatientId);
                patientModel.PhoneNumber =await _userManager.GetPhoneNumberAsync(user);
                patientModel.Email =await _userManager.GetEmailAsync(user);
                result.Add(patientModel);
            }
            return View(result);

        }
        private async Task<List<int>> GetAvaialableHours(int doctId, DateTime date)
        {
            var reservations = await _appoimentRepository.GetAll()
                .Where(x => x.DoctorId == doctId && x.VisitTime.Date == date.Date)
                .Select(x => x.VisitTime.Hour).ToListAsync();

            var doctor = await _doctorRepository.GetByIdAsync(doctId);
            var doctorHours = new List<int>();
            for (int i = doctor.StartWorkHour; i < doctor.EndWorkHour; i++)
            {
                if(reservations.All(x=>x != i))
                    doctorHours.Add(i);
            }
           
            return doctorHours;
        }
    }
}
