using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Repositories
{
    public class CategoryRepository : BaseRepository, ICategoryRepository
    {
        private readonly AsiBasecodeDBContext _dbContext;

        public CategoryRepository(AsiBasecodeDBContext dbContext, UnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = dbContext;
        }

        public Category GetCategoryById(int categoryId)
        {
            return _dbContext.Set<Category>().Find(categoryId);
        }

        public List<Category> GetAllCategories()
        {
            return _dbContext.Set<Category>().ToList();
        }

        public void AddCategory(Category category)
        {
            _dbContext.Set<Category>().Add(category);
            _dbContext.SaveChanges();
        }

        public void UpdateCategory(Category category)
        {
            _dbContext.Set<Category>().Update(category);
            _dbContext.SaveChanges();
        }

        public void DeleteCategory(int categoryId)
        {
            var category = _dbContext.Set<Category>().Find(categoryId);
            if (category != null)
            {
                _dbContext.Set<Category>().Remove(category);
                _dbContext.SaveChanges();
            }
        }

        //working
    }
}
