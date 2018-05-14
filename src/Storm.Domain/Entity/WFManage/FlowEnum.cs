using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storm.Domain.Entity.WFManage
{
    /// <summary>
    /// 步骤类型
    /// </summary>
    public enum StepType
    {
        [Description("一人通过")]
        One = 1,
        [Description("所有通过")]
        All = 2
    }
    /// <summary>
    /// 驳回类型
    /// </summary>
    public enum RejectType
    {
        [Description("驳回申请人")]
        Reviewer = 1,
        [Description("驳回上一节点")]
        Last = 2,
        [Description("驳回指定节点")]
        Specified = 3,
        [Description("结束")]
        End = 4
    }
    /// <summary>
    /// 审核人类型
    /// </summary>
    public enum ReviewerType
    {
        [Description("岗位")]
        Post = 1,
        [Description("指定人")]
        Specified = 2,
        [Description("上一级领导")]
        Last = 3
    }
    /// <summary>
    /// 消息提醒类型
    /// </summary>
    public enum MessageType
    {
        [Description("系统提醒")]
        System = 1,
        [Description("邮件提醒")]
        Email = 2,
        [Description("短信提醒")]
        Msg = 3
    }
    /// <summary>
    /// 策略类型
    /// </summary>
    public enum StrategiesType
    {
        [Description("表单策略")]
        Form = 1,
        [Description("SQL脚本")]
        Sql = 2
    }
    /// <summary>
    /// 消息提醒类型
    /// </summary>
    public enum FormDefaultProgram
    {
        [Description("当前步骤用户ID")]
        StepUserID = 1,
        [Description("当前步骤用户姓名")]
        StepUserName = 2,
        [Description("当前步骤用户部门ID")]
        StepUserDeptID = 3,
        [Description("当前步骤用户部门名称")]
        StepUserDeptName = 4,
        [Description("流程申请人ID")]
        ApplyUserID = 5,
        [Description("流程申请人姓名")]
        ApplyUserName = 6,
        [Description("流程申请人部门ID")]
        ApplyUserDeptID = 7,
        [Description("流程申请人部门名称")]
        ApplyUserDeptName = 8,
        [Description("短日期格式(yyyy-MM-dd)")]
        ShortDate = 9,
        [Description("长日期格式(yyyy年MM月dd日)")]
        LongDate = 10,
        [Description("短时间格式(HH:mm)")]
        ShortDateTime = 11,
        [Description("长时间格式(HH时mm分)")]
        LongDateTime = 12,
        [Description("短日期时间格式(yyyy-MM-dd HH:mm)")]
        ShortDateAndDateTime = 13,
        [Description("长日期时间格式(yyyy年MM月dd日 HH时mm分)")]
        LongDateAndDateTime = 14,
        [Description("当前流程名称")]
        FlowName = 15,
        [Description("当前步骤名称")]
        StepName = 16
    }
    /// <summary>
    /// 自定义控件类型
    /// </summary>
    public enum FormCustomControlType
    {
        [Description("文本框")]
        Text = 1,
        [Description("文本域")]
        Textarea = 2,
        [Description("单选按钮组")]
        Redio = 3,
        [Description("复选按钮组")]
        CheckBox = 4,
        [Description("隐藏域")]
        Hidden = 5,
        [Description("Label标签")]
        Lable= 6,
        [Description("下拉列表框")]
        Select = 7,
        [Description("下拉组合框")]
        ComBox = 8,
        [Description("组织机构选择框")]
        Org = 9,
        [Description("日期选择")]
        Date = 10,
        [Description("日期时间选择")]
        DateTime = 11,
        [Description("附件上传")]
        Files = 12
    }

    /// <summary>
    /// 表单类型
    /// </summary>
    public enum FormType
    {
        [Description("自定义表单")]
        Custom = 1,
        //[Description("系统表单")]
        //System = 2
    }
    /// <summary>
    /// 申请状态
    /// </summary>
    public enum WorkStatus
    {
        [Description("已保存")]
        Save = 1,
        [Description("申请中")]
        Applying = 2,
        [Description("审核通过")]
        Success = 3,
        [Description("审核不通过")]
        Fail = 4,
        [Description("已撤回")]
        Retract = 5
    }
    /// <summary>
    /// 审核状态
    /// </summary>
    public enum ApprovalStatus
    {
        [Description("通过")]
        Pass = 1,
        [Description("不通过")]
        Fail = 2
    }
}
