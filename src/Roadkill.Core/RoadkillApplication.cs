using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System;
using Roadkill.Core.Files;
using StructureMap;
using Roadkill.Core.Configuration;
using Roadkill.Core.Converters;

namespace Roadkill.Core
{
	/// <summary>
	/// The entry point application (Global.asax) for Roadkill.
	/// </summary>
	public class RoadkillApplication : HttpApplication
	{
		protected void Application_Start()
		{
			IoCConfigurator.Setup();
			AttachmentRouteHandler.Register(ObjectFactory.GetInstance<IConfigurationContainer>());

			AreaRegistration.RegisterAllAreas();
			RegisterRoutes(RouteTable.Routes);

			// MVC Object factories for view models that require IOC, and all controllers
			ModelBinders.Binders.Add(typeof(UserSummary),new UserSummaryModelBinder());
			ModelBinders.Binders.Add(typeof(SettingsSummary),new SettingsSummaryModelBinder());
			ControllerBuilder.Current.SetControllerFactory(new StructureMapControllerFactory());
		}

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			// For the jQuery ajax file manager
			routes.MapLowercaseRoute(
				"FileFolder",
				"Files/Folder/{dir}",
				new { controller = "Files", action = "Folder", dir = UrlParameter.Optional }
			);

			// The default way of getting to a page: "/wiki/123/page-title"
			routes.MapLowercaseRoute(
				"Wiki",
				"Wiki/{id}/{title}",
				new { controller = "Wiki", action = "Index", title = UrlParameter.Optional }
			);

			// Don't lowercase pages that use Base64
			routes.MapRoute(
				"Pages",
				"pages/byuser/{id}/{encoded}",
				new { controller = "Pages", action = "ByUser", title = UrlParameter.Optional }
			);

			// Default
			routes.MapLowercaseRoute(
				"Default", // Route name
				"{controller}/{action}/{id}", // URL with parameters
				new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
			);
		}
	}
}