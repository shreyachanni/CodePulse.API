using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
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
        private readonly IBlogPostRepository _blogPostRepository;

        public BlogPostsController(IBlogPostRepository blogPostRepository)
        {
            _blogPostRepository = blogPostRepository;   
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

            await _blogPostRepository.CreateAsync(blogs);

            return Ok(blogs);
        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogPost>>> GetAllBlogs()
        {
            var blogs = await _blogPostRepository.GetAllBlogPostsAsync();

            var resopnse = new List<BlogPostDto>();
            foreach (var blog in blogs)
            {
                resopnse.Add(new BlogPostDto
                {
                    Id = blog.Id,
                    Title = blog.Title,
                    ShortDescription = blog.ShortDescription,
                    Content = blog.Content,
                    FeaturedImageUrl = blog.FeaturedImageUrl,
                    UrlHandle = blog.UrlHandle,
                    PublishedDate = DateTime.Now,
                    Author = blog.Author,
                    IsVisible = true
                });
            }

            if (resopnse == null)
            {
                return NotFound();
            }

            else
            {
                return Ok(resopnse);
            }
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetBlogPostByID([FromRoute] Guid id)
        {
            var exists = await _blogPostRepository.GetBlogPostByIdAsync(id);

            if (exists == null)
            {
                return NotFound();
            }
            else
            {
                var response = new BlogPostDto
                {
                    Id = exists.Id,
                    Title = exists.Title,
                    ShortDescription = exists.ShortDescription,
                    Content = exists.Content,
                    FeaturedImageUrl = exists.FeaturedImageUrl,
                    UrlHandle = exists.UrlHandle,
                    PublishedDate = DateTime.Now,
                    Author = exists.Author,
                    IsVisible = true
                };
                return Ok(response);
            }
        }


        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateBlogPost([FromRoute] Guid id, [FromBody] UpdateBlogPostDto request)
        {
            var blogPost = new BlogPost
            {
                Id = id,
                Title = request.Title,
                ShortDescription = request.ShortDescription,
                Content = request.Content,
                FeaturedImageUrl = request.FeaturedImageUrl,
                UrlHandle = request.UrlHandle,
                PublishedDate = DateTime.Now,
                Author = request.Author,
                IsVisible = true
            };

            blogPost = await _blogPostRepository.UpdateBlogPostAsync(blogPost);

            if (blogPost != null) 
            {
                var response = new BlogPostDto
                {
                    Id = blogPost.Id,
                    Title = blogPost.Title,
                    ShortDescription = blogPost.ShortDescription,
                    Content = blogPost.Content,
                    FeaturedImageUrl= blogPost.FeaturedImageUrl,
                    UrlHandle = blogPost.UrlHandle,
                    PublishedDate = DateTime.Now,
                    Author = blogPost.Author,
                    IsVisible = true
                };

                return Ok(response);
            }
           
            return NotFound();
            
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteBlogPost([FromRoute] Guid id)
        {
            var blogPost = await _blogPostRepository.DeleteBlogPostAsync(id);

            if(blogPost != null)
            {
                var response = new BlogPostDto
                {
                    Id = blogPost.Id,
                    Title = blogPost.Title,
                    ShortDescription = blogPost.ShortDescription
                };
                return Ok(new
                {
                    Message = $"{response.Title} deleted successfully.",
                });
                
            }

            return NotFound();
        }

    }
}
