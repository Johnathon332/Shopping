using Moq;
using System.Data.Entity;
using System.Linq;

namespace StrataShoppingLib.UnitTest.Database.TestHelpers
{
	/// <summary>
	/// Helper class to generate dbset mocks to mock out our
	/// db context
	/// </summary>
	public static class DbSetMocking
	{
		/// <summary>
		/// Generates DbSet mocks
		/// </summary>
		/// <typeparam name="T">Entity type of the db set</typeparam>
		/// <param name="data">The data we want to store in the dbset</param>
		/// <returns>A mocked DbSet</returns>
		public static Mock<DbSet<T>> CreateMockSet<T>(IQueryable<T> data) where T : class
		{
			IQueryable<T> queryableData = data.AsQueryable();
			Mock<DbSet<T>> mockSet = new Mock<DbSet<T>>();

			mockSet.As<IQueryable<T>>().Setup(m => m.Provider)
				.Returns(queryableData.Provider);
			mockSet.As<IQueryable<T>>().Setup(m => m.Expression)
					.Returns(queryableData.Expression);
			mockSet.As<IQueryable<T>>().Setup(m => m.ElementType)
					.Returns(queryableData.ElementType);
			mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator())
					.Returns(queryableData.GetEnumerator());
			return mockSet;
		}
	}
}