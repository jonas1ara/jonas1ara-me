namespace BlazorApp.Models
{
    public class GitHubRepo
    {
        public string name { get; set; } = "";
        public string description { get; set; } = "";
        public string html_url { get; set; } = "";
        public bool fork { get; set; }
        public int forks_count { get; set; }
        public int stargazers_count { get; set; }
        public bool archived { get; set; }
    }
}

