﻿using System;
using RayWorkflow.Domain.Shared.Workflow;
using RayWorkflow.Domain.Workflow;
using RayWorkflow.Grains.States;
using RayWorkflow.IGrains;
using Ray.Core;
using Ray.Core.Observer;
using Microsoft.Extensions.DependencyInjection;
using Ray.Core.Event;
using RayWorkflow.Domain;
using RayWorkflow.IGrains.Events;
using System.Threading.Tasks;

namespace RayWorkflow.Grains
{
    [Observer(DefaultObserverGroup.primary, "db", typeof(WorkflowFormGrain))]
    public class WorkflowFormDbGrain : CrudDbGrain<WorkflowFormGrain, WorkflowFormState, Guid, WorkflowForm>, IWorkflowFormDbGrain
    {
        public async Task EventHandle(EnabledEvent evt)
        {
            using var repository = ServiceProvider.GetService<IGrainRepository<WorkflowForm, Guid>>();
            var form = await repository.FirstOrDefaultAsync(evt.Id);
            WorkflowFormHandler.EventHandle(form, evt);
            await repository.CommitAsync();
        }

        public async Task EventHandle(DisabledEvent evt)
        {
            using var repository = ServiceProvider.GetService<IGrainRepository<WorkflowForm, Guid>>();
            var form = await repository.FirstOrDefaultAsync(evt.Id);
            WorkflowFormHandler.EventHandle(form, evt);
            await repository.CommitAsync();
        }
    }
}
