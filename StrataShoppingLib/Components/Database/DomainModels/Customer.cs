using System.ComponentModel.DataAnnotations;

namespace StrataShoppingLib.Components.Database.DomainModels
{
	/// <summary>
	/// Customer domain model
	/// </summary>
	public class Customer
	{
		/// <summary>
		/// The name of the customer
		/// </summary>
		[Key]
		public string Name { get; set; }

		/// <summary>
		/// Customer membership type
		/// </summary>
		public MemberType Type { get; set; }

		/// <summary>
		/// Address of the customer
		/// </summary>
		public string Address { get; set; }

		/// <summary>
		/// Email of the customer
		/// </summary>
		public string Email { get; set; }
	}
}