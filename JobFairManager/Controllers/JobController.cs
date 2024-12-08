using JobFairManager.Data;
using JobFairManager.Dto;
using JobFairManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Web;

namespace JobFairManager.Controllers
{
    public class JobController : Controller
    {
        private readonly JobFairDbContext _context;

        public JobController(JobFairDbContext context)
        {
            _context = context;
        }

        
        public IActionResult Details(int id)
        {
            var article = _context.Jobs.Where(m => m.JobId == id).FirstOrDefault();

            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Jobs entity)
        {
            if (!string.IsNullOrWhiteSpace(entity.JobTitle) && !string.IsNullOrWhiteSpace(entity.JobDescription))
            {
                entity.JobTitle = HttpUtility.HtmlEncode(entity.JobTitle);
                entity.JobDescription = HttpUtility.HtmlEncode(entity.JobDescription);
                entity.Requirements = HttpUtility.HtmlEncode(entity.Requirements);
                entity.CreatedDate = DateTime.Now;

                _context.Jobs.Add(entity);
                _context.SaveChanges();
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }


        // 招聘列表页面
        public IActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var jobs = _context.Jobs.AsQueryable();

            // 搜索功能
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                jobs = jobs.Where(j => j.JobTitle.Contains(searchString));
            }

            // 分页功能
            var paginatedJobs = jobs
                                .OrderByDescending(j => j.CreatedDate) // 按发布日期排序
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();

            // 计算总页数
            var totalJobs = jobs.Count();
            var totalPages = (int)Math.Ceiling(totalJobs / (double)pageSize);

            // 传递分页相关数据到前端
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchString = searchString;

            return View(paginatedJobs);
        }


        // 显示所有简历提交记录
        public IActionResult ResumeList()
        {
            List<ResumesDto> resumesDto = new List<ResumesDto>();
            var currentUser = HttpContext.Session.GetString("Username");
            if (currentUser == "admin")
            {
                var resumes = _context.Resumes
                               .ToList();
                var userids = resumes.Select(m => m.UserId).ToArray();
                var jobids = resumes.Select(m => m.JobId).ToArray();
                var users = _context.Users.Where(m => userids.Contains(m.Id)).ToList();
                var jobs = _context.Jobs.Where(m => jobids.Contains(m.JobId)).ToList();

               
                foreach (var item in resumes)
                {
                    ResumesDto dto = new ResumesDto();
                    dto.UserName = users.FirstOrDefault(m => m.Id == item.UserId).Username;
                    dto.JobName = jobs.FirstOrDefault(m => m.JobId == item.JobId).JobTitle;
                    dto.ResumesId = item.ResumeId;
                    dto.FilePath = item.FilePath;
                    dto.SubmissionDate = item.SubmissionDate;
                    resumesDto.Add(dto);
                }
                return View(resumesDto);
            }
            return View(resumesDto);
           
        }

        
    }
}
