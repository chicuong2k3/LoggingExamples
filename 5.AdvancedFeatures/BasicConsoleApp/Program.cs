using System.Text.Json;
using BasicConsoleApp;
using Microsoft.Extensions.Logging;

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddJsonConsole(x =>
    {
        x.JsonWriterOptions = new JsonWriterOptions
        {
            Indented = true
        };
    });
    builder.SetMinimumLevel(LogLevel.Information);
});

ILogger logger = loggerFactory.CreateLogger<Program>();

//var paymentId = 1;
//var amount = 15.99;

var paymentData = new PaymentData(1, 15.99m);

//logger.LogInformation(
//    "New Payment with id {PaymentId} for ${Total:c}", paymentId, amount);
logger.LogInformation(
    "New Payment with data {PaymentData}", paymentData);

logger.LogInformation(
    "New Payment with data {PaymentData}", JsonSerializer.Serialize(paymentData));

await Task.Delay(1000);

// Timed log
using (logger.BeginTimedOperation("Processing payment"))
{
    await Task.Delay(200);
}

record PaymentData(int PaymentId, decimal Amount);
