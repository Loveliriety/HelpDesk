using System.Collections.Generic;
using ASI.Basecode.Data.Models; 
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Data.Repositories; 

namespace ASI.Basecode.Services.Interfaces
{
    public interface ICategoryService
    {
        Category GetCategoryById(int categoryId);
        List<Category> GetAllCategories();
        void AddCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(int categoryId);
    }
}
