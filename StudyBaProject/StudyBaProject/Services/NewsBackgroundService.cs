using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using StudyBaProject.Services;
using StudyBa.Models;
using StudyBaProject.Data;
using Microsoft.Extensions.Logging; // Add this namespace


namespace StudyBaProject.Services
{
    public class NewsBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _executionTime = new TimeSpan(2, 0, 0); // 2 am CET
        private readonly ILogger<NewsBackgroundService> _logger; // Add ILogger


        public NewsBackgroundService(IServiceProvider serviceProvider, ILogger<NewsBackgroundService> logger) // Modify constructor
        {
            _serviceProvider = serviceProvider;
            _logger = logger; // Assign logger
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Calculate the delay until the next execution time
                var now = DateTime.UtcNow;
                var nextExecutionTime = new DateTime(now.Year, now.Month, now.Day, _executionTime.Hours, _executionTime.Minutes, _executionTime.Seconds);
                if (now > nextExecutionTime)
                {
                    nextExecutionTime = nextExecutionTime.AddDays(1); // Next day
                }
                var delay = nextExecutionTime - now;

                // Delay the execution until the next execution time
                await Task.Delay(delay, stoppingToken);

                // Perform scraping and saving news
                await ScrapeAndSaveNewsAsync(stoppingToken);

                // Wait for one day
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }

        private async Task ScrapeAndSaveNewsAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var scraperService = scope.ServiceProvider.GetRequiredService<NewsScraperService>();

                var scrapingConfigs = new List<ScrapingConfig>
                {
                    new ScrapingConfig
                    {
                        Url = "https://etf.unsa.ba/obavjestenja",
                        HeadlineCSSSelector = "div.mb-3.bg-light.p-3 > a",
                        ArticleUrlCSSSelector = "div.mb-3.bg-light.p-3 > a[href]"
                    },
                    new ScrapingConfig
                    {
                        Url = "https://af.unsa.ba/obavjestenja",
                        HeadlineCSSSelector = ".post-title a",
                        ArticleUrlCSSSelector = ".post-title a"
                    },
                    new ScrapingConfig
                    {
                        Url = "https://www.efsa.unsa.ba/ef/bs/oglasna-ploca-obavjestenja",
                        HeadlineCSSSelector = ".views-field-title a",
                        ArticleUrlCSSSelector = ".views-field-title a"
                    },
                    new ScrapingConfig
                    {
                        Url = "https://gf.unsa.ba/category/obavjestenja/",
                        HeadlineCSSSelector = ".entry-title a",
                        ArticleUrlCSSSelector = ".entry-title a"
                    }
                    // Add more configurations for other websites as needed
                };

                // Clear existing data and then scrape and save news
                await ClearAndScrapeNewsAsync(scraperService, scrapingConfigs, stoppingToken);
            }
        }

        private async Task ClearAndScrapeNewsAsync(NewsScraperService scraperService, List<ScrapingConfig> configs, CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Clear existing news data
                dbContext.News.RemoveRange(dbContext.News);

                await dbContext.SaveChangesAsync(stoppingToken);
                _logger.LogInformation("Existing news data cleared successfully."); // Log success
            }

            // Scrape and save news
            await scraperService.ScrapeAndSaveNewsAsync(_serviceProvider, configs);
        }

        public async Task PopulateDatabaseOnceAsync()
        {
            await ScrapeAndSaveNewsAsync(CancellationToken.None);
        }
    }

}
