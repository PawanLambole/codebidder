using CodeBidder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
using CodeBidder.Data;

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

    }
}
