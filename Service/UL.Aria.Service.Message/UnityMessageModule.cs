using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Message.Common;
using UL.Aria.Service.Message.Implementation;
using UL.Aria.Service.Message.Provider;
using UL.Aria.Service.Message.Repository;
using UL.Enterprise.Foundation.Logging;
using UL.Enterprise.Foundation.Mapper;
using UL.Enterprise.Foundation.Service.Logging;
using UL.Enterprise.Foundation.Unity;

namespace UL.Aria.Service.Message
{
    ///// <summary>
    ///// Provides unity registartion for all types, and is configured to
    ///// be used by the UnityModuleManager for Automatic boot loading.
    ///// </summary>
    //[UnityBootStrap(BootOrder = 0)]
    //public class UnityMessageModule : IUnityModule
    //{
    //    /// <summary>
    //    /// Registers the specified unity container.
    //    /// </summary>
    //    /// <param name="unityContainer">The unity container.</param>
    //    public void Register(Microsoft.Practices.Unity.IUnityContainer unityContainer)
    //    {
    //        //force registrations
    //        new Implementation.ServiceMapperRegistry();
    //        unityContainer
    //           .RegisterType<IOrderMessageRepository, OrderMessageRepository>(new ContainerControlledLifetimeManager())
    //           .RegisterType<IOrderMessageProvider, OrderMessageProvider>(new ContainerControlledLifetimeManager())
    //           .RegisterType<ILogManager, LogManager>(new ContainerControlledLifetimeManager())
    //           .RegisterType<IMapperRegistry, Implementation.ServiceMapperRegistry>(new ContainerControlledLifetimeManager())
    //           .RegisterType<ILogCategoryResolver, LogCategoryResolver>(new ContainerControlledLifetimeManager())
    //           .RegisterType<IOperationBehavior, LoggingOperationBehavior>(typeof(LoggingOperationBehavior).FullName, new ContainerControlledLifetimeManager())
    //           .RegisterType<IOrderMessageService, OrderMessageService>(new ContainerControlledLifetimeManager());
    //    }
    //}
}
