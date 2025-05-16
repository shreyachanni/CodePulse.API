using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
    public class BlogPostRepository : IBlogPostRepository

    {
        private readonly ApplicationDbContext _context;
        public BlogPostRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
            await _context.BlogPosts.AddAsync(blogPost);
            await _context.SaveChangesAsync();

            return blogPost;
        }


        public async Task<IEnumerable<BlogPost>> GetAllBlogPostsAsync()
        {
            return await _context.BlogPosts.ToListAsync();
        }

        public async Task<BlogPost?> GetBlogPostByIdAsync(Guid id)
        {
            return await _context.BlogPosts.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<BlogPost?> UpdateBlogPostAsync(BlogPost blogPost)
        {
            var exists = await _context.BlogPosts.FirstOrDefaultAsync(b => b.Id == blogPost.Id);

            if (exists != null)
            {
                _context.Entry(exists).CurrentValues.SetValues(blogPost);
                await _context.SaveChangesAsync();
                return blogPost;
            }
            return null;
        }

        public async Task<BlogPost?> DeleteBlogPostAsync(Guid id)
        {
            var exist = await _context.BlogPosts.FirstOrDefaultAsync(b => b.Id == id);

            if(exist != null)
            {
                _context.BlogPosts.Remove(exist);
                await _context.SaveChangesAsync();
                return exist;
            }
            return null;
        }
    }
}
