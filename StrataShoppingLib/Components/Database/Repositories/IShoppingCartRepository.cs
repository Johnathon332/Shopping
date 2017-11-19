using StrataShoppingLib.Components.Database.DomainModels;

namespace StrataShoppingLib.Components.Database.Repositories
{
	public interface IShoppingCartRepository : IRepository<ShoppingCart>
	{
		/// <summary>
		/// Retrieves the customers shopping cart
		/// </summary>
		/// <param name="customerName"></param>
		/// <returns></returns>
		ShoppingCart GetCustomerShoppingCart(string customerName);
	}
}