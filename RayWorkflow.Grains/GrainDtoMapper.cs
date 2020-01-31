using AutoMapper;
using RayWorkflow.Domain.Workflow;
using RayWorkflow.Grains.States;

namespace RayWorkflow.Grains
{
    public class GrainDtoMapper : Profile
    {
        public GrainDtoMapper()
        {
            CreateMap<WorkflowForm, WorkflowFormState>().ReverseMap();
        }
    }
}