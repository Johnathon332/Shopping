using System;
using System.Collections.Generic;

namespace StrataShoppingLib.Components.Database.Repositories
{
	/// <summary>
	/// Base repository interface
	/// </summary>
	public interface IRepository<T> where T : class
	{
		/// <summary>
		/// Returns all rows from the datastore
		/// </summary>
		/// <returns>All rows of T from the database as an IEnumerable</returns>
		IEnumerable<T> Get();

		/// <summary>
		/// Inserts the entity into the datastore
		/// </summary>
		/// <param name="entity">The entity to be inserted</param>
		void Save(T entity);

		/// <summary>
		/// Update the entity, the entity will be searched by via the keys and will be updated
		/// </summary>
		/// <param name="entity">The entity to be updated by</param>
		void Update(T entity);

		/// <summary>
		/// Remove the entity from the datastore
		/// </summary>
		/// <param name="entity">THe entity to be removed</param>
		void Remove(T entity);
	}
}