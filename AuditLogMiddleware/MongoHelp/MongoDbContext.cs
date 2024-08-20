using AuditLogMiddleware.Entity;
using MongoDB.Driver;

namespace AuditLogMiddleware.MongoHelp;

/// <summary>
/// MongoDB数据库上下文
/// </summary>
public class MongoDbContext
{
    /// <summary>
    /// MongoDB数据库
    /// </summary>
    private readonly IMongoDatabase _database;
    /// <summary>
    /// 审计日志集合名称
    /// </summary>
    private readonly string AuditLogName = "AuditLogs";

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="connectionString">链接字符串</param>
    /// <param name="databaseName">数据库名称</param>
    /// <param name="auditLogName">审计日志集合名称</param>
    public MongoDbContext(string connectionString, string databaseName, string auditLogName = "AuditLogs")
    {
        var client = new MongoClient(connectionString);
        AuditLogName = auditLogName;
        _database = client.GetDatabase(databaseName);
    }
    /// <summary>
    /// 获取审计日志集合
    /// </summary>
    public IMongoCollection<AuditLog> AuditLogs => _database.GetCollection<AuditLog>(AuditLogName);
}
