using System.Net.Http.Json;
using BlazorApp.Models;

public class GitHubService
{
    private readonly HttpClient _http;

    public GitHubService(HttpClient http)
    {
        _http = http;

        // GitHub exige User-Agent
        _http.DefaultRequestHeaders.UserAgent.ParseAdd("MyPortfolioApp");
    }

    public async Task<List<Project>> GetTopRepositoriesAsync(string username, int count = 6)
    {
        try
        {
            System.Diagnostics.Debug.WriteLine(">>> Llamando a GitHub API...");

            var url = $"https://api.github.com/users/{username}/repos";
            System.Diagnostics.Debug.WriteLine($">>> URL: {url}");

            var repos = await _http.GetFromJsonAsync<List<GitHubRepo>>(url);

            if (repos is null)
            {
                System.Diagnostics.Debug.WriteLine(">>> repos es NULL");
                return new();
            }

            System.Diagnostics.Debug.WriteLine($">>> Repos recibidos: {repos.Count}");

            var allowedForks = new[] { "fsharp", "fitch" };

            return repos
                .Where(r => (!r.fork || allowedForks.Contains(r.name.ToLower())) && !r.archived)
                .OrderByDescending(r => allowedForks.Contains(r.name.ToLower())) 
                .ThenByDescending(r => r.stargazers_count)                       
                .Take(count)
                .Select(r => new Project
                {
                    Title = r.name,
                    Description = string.IsNullOrWhiteSpace(r.description)
                        ? "No description available."
                        : r.description,
                    Url = r.html_url,
                    Stars = r.stargazers_count,
                    Count = r.forks_count
                })
                .ToList();

        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($">>> ERROR: {ex.Message}");
            return new();
        }
    }
}
