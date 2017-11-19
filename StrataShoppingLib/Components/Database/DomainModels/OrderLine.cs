using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StrataShoppingLib.Components.Database.DomainModels
{
	/// <summary>
	/// Order line class
	/// </summary>
	public class OrderLine
	{
		/// <summary>
		/// Order Id the product is associated with
		/// </summary>
		[Key, Column(Order = 1)]
		public Guid OrderId { get; set; }

		/// <summary>
		/// The product code for the product
		/// </summary>
		[Key, Column(Order = 2)]
		public string ProductCode { get; set; }

		/// <summary>
		/// The quantity of the product wanted
		/// </summary>
		public int Quantity { get; set; }

		/// <summary>
		/// Unit price of product
		/// </summary>
		public decimal UnitPrice { get; set; }

		/// <summary>
		/// Navigation property for Order
		/// </summary>
		[ForeignKey("OrderId")]
		public virtual Order Order { get; set; }

		/// <summary>
		/// Navigation property for Product
		/// </summary>
		[ForeignKey("ProductCode")]
		public virtual Product Product { get; set; }
	}
}