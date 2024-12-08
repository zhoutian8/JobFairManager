using JobFairManager.Data;
using JobFairManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace JobFairManager.Controllers
{
    /// <summary>
    /// 主页
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly JobFairDbContext _context;

        public HomeController(ILogger<HomeController> logger,JobFairDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            int pagesize = 10;
            var total = _context.Jobs.Count();
            var totalPages = (int)Math.Ceiling(total / (double)pagesize);

            var Jobs = _context.Jobs
                .OrderByDescending(a => a.CreatedDate)
                .Take(pagesize)
                .ToList();

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = 1;

            return View(Jobs);
        }

    }
}
