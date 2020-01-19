using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace RayWorkflow.Domain.Workflow
{
    /// <summary>
    /// 工作流表单模板
    /// </summary>
    [Serializable]
    public class WorkflowForm : AuditedEntity<Guid>
    {
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
        public int SortCode { get; set; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        public int Disabled { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(512)]
        public string Description { get; set; }
    }
}