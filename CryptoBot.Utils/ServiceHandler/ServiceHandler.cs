using System.Collections.Generic;

namespace CryptoBot.Utils.ServiceHandler
{
    /// <summary>
    /// Helper class to manage services
    /// </summary>
    public sealed class ServiceHandler
    {
        private readonly IList<IManagedService> _managedServices = new List<IManagedService>();

        /// <summary>
        /// Start and manage a service
        /// </summary>
        /// <param name="service">Service to start and manage</param>
        /// <returns>The started service</returns>
        public T Start<T> (T service) where T : IManagedService
        {   
            _managedServices.Add(service);

            service.Start();

            return service;
        }

        /// <summary>
        /// Stop all managed services
        /// </summary>
        public void StopAll()
        {
            foreach (var service in _managedServices)
            {
                service.Stop();
            }

            _managedServices.Clear();
        }
    }
}
