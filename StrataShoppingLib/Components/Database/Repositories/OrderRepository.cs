using System;
using System.Collections.Generic;
using StrataShoppingLib.Components.Database.DomainModels;
using System.Linq;

namespace StrataShoppingLib.Components.Database.Repositories
{
	public class OrderRepository : IOrderRepository
	{
		#region Member variables

		private readonly ShoppingContext mContext; ///< Context to access the datastore

		#endregion Member variables

		#region Constructor

		/// <summary>
		/// Constructor
		/// </summary>
		public OrderRepository(ShoppingContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context", "No context was passed in");
			}

			mContext = context;
		}

		#endregion Constructor

		#region Repository methods

		public IEnumerable<Order> Get()
		{
			return mContext.Orders;
		}

		public Order GetById(Guid Id)
		{
			return mContext.Orders.FirstOrDefault(o => o.Id == Id);
		}

		public void Remove(Order entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity", "No entity has been passed in");
			}

			mContext.Orders.Remove(entity);
			mContext.SaveChanges();
		}

		public void Save(Order entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity", "No entity has been passed in");
			}

			mContext.Orders.Add(entity);
			mContext.SaveChanges();
		}

		public void Update(Order entity)
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