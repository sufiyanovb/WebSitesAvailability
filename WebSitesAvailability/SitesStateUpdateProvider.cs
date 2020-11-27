using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using WebSitesAvailability.Models;

namespace WebSitesAvailability
{
    public class SitesStateUpdateProvider
    {
        public async Task UpdateStates(IServiceScopeFactory scopeFactory, CancellationToken cancellationToken)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                using (var db = scope.ServiceProvider.GetService<ApplicationContext>())
                {
                    var sw = new Stopwatch();
                    sw.Start();
                    var current = 0;
                    Parallel.ForEach(db.Sites, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
                                       site =>
                                       {
                                           try
                                           {
                                               using (HttpWebResponse response = (HttpWebResponse)WebRequest.Create(site.Url).GetResponse())
                                               {
                                                   site.IsAvailable = response.StatusCode == HttpStatusCode.OK;
                                               }
                                           }
                                           catch (Exception ex)
                                           {
                                               site.IsAvailable = false;
                                           }
                                           finally
                                           {
                                               site.CheckDate = DateTime.Now;
                                               Interlocked.Increment(ref current);
                                           }
                                       });
                    await db.SaveChangesAsync(cancellationToken);
                    sw.Stop();

                }
            }
        }
    }
}

