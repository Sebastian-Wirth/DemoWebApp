using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DemoWebApp.Data;
using DemoWebApp.Models;
using Azure.Security.KeyVault.Keys.Cryptography;
using Azure.Identity;
using System.Text;

namespace DemoWebApp.Controllers
{
    public class CustomersController : Controller
    {
        private readonly DemoDbContext _context;
        private readonly CryptographyClient _cryptoClient;

        public CustomersController(DemoDbContext context)
        {
            _context = context;
            Uri keyId = new ("https://wirthwebkeyvault.vault.azure.net/keys/WirthWebKey/");
            _cryptoClient = new CryptographyClient(keyId, new DefaultAzureCredential());
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
              return View(await _context.Customer.ToListAsync());
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Customer == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CreditCardNo")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.CreditCardNo = Encrypt(customer.CreditCardNo);
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), "Merchant");
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Customer == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            customer.CreditCardNo = Decrypt(customer.CreditCardNo);
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CreditCardNo")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                customer.CreditCardNo = Encrypt(customer.CreditCardNo);
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), "Merchant");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Customer == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            customer.CreditCardNo = Decrypt(customer.CreditCardNo);
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Customer == null)
            {
                return Problem("Entity set 'DemoDbContext.Customer'  is null.");
            }
            var customer = await _context.Customer.FindAsync(id);
            if (customer != null)
            {
                _context.Customer.Remove(customer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), "Merchant");
        }

        private bool CustomerExists(int id)
        {
          return _context.Customer.Any(e => e.Id == id);
        }

        private string Encrypt(string input)
        {
            byte[] inputAsByteArray = Encoding.UTF8.GetBytes(input);
            EncryptResult encryptResult = _cryptoClient.Encrypt(EncryptionAlgorithm.RsaOaep, inputAsByteArray);
            return Convert.ToBase64String(encryptResult.Ciphertext);
        }

        private string Decrypt(string input)
        {
            byte[] inputAsByteArray = Convert.FromBase64String(input);
            DecryptResult decryptResult = _cryptoClient.Decrypt(EncryptionAlgorithm.RsaOaep, inputAsByteArray);
            return Encoding.Default.GetString(decryptResult.Plaintext);
        }
    }
}
