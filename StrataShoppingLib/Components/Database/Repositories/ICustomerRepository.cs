using StrataShoppingLib.Components.Database.DomainModels;
using System;

namespace StrataShoppingLib.Components.Database.Repositories
{
	/// <summary>
	/// Interface for customer repository
	/// </summary>
	public interface ICustomerRepository : IRepository<Customer>
	{
		/// <summary>
		/// Returns one item from the datastore depending searching by an indentifier
		/// </summary>
		/// <param name="identifier">Field that we are searching by</param>
		/// <returns>Customer matching the identifier</returns>
		Customer GetByUniqueIdentifier(string identifier);
	}
}