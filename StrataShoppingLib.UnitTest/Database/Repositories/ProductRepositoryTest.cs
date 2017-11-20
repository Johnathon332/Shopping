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
	internal class ProductRepositoryTest
	{
		#region member variables

		private Mock<ShoppingContext> mMockContext; ///< Fake of the context
		private IProductRepository mRepository; ///< The repository under test

		#endregion member variables

		#region Setup

		/// <summary>
		/// One time setup for the class
		/// </summary>
		[SetUp]
		public void Setup()
		{
			mMockContext = new Mock<ShoppingContext>();

			mRepository = RepositoryFactory.RepositoryGenerator<IProductRepository>(mMockContext);
		}

		#endregion Setup

		#region Constructor Tests

		/// <summary>
		/// When no db context has been passed in it should throw an ArgumentNullException with
		/// the expected output message
		/// </summary>
		[Test]
		public void Constructor_PassNullContext_ShouldThrowArgumentNullException()
		{
			// Arrange
			string expectedExceptionMessage = "No context was passed in";

			// Assert
			Assert.That(() => new ProductRepository(null),
				Throws.TypeOf<ArgumentNullException>().With.Message.Contains(expectedExceptionMessage));
		}

		#endregion Constructor Tests

		#region Repository method tests

		/// <summary>
		/// When calling Get and there is no data in the datastore it should return an IEnumerable with a count
		/// of zero
		/// </summary>
		[Test]
		public void Get_CallingGetNoDataInDatastore_ShouldReturnAnIEnumerableWithCountOfZero()
		{
			// Arrange
			IQueryable<Product> Products = new List<Product>().AsQueryable();

			Mock<DbSet<Product>> mockSet = DbSetMocking.CreateMockSet(Products);

			mMockContext.Setup(c => c.Products).Returns(mockSet.Object);

			// Act
			IEnumerable<Product> result = mRepository.Get();

			// Assert
			Assert.That(result.Count, Is.EqualTo(0));
		}

		/// <summary>
		/// Get all Products from the datastore
		/// </summary>
		[Test]
		public void Get_CallingGet_ShouldReturnExpectedProduct()
		{
			// Arrange
			Product expectedProduct = new Product()
			{
				Code = "code",
				Description = "Food",
				UnitPrice = 12.5m
			};

			IQueryable<Product> Products = new List<Product>() { expectedProduct }.AsQueryable();

			Mock<DbSet<Product>> mockSet = DbSetMocking.CreateMockSet(Products);

			mMockContext.Setup(c => c.Products).Returns(mockSet.Object);

			// Act
			Product result = mRepository.Get().First();

			// Assert
			Assert.That(result.Code, Is.EqualTo(expectedProduct.Code));
			Assert.That(result.Description, Is.EqualTo(expectedProduct.Description));
			Assert.That(result.UnitPrice, Is.EqualTo(expectedProduct.UnitPrice));
		}

		/// <summary>
		/// Check that if nothing can be found in the datastore we return null
		/// </summary>
		[Test]
		public void GetByProductCode_PassIdentifierThatDoesNotExist_ShouldReturnNull()
		{
			// Arrange
			IQueryable<Product> Products = new List<Product>().AsQueryable();

			Mock<DbSet<Product>> mockSet = DbSetMocking.CreateMockSet(Products);

			mMockContext.Setup(c => c.Products).Returns(mockSet.Object);

			// Act
			Product result = mRepository.GetByProductCode("hello");

			// Assert
			Assert.That(result, Is.Null);
		}

		/// <summary>
		/// When passing null or empty string it should throw an ArgumentNullException
		/// </summary>
		/// <param name="idenfifier"></param>
		[TestCase("")]
		[TestCase(null)]
		public void GetByProductCode_PassInvalidIdentifier_ShouldThrowArgumentNullException(string identifier)
		{
			// Arrange
			string expectedMessage = "No identifier has been specified";

			// Assert
			Assert.That(() => mRepository.GetByProductCode(identifier),
				Throws.TypeOf<ArgumentNullException>().With.Message.Contains(expectedMessage));
		}

		/// <summary>
		/// Test when GetByUniqueIdentifier was called then we get a Product with the name we are expecting
		/// </summary>
		[Test]
		public void GetByProductCode_PassValidIdenfitier_ShouldReturnExpectedProductWithNameStrata()

		{
			// Arrange
			string expectedCode = "12";
			Product expectedProduct = new Product()
			{
				Code = expectedCode
			};

			IQueryable<Product> Products = new List<Product>() { expectedProduct, new Product() { Code = "Bob" } }.AsQueryable();

			Mock<DbSet<Product>> mockSet = DbSetMocking.CreateMockSet(Products);

			mMockContext.Setup(c => c.Products).Returns(mockSet.Object);

			// Act
			Product result = mRepository.GetByProductCode(expectedCode);

			// Assert
			Assert.That(result.Code, Is.EqualTo(expectedCode));
		}

		/// <summary>
		/// When passing a null Product to the remove method it should thwo ArgumentNullException with an expected message
		/// </summary>
		[Test]
		public void Remove_PassNullProduct_ShouldThrowArgumentNullException()
		{
			string expectedMessage = "No entity has been passed in";

			Assert.That(() => mRepository.Remove(null),
				Throws.TypeOf<ArgumentNullException>().With.Message.Contains(expectedMessage));
		}

		/// <summary>
		/// Verify that remove and SaveChanges is called with the passed in entity
		/// </summary>
		[Test]
		public void Remove_PassValidProduct_ShouldCallDeleteOnTheContextWithProduct()
		{
			// Arrange
			Product expectedProduct = new Product()
			{
				Code = "Strata",
			};

			mMockContext.Setup(r => r.Products.Remove(It.Is<Product>(s => s.Code == expectedProduct.Code)));
			mMockContext.Setup(s => s.SaveChanges());

			// Act
			mRepository.Remove(expectedProduct);

			// Assert
			mMockContext.Verify(c => c.Products.Remove(It.Is<Product>(s => s.Code == expectedProduct.Code)), Times.Once());
			mMockContext.Verify(s => s.SaveChanges(), Times.Once());
		}

		/// <summary>
		/// When passing a null Product to the Save method it should thwo ArgumentNullException with an expected message
		/// </summary>
		[Test]
		public void Save_PassNullProduct_ShouldThrowArgumentNullException()
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
		public void Save_PassValidProduct_ShouldCallSaveOnTheContextWithProduct()
		{
			// Arrange
			Product expectedProduct = new Product()
			{
				Code = "Strata",
			};

			mMockContext.Setup(r => r.Products.Add(It.Is<Product>(s => s.Code == expectedProduct.Code)));
			mMockContext.Setup(s => s.SaveChanges());

			// Act
			mRepository.Save(expectedProduct);

			// Assert
			mMockContext.Verify(c => c.Products.Add(It.Is<Product>(s => s.Code == expectedProduct.Code)), Times.Once());
			mMockContext.Verify(s => s.SaveChanges(), Times.Once());
		}

		/// <summary>
		/// WHen passing null Product to the update method in the repository it should throw an ArgumentNullException
		/// with the expected message
		/// </summary>
		[Test]
		public void Update_PassNullProduct_ShouldThrowArgumentNullException()
		{
			// Arrange
			string expectedMessage = "No entity has been passed in";

			// Assert
			Assert.That(() => mRepository.Update(null),
				Throws.TypeOf<ArgumentNullException>().With.Message.Contains(expectedMessage));
		}

		[Test]
		public void Update_PassValidProduct_ShouldCallUpdateAndSaveChanges()
		{
			// Arrange
			decimal expectedUnitPrice = 12.5m;
			Product expectedProduct = new Product()
			{
				Code = "Strata",
			};

			IQueryable<Product> Products = new List<Product>() { expectedProduct }.AsQueryable();

			Mock<DbSet<Product>> mockSet = DbSetMocking.CreateMockSet(Products);

			mMockContext.Setup(c => c.Products).Returns(mockSet.Object);

			expectedProduct.UnitPrice = expectedUnitPrice;

			// Act
			mRepository.Update(expectedProduct);

			// Assert
			mMockContext.Verify(e => e.ModifyEntity(It.Is<Product>(s => s.Code == expectedProduct.Code && s.UnitPrice == expectedUnitPrice)));
			mMockContext.Verify(s => s.SaveChanges(), Times.Once());
		}

		#endregion Repository method tests
	}
}