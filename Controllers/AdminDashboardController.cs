using Microsoft.AspNetCore.Mvc;
using System.Linq;
using CodeBidder.Models; // Namespace where your models are located
using Microsoft.EntityFrameworkCore;
using CodeBidder.Data;
using System.Collections.Generic;
using System; // For Entity Framework

namespace CodeBidder.Controllers
{
    public class AdminDashboardController : Controller
    {
        private readonly AppDbContext _context;

        public AdminDashboardController(AppDbContext context)
        {
            _context = context;
        }

        // Action method for Admin Dashboard
        public IActionResult AIndex()
        {
            // Fetch the data from the database
            var totalUsers = _context.Users.Where(u=> u.UserType == "Client").Count();
            var totaldUsers = _context.Users.Where(u => u.UserType == "Developer").Count();
            // var activeProjects = _context.Projects.Where(p => p.Status == "Active").Count();
            // var totalQuotations = _context.Quotations.Count();
            //  var recentActivities = _context.Activities.OrderByDescending(a => a.Date).Take(5).ToList();

            // Pass the data to the view using ViewData
            ViewData["TotalUsers"] = totalUsers;
            ViewData["TotaldUsers"] = totaldUsers;
            // ViewData["ActiveProjects"] = activeProjects;
            // ViewData["TotalQuotations"] = totalQuotations;
            // ViewData["RecentActivities"] = recentActivities;

            return View();
        }
        // Search Action
        public IActionResult Search(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return View("SearchResults", new List<object>()); // Empty result
            }

            // Search logic (example: searching users and projects)
            var userResults = _context.Users
                .Where(u => u.FirstName.Contains(query) || u.LastName.Contains(query) || u.Email.Contains(query))
                .ToList();

            var projectResults = _context.ProjectDetails
                .Where(p => p.ProjectName.Contains(query) || p.ProjectDescription.Contains(query))
                .ToList();

            // Combine results into a dynamic object or a view model
            var searchResults = new
            {
                Users = userResults,
                Projects = projectResults
            };

            return View(searchResults);
        }
        public IActionResult AddAdmin()
        {
            
            return View(); 
        }

        [HttpPost]
        public IActionResult CreateAdmin(string firstName, string lastName, string mobileNumber, string username, string password, string confirmPassword)
        {
            // Validate Confirm Password
            if (password != confirmPassword)
            {
                TempData["ErrorMessage"] = "Passwords do not match.";
                return RedirectToAction("AddAdmin");
            }

            // Check if username already exists
            var existingUser = _context.Users.FirstOrDefault(u => u.Username == username);
            if (existingUser != null)
            {
                TempData["ErrorMessage"] = "Username already exists. Please choose another one.";
                return RedirectToAction("AddAdmin");
            }

            // Assign email (Here we are using username as the email for simplicity)
            string email = $"{username}@example.com"; // Placeholder email (you can customize this or ask for email input)

            // Add logic to create a new admin
            var newAdmin = new User
            {
                FirstName = firstName,
                LastName = lastName,
                MobileNumber = mobileNumber,
                Username = username,
                Email = email, // Use the email here
                Password = HashPassword(password), // Use a secure hashing method for password storage
                UserType = "Admin"
            };

            try
            {
                _context.Users.Add(newAdmin);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Admin added successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while creating the admin.";
                // Log the error
            }

            return RedirectToAction("AIndex"); // Redirect to an appropriate page
        }

        // Example of password hashing function
        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }




        public IActionResult ManageClients()
        {
            // Fetch all users with UserType "Client"
            var clients = _context.Users
                .Where(u => u.UserType == "Client")
                .ToList();

            return View(clients);
        }
        public IActionResult ManageDevelopers()
        {
            // Fetch all users with UserType "Client"
            var user = _context.Users
                .Where(u => u.UserType == "Developer")
                .ToList();

            return View(user);

        }
        public IActionResult UserProfile(int id)
        {
            // Fetch the user by their ID
            var user = _context.Users.Include(u => u.Enrollment) // Include related data like projects
                                     .FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound(); // Handle user not found case
            }

            return View(user); // Pass the user to the view
        }
        public IActionResult UserProjects(int userId)
        {
            // Fetch the user details
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return NotFound(); // Return 404 if the user doesn't exist
            }

            // Fetch projects associated with the user
            var project = _context.UserProject
                   .Include(up => up.ProjectDetail) // Include the project details
                   .Where(up => up.UserId == userId ) // Filter by logged-in user ID and project ID
                   .Select(up => up.ProjectDetail) // Select the related ProjectDetails
                   .ToList();
                                    

            // Pass user and projects data to the view
            ViewBag.User = user;
            return View(project);
        }

    }
}
