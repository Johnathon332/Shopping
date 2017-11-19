using StrataShoppingLib.Components.Database.DomainModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrataShoppingLib.Components.Database
{
	/// <summary>
	/// Interface for dbcontext so it can be mocked
	/// </summary>
	public interface IDbContext
	{
		/// <summary>
		/// Represents a collection of all the customers
		/// </summary>
		DbSet<Customer> Customers { get; set; }

		/// <summary>
		/// Represents a collection of all the shopping cart items
		/// </summary>
		DbSet<ShoppingCart> ShoppingCarts { get; set; }

		/// <summary>
		/// Represents a collection of all products
		/// </summary>
		DbSet<Product> Products { get; set; }

		/// <summary>
		/// Represents a collection of all orders
		/// </summary>
		DbSet<Order> Orders { get; set; }

		/// <summary>
		/// Represents a collection of all items in the order line
		/// </summary>
		DbSet<OrderLine> OrderLines { get; set; }
	}
}