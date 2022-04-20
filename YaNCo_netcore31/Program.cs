using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dbosoft.YaNco;

namespace YaNCo_netcore31
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var settings = new Dictionary<string, string>
            {
                { "ashost", "dummy" },
                { "sysnr", "00" },
                { "client", "001" },
                { "user", "dummy" },
                { "passwd", "peng" },
                { "lang", "EN" }

            };

            var rfcConnnectionBuilder = new ConnectionBuilder(settings);


            using (var context = new RfcContext(rfcConnnectionBuilder.Build()))
            {
                var res = await context.CallFunction(
                        "FUNC", f => f,
                        f => f
                            .HandleReturn()
                            .MapTable("TAB", s =>
                                from field in s.GetField<string>("FIELD")
                                select new { field }
                            )

                    )

                    .IfLeft(l => throw new Exception(l.Message));





            }
        }
    }
}
