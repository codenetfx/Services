using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Update.Managers;

namespace UL.Aria.Service.Update.Factory
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdateManagerFactory:IUpdateManagerFactory
    {
        private readonly IUnityContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateManagerFactory"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public UpdateManagerFactory(IUnityContainer container)
        {
            this._container = container;
        }

        /// <summary>
        /// Gets the manager.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IUpdateManager GetManager(Contracts.Dto.EntityTypeEnumDto entityType)
        {
            return this._container.Resolve<IUpdateManager>(entityType.ToString());
        }
    }
}
