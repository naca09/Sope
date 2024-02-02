using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Rolepp.Data;
using Rolepp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Rolepp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Product
        public IActionResult Index()
        {
            // Retrieve data from the ProductsTest table with Warehouse information
            var products = _context.Products
                .Include(p => p.Warehouse) // Include Warehouse information
                .Take(1000) // Limit to top 1000 rows
                .ToList(); // Execute the query and convert to a list

            // Pass the data to the view
            return View(products);
        }

        // GET: Product/Details/5
        

        // GET: Product/Create
        public IActionResult Create()
        {
            // Get the list of warehouses to populate the dropdown
            ViewBag.Warehouses = new SelectList(_context.Warehouses, "WarehouseId", "WarehouseName");
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,ProductName,Price,Quantity,ProductCode,WarehouseId")] Product product)
        {
            if (product.ProductID != null && product.ProductName != null && product.Price != null && product.Quantity != null && product.ProductCode != null && product.WarehouseId != null && product.Warehouse == null)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Warehouses = new SelectList(_context.Warehouses, "WarehouseId", "WarehouseName", product.WarehouseId);
            return View(product);
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Get the list of warehouses to populate the dropdown
            ViewBag.Warehouses = new SelectList(_context.Warehouses, "WarehouseId", "WarehouseName", product.WarehouseId);
            return View(product);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,ProductName,Price,Quantity,ProductCode,WarehouseId")] Product product)
        {
            if (id != product.ProductID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // If validation fails, re-populate the dropdown with the list of warehouses
            ViewBag.Warehouses = new SelectList(_context.Warehouses, "WarehouseId", "WarehouseName", product.WarehouseId);
            return View(product);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductID == id);
        }
    }
}