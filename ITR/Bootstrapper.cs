using System.Web.Mvc;
using Microsoft.Practices.Unity;
using ITR.DataRepository;
using Microsoft.Practices.Unity.Mvc;
namespace ITR
{
    /// <summary>
    /// Created By:Vishnu
    /// Created On: 23-Aug-2016
    /// Used as Part of Dependency resolver
    /// </summary>
  public static class Bootstrapper
  {
    public static IUnityContainer Initialise()
    {
      var container = BuildUnityContainer();

      DependencyResolver.SetResolver(new UnityDependencyResolver(container));

      return container;
    }

    private static IUnityContainer BuildUnityContainer()
    {
      var container = new UnityContainer();

      // register all your components with the container here
      // it is NOT necessary to register your controllers

      // e.g. container.RegisterType<ITestService, TestService>();    
      RegisterTypes(container);

      return container;
    }

    public static void RegisterTypes(IUnityContainer container)
    {
        container.RegisterType<IHomeRepository, DataRepository.HomeRepository>();
        container.RegisterType<IDatacastHomeRepository, DataRepository.DatacastHomeRepository>();
        container.RegisterType<DatacastDBWidgetIDataRepository, DataRepository.DatacastDBWidgetDataRepository>();
    }
  }
}