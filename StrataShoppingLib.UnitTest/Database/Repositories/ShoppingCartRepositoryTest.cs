using Moq;
using NUnit.Framework;
using StrataShoppingLib.Components.Database;
using StrataShoppingLib.Components.Database.Repositories;
using StrataShoppingLib.UnitTest.Database.TestHelpers;

namespace StrataShoppingLib.UnitTest.Database.Repositories
{
	[TestFixture]
	public class ShoppingCartRepositoryTest
	{
		#region member variables

		private Mock<ShoppingContext> mMockContext; ///< Fake of the context
		private IShoppingCartRepository mRepository; ///< The repository under test

		#endregion member variables

		#region Setup

		/// <summary>
		/// One time setup for the class
		/// </summary>
		[SetUp]
		public void Setup()
		{
			mMockContext = new Mock<ShoppingContext>();

			mRepository = RepositoryFactory.RepositoryGenerator<IShoppingCartRepository>(mMockContext);
		}

		#endregion Setup
	}
}