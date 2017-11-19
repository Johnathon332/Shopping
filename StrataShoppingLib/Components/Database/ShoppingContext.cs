using StrataShoppingLib.Components.Database.DomainModels;
using System.Data.Entity;

namespace StrataShoppingLib.Components.Database
{
	/// <summary>
	/// Class for accessing the database
	/// </summary>
	public class ShoppingContext : DbContext, IDbContext
	{
		#region Constructor

		/// <summary>
		/// Constructor
		/// </summary>
		public ShoppingContext() : base("StrataShopping")
		{
		}

		#endregion Constructor

		#region Context methods

		//
		// Summary:
		//     This method is called when the model for a derived context has been initialized,
		//     but before the model has been locked down and used to initialize the context.
		//     The default implementation of this method does nothing, but it can be overridden
		//     in a derived class such that the model can be further configured before it is
		//     locked down.
		//
		// Parameters:
		//   modelBuilder:
		//     The builder that defines the model for the context being created.
		//
		// Remarks:
		//     Typically, this method is called only once when the first instance of a derived
		//     context is created. The model for that context is then cached and is for all
		//     further instances of the context in the app domain. This caching can be disabled
		//     by setting the ModelCaching property on the given ModelBuidler, but note that
		//     this can seriously degrade performance. More control over caching is provided
		//     through use of the DbModelBuilder and DbContextFactory classes directly.
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}

		#endregion Context methods

		#region DbSets

		/// <summary>
		/// Represents a collection of all the customers
		/// </summary>
		public DbSet<Customer> Customers { get; set; }

		/// <summary>
		/// Represents a collection of all the shopping cart items
		/// </summary>
		public DbSet<ShoppingCart> ShoppingCarts { get; set; }

		/// <summary>
		/// Represents a collection of all products
		/// </summary>
		public DbSet<Product> Products { get; set; }

		/// <summary>
		/// Represents a collection of all orders
		/// </summary>
		public DbSet<Order> Orders { get; set; }

		/// <summary>
		/// Represents a collection of all items in the order line
		/// </summary>
		public DbSet<OrderLine> OrderLines { get; set; }

		#endregion DbSets
	}
}