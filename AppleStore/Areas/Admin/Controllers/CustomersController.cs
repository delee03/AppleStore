using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppleStore.Models;
using Newtonsoft.Json;

namespace AppleStore.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class CustomersController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;

		public CustomersController(UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
		}

		// GET: Customer
		public async Task<IActionResult> Index()
		{
			// Get a list of all customers
			var Customers = await _userManager.GetUsersInRoleAsync("Customer");
			ViewBag.Customers = Customers;
			return View();
		}

		// GET: Customer/Details/5
		public async Task<IActionResult> Detail(string id)
		{
			var customer = await _userManager.FindByIdAsync(id);
			if (customer == null)
			{
				return RedirectToAction("Index");
			}

			ViewBag.Customer = customer;

			return View();
		}

		// GET: Customer/Delete/5
		public async Task<IActionResult> Delete(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var customer = await _userManager.FindByIdAsync(id);
			if (customer == null)
			{
				return NotFound();
			}

			return View(customer);
		}

		// POST: Customer/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(string id)
		{
			var customer = await _userManager.FindByIdAsync(id);
			if (customer == null)
			{
				return NotFound();
			}

			// Delete the customer
			var result = await _userManager.DeleteAsync(customer);
			if (result.Succeeded)
			{
				return RedirectToAction(nameof(Index));
			}

			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}

			return View(customer);
		}
	}
}