using System;
using System.Collections.Generic;
using StrataShoppingLib.Components.Database.DomainModels;
using System.Linq;

namespace StrataShoppingLib.Components.Database.Repositories
{
	/// <summary>
	/// Implementation class for the Shopping cart repository
	/// </summary>
	public class ShoppingCartRepository : IShoppingCartRepository
	{
		#region Member variables

		private readonly ShoppingContext mContext; ///< Context to access the datastore

		#endregion Member variables

		#region Constructor

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="context">Context to access the datastore</param>
		public ShoppingCartRepository(ShoppingContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context", "No context was passed in");
			}

			mContext = context;
		}

		#endregion Constructor

		/// <summary>
		/// Returns all rows from the datastore
		/// </summary>
		/// <returns>All rows of T from the database as an IEnumerable</returns>
		public IEnumerable<ShoppingCart> Get()
		{
			return mContext.ShoppingCarts;
		}

		public IEnumerable<ShoppingCart> GetCustomerShoppingCart(string customerName)
		{
			if (String.IsNullOrEmpty(customerName))
			{
				throw new ArgumentNullException("customerName", "No customer name has been specified");
			}

			return mContext.ShoppingCarts.Where(s => s.CustomerName == customerName);
		}

		/// <summary>
		/// Remove the entity from the datastore
		/// </summary>
		/// <param name="entity">THe entity to be removed</param>
		public void Remove(ShoppingCart entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity", "No entity has been passed in");
			}

			mContext.ShoppingCarts.Remove(entity);
			mContext.SaveChanges();
		}

		/// <summary>
		/// Inserts the entity into the datastore
		/// </summary>
		/// <param name="entity">The entity to be inserted</param>
		public void Save(ShoppingCart entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity", "No entity has been passed in");
			}

			mContext.ShoppingCarts.Add(entity);
			mContext.SaveChanges();
		}

		/// <summary>
		/// Update the entity, the entity will be searched by via the keys and will be updated
		/// </summary>
		/// <param name="entity">The entity to be updated by</param>
		public void Update(ShoppingCart entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity", "No entity has been passed in");
			}

			mContext.ModifyEntity(entity);
			mContext.SaveChanges();
		}
	}
}