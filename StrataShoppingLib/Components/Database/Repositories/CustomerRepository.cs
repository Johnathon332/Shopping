using StrataShoppingLib.Components.Database.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StrataShoppingLib.Components.Database.Repositories
{
	/// <summary>
	/// Implementation of the ICustomerRepository
	/// </summary>
	public class CustomerRepository : ICustomerRepository
	{
		#region Member variables

		private readonly ShoppingContext mContext; ///< Context to access the datastore

		#endregion Member variables

		#region Constructor

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="context">Context to access the datastore</param>
		public CustomerRepository(ShoppingContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context", "No context was passed in");
			}

			mContext = context;
		}

		#endregion Constructor

		#region Repository methods

		/// <summary>
		/// Returns all rows from the datastore
		/// </summary>
		/// <returns>All rows of T from the database as an IEnumerable</returns>
		public IEnumerable<Customer> Get()
		{
			return mContext.Customers;
		}

		/// <summary>
		/// Returns one item from the datastore depending searching by an indentifier
		/// </summary>
		/// <param name="identifier">Field that we are searching by</param>
		/// <returns>Customer matching the identifier</returns>
		public Customer GetByUniqueIdentifier(string identifier)
		{
			if (String.IsNullOrEmpty(identifier))
			{
				throw new ArgumentNullException("identifier", "No identifier has been specified");
			}

			Customer customer = mContext.Customers.Where(c => c.Name == identifier).FirstOrDefault();
			return customer;
		}

		/// <summary>
		/// Remove the entity from the datastore
		/// </summary>
		/// <param name="entity">THe entity to be removed</param>
		public void Remove(Customer entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity", "No entity has been passed in");
			}

			mContext.Customers.Remove(entity);
			mContext.SaveChanges();
		}

		/// <summary>
		/// Inserts the entity into the datastore
		/// </summary>
		/// <param name="entity">The entity to be inserted</param>
		public void Save(Customer entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity", "No entity has been passed in");
			}

			mContext.Customers.Add(entity);
			mContext.SaveChanges();
		}

		/// <summary>
		/// Update the entity, the entity will be searched by via the keys and will be updated
		/// </summary>
		/// <param name="entity">The entity to be updated by</param>
		public void Update(Customer entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity", "No entity has been passed in");
			}

			mContext.ModifyEntity(entity);
			mContext.SaveChanges();
		}

		#endregion Repository methods
	}
}