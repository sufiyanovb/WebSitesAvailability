using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
                using (var dBContext = scope.ServiceProvider.GetService<ApplicationContext>())
                {
                    var sw = new Stopwatch();

                    sw.Start();

                    Parallel.ForEach(dBContext.Sites, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
                                                          async site =>
                                                          {
                                                              try
                                                              {
                                                                  using (HttpClient httpClient = new HttpClient())
                                                                  {
                                                                      dBContext.Entry(site).State = EntityState.Modified;

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
                                                              }
                                                          });
                    await dBContext.SaveChangesAsync(cancellationToken);
                    sw.Stop();

                }
            }
        }
    }
}

