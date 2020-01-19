namespace RayWorkflow.Domain.Shared
{
    /// <summary>
    /// 操作类型，发起审批申请=1，重新提交审批=2，通过=3，拒绝=4，不同意=5，评论=6，撤销=7，转交=8
    /// </summary>
    public enum OperatorType
    {
        /// <summary>
        /// 发起审批申请
        /// </summary>
        StartApproval = 1,

        /// <summary>
        /// 重新提交审批
        /// </summary>
        ReStartApproval = 2,

        /// <summary>
        /// 通过
        /// </summary>
        Passed = 3,

        /// <summary>
        /// 拒绝
        /// </summary>
        Reject = 4,

        /// <summary>
        /// 不同意
        /// </summary>
        Disagree = 5,

        /// <summary>
        /// 评论
        /// </summary>
        Comment = 6,

        /// <summary>
        /// 撤销
        /// </summary>
        Revoke = 7,

        /// <summary>
        /// 转交
        /// </summary>
        Deliver = 8
    }
}
