using System.ComponentModel.DataAnnotations;

namespace StrataShoppingLib.Components.Database.DomainModels
{
	/// <summary>
	/// Model to hold product information
	/// </summary>
	public class Product
	{
		/// <summary>
		/// The product code
		/// </summary>
		[Key]
		public string Code { get; set; }

		/// <summary>
		/// Description of the product
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Price of one unit of the product
		/// </summary>
		public decimal UnitPrice { get; set; }
	}
}