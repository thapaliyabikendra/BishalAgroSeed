using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BishalAgroSeed.Data;
using BishalAgroSeed.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace BishalAgroSeed.Pages.Products
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly BishalAgroSeed.Data.ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public EditModel(BishalAgroSeed.Data.ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;

        }

        [BindProperty]
        public Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product = await _context.Product.FirstOrDefaultAsync(m => m.Id == id);

            if (Product == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Product).State = EntityState.Modified;
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

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(Product.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
