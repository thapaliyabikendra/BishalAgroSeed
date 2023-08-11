using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BishalAgroSeed.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BishalAgroSeed.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly BishalAgroSeed.Data.ApplicationDbContext _context;


        public IndexModel(ILogger<IndexModel> logger, BishalAgroSeed.Data.ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;

        }
        public IList<Product> Products { get;set; }


        public async Task OnGetAsync()
        {

            Products = await _context.Product.Take(6).ToListAsync();

        }
    }
}
