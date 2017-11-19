using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrataShoppingLib.Components.Database.DomainModels
{
	/// <summary>
	/// Order model
	/// </summary>
	public class Order
	{
		/// <summary>
		/// The id of the order
		/// </summary>
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		/// <summary>
		/// The customer name
		/// </summary>
		[ForeignKey("Customer")]
		public string CustomerName { get; set; }

		/// <summary>
		/// Date the order was made
		/// </summary>
		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime Date { get; set; }

		/// <summary>
		/// The customers address
		/// </summary>
		public string CustomerAddress { get; set; }

		/// <summary>
		/// Navigation property for the customer foreign key
		/// </summary>
		public virtual Customer Customer { get; set; }

		/// <summary>
		/// The total amount of the items in the shopping cart
		/// </summary>
		public decimal Amount { get; set; }
	}
}