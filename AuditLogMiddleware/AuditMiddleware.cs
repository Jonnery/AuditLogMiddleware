using AuditLogMiddleware.Entity;
using AuditLogMiddleware.MongoHelp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Features;
using MongoDB.Bson;
using System.Text;
using UAParser;

namespace AuditLogMiddleware;

/// <summary>
/// 审计中间件
/// </summary>
public class AuditMiddleware
{
    private readonly RequestDelegate _next;
    private readonly MongoDbContext _mongoDbContext;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="next">请求委托</param>
    /// <param name="mongoDbContext">MongoDB上下文</param>
    public AuditMiddleware(RequestDelegate next, MongoDbContext mongoDbContext)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _mongoDbContext = mongoDbContext ?? throw new ArgumentNullException(nameof(mongoDbContext));
    }

    /// <summary>
    /// 中间件方法
    /// </summary>
    /// <param name="context">HTTP上下文</param>
    /// <returns>异步任务</returns>
    public async Task Invoke(HttpContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
        var auditLogAttribute = endpoint?.Metadata.GetMetadata<AuditLogAttribute>();

        if (auditLogAttribute != null)
        {
            var auditLog = CreateAuditLog(context, auditLogAttribute);

            // 读取请求体
            if (auditLogAttribute.LogRequestBody)
            {
                await LogRequestBody(context, auditLog);
            }
                

            var originalBodyStream = context.Response.Body;

            // 读取响应体
            if (auditLogAttribute.LogResponseBody)
            {
                using (var responseBody = new MemoryStream())
                {
                    context.Response.Body = responseBody;

                    await _next(context);

                    await LogResponseBody(context, auditLog);

                    await responseBody.CopyToAsync(originalBodyStream);
                }
            }
            // 将审计日志写入 MongoDB
            await _mongoDbContext.AuditLogs.InsertOneAsync(auditLog);
        }
        else
        {
            // 不记录日志，直接处理请求
            await _next(context);
        }
    }

    /// <summary>
    /// 创建审计日志对象
    /// </summary>
    /// <param name="context">HTTP上下文</param>
    /// <returns>审计日志对象</returns>
    private AuditLog CreateAuditLog(HttpContext context, AuditLogAttribute auditLogAttribute)
    {
        var auditLog = new AuditLog(ObjectId.GenerateNewId().ToString())
        {
            UserId = context.User.Identity?.Name ?? "Anonymous",
            Action = context.Request.Method,
            Controller = context.Request.Path,
            ActionName = context.Request.Path.Value.Split('/').Length > 1 ? context.Request.Path.Value.Split('/')[1] : string.Empty,
            Timestamp = DateTime.UtcNow,
            IPAddress = context.Connection.RemoteIpAddress?.ToString(),
            UserAgent = context.Request.Headers["User-Agent"].ToString(),
            ReferrerUrl = context.Request.Headers["Referer"].ToString(),
            RequestUrl = context.Request.GetDisplayUrl(),
            LogLevel = auditLogAttribute.LogLevel,
            Description = auditLogAttribute.Description,
        };

        // 使用 UAParser 解析 User-Agent 信息
        var uaParser = Parser.GetDefault();
        var clientInfo = uaParser.Parse(auditLog.UserAgent);
        auditLog.OperatingSystem = clientInfo.OS.ToString();
        auditLog.DeviceType = clientInfo.Device.ToString();

        return auditLog;
    }

    /// <summary>
    /// 记录请求体
    /// </summary>
    /// <param name="context">HTTP上下文</param>
    /// <param name="auditLog">审计日志对象</param>
    /// <returns>异步任务</returns>
    private async Task LogRequestBody(HttpContext context, AuditLog auditLog)
    {
        context.Request.EnableBuffering();

        using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
        {
            auditLog.RequestBody = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;
        }
    }

    /// <summary>
    /// 记录响应体
    /// </summary>
    /// <param name="context">HTTP上下文</param>
    /// <param name="auditLog">审计日志对象</param>
    /// <returns>异步任务</returns>
    private async Task LogResponseBody(HttpContext context, AuditLog auditLog)
    {
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        auditLog.ResponseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
        context.Response.Body.Seek(0, SeekOrigin.Begin);
    }
}
