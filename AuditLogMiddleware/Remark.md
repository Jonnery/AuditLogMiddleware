## AuditLogMiddleware(审计中间件)
### 注册方法
public void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton(new MongoDbContext("your-mongodb-connection-string", "your-database-name"));
    // 
}

public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    app.UseMiddleware<AuditMiddleware>();
    // 其他中间件
}
### 使用方法
[AuditLog(LogLevel.Info, IncludeRequestBody = true, IncludeResponseBody = false)]
public class SampleController : ControllerBase
{
    [HttpGet]
    public IActionResult GetData()
    {
        // 控制器逻辑
        return Ok("数据已获取");
    }

    [HttpPost]
    [AuditLog(LogLevel.Error, IncludeRequestBody = true)]
    public IActionResult SaveData([FromBody] string data)
    {
        // 控制器逻辑
        return Ok("数据已保存");
    }
}

