using BasicConsoleApp;
using Destructurama;
using Serilog;
using Serilog.Context;
using Serilog.Formatting.Json;
using Serilog.Sinks.SystemConsole.Themes;
using SerilogTimings.Extensions;

ILogger logger = new LoggerConfiguration()
    //.WriteTo.Async(x => x.Console(theme:AnsiConsoleTheme.Code), 10)
    .WriteTo.Console(new JsonFormatter())
    .WriteTo.NickSink()
    .Enrich.FromLogContext()
    .Destructure.ByTransforming<Payment>(p => new { p.PaymentId, p.Email })
    //.Destructure.UsingAttributes()
    .CreateLogger();

Log.Logger = logger;

var payment = new Payment
{
    PaymentId = 1,
    Email = "nick@dometrain.com",
    UserId = Guid.NewGuid(),
    OccuredAt = DateTime.UtcNow
};

using (LogContext.PushProperty("PaymentId", payment.PaymentId))
{
    logger.Information("Received payment by user with id {UserId}", payment.UserId);
}

// @ -> deserialize the object
// $ -> name of the object
logger.Information("Received payment with details {@PaymentData}", payment);

Log.CloseAndFlush();



