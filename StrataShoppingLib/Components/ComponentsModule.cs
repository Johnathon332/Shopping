﻿using Ninject.Modules;
using Ninject.Web.Common;
using StrataShoppingLib.Components.Database;
using StrataShoppingLib.Components.Database.Repositories;

namespace StrataShoppingLib.Components
{
	public class ComponentsModule : NinjectModule
	{
		/// <summary>
		/// Loads ninject bindings
		/// </summary>
		public override void Load()
		{
			Bind<ShoppingContext>().ToSelf().InRequestScope();
			Bind<ICustomerRepository>().To<CustomerRepository>().InRequestScope();
			Bind<IShoppingCartRepository>().To<ShoppingCartRepository>().InRequestScope();
			Bind<IProductRepository>().To<ProductRepository>().InRequestScope();
			Bind<IOrderRepository>().To<OrderRepository>().InRequestScope();
		}
	}
}