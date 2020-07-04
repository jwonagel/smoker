using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace api.Services
{

    public interface ISlackMessageSender
    {
        Task SendMessageOnSmokerAsync(string text);        
    }

    public class SlackMessageSender : ISlackMessageSender
    {
        
        private readonly HttpClient _client;

        private readonly string _randomChannel = Environment.GetEnvironmentVariable("slackWebHook");
        public SlackMessageSender(HttpClient client)
        {
            _client = client;
        }

        public async Task SendMessageOnSmokerAsync(string text)
        {
            var contentObject = new { text = text };
            var contentObjectJson = JsonSerializer.Serialize(contentObject);
            var content = new StringContent(contentObjectJson, Encoding.UTF8, "application/json");

            var result = await _client.PostAsync(_randomChannel, content);
            var resultContent = await result.Content.ReadAsStringAsync();
            if (!result.IsSuccessStatusCode)
            {
                throw new Exception("Task failed.");
            }
        }
    }
}