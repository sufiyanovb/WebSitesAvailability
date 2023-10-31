using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
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
                                                          async site =>
                                                          {
                                                              try
                                                              {
                                                                  using (HttpClient httpClient = new HttpClient())
                                                                  {
                                                                      var ret = await httpClient.GetAsync(site.Url);
                                                                      site.IsAvailable = ret.StatusCode == HttpStatusCode.OK;
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

                                                                  Debug.WriteLine($"Обработано сайтов:{current}");

                                                              }
                                                          });
                    await db.SaveChangesAsync(cancellationToken);
                    sw.Stop();

                }
            }
        }
    }
}

