using StrataShoppingLib.Components.Database.DomainModels;

namespace StrataShoppingLib.Components.Database.Repositories
{
	/// <summary>
	/// Interface for the product repository
	/// </summary>
	public interface IProductRepository : IRepository<Product>
	{
		/// <summary>
		/// Gets a product from the datastore given a product code
		/// </summary>
		/// <param name="productCode">The product code of the product</param>
		/// <returns>A product</returns>
		Product GetByProductCode(string productCode);
	}
}