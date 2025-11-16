using CodeBidder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
using CodeBidder.Data;
using Microsoft.AspNetCore.Http;
using CodeBidder.ViewModels;

namespace CodeBidder.Controllers
{
    public class DeveloperDashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DeveloperDashboardController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ViewProjects()
        {
            try
            {
                // Fetch all projects from the ProjectDetails table
                var allProjects = _context.ProjectDetails.ToList();

                // Check if there are projects in the database
                if (allProjects.Any())
                {
                    ViewBag.ErrorMessage = null; // Clear any previous error message
                    return View(allProjects); // Pass all projects to the view
                }
                else
                {
                    ViewBag.ErrorMessage = "No projects found in the database.";
                    return View(new List<ProjectDetailsModel>()); // Pass an empty list to the view
                }
            }
            catch (Exception)
            {
                // Handle unexpected exceptions
                ViewBag.ErrorMessage = "An error occurred while retrieving projects. Please try again later.";
                return View(new List<ProjectDetailsModel>()); // Pass an empty list to the view
            }
        }
        [HttpGet]
        public IActionResult ProjectDetails(int projectId)
        {
            try
            {
                // Retrieve the project with the given ID
                var project = _context.ProjectDetails.FirstOrDefault(p => p.Id == projectId);

                if (project == null)
                {
                    // If project is not found, show an error message
                    ViewBag.ErrorMessage = "The requested project could not be found.";
                    return RedirectToAction("ViewProjects"); // Redirect back to the project list
                }

                // Pass the project details to the view
                return View(project);
            }
            catch (Exception)
            {
                // Handle unexpected exceptions
                ViewBag.ErrorMessage = "An error occurred while retrieving the project details. Please try again later.";
                return RedirectToAction("ViewProjects"); // Redirect back to the project list
            }
        }

        [HttpGet]
        public IActionResult SubmitQuotation(int projectId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "User");
            }

            var project = _context.ProjectDetails.FirstOrDefault(p => p.Id == projectId);
            if (project == null)
            {
                TempData["ErrorMessage"] = "Project not found.";
                return RedirectToAction("ViewProjects");
            }

            ViewBag.ProjectId = project.Id;
            ViewBag.ProjectName = project.ProjectName;
            return View();
        }

        // POST: Submit Quotation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitQuotation(Quotation quotation)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "User");
            }

            if (ModelState.IsValid)
            {
                quotation.DeveloperId = userId.Value;
                quotation.SubmissionDate = DateTime.Now;

                _context.Quotations.Add(quotation);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Quotation submitted successfully!";
                return RedirectToAction("ViewProjects");
            }

            TempData["ErrorMessage"] = "Failed to submit the quotation. Please ensure all fields are filled correctly.";
            return RedirectToAction("SubmitQuotation", new { projectId = quotation.ProjectId });
        }


        public IActionResult MyQuotations()
        {
            // Get the logged-in developer's ID from the session
            var developerId = HttpContext.Session.GetInt32("UserId");

            if (developerId == null)
            {
                // If no developer is logged in, redirect to login page
                return RedirectToAction("Login", "User");
            }

            // Fetch all quotations submitted by the logged-in developer
            var quotations = _context.Quotations
                .Where(q => q.DeveloperId == developerId)
                .Include(q => q.Project) // Include project details if necessary
                .ToList();

            // Return the quotations to the view
            return View(quotations);
        }

        public IActionResult ViewQuotationDetails(int id)
        {
            var quotation = _context.Quotations
                .Include(q => q.Project) // Include project details
                .FirstOrDefault(q => q.Id == id);

            if (quotation == null)
            {
                // Handle case where quotation doesn't exist
                return RedirectToAction("MyQuotations");
            }

            return View(quotation);
        }


        public IActionResult DProfile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "User");
            }

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return RedirectToAction("Login", "User");
            }

            return View(user);
        }

        public IActionResult DPrintQuotationReport(int quotationId)
        {
            // Get the quotation with related project and developer details
            var quotation = _context.Quotations
                .Include(q => q.Project) // Ensure Project is loaded
                .Include(q => q.Developer) // Ensure Developer is loaded
                .FirstOrDefault(q => q.Id == quotationId);

            if (quotation == null)
            {
                return NotFound(); // Return a NotFound result if the quotation does not exist
            }

            return View(quotation);
        }

        // GET: Update Profile
        public IActionResult DUpdate()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "User");
            }

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return RedirectToAction("Login", "User");
            }

            // Populate ViewModel
            var viewModel = new UpdateProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                MobileNumber = user.MobileNumber,
                Email = user.Email,
                Username = user.Username
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DUpdate(UpdateProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "User");
            }

            var existingUser = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (existingUser == null)
            {
                return RedirectToAction("Login", "User");
            }

            // Update user fields
            existingUser.FirstName = model.FirstName;
            existingUser.LastName = model.LastName;
            existingUser.MobileNumber = model.MobileNumber;
            existingUser.Email = model.Email;
            existingUser.Username = model.Username;

            try
            {
                _context.Attach(existingUser);
                _context.Entry(existingUser).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Failed to update user details. Please try again.");
                return View(model);
            }

            return RedirectToAction("DProfile", "DeveloperDashboard");
        }

    }
}
