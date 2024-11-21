using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Interfaces
{
    public interface ICategoryRepository
    {
        public Category GetCategoryById(int categoryId);
        public List<Category> GetAllCategories();
        public void AddCategory(Category category);
        public void UpdateCategory(Category category);
        public void DeleteCategory(int categoryId);

        //working
    }
}
