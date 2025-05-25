using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Security.Claims;
using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic; // For List<int>
using Microsoft.Extensions.Logging; // For ILogger

namespace Ecommerce.Controllers
{
    [Authorize] // Ensure only logged-in users can access Checkout
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CheckoutController>? _logger;

        public CheckoutController(ApplicationDbContext context, ILogger<CheckoutController>? logger = null)
        {
            _context = context;
            _logger = logger;
        }

        // POST: /Checkout/ProcessSelectedItems
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessSelectedItems(string selectedCartItemIds)
        {
            // Retrieve User ID from claims
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null || !int.TryParse(userIdString, out int userId))
            {
                _logger?.LogError("User ID not found or invalid in claims for Checkout. User.Identity.Name: {UserName}", User.Identity?.Name);
                TempData["ErrorMessage"] = "Autentikasi diperlukan untuk melanjutkan checkout. Silakan login kembali.";
                return RedirectToAction("Login", "Account"); // Redirect to login if user ID is not found or invalid
            }

            // Validate selectedCartItemIds input
            if (string.IsNullOrWhiteSpace(selectedCartItemIds))
            {
                _logger?.LogWarning("No cart items selected for checkout by user {UserId}.", userId);
                TempData["ErrorMessage"] = "Tidak ada item yang dipilih untuk checkout.";
                return RedirectToAction("Index", "Cart");
            }

            // Parse selected IDs from the string
            var selectedIds = selectedCartItemIds.Split(',')
                                             .Where(id => int.TryParse(id, out _)) // Filter out any non-integer values
                                             .Select(int.Parse)
                                             .ToList();

            if (!selectedIds.Any())
            {
                _logger?.LogWarning("No valid cart items found after parsing selected IDs for user {UserId}. Input: {SelectedIdsInput}", userId, selectedCartItemIds);
                TempData["ErrorMessage"] = "Tidak ada item yang valid dipilih untuk checkout.";
                return RedirectToAction("Index", "Cart");
            }

            // Fetch cart items from the database, ensuring they belong to the current user
            var cartItemsToCheckout = await _context.CartItems
                                                    .Where(ci => selectedIds.Contains(ci.Id) && ci.UserId == userId)
                                                    .Include(ci => ci.Product) // Include Product to get product name for notification
                                                    .ToListAsync();

            if (!cartItemsToCheckout.Any())
            {
                _logger?.LogWarning("Selected cart items {SelectedIds} not found for user {UserId} or already processed.", string.Join(",", selectedIds), userId);
                TempData["ErrorMessage"] = "Item yang dipilih tidak ditemukan di keranjang Anda atau sudah dihapus.";
                return RedirectToAction("Index", "Cart");
            }

            // --- Simulate Checkout Process ---
            // In a real application, this is where you would:
            // 1. Create an Order header record.
            // 2. Create OrderItem records for each item in cartItemsToCheckout.
            // 3. Update product stock (decrement quantity).
            // 4. Handle payment processing (e.g., integrate with a payment gateway).
            // 5. Potentially send confirmation emails.

            // For this example, we will just simulate success and remove items from the cart.

            // Construct success message with product names
            var productNames = cartItemsToCheckout.Select(ci => ci.Product.Name).ToList();
            TempData["SuccessMessage"] = $"Berhasil checkout item: {string.Join(", ", productNames)}. Terima kasih!";

            // Remove processed items from the cart
            _context.CartItems.RemoveRange(cartItemsToCheckout);
            await _context.SaveChangesAsync(); // Persist changes to the database

            _logger?.LogInformation("User {UserId} successfully checked out {ItemCount} items: {ProductNames}", userId, cartItemsToCheckout.Count, string.Join(", ", productNames));

            // Redirect back to the cart page, which will now show the updated (empty or reduced) cart
            return RedirectToAction("Index", "Cart");
        }
    }
}