using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Controllers
{
    // https://localhost:xxxx/api/BlogPosts
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BlogPostsController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpPost]
        public async Task<IActionResult> CreateBlog(CreateBlogPostRequestDto request)
        {
            var blogs = new BlogPost
            {
                Title = request.Title,
                ShortDescription = request.ShortDescription,
                Content = request.Content,
                FeaturedImageUrl = request.FeaturedImageUrl,
                UrlHandle = request.UrlHandle,
                PublishedDate = DateTime.Now,
                Author = request.Author,
                IsVisible = true
            };

            await _context.BlogPosts.AddAsync(blogs);
            await _context.SaveChangesAsync();

            return Ok(blogs);
        }



        [HttpGet]
        public async Task<IActionResult> GetAllBlogs()
        {
            return Ok();
        }
    }
}
