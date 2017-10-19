using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FirstPass.Models;

namespace FirstPass.Controllers
{
    public class AddressesController : Controller
    {
        private readonly FirstPassContext _context;

        public AddressesController(FirstPassContext context)
        {
            _context = context;
        }

        // GET: Addresses
        public async Task<IActionResult> Index()
        {
            return View(await _context.Address.ToListAsync());
        }

        // GET: Addresses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.Address
                .SingleOrDefaultAsync(m => m.ID == id);
            if (address == null)
            {
                return NotFound();
            }

            return View(address);
        }

        // GET: Addresses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Addresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string fullAddress)
        {
            Address address = new Address();
            string[] addressList = fullAddress.Split(",");
            if (addressList.Length == 6 && int.TryParse(addressList[5], out var zip))
            {
                address.Name = addressList[0];
                address.Address1 = addressList[1];
                address.Address2 = addressList[2];
                address.City = addressList[3];
                address.State = addressList[4];
                address.Zip = zip;
                TempData["Message"] = address.Name + " added";

                _context.Add(address);
                await _context.SaveChangesAsync();
            }
            else
            {
                TempData["Message"] = "Invalid format, no data was added";
            }

            return RedirectToAction(nameof(Create));
        }

        // GET: Addresses/DeleteScan
        public IActionResult DeleteScan()
        {
            return View();
        }

        // POST: Addresses/DeleteScan
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteScan(string fullAddress)
        {
            string[] addressList = fullAddress.Split(",");
            if (addressList.Length == 6 && int.TryParse(addressList[5], out var zip))
            {
                try
                {
                    var address = await _context.Address.FirstAsync(
                        m => m.Name == addressList[0] &&
                        m.Address1 == addressList[1] &&
                        m.Address2 == addressList[2] &&
                        m.City == addressList[3] &&
                        m.State == addressList[4] &&
                        m.Zip == zip);
                    TempData["Message"] = address.Name + " deleted";
                    _context.Address.Remove(address);
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    TempData["Message"] = "Not in Database, no data was deleted";
                }
                
            }
            else
            {
                TempData["Message"] = "Invalid format, no data was deleted";
            }
            return RedirectToAction(nameof(DeleteScan));
        }

        // GET: Addresses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.Address
                .SingleOrDefaultAsync(m => m.ID == id);
            if (address == null)
            {
                return NotFound();
            }

            return View(address);
        }

        // POST: Addresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var address = await _context.Address.SingleOrDefaultAsync(m => m.ID == id);
            _context.Address.Remove(address);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AddressExists(int id)
        {
            return _context.Address.Any(e => e.ID == id);
        }
    }
}
