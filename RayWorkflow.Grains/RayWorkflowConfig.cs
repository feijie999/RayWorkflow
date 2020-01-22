using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Ray.Core.Abstractions;

namespace RayWorkflow.Grains
{
    public class RayWorkflowConfig : IStartupConfig
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            throw new NotImplementedException();
        }

        public Task ConfigureObserverUnit(IServiceProvider serviceProvider, IObserverUnitContainer followUnitContainer)
        {
            return Task.CompletedTask;
        }
    }
}
