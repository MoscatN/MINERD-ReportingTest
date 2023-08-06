using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SalesReporter.Controllers;

[Authorize(AuthenticationSchemes = "Cookies")]
public class ProductsController : Controller {
    private StoredbContext _context;

    public ProductsController(StoredbContext context) {
        _context = context;
    }

    public IActionResult Index() 
    {
        return RedirectToAction("Products");
    }

    [HttpGet]
    public async Task<IActionResult> Products() {
        List<Product> products = await _context
                          .Products
                          .OrderByDescending(x => x.Price)
                          .ToListAsync();
        if(products.Count == 0) {
            return View();
        }

        return View(products);
    }

    [HttpGet]
    public async Task<IActionResult> Create() 
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Product product) 
    {
        if (ModelState.IsValid) {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Products");
        }
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id) 
    {
        Product product = await _context.Products.FindAsync(id);

        return View(product);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProduct(int id, Product product) 
    {
        if (product == null) {
            return View("Products");
        }

        Product dbProd = await _context
                    .Products
                    .FindAsync(id);

        if(dbProd != null) 
        {
            dbProd.ProductName = product.ProductName;
            dbProd.Ammount = product.Ammount;
            dbProd.Price = product.Price;

            await _context.SaveChangesAsync();

            return RedirectToAction("Products");
        }

        if (dbProd == null) return View("Products");

        return View("Products");
    }

    public PartialViewResult GetReport() 
    {
        return PartialView("_Report");
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id) 
    {
        Product product = await _context.Products.FindAsync(id);

        return View(product);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteProduct(Product productDelete) 
    {
        if (productDelete == null) 
        {
            return RedirectToAction("Products");
        }

        Product product = await _context.Products.FindAsync(productDelete.Id);
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return View("Products");
    }
}
