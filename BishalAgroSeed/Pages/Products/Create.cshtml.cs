using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BishalAgroSeed.Data;
using BishalAgroSeed.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace BishalAgroSeed.Pages.Products
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly BishalAgroSeed.Data.ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public CreateModel(BishalAgroSeed.Data.ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Product.Add(Product);
            var productId = Product.Id;
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                string imagePath = @"images/products/";
                var wwwRothPath = _hostingEnvironment.WebRootPath;
                var fileExtension = Path.GetExtension(files[0].FileName);
                var relPath = imagePath + productId + fileExtension;
                var absPath = Path.Combine(wwwRothPath, relPath);
                Product.ImagePath = relPath;
                using (var fs = new FileStream(absPath, FileMode.Create))
                {
                    files[0].CopyTo(fs);
                }
            }
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
