using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CategoryRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Category> CreateAsync(Category category)
        {
            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();

            return category;
        }

        public async Task<Category?> DeleteCategoryAsync(Guid id)
        {
            var existingItem = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (existingItem != null)
            {
                dbContext.Categories.Remove(existingItem);
                await dbContext.SaveChangesAsync();
                return existingItem;
            }
            return null;
        }

        public async Task<IEnumerable<Category>> GetAllCategoryAsync()
        {
            return await dbContext.Categories.ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(Guid id)
        {
            return await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Category?> UpdateCategoryAsync(Category category)
        {
            var existingCategory =  await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);

            if (existingCategory != null)
            {
                dbContext.Entry(existingCategory).CurrentValues.SetValues(category);
                await dbContext.SaveChangesAsync();
                return category;
            }

            return null;

        }


    }
}
