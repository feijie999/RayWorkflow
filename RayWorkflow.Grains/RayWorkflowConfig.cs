using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Ray.Core.Abstractions;
using Ray.Core.Storage;
using Ray.Storage.PostgreSQL;
using Ray.Storage.SQLCore.Configuration;

namespace RayWorkflow.Grains
{
    public class RayWorkflowConfig : IStartupConfig
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IConfigureBuilder<Guid, WorkflowFormGrain>>(new PSQLConfigureBuilder<Guid, WorkflowFormGrain>((provider, id, parameter) =>
                new GuidKeyOptions(provider, "core_event", "WorkflowForm")).AutoRegistrationObserver());
            serviceCollection.AddTransient<IWorkflowFormHandler, WorkflowFormHandler>();
        }

        public Task ConfigureObserverUnit(IServiceProvider serviceProvider, IObserverUnitContainer followUnitContainer)
        {
            return Task.CompletedTask;
        }
    }
}
