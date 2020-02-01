using System;
using System.ComponentModel.DataAnnotations;

namespace RayWorkflow.Domain.Shared.Workflow
{
    [Serializable]
    public class WorkflowFormDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 表单名称
        /// </summary>
        [MaxLength(128)]
        public string Name { get; set; }

        /// <summary>
        /// 字段个数
        /// </summary>
        public int Fields { get; set; }

        /// <summary>
        /// 表单中的控件列表Json
        /// </summary>
        public string FormControlsJson { get; set; }

        /// <summary>
        /// 排序码
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Description { get; set; }

        public virtual DateTime? LastModificationTime { get; set; }

        public virtual Guid? LastModifierId { get; set; }

        public virtual DateTime CreationTime { get; set; }

        public virtual Guid? CreatorId { get; set; }

    }
}
