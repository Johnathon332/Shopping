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
	/// Test class for the customer repository
	/// </summary>
	[TestFixture]
	public class CustomerRepositoryTest
	{
		#region member variables

		private Mock<ShoppingContext> mMockContext; ///< Fake of the context
		private ICustomerRepository mRepository; ///< The repository under test

		#endregion member variables

		#region Setup

		/// <summary>
		/// One time setup for the class
		/// </summary>
		[SetUp]
		public void Setup()
		{
			mMockContext = new Mock<ShoppingContext>();

			mRepository = RepositoryFactory.RepositoryGenerator<ICustomerRepository>(mMockContext);
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
			string expectedExceptionMessage = "No context was passed in";
			Assert.That(() => new CustomerRepository(null),
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
			IQueryable<Customer> customers = new List<Customer>().AsQueryable();

			Mock<DbSet<Customer>> mockSet = DbSetMocking.CreateMockSet(customers);

			mMockContext.Setup(c => c.Customers).Returns(mockSet.Object);

			// Act
			IEnumerable<Customer> result = mRepository.Get();

			// Assert
			Assert.That(result.Count(), Is.EqualTo(0));
		}

		/// <summary>
		/// Get all customers from the datastore
		/// </summary>
		[Test]
		public void Get_CallingGet_ShouldReturnExpectedCustomer()
		{
			// Arrange
			Customer expectedCustomer = new Customer()
			{
				Name = "Strata",
				Address = "2 Strata street",
				Email = "Strata@strata.com",
				Type = MemberType.None
			};

			IQueryable<Customer> customers = new List<Customer>() { expectedCustomer }.AsQueryable();

			Mock<DbSet<Customer>> mockSet = DbSetMocking.CreateMockSet(customers);

			mMockContext.Setup(c => c.Customers).Returns(mockSet.Object);

			// Act
			Customer result = mRepository.Get().First();

			// Assert
			Assert.That(result.Name, Is.EqualTo(expectedCustomer.Name));
			Assert.That(result.Address, Is.EqualTo(expectedCustomer.Address));
			Assert.That(result.Email, Is.EqualTo(expectedCustomer.Email));
			Assert.That(result.Type, Is.EqualTo(expectedCustomer.Type));
		}

		/// <summary>
		/// Check that if nothing can be found in the datastore we return null
		/// </summary>
		[Test]
		public void GetUniqueIdentifier_PassIdentifierThatDoesNotExist_ShouldReturnNull()
		{
			// Arrange
			IQueryable<Customer> customers = new List<Customer>().AsQueryable();

			Mock<DbSet<Customer>> mockSet = DbSetMocking.CreateMockSet(customers);

			mMockContext.Setup(c => c.Customers).Returns(mockSet.Object);

			// Act
			Customer result = mRepository.GetByUniqueIdentifier("");

			// Assert
			Assert.That(result, Is.Null);
		}

		/// <summary>
		/// Test when GetByUniqueIdentifier was called then we get a customer with the name we are expecting
		/// </summary>
		[Test]
		public void GetByUniqueIdentifier_PassValidIdenfitier_ShouldReturnExpectedCustomerWithNameStrata()

		{
			// Arrange
			string expectedName = "Strata";
			Customer expectedCustomer = new Customer()
			{
				Name = expectedName
			};

			IQueryable<Customer> customers = new List<Customer>() { expectedCustomer, new Customer() { Name = "Bob" } }.AsQueryable();

			Mock<DbSet<Customer>> mockSet = DbSetMocking.CreateMockSet(customers);

			mMockContext.Setup(c => c.Customers).Returns(mockSet.Object);

			// Act
			Customer result = mRepository.GetByUniqueIdentifier(expectedName);

			// Assert
			Assert.That(result.Name, Is.EqualTo(expectedCustomer.Name));
		}

		/// <summary>
		/// When passing a null customer to the remove method it should thwo ArgumentNullException with an expected message
		/// </summary>
		[Test]
		public void Remove_PassNullCustomer_ShouldThrowArgumentNullException()
		{
			string expectedMessage = "No entity has been passed in";

			Assert.That(() => mRepository.Remove(null),
				Throws.TypeOf<ArgumentNullException>().With.Message.Contains(expectedMessage));
		}

		/// <summary>
		/// Verify that remove and SaveChanges is called with the passed in entity
		/// </summary>
		[Test]
		public void Remove_PassValidCustomer_ShouldCallDeleteOnTheContextWithCustomer()
		{
			// Arrange
			Customer expectedCustomer = new Customer()
			{
				Name = "Strata",
			};

			mMockContext.Setup(r => r.Customers.Remove(It.Is<Customer>(s => s.Name == expectedCustomer.Name)));
			mMockContext.Setup(s => s.SaveChanges());

			// Act
			mRepository.Remove(expectedCustomer);

			// Assert
			mMockContext.Verify(c => c.Customers.Remove(It.Is<Customer>(s => s.Name == expectedCustomer.Name)), Times.Once());
			mMockContext.Verify(s => s.SaveChanges(), Times.Once());
		}

		/// <summary>
		/// When passing a null customer to the Save method it should thwo ArgumentNullException with an expected message
		/// </summary>
		[Test]
		public void Save_PassNullCustomer_ShouldThrowArgumentNullException()
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
		public void Save_PassValidCustomer_ShouldCallSaveOnTheContextWithCustomer()
		{
			// Arrange
			Customer expectedCustomer = new Customer()
			{
				Name = "Strata",
			};

			mMockContext.Setup(r => r.Customers.Add(It.Is<Customer>(s => s.Name == expectedCustomer.Name)));
			mMockContext.Setup(s => s.SaveChanges());

			// Act
			mRepository.Save(expectedCustomer);

			// Assert
			mMockContext.Verify(c => c.Customers.Add(It.Is<Customer>(s => s.Name == expectedCustomer.Name)), Times.Once());
			mMockContext.Verify(s => s.SaveChanges(), Times.Once());
		}

		/// <summary>
		/// WHen passing null customer to the update method in the repository it should throw an ArgumentNullException
		/// with the expected message
		/// </summary>
		[Test]
		public void Update_PassNullCustomer_ShouldThrowArgumentNullException()
		{
			// Arrange
			string expectedMessage = "No entity has been passed in";

			// Assert
			Assert.That(() => mRepository.Update(null),
				Throws.TypeOf<ArgumentNullException>().With.Message.Contains(expectedMessage));
		}

		[Test]
		public void Update_PassValidCustomer_ShouldCallUpdateAndSaveChanges()
		{
			// Arrange
			Customer expectedCustomer = new Customer()
			{
				Name = "Strata",
			};

			IQueryable<Customer> customers = new List<Customer>() { expectedCustomer }.AsQueryable();

			Mock<DbSet<Customer>> mockSet = DbSetMocking.CreateMockSet(customers);

			mMockContext.Setup(c => c.Customers).Returns(mockSet.Object);

			expectedCustomer.Address = "Hello";

			// Act
			mRepository.Update(expectedCustomer);

			// Assert
			mMockContext.Verify(e => e.ModifyEntity(It.Is<Customer>(s => s.Name == expectedCustomer.Name && s.Address == "Hello")));
			mMockContext.Verify(s => s.SaveChanges(), Times.Once());
		}

		#endregion Repository method tests
	}
}