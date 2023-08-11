using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BishalAgroSeed.Data;
using BishalAgroSeed.Models;
using BishalAgroSeed.Extensions;

namespace BishalAgroSeed.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly BishalAgroSeed.Data.ApplicationDbContext _context;

        public IndexModel(BishalAgroSeed.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        //public IList<Product> Product { get;set; }
        public PaginatedList<Product> Products { get; set; }
        public string CurrentFilter { get; set; }

        public async Task OnGetAsync(string searchString, int? pageIndex = 1)
        {
            CurrentFilter = searchString;

            if (searchString != null)
            {
                pageIndex = 1;
            }


            var products = from product in _context.Product select product;
            var productsCount = _context.Product.Count();

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.Name.Contains(searchString));
                productsCount = products.Count();
            }


            int pageSize = 6;
            Products = await PaginatedList<Product>.CreateAsync(
                products.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}
