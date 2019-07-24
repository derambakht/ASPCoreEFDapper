using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tamrin04.Entities;
using Tamrin04.GenericRepository;
using Tamrin04.Services;

namespace Tamrin04.Controllers
{
    public class ProductController : Controller
    {
        //ef
        private IGenericRepository<Products> _productService;

        //dapper
        private IProductService _dapperProductService;

        public ProductController(IGenericRepository<Products> productService,
            IProductService dapperProductService)
        {
            _productService = productService;
            _dapperProductService = dapperProductService;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageItemSize = 10;
            int skip = (page - 1) * pageItemSize;

            int totalItemCount = await _productService.CountAsync();

            var result = await _productService.GetAsync(null, q => q.OrderBy(c => c.ProductId),
                skip: skip, take:pageItemSize);


            ViewData["PageItemSize"] = pageItemSize;
            ViewData["TotalItemCount"] = totalItemCount;

            ViewData["PageItemCount"] = Math.Ceiling((decimal)totalItemCount / pageItemSize);
            ViewData["CurrentPage"] = page;
            return View(result);
        }

        public async Task<IActionResult> GetProducts(int pageNumber)
        {
            int pageItemSize = 10;
            int skip = (pageNumber - 1) * pageItemSize;

            var result = await _productService.GetAsync(null, q => q.OrderBy(c => c.ProductId),
                skip: skip, take: pageItemSize);

            return Json(result);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Products model)
        {
            //call with command text
            await _dapperProductService.AddAsync(model);

            //call with sp
            int id = await _dapperProductService.AddWithSPAsync(model);
            return View();
        }
    }
}