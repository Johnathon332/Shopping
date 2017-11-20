using System.Collections.Generic;
using StrataShoppingLib.Components.Database.DomainModels;
using System;
using System.Linq;

namespace StrataShoppingLib.Components.Database.Repositories
{
	public class ProductRepository : IProductRepository
	{
		#region Member variables

		private readonly ShoppingContext mContext; ///< Context to access the datastore

		#endregion Member variables

		#region Constructor

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="context">Context to access the datastore</param>
		public ProductRepository(ShoppingContext context)
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
		public IEnumerable<Product> Get()
		{
			return mContext.Products;
		}

		/// <summary>
		/// Gets a product from the datastore given a product code
		/// </summary>
		/// <param name="productCode">The product code of the product</param>
		/// <returns>A product</returns>
		public Product GetByProductCode(string productCode)
		{
			if (String.IsNullOrEmpty(productCode))
			{
				throw new ArgumentNullException("productCode", "No identifier has been specified");
			}

			return mContext.Products.FirstOrDefault(p => p.Code == productCode);
		}

		/// <summary>
		/// Remove the entity from the datastore
		/// </summary>
		/// <param name="entity">THe entity to be removed</param>
		public void Remove(Product entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity", "No entity has been passed in");
			}

			mContext.Products.Remove(entity);
			mContext.SaveChanges();
		}

		/// <summary>
		/// Inserts the entity into the datastore
		/// </summary>
		/// <param name="entity">The entity to be inserted</param>
		public void Save(Product entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity", "No entity has been passed in");
			}

			mContext.Products.Add(entity);
			mContext.SaveChanges();
		}

		/// <summary>
		/// Update the entity, the entity will be searched by via the keys and will be updated
		/// </summary>
		/// <param name="entity">The entity to be updated by</param>
		public void Update(Product entity)
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