using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace AVSSalesExplorer
{
    public class DependencyResolver
    {
        private IHost _host;
        private DependencyResolver() { }
        public static DependencyResolver Instance { get; } = new DependencyResolver();

        public void Init(IHost host)
        {
            if (host == null)
            {
                throw new ArgumentNullException(nameof(host));
            }

            _host = host;
        }

        public T GetRequiredService<T>() where T: class
        {            
            if (_host == null)
            {
                throw new InvalidOperationException("[DependencyResolver] Run 'Init' method first.");
            }

            return _host.Services.GetRequiredService<T>();
        }
    }
}