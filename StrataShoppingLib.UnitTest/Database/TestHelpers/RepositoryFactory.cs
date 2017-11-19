using Moq;
using Ninject;
using Ninject.Parameters;
using StrataShoppingLib.Components;
using StrataShoppingLib.Components.Database;

namespace StrataShoppingLib.UnitTest.Database.TestHelpers
{
	public static class RepositoryFactory
	{
		public static T RepositoryGenerator<T>(Mock<ShoppingContext> mockShoppingContext)
		{
			IKernel kernel = new StandardKernel(new ComponentsModule());
			ConstructorArgument mockContext = new ConstructorArgument("context", mockShoppingContext.Object);

			return kernel.Get<T>(mockContext);
		}
	}
}