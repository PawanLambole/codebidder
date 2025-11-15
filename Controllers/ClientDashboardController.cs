using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CodeBidder.Data;
using CodeBidder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeBidder.Controllers
{
    public class ClientDashboardController : Controller
    {
        private readonly AppDbContext _context;

        public ClientDashboardController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            // Get the username from session
            var username = HttpContext.Session.GetString("Username");

            // Pass the username to the view via ViewData
            ViewData["Username"] = username;

            return View();
        }

        // GET: ClientDashboard/Submit
        public IActionResult Submit()
        {
            return View();
        }

        public IActionResult Requirements()
        {
            return View();
        }
        [HttpGet]
        public IActionResult MyProjectsD(int projectId)
        {
            try
            {
                // Retrieve the currently logged-in user's ID from session
                if (HttpContext.Session.GetInt32("UserId") == null)
                {
                    ViewBag.ErrorMessage = "You are not logged in.";
                    return RedirectToAction("Login", "User"); // Redirect to login page if user ID is not found in session
                }

                int userId = (int)HttpContext.Session.GetInt32("UserId");

                // Fetch the project details based on the provided project ID
                var project = _context.UserProject
                    .Include(up => up.ProjectDetail) // Include the project details
                    .Where(up => up.UserId == userId && up.ProjectDetail.Id == projectId) // Filter by logged-in user ID and project ID
                    .Select(up => up.ProjectDetail) // Select the related ProjectDetails
                    .FirstOrDefault();

                if (project == null)
                {
                    ViewBag.ErrorMessage = "Project not found or you do not have permission to view it.";
                    return RedirectToAction("MyProjects"); // Redirect back to the list if no project is found
                }

                return View(project); // Pass the project to the view
            }
            catch (Exception )
            {
                // Handle unexpected exceptions
                ViewBag.ErrorMessage = "An error occurred while retrieving the project details. Please try again later.";
                return RedirectToAction("MyProjects"); // Redirect back to the projects list on error
            }
        }





        [HttpGet]
        public IActionResult MyProjects()
        {
            try
            {
                // Retrieve the currently logged-in user's ID from session
                if (HttpContext.Session.GetInt32("UserId") == null)
                {
                    ViewBag.ErrorMessage = "You are not logged in.";
                    return RedirectToAction("Login", "User"); // Redirect to login page if user ID is not found in session
                }

                int userId = (int)HttpContext.Session.GetInt32("UserId");

                // Fetch projects associated with this user from the UserProject table
                var userProjects = _context.UserProject
                    .Include(up => up.ProjectDetail) // Include the project details
                    .Where(up => up.UserId == userId) // Filter by logged-in user ID
                    .Select(up => up.ProjectDetail) // Select the related ProjectDetails
                    .ToList();

                // Check if the user has submitted any projects
                if (userProjects.Count > 0)
                {
                    ViewBag.ErrorMessage = null; // Clear any previous error message
                    return View(userProjects);
                }
                else
                {
                    ViewBag.ErrorMessage = "You have not submitted any projects yet.";
                    return View(new List<ProjectDetailsModel>()); // Pass an empty list to the view
                }
            }
            catch (Exception )
            {
                // Handle unexpected exceptions
                ViewBag.ErrorMessage = "An error occurred while retrieving your projects. Please try again later.";
                return View(new List<ProjectDetailsModel>()); // Pass an empty list to the view
            }
        }

        // POST: ClientDashboard/Submit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Requirements(ProjectDetailsModel model)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the currently logged-in user's ID from session
                if (HttpContext.Session.GetInt32("UserId") == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid user ID. Please log in again.");
                    return RedirectToAction("Login", "User"); // Redirect to login page if user ID is not found in session
                }

                int userId = (int)HttpContext.Session.GetInt32("UserId");

                // Save project details
                _context.ProjectDetails.Add(model);
                await _context.SaveChangesAsync();

                // Get the newly created project ID
                int projectId = model.Id;

                // Create and save the UserProject entry
                var userProject = new UserProject
                {
                    UserId = userId,
                    ProjectDetailId = projectId
                };

                _context.UserProject.Add(userProject);
                await _context.SaveChangesAsync();

                // Redirect to dashboard after submission
                return RedirectToAction("Submit", "ClientDashboard");
            }

            return View(model);
        }
        public IActionResult Details()
        {
            return View();
        }
    }
}
