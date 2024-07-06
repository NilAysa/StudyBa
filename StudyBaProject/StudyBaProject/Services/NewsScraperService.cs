using HtmlAgilityPack;
using StudyBaProject.Data;
using StudyBa.Models;
using Microsoft.Extensions.Logging;
using AngleSharp.Html.Parser;

namespace StudyBaProject.Services
{
    public class NewsScraperService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<NewsScraperService> _logger;

        public NewsScraperService(HttpClient httpClient, ILogger<NewsScraperService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task ScrapeAndSaveNewsAsync(IServiceProvider serviceProvider, List<ScrapingConfig> configs)
        {
            var allArticles = new List<News>();

            foreach (var config in configs)
            {
                var articles = await ScrapeNewsAsync(config);
                allArticles.AddRange(articles);
            }

            // Save to database
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                dbContext.News.AddRange(allArticles);
                await dbContext.SaveChangesAsync();
            }

            _logger.LogInformation("News scraped and saved successfully.");
        }


        private async Task<List<News>> ScrapeNewsAsync(ScrapingConfig config)
        {
            var articles = new List<News>();

            try
            {
                var response = await _httpClient.GetAsync(config.Url);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                var parser = new HtmlParser();
                var document = parser.ParseDocument(content);

                var headlineNodes = document.QuerySelectorAll(config.HeadlineCSSSelector);
                var articleUrlNodes = document.QuerySelectorAll(config.ArticleUrlCSSSelector);

                for (int i = 0; i < Math.Min(headlineNodes.Length, articleUrlNodes.Length); i++)
                {
                    var headline = headlineNodes[i].TextContent.Trim();
                    var articleUrl = articleUrlNodes[i].GetAttribute("href").Trim();

                    if (!string.IsNullOrEmpty(headline) && !string.IsNullOrEmpty(articleUrl))
                    {
                        // Ensure the URL is absolute
                        if (!Uri.IsWellFormedUriString(articleUrl, UriKind.Absolute))
                        {
                            articleUrl = new Uri(new Uri(config.Url), articleUrl).ToString();
                        }

                        articles.Add(new News
                        {
                            Title = headline,
                            SourceLink = articleUrl
                        });
                    }
                }
                _logger.LogInformation("News scraped successfully from {Url}.", config.Url);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while scraping {config.Url}: {ex.Message}");
            }

            return articles;
        }
    }


}

public class ScrapingConfig
{
    public string Url { get; set; }
    public string HeadlineCSSSelector { get; set; }
    public string ArticleUrlCSSSelector { get; set; }
}
