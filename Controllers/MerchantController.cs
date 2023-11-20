using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DemoWebApp.Data;
using DemoWebApp.Models;

namespace DemoWebApp.Controllers
{
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
            mvm.Products = _context.Product.ToList();
            mvm.Customers = _context.Customer.ToList();

            return View(mvm);
        }
    }
}
