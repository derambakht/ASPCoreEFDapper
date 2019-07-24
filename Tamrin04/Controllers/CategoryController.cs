using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tamrin04.Entities;
using Tamrin04.GenericRepository;

namespace Tamrin04.Controllers
{
    public class CategoryController : Controller
    {
      private IGenericRepository<Categories> _Category;
        public CategoryController(IGenericRepository<Categories> Category)
        {
            _Category = Category;
        }
        public async Task<IActionResult> Index()
        {
            var result = await _Category.GetAsync(null, q=> q.OrderBy(c=>c.CategoryName));
            return View(result);
        }
    }
}