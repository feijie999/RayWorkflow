using AutoMapper;
using RayWorkflow.Domain.Workflow;
using RayWorkflow.Grains.States;
using RayWorkflow.Domain.Shared.Workflow;

namespace RayWorkflow.Grains
{
    public class GrainDtoMapper : Profile
    {
        public GrainDtoMapper()
        {
            CreateMap<WorkflowForm, WorkflowFormState>().ReverseMap();
            CreateMap<WorkflowFormDto, WorkflowFormState>().ReverseMap();
        }
    }
}