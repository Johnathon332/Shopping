using StrataShoppingLib.Components.Database.DomainModels;
using System.Collections.Generic;

namespace StrataShoppingLib.Components.Database.Repositories
{
	/// <summary>
	/// Interface for shopping cart repository
	/// </summary>
	public interface IShoppingCartRepository : IRepository<ShoppingCart>
	{
		/// <summary>
		/// Retrieves the customers shopping cart
		/// </summary>
		/// <param name="customerName"></param>
		/// <returns>A list of shopping cart items belonging to the customer</returns>
		IEnumerable<ShoppingCart> GetCustomerShoppingCart(string customerName);
	}
}