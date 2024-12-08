using JobFairManager.Data;
using JobFairManager.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobFairManager.Controllers
{
    public class ResumeController : Controller
    {
        private readonly JobFairDbContext _context;

        public ResumeController(JobFairDbContext context)
        {
            _context = context;
        }

        // 提交简历页面
        [HttpGet]
        public IActionResult Submit(int jobId)
        {
            ViewBag.JobId = jobId;
            return View();
        }

        // 处理简历提交
        [HttpPost]
        public async Task<IActionResult> Submit(int jobId, IFormFile resumeFile)
        {
            if (resumeFile != null && resumeFile.Length > 0)
            {
                // 定义简历保存路径
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }

                // 保存简历文件
                var filePath = Path.Combine(uploads, resumeFile.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await resumeFile.CopyToAsync(fileStream);
                }


                var currentUser = HttpContext.Session.GetString("UserId");

                // 保存简历信息到数据库
                var resume = new Resumes
                {
                    JobId = jobId,
                    UserId = Convert.ToInt32(currentUser),
                    FilePath = "/uploads/" + resumeFile.FileName,
                    SubmissionDate = DateTime.Now
                };

                _context.Resumes.Add(resume);
                await _context.SaveChangesAsync();

                return RedirectToAction("Success");
            }

            ModelState.AddModelError("", "请上传文件");
            return View();
        }

        // 提交成功页面
        public IActionResult Success()
        {
            return View();
        }
    }
}
