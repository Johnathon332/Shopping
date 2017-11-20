using StrataShoppingLib.Components.Database.DomainModels;
using System;

namespace StrataShoppingLib.Components.Database.Repositories
{
	/// <summary>
	/// Interface for order repository
	/// </summary>
	public interface IOrderRepository : IRepository<Order>
	{
		/// <summary>
		/// Get an Order by its Id
		/// </summary>
		/// <param name="Id">Id of the order</param>
		/// <returns>An order</returns>
		Order GetById(Guid Id);
	}
}