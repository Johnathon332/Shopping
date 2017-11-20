using Moq;
using NUnit.Framework;
using StrataShoppingLib.Components.Database;
using StrataShoppingLib.Components.Database.DomainModels;
using StrataShoppingLib.Components.Database.Repositories;
using StrataShoppingLib.UnitTest.Database.TestHelpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

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

		#region Constructor tests

		/// <summary>
		/// When no db context has been passed in it should throw an ArgumentNullException with
		/// the expected output message
		/// </summary>
		[Test]
		public void Constructor_PassNullContext_ShouldThrowArgumentNullException()
		{
			string expectedExceptionMessage = "No context was passed in";
			Assert.That(() => new ShoppingCartRepository(null),
				Throws.TypeOf<ArgumentNullException>().With.Message.Contains(expectedExceptionMessage));
		}

		#endregion Constructor tests

		#region Repository method tests

		/// <summary>
		/// When calling Get and there is no data in the datastore it should return an IEnumerable with a count
		/// of zero
		/// </summary>
		[Test]
		public void Get_CallingGetNoDataInDatastore_ShouldReturnAnIEnumerableWithCountOfZero()
		{
			// Arrange
			IQueryable<ShoppingCart> ShoppingCarts = new List<ShoppingCart>().AsQueryable();

			Mock<DbSet<ShoppingCart>> mockSet = DbSetMocking.CreateMockSet(ShoppingCarts);

			mMockContext.Setup(c => c.ShoppingCarts).Returns(mockSet.Object);

			// Act
			IEnumerable<ShoppingCart> result = mRepository.Get();

			// Assert
			Assert.That(result.Count(), Is.EqualTo(0));
		}

		/// <summary>
		/// Get all ShoppingCarts from the datastore
		/// </summary>
		[Test]
		public void Get_CallingGet_ShouldReturnExpectedShoppingCart()
		{
			// Arrange
			ShoppingCart expectedShoppingCart = new ShoppingCart()
			{
				CustomerName = "Strata",
				ProductCode = "12345",
				Quantity = 5,
				UnitPrice = 12.50m
			};

			IQueryable<ShoppingCart> shoppingCarts = new List<ShoppingCart>() { expectedShoppingCart }.AsQueryable();

			Mock<DbSet<ShoppingCart>> mockSet = DbSetMocking.CreateMockSet(shoppingCarts);

			mMockContext.Setup(c => c.ShoppingCarts).Returns(mockSet.Object);

			// Act
			ShoppingCart result = mRepository.Get().First();

			// Assert
			Assert.That(result.CustomerName, Is.EqualTo(expectedShoppingCart.CustomerName));
			Assert.That(result.ProductCode, Is.EqualTo(expectedShoppingCart.ProductCode));
			Assert.That(result.Quantity, Is.EqualTo(expectedShoppingCart.Quantity));
			Assert.That(result.UnitPrice, Is.EqualTo(expectedShoppingCart.UnitPrice));
		}

		/// <summary>
		/// When passing null or empty string it should throw an ArgumentNullException
		/// </summary>
		/// <param name="idenfifier"></param>
		[TestCase("")]
		[TestCase(null)]
		public void GetCustomerShoppingCart_PassInvalidIdentifier_ShouldThrowArgumentNullException(string customerName)
		{
			// Arrange
			string expectedMessage = "No customer name has been specified";

			// Assert
			Assert.That(() => mRepository.GetCustomerShoppingCart(customerName),
				Throws.TypeOf<ArgumentNullException>().With.Message.Contains(expectedMessage));
		}

		/// <summary>
		/// Check that if nothing can be found in the datastore we return null
		/// </summary>
		[Test]
		public void GetCustomerShoppingCart_PassIdentifierThatDoesNotExist_ShouldReturnNull()
		{
			// Arrange
			IQueryable<ShoppingCart> ShoppingCarts = new List<ShoppingCart>().AsQueryable();

			Mock<DbSet<ShoppingCart>> mockSet = DbSetMocking.CreateMockSet(ShoppingCarts);

			mMockContext.Setup(c => c.ShoppingCarts).Returns(mockSet.Object);

			// Act
			IEnumerable<ShoppingCart> result = mRepository.GetCustomerShoppingCart("hello");

			// Assert
			Assert.That(result.Count(), Is.EqualTo(0));
		}

		/// <summary>
		/// Test when GetByUniqueIdentifier was called then we get a ShoppingCart with the name we are expecting
		/// </summary>
		[Test]
		public void GetCustomerShoppingCart_PassValidIdenfitier_ShouldReturnExpectedShoppingCartWithNameStrata()

		{
			// Arrange
			string expectedName = "Strata";
			ShoppingCart expectedShoppingCart = new ShoppingCart()
			{
				CustomerName = expectedName,
				ProductCode = "1234"
			};

			IQueryable<ShoppingCart> ShoppingCarts = new List<ShoppingCart>()
			{
				expectedShoppingCart,
				new ShoppingCart() { CustomerName = "Bob", ProductCode = "1234" }
			}.AsQueryable();

			Mock<DbSet<ShoppingCart>> mockSet = DbSetMocking.CreateMockSet(ShoppingCarts);

			mMockContext.Setup(c => c.ShoppingCarts).Returns(mockSet.Object);

			// Act
			IEnumerable<ShoppingCart> result = mRepository.GetCustomerShoppingCart(expectedName);

			// Assert
			Assert.That(result.FirstOrDefault().CustomerName, Is.EqualTo(expectedShoppingCart.CustomerName));
			Assert.That(result.FirstOrDefault().ProductCode, Is.EqualTo(expectedShoppingCart.ProductCode));
		}

		/// <summary>
		/// When passing a null ShoppingCart to the remove method it should thwo ArgumentNullException with an expected message
		/// </summary>
		[Test]
		public void Remove_PassNullShoppingCart_ShouldThrowArgumentNullException()
		{
			string expectedMessage = "No entity has been passed in";

			Assert.That(() => mRepository.Remove(null),
				Throws.TypeOf<ArgumentNullException>().With.Message.Contains(expectedMessage));
		}

		/// <summary>
		/// Verify that remove and SaveChanges is called with the passed in entity
		/// </summary>
		[Test]
		public void Remove_PassValidShoppingCart_ShouldCallDeleteOnTheContextWithShoppingCart()
		{
			// Arrange
			ShoppingCart expectedShoppingCart = new ShoppingCart()
			{
				CustomerName = "Strata",
			};

			mMockContext.Setup(r => r.ShoppingCarts.Remove(It.Is<ShoppingCart>(s => s.CustomerName == expectedShoppingCart.CustomerName)));
			mMockContext.Setup(s => s.SaveChanges());

			// Act
			mRepository.Remove(expectedShoppingCart);

			// Assert
			mMockContext.Verify(c => c.ShoppingCarts.Remove(It.Is<ShoppingCart>(s => s.CustomerName == expectedShoppingCart.CustomerName)), Times.Once());
			mMockContext.Verify(s => s.SaveChanges(), Times.Once());
		}

		/// <summary>
		/// When passing a null ShoppingCart to the Save method it should thwo ArgumentNullException with an expected message
		/// </summary>
		[Test]
		public void Save_PassNullShoppingCart_ShouldThrowArgumentNullException()
		{
			// Arrange
			string expectedMessage = "No entity has been passed in";

			// Assert
			Assert.That(() => mRepository.Save(null),
				Throws.TypeOf<ArgumentNullException>().With.Message.Contains(expectedMessage));
		}

		/// <summary>
		/// Verify that Add and SaveChanges is called with the passed in entity
		/// </summary>
		[Test]
		public void Save_PassValidShoppingCart_ShouldCallSaveOnTheContextWithShoppingCart()
		{
			// Arrange
			ShoppingCart expectedShoppingCart = new ShoppingCart()
			{
				CustomerName = "Strata"
			};

			mMockContext.Setup(r => r.ShoppingCarts.Add(It.Is<ShoppingCart>(s => s.CustomerName == expectedShoppingCart.CustomerName)));
			mMockContext.Setup(s => s.SaveChanges());

			// Act
			mRepository.Save(expectedShoppingCart);

			// Assert
			mMockContext.Verify(c => c.ShoppingCarts.Add(It.Is<ShoppingCart>(s => s.CustomerName == expectedShoppingCart.CustomerName)), Times.Once());
			mMockContext.Verify(s => s.SaveChanges(), Times.Once());
		}

		/// <summary>
		/// WHen passing null ShoppingCart to the update method in the repository it should throw an ArgumentNullException
		/// with the expected message
		/// </summary>
		[Test]
		public void Update_PassNullShoppingCart_ShouldThrowArgumentNullException()
		{
			// Arrange
			string expectedMessage = "No entity has been passed in";

			// Assert
			Assert.That(() => mRepository.Update(null),
				Throws.TypeOf<ArgumentNullException>().With.Message.Contains(expectedMessage));
		}

		[Test]
		public void Update_PassValidShoppingCart_ShouldCallUpdateAndSaveChanges()
		{
			// Arrange
			ShoppingCart expectedShoppingCart = new ShoppingCart()
			{
				CustomerName = "Strata",
				ProductCode = "1234"
			};

			IQueryable<ShoppingCart> ShoppingCarts = new List<ShoppingCart>() { expectedShoppingCart }.AsQueryable();

			Mock<DbSet<ShoppingCart>> mockSet = DbSetMocking.CreateMockSet(ShoppingCarts);

			mMockContext.Setup(c => c.ShoppingCarts).Returns(mockSet.Object);

			expectedShoppingCart.ProductCode = "123";

			// Act
			mRepository.Update(expectedShoppingCart);

			// Assert
			mMockContext.Verify(e => e.ModifyEntity(It.Is<ShoppingCart>(s => s.CustomerName == expectedShoppingCart.CustomerName
				&& s.ProductCode == "123")));
			mMockContext.Verify(s => s.SaveChanges(), Times.Once());
		}

		#endregion Repository method tests
	}
}