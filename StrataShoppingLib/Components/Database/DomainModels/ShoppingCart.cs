using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StrataShoppingLib.Components.Database.DomainModels
{
	/// <summary>
	/// Shopping cart domain model
	/// </summary>
	public class ShoppingCart
	{
		/// <summary>
		/// Foreign key for the customer
		/// </summary>
		[Key, Column(Order = 1)]
		public string CustomerName { get; set; }

		/// <summary>
		/// Navigation property for customer id foreign key
		/// </summary>
		[ForeignKey("CustomerName")]
		public virtual Customer Customer { get; set; }

		/// <summary>
		/// The item the user is buying
		/// </summary>
		[Key, Column(Order = 2)]
		public string ProductCode { get; set; }

		/// <summary>
		/// Navigation property for ProductCode foreign key
		/// </summary>
		[ForeignKey("ProductCode")]
		public virtual Product Product { get; set; }

		/// <summary>
		/// The quantity of the product the user wants to purchase
		/// </summary>
		public int Quantity { get; set; }

		/// <summary>
		/// Price of one unit
		/// </summary>
		public decimal UnitPrice { get; set; }
	}
}