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
	/// <summary>
	/// Test class for OrderRepository
	/// </summary>
	[TestFixture]
	public class OrderRepositoryTest
	{
		#region member variables

		private Mock<ShoppingContext> mMockContext; ///< Fake of the context
		private IOrderRepository mRepository; ///< The repository under test

		#endregion member variables

		#region Setup

		/// <summary>
		/// One time setup for the class
		/// </summary>
		[SetUp]
		public void Setup()
		{
			mMockContext = new Mock<ShoppingContext>();

			mRepository = RepositoryFactory.RepositoryGenerator<IOrderRepository>(mMockContext);
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
			Assert.That(() => new OrderRepository(null),
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
			IQueryable<Order> orders = new List<Order>().AsQueryable();

			Mock<DbSet<Order>> mockSet = DbSetMocking.CreateMockSet(orders);

			mMockContext.Setup(c => c.Orders).Returns(mockSet.Object);

			// Act
			IEnumerable<Order> result = mRepository.Get();

			// Assert
			Assert.That(result.Count, Is.EqualTo(0));
		}

		/// <summary>
		/// Get all Orders from the datastore
		/// </summary>
		[Test]
		public void Get_CallingGet_ShouldReturnExpectedOrder()
		{
			// Arrange
			Guid expectedId = new Guid();
			Order expectedOrder = new Order()
			{
				Id = expectedId,
			};

			IQueryable<Order> Orders = new List<Order>() { expectedOrder }.AsQueryable();

			Mock<DbSet<Order>> mockSet = DbSetMocking.CreateMockSet(Orders);

			mMockContext.Setup(c => c.Orders).Returns(mockSet.Object);

			// Act
			Order result = mRepository.Get().First();

			// Assert
			Assert.That(result.Id, Is.EqualTo(expectedOrder.Id));
		}

		/// <summary>
		/// Check that if nothing can be found in the datastore we return null
		/// </summary>
		[Test]
		public void GetById_PassIdThatDoesNotExist_ShouldReturnNull()
		{
			// Arrange
			IQueryable<Order> Orders = new List<Order>().AsQueryable();

			Mock<DbSet<Order>> mockSet = DbSetMocking.CreateMockSet(Orders);

			mMockContext.Setup(c => c.Orders).Returns(mockSet.Object);

			// Act
			Order result = mRepository.GetById(new Guid());

			// Assert
			Assert.That(result, Is.Null);
		}

		/// <summary>
		/// Test when GetById was called then we get a Order with the name we are expecting
		/// </summary>
		[Test]
		public void GetByUniqueIdentifier_PassValidIdenfitier_ShouldReturnExpectedOrderWithNameStrata()
		{
			// Arrange
			Guid expectedId = new Guid();
			Order expectedOrder = new Order()
			{
				Id = expectedId
			};

			IQueryable<Order> Orders = new List<Order>() { expectedOrder, new Order() { Id = new Guid() } }.AsQueryable();

			Mock<DbSet<Order>> mockSet = DbSetMocking.CreateMockSet(Orders);

			mMockContext.Setup(c => c.Orders).Returns(mockSet.Object);

			// Act
			Order result = mRepository.GetById(expectedId);

			// Assert
			Assert.That(result.Id, Is.EqualTo(expectedOrder.Id));
		}

		/// <summary>
		/// When passing a null Order to the remove method it should thwo ArgumentNullException with an expected message
		/// </summary>
		[Test]
		public void Remove_PassNullOrder_ShouldThrowArgumentNullException()
		{
			string expectedMessage = "No entity has been passed in";

			Assert.That(() => mRepository.Remove(null),
				Throws.TypeOf<ArgumentNullException>().With.Message.Contains(expectedMessage));
		}

		/// <summary>
		/// Verify that remove and SaveChanges is called with the passed in entity
		/// </summary>
		[Test]
		public void Remove_PassValidOrder_ShouldCallDeleteOnTheContextWithOrder()
		{
			// Arrange
			Guid expectedId = new Guid();
			Order expectedOrder = new Order()
			{
				Id = expectedId
			};

			mMockContext.Setup(r => r.Orders.Remove(It.Is<Order>(s => s.Id == expectedOrder.Id)));
			mMockContext.Setup(s => s.SaveChanges());

			// Act
			mRepository.Remove(expectedOrder);

			// Assert
			mMockContext.Verify(c => c.Orders.Remove(It.Is<Order>(s => s.Id == expectedOrder.Id)), Times.Once());
			mMockContext.Verify(s => s.SaveChanges(), Times.Once());
		}

		/// <summary>
		/// When passing a null Order to the Save method it should thwo ArgumentNullException with an expected message
		/// </summary>
		[Test]
		public void Save_PassNullOrder_ShouldThrowArgumentNullException()
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
		public void Save_PassValidOrder_ShouldCallSaveOnTheContextWithOrder()
		{
			// Arrange
			Guid expectedId = new Guid();
			Order expectedOrder = new Order()
			{
				Id = expectedId
			};

			mMockContext.Setup(r => r.Orders.Add(It.Is<Order>(s => s.Id == expectedOrder.Id)));
			mMockContext.Setup(s => s.SaveChanges());

			// Act
			mRepository.Save(expectedOrder);

			// Assert
			mMockContext.Verify(c => c.Orders.Add(It.Is<Order>(s => s.Id == expectedOrder.Id)), Times.Once());
			mMockContext.Verify(s => s.SaveChanges(), Times.Once());
		}

		/// <summary>
		/// WHen passing null Order to the update method in the repository it should throw an ArgumentNullException
		/// with the expected message
		/// </summary>
		[Test]
		public void Update_PassNullOrder_ShouldThrowArgumentNullException()
		{
			// Arrange
			string expectedMessage = "No entity has been passed in";

			// Assert
			Assert.That(() => mRepository.Update(null),
				Throws.TypeOf<ArgumentNullException>().With.Message.Contains(expectedMessage));
		}

		[Test]
		public void Update_PassValidOrder_ShouldCallUpdateAndSaveChanges()
		{
			// Arrange
			Guid expectedId = new Guid();
			string expectedCustomerName = "Strata";
			Order expectedOrder = new Order()
			{
				Id = expectedId
			};

			IQueryable<Order> Orders = new List<Order>() { expectedOrder }.AsQueryable();

			Mock<DbSet<Order>> mockSet = DbSetMocking.CreateMockSet(Orders);

			mMockContext.Setup(c => c.Orders).Returns(mockSet.Object);

			expectedOrder.CustomerName = "Strata";

			// Act
			mRepository.Update(expectedOrder);

			// Assert
			mMockContext.Verify(e => e.ModifyEntity(It.Is<Order>(s => s.Id == expectedOrder.Id && s.CustomerName == expectedCustomerName)));
			mMockContext.Verify(s => s.SaveChanges(), Times.Once());
		}

		#endregion Repository method tests
	}
}