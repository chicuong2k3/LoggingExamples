# Catalogue of Logging

- Log Category
- Structured Logging
	- Message Template
	- Log Entry Parameters
- Log Level
- Log Event Id
- Log Configuration
- Packages
	- Microsoft.Extensions.Logging -> The actual implementation of logging
	- Microsoft.Extensions.Logging.Abstractions
	- Microsoft.Extensions.Logging.Console -> The logging provider
- Host and non-host applications
	- Microsoft.Extensions.Hosting
	- Microsoft.Extensions.Hosting.Abstractions
- Customizing the loggers and providers
- Log Filters

```c#
using IHost host = Host.CreateDefaultBuilder(args)
	.ConfigureLogging(logging =>
	{
		logging.ClearProviders();
		logging.AddJsonConsole(options => 
		{
			options.IncludeScopes = false;
			options.TimestampFormat = "HH:mm:ss";
			options.JsonWriterOptions = new JsonWriterOptions
			{
				Indented = true
			};
		});
		logging.SetMinimumLevel(LogLevel.Debug);
		logging.AddFilter("Microsoft", LogLevel.Warning);
		logging.AddFilter(x => x >= LogLevel.Warning);

		logging.AddFilter((provider, category, logLevel) => 
		{
			return provider.Contains("Console") 
				&& category.Contains("Microsoft")
				&& logLevel == LogLevel.Debug;
		});

		logging.AddFilter<ConsoleLoggerProvider>((provider, category, logLevel) => 
		{
			return provider.Contains("Console") 
				&& category.Contains("Microsoft")
				&& logLevel == LogLevel.Debug;
		});
	})
	.Build();

var logger = host.Services.GetRequiredService<ILogger<Program>>();

host.Run();
```

- Log Category Configuration

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
	  "Microsoft.Hosting.Lifetime": "Warning"
    },
	// override the configuration for the Console provider
	"Console": {
		"LogLevel": {
		  "Default": "Information",
		  "Microsoft.AspNetCore": "Information"
		},
		"FormatName": "json",
		"FormatterOptions": {
			"SingleLine": false,
			"IncludeScopes": false,
			"TimestampFormat": "HH:mm:ss",
			"JsonWriterOptions": {
				"Indented": true
			}
		}"
	}
  }
}
```

- Exception Logging

```c#
try
{
	// code that may throw an exception
}
catch (Exception ex)
{
	logger.LogError(ex, "An error occurred");
}
```

- Logging Providers
	- Application Insights
	- Console
- Custom Logging Providers
- Log Scopes

```c#
using (logger.BeginScope("ScopeId 1: {ScopeId1}", Guid.NewGuid()))
using (logger.BeginScope("ScopeId 2: {ScopeId2}", Guid.NewGuid()))
{
	try
	{
		logger.LogInformation("");
	}
	catch (Exception ex)
	{
		logger.LogError(ex, "An error occurred");
	}
	
}
```

- Change log level during runtime

- Serilog (Examples are in BasicWebApi)
- What is Sink?
- Use Serilog in ASP.NET Core -> Serilog.AspNetCore
- Enrich Serilog 
    -> Serilog.Enrichers.Environment
    -> Serilog.Enrichers.Thread
    -> Serilog.Enrichers.Process

- Log Context -> equivalent to Log Scope in Microsoft Logging
- Timing in Serilog -> SerilogTimings

```c#
using (logger.TimeOperation("Processing something with id {Id}", obj.Id))
{
	// code to be timed
}

// another 
var op = logger.BeginOperation("Processing something with id {Id}", obj.Id);
// code to be timed
op.Complete();
//op.Abandon();
```

- Masking Sensitive data -> Destructurama.Attributed
- Async Logging -> Serilog.Sinks.Async

```c#
// keep only 10 logs in buffer

ILogger logger = new LoggerConfiguration()
    .WriteTo.Async(x => x.Console(theme:AnsiConsoleTheme.Code), 10)
	.CreateLogger();
```

- Optimizing Logging Performance -> Logging Source Generator

**Monitoring is a subset of Oservability**
- Application Insights: Monitor -> Alerts -> Create Alert Rule