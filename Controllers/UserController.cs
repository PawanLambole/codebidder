using Microsoft.AspNetCore.Http; // Required for session handling
using Microsoft.AspNetCore.Mvc;
using CodeBidder.Data; // Your database context namespace
using CodeBidder.Models; // Your models namespace
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography; // For password hashing
using System.Text;
using System;
using Microsoft.EntityFrameworkCore;
using CodeBidder.ViewModels; // For encoding passwords

namespace CodeBidder.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        // Constructor that initializes the DbContext
        public UserController(AppDbContext context)
        {
            _context = context;
        }

        // Action method for user registration (GET)
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Action method to handle registration post request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            if (ModelState.IsValid)
            {
                // Check if the username or email already exists
                var existingUser = _context.Users
                    .Any(u => u.Username == user.Username || u.Email == user.Email);

                if (existingUser)
                {
                    ModelState.AddModelError(string.Empty, "Username or email already in use.");
                    return View(user);
                }

                // Hash the password before saving to the database
                user.Password = HashPassword(user.Password);

                // Add new user to the Users table
                _context.Users.Add(user);
                await _context.SaveChangesAsync(); // Save changes to the database

                return RedirectToAction("Index", "Home"); // Redirect to home after registration
            }

            return View(user);
        }

        // Action method for user login (GET)
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Action method to handle login post request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(User user, string LoginType)
        {
            // Predefined admin credentials
            const string adminUsername = "admin";
            const string adminPassword = "admin123";

            // Check if the user is logging in as an admin
            if (user.Username == adminUsername && user.Password == adminPassword)
            {
                // Verify the login type is "Admin"
                if (LoginType == "Admin")
                {
                    // Set the admin session or TempData
                    TempData["AdminName"] = "Administrator";

                    // Redirect to the admin dashboard
                    return RedirectToAction("AIndex", "AdminDashboard");
                }
                else
                {
                    // Login type mismatch for admin
                    return RedirectToAction("NoUser", "User");
                }
            }

            // Find the user based on username
                 var existingUser = _context.Users
                .FirstOrDefault(u => u.Username == user.Username);

            if (existingUser != null && VerifyPassword(user.Password, existingUser.Password))
            {
                // Check if the selected login type matches the user's type in the database
                if (existingUser.UserType != LoginType)
                {
                    // Redirect to NoUser page if the login type does not match
                    return RedirectToAction("NoUser", "User");
                }

                // If login type matches, proceed with login
                TempData["ClientName"] = existingUser.FirstName;

                // Store the user's ID in Session
                HttpContext.Session.SetInt32("UserId", existingUser.Id);

                // Redirect based on the user type
                switch (existingUser.UserType)
                {
                    case "Developer":
                        return RedirectToAction("Index", "DeveloperDashboard");
                    case "Client":
                        return RedirectToAction("Index", "ClientDashboard");
                    case "Admin":
                        return RedirectToAction("AIndex", "AdminDashboard");
                    default:
                        // Redirect to NoUser if the UserType is invalid or not recognized
                        return RedirectToAction("NoUser", "User");
                }
            }
            else
            {
                // If user not found or password does not match, redirect to the "NoUser" page
                return RedirectToAction("NoUser", "User");
            }
        }

        // Optional: Action to handle logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Clear all session data
            return RedirectToAction("Index", "Home");
        }

        // Fallback action in case of invalid user session
        public IActionResult NoUser()
        {
            return View();
        }

        // Password hashing function
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        // Password verification function
        private bool VerifyPassword(string enteredPassword, string storedHash)
        {
            var enteredPasswordHash = HashPassword(enteredPassword);
            return enteredPasswordHash == storedHash;
        }

        public IActionResult Profile()
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



        // Action method for deleting the user account (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAccount()
        {
            // Ensure the user is logged in
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "User"); // Redirect if not logged in
            }

            // Ensure the context is valid
            if (_context == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database context is not available.");
            }

            try
            {
                // Find the user
                var user = _context.Users.FirstOrDefault(u => u.Id == userId);
                if (user == null)
                {
                    return RedirectToAction("Login", "User"); // Redirect if user not found
                }

                // Remove the user
                _context.Users.Remove(user);
                _context.SaveChanges();

                // Clear session
                HttpContext.Session.Clear();

                // Redirect to the homepage
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // Log exception (implement your logging mechanism)
                Console.WriteLine($"Error deleting account: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the account.");
            }
        }

       
        // GET: Update Profile
        public IActionResult Update()
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
        public IActionResult Update(UpdateProfileViewModel model)
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

            return RedirectToAction("Profile", "User");
        }


    }
}
