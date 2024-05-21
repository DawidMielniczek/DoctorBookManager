using AutoMapper;
using DoctorBookManager.Data.Entities;
using DoctorBookManager.Data.Repositories;
using DoctorBookManager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace DoctorBookManager.Controllers
{
    public class ReviewController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IRepository<DoctorModel> _doctorRepository;
        private readonly IRepository<DoctorReviewModel> _reviewRepository;
        List<GetDoctorReviewModel> _doctorReviewList = new List<GetDoctorReviewModel>();


        public ReviewController(ILogger<ReviewController> logger, UserManager<IdentityUser> userManager, IMapper mapper, IRepository<DoctorReviewModel> reviewRepository, IRepository<DoctorModel> doctorRepository)
        {
            _userManager = userManager;
            _mapper = mapper;
            _reviewRepository = reviewRepository;
            _doctorRepository = doctorRepository;

        }

        public async Task<ActionResult> Index()
        {
            var reviews = await _reviewRepository.GetAll().ToListAsync();
            var doctors = await _doctorRepository.GetAll().ToListAsync();

            for (int i = 0; i < reviews.Count; i++)
            {
                for (int j = 0; j < doctors.Count; j++)
                {
                    if (doctors[j].Id == reviews[i].DoctorId)
                    {
                        _doctorReviewList.Add(new GetDoctorReviewModel() { Id = doctors[j].Id, Name = doctors[j].FullName, Content = reviews[i].Content, Grade = reviews[i].Grade, CreationDate = reviews[i].CreationTime, IdReview = reviews[i].Id });
                    }
                }
            }


            return View(_doctorReviewList);

        }


        [HttpGet]
        public async Task<IActionResult> AddReview()
        {
            var doctors = await _doctorRepository.GetAll().ToListAsync();
            ViewBag.NazwaLekarza = new List<SelectListItem>();
            foreach (var doctor in doctors)
            {
                ViewBag.NazwaLekarza.Add(new SelectListItem { Text = doctor.FullName, Value = doctor.Id.ToString() });
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddReview(DoctorReviewModel reviewModel)
        {
            reviewModel.CreatorUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ModelState["Doctor"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;

            if (ModelState.IsValid)
            {
                try
                {
                    await _reviewRepository.InsertAsync(reviewModel);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Wystąpił błąd podczas dodawania lekarza: " + ex.Message;
                    return View(reviewModel);
                }
            }
            else
            {
                return View();
            }
        }

        // GET: ReviewController/Edit/5
        public async Task<IActionResult> Details(int id)
        {
            var reviews = await _reviewRepository.GetAll().Where(x => x.Id == id).ToListAsync();
            var doctors = await _doctorRepository.GetAll().ToListAsync();
            var review = new GetDoctorReviewModel();
            for (int i = 0; i < reviews.Count; i++)
            {
                for (int j = 0; j < doctors.Count; j++)
                {
                    if (doctors[j].Id == reviews[i].DoctorId)
                    {
                        review = new GetDoctorReviewModel() { Id = doctors[j].Id, Name = doctors[j].FullName, Content = reviews[i].Content, Grade = reviews[i].Grade, CreationDate = reviews[i].CreationTime, IdReview = reviews[i].Id };
                    }
                }
            }


            return View(review);
        }


        [HttpGet]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var delReview = _reviewRepository.GetAll().Where(x => x.Id == id).FirstOrDefault();
            var doctors = await _doctorRepository.GetAll().ToListAsync();

            ViewBag.NazwaLekarza = new List<SelectListItem>();
            foreach (var doctor in doctors)
            {
                ViewBag.NazwaLekarza.Add(new SelectListItem { Text = doctor.FullName, Value = doctor.Id.ToString() });
            }
            return View(delReview);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteReview(int id, DoctorReviewModel reviewModel)
        {

            var currentRole = User.FindFirstValue(ClaimTypes.Role);
            if (currentRole == "Admin")
                await _doctorRepository.DeleteAsync(reviewModel.Id);
            else
                throw new Exception("You don't have permission to do this action");

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UpdateReview(int id)
        {
            var doctors = await _doctorRepository.GetAll().ToListAsync();

            ViewBag.NazwaLekarza = new List<SelectListItem>();
            foreach (var doctor in doctors)
            {
                ViewBag.NazwaLekarza.Add(new SelectListItem { Text = doctor.FullName, Value = doctor.Id.ToString() });
            }

            return View(_reviewRepository.GetAll().Where(x => x.Id == id).FirstOrDefault());
        }


        // POST: TaskController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateReview(int id, DoctorReviewModel reviewModel)
        {

            ModelState["Doctor"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;

            if (ModelState.IsValid)
            {
                await _reviewRepository.UpdateAsync(reviewModel);
                return RedirectToAction(nameof(Index));
            }

            return View();
        }


    }
}
