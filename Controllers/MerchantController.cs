using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DemoWebApp.Data;
using DemoWebApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace DemoWebApp.Controllers
{
    [AllowAnonymous]
    public class MerchantController : Controller
    {
        private readonly DemoDbContext _context;

        public MerchantController(DemoDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            MixedViewModel mvm = new MixedViewModel();
            mvm.Products = await _context.Product.ToListAsync();
            mvm.Customers = await _context.Customer.ToListAsync();

            return View(mvm);
        }
    }
}
