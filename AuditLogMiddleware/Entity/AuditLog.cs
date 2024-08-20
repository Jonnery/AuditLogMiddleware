using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AuditLogMiddleware.Entity;

/// <summary>
/// 审计日志实体类
/// </summary>
public class AuditLog
{
    /// <summary>
    /// 审计日志实体类的唯一标识
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="id"></param>
    public AuditLog(string id)
    {
        Id = id;
    }

    /// <summary>
    /// 用户ID
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// 请求方式（例如 GET、POST）
    /// </summary>
    public string? Action { get; set; }

    /// <summary>
    /// 控制器名称
    /// </summary>
    public string? Controller { get; set; }

    /// <summary>
    /// 操作名称（方法名称）
    /// </summary>
    public string? ActionName { get; set; }

    /// <summary>
    /// 请求时间
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// 请求来源IP地址
    /// </summary>
    public string? IPAddress { get; set; }

    /// <summary>
    /// 请求体
    /// </summary>
    public string? RequestBody { get; set; }

    /// <summary>
    /// 响应体
    /// </summary>
    public string? ResponseBody { get; set; }

    /// <summary>
    /// 日志级别
    /// </summary>
    public LogLevel LogLevel { get; set; }

    /// <summary>
    /// 可选的自定义描述信息
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 客户端浏览器信息
    /// </summary>
    public string? UserAgent { get; set; }

    /// <summary>
    /// 操作系统信息
    /// </summary>
    public string? OperatingSystem { get; set; }

    /// <summary>
    /// 设备类型（例如 PC、Mobile）
    /// </summary>
    public string? DeviceType { get; set; }

    /// <summary>
    /// 引荐来源URL
    /// </summary>
    public string? ReferrerUrl { get; set; }

    /// <summary>
    /// 请求的完整URL
    /// </summary>
    public string? RequestUrl { get; set; }
}
