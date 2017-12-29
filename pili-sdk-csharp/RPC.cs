using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Qiniu.Pili
{
    internal sealed class RPC
    {
        public RPC(Mac mac)
        {
            Mac = mac;
            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders.UserAgent.ParseAdd(Config.APIUserAgent);
        }

        public Mac Mac { get; }

        public HttpClient HttpClient { get; }

        public async Task<string> CallWithJsonAsync(string urlStr, string json, CancellationToken cancellationToken = default(CancellationToken))
        {
            const string contentType = "application/json";

            var uri = new Uri(urlStr);
            var body = Encoding.UTF8.GetBytes(json);
            var macToken = Mac.SignRequest(uri, HttpMethod.Post, body, contentType);

            var content = new ByteArrayContent(body);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);

            var request = new HttpRequestMessage(HttpMethod.Post, uri) { Content = content };
            request.Headers.Add("Authorization", $"Qiniu {macToken}");

            var response = await HttpClient.SendAsync(request, cancellationToken);

            try
            {
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException)
            {
                throw new PiliException(response);
            }
            catch (Exception e)
            {
                throw new PiliException(e);
            }
        }

        public async Task<string> CallWithGetAsync(string urlStr, CancellationToken cancellationToken = default(CancellationToken))
        {
            var uri = new Uri(urlStr);
            var macToken = Mac.SignRequest(uri, HttpMethod.Get, null, null);

            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Add("Authorization", $"Qiniu {macToken}");

            var response = await HttpClient.SendAsync(request, cancellationToken);

            try
            {
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException)
            {
                throw new PiliException(response);
            }
            catch (Exception e)
            {
                throw new PiliException(e);
            }
        }

        public async Task<string> CallWithDeleteAsync(string urlStr, CancellationToken cancellationToken = default(CancellationToken))
        {
            var uri = new Uri(urlStr);
            var macToken = Mac.SignRequest(uri, HttpMethod.Delete, null, null);

            var request = new HttpRequestMessage(HttpMethod.Delete, uri);
            request.Headers.Add("Authorization", $"Qiniu {macToken}");

            var response = await HttpClient.SendAsync(request, cancellationToken);

            try
            {
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException)
            {
                throw new PiliException(response);
            }
            catch (Exception e)
            {
                throw new PiliException(e);
            }
        }
    }
}
