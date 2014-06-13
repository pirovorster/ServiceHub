[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(ServiceHub.Website.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(ServiceHub.Website.App_Start.NinjectWebCommon), "Stop")]

namespace ServiceHub.Website.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
	using ServiceHub.Model;
	using Microsoft.AspNet.Identity;
    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {

			kernel.Bind<INotificationService>().To<NotificationService>().InRequestScope();

			kernel.Bind<ServiceHubEntities>().To<ServiceHubEntities>().InRequestScope();

			kernel.Bind<ServiceProviderService>().To<ServiceProviderService>().InRequestScope().WithConstructorArgument("aspNetUserId",(context,o)=> HttpContext.Current.User.Identity.GetUserId());

			kernel.Bind<ClientService>().To<ClientService>().InRequestScope().WithConstructorArgument("aspNetUserId", (context, o) => HttpContext.Current.User.Identity.GetUserId());

			kernel.Bind<UserProfileService>().To<UserProfileService>().InRequestScope().WithConstructorArgument("aspNetUserId", (context, o) => HttpContext.Current.User.Identity.GetUserId());

			kernel.Bind<LookupService>().To<LookupService>().InRequestScope();

			kernel.Bind<DirectoryService>().To<DirectoryService>().InRequestScope();
        }        
    }
}
