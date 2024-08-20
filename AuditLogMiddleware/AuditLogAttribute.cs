namespace AuditLogMiddleware;

using System;

/// <summary>
/// 审计日志特性，用于标记需要记录审计日志的方法或类。
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public class AuditLogAttribute : Attribute
{
    /// <summary>
    /// 获取或设置审计日志的日志级别。
    /// </summary>
    public LogLevel LogLevel { get; set; } = LogLevel.Info;
    /// <summary>
    /// 指示是否记录请求体。
    /// 默认值为 true。
    /// </summary>
    public bool LogRequestBody { get; set; } = true;

    /// <summary>
    /// 指示是否记录响应体。
    /// 默认值为 true。
    /// </summary>
    public bool LogResponseBody { get; set; } = true;

   /* /// <summary>
    /// 指示是否记录用户信息。
    /// 默认值为 true。
    /// </summary>
    public bool LogUserInfo { get; set; } = true;

    /// <summary>
    /// 指示是否记录请求头。
    /// 默认值为 false。
    /// </summary>
    public bool LogRequestHeaders { get; set; } = false;

    /// <summary>
    /// 指示是否记录响应头。
    /// 默认值为 false。
    /// </summary>
    public bool LogResponseHeaders { get; set; } = false;*/

    /// <summary>
    /// 可选的自定义描述信息，用于标识日志的特定上下文或目的。
    /// </summary>
    public string? Description { get; set; }
}

