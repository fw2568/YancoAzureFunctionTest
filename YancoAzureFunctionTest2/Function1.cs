using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Dbosoft.YaNco;
using LanguageExt.UnsafeValueAccess;
//using Dbosoft.YaNco;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace YancoAzureFunctionTest2
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            var settings = new Dictionary<string, string>
            {
                {"ashost", "dummy"},
                {"sysnr", "00"},
                {"client","001"},
                {"user", "dummy"},
                {"passwd", "peng"},
                {"lang", "EN"}

            };

            var rfcConnnectionBuilder = new ConnectionBuilder(settings);


            using (var context = new RfcContext(rfcConnnectionBuilder.Build()))
            {
                var res = await context.CallFunction("FUNC", f => f, f => f).ToEither();
                res.IfLeft(l => log.LogError(l.Message));
            }


            return new OkObjectResult(responseMessage);
        }
    }
}
