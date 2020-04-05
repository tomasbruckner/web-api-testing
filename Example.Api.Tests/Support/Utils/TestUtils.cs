using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace Example.Api.Tests.Support.Utils
{
    public static class TestUtils
    {
        public static StringContent CreateContent(object obj)
        {
            return new StringContent(
                JsonConvert.SerializeObject(obj),
                Encoding.UTF8,
                "application/json"
            );
        }

        public static HttpContent CreateEmptyObjectContent()
        {
            return CreateContent(new EmptyObject());
        }

        public static async Task<object> SendOkRequest(
            HttpClient client,
            HttpRequestMessage request
        )
        {
            var response = await client.SendAsync(request);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            return await Deserialize(response);
        }

        public static async Task<T> SendOkRequest<T>(
            HttpClient client,
            HttpRequestMessage request
        )
        {
            var response = await client.SendAsync(request);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            return await Deserialize<T>(response);
        }

        public static HttpRequestMessage CreatePostMessageAsAdmin(
            string url,
            object content = null,
            bool withEmptyContent = true
        )
        {
            var message = CreatePostMessage(url, content, withEmptyContent);
            AuthorizationSupport.AddAdminHeaders(message);

            return message;
        }

        public static HttpRequestMessage CreatePostMessage(
            string url,
            object content = null,
            bool withEmptyContent = true
        )
        {
            HttpContent messageContent = null;

            if (content != null)
            {
                messageContent = CreateContent(content);
            }
            else if (withEmptyContent)
            {
                messageContent = CreateEmptyObjectContent();
            }

            return CreateMessage(url, HttpMethod.Post, messageContent);
        }

        public static HttpRequestMessage CreateGetRequestAsAdmin(string url)
        {
            var message = CreateMessage(url, HttpMethod.Get);
            AuthorizationSupport.AddAdminHeaders(message);

            return message;
        }

        public static HttpRequestMessage CreateGetRequest(string url)
        {
            return CreateMessage(url, HttpMethod.Get);
        }

        public static HttpRequestMessage CreateDeleteRequest(string url)
        {
            return CreateMessage(url, HttpMethod.Delete);
        }

        public static HttpRequestMessage CreateDeleteRequestAsAdmin(string url)
        {
            var message = CreateMessage(url, HttpMethod.Delete);
            AuthorizationSupport.AddAdminHeaders(message);

            return message;
        }

        public static HttpRequestMessage CreatePutRequestAsAdmin(string url, object content)
        {
            var message = CreatePutRequest(url, content);
            AuthorizationSupport.AddAdminHeaders(message);

            return message;
        }

        public static HttpRequestMessage CreatePutRequest(string url, object content)
        {
            if (content == null)
            {
                return CreateMessage(url, HttpMethod.Put);
            }

            return CreateMessage(url, HttpMethod.Put, CreateContent(content));
        }

        private static HttpRequestMessage CreateMessage(string url, HttpMethod method, HttpContent content = null)
        {
            if (content == null)
            {
                return new HttpRequestMessage(method, url);
            }

            return new HttpRequestMessage(method, url)
            {
                Content = content
            };
        }

        public static async Task SendRequestAssertStatus(
            HttpClient client,
            HttpRequestMessage request,
            HttpStatusCode status
        )
        {
            var response = await client.SendAsync(request);

            Assert.Equal(status, response.StatusCode);
        }

        public static async Task SendNoContentRequest(HttpClient client, HttpRequestMessage request)
        {
            await SendRequestAssertStatus(client, request, HttpStatusCode.NoContent);
        }

        public static async Task SendForbiddenRequest(HttpClient client, HttpRequestMessage request)
        {
            await SendRequestAssertStatus(client, request, HttpStatusCode.Forbidden);
        }

        public static async Task SendUnauthorizedRequest(HttpClient client, HttpRequestMessage request)
        {
            await SendRequestAssertStatus(client, request, HttpStatusCode.Unauthorized);
        }

        public static async Task SendConflictRequest(HttpClient client, HttpRequestMessage request)
        {
            await SendRequestAssertStatus(client, request, HttpStatusCode.Conflict);
        }

        public static async Task SendNotFoundRequest(HttpClient client, HttpRequestMessage request)
        {
            await SendRequestAssertStatus(client, request, HttpStatusCode.NotFound);
        }

        public static async Task<object> Deserialize(HttpResponseMessage response)
        {
            var stringResponseAll = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject(stringResponseAll);
        }

        public static async Task<dynamic> Deserialize<T>(HttpResponseMessage response)
        {
            var stringResponseAll = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(stringResponseAll);
        }


        public static async Task IterateAllRolesAsync(Func<Action<HttpRequestMessage>, string, Task> testFunction)
        {
            foreach (var (addTokenHeader, name) in AuthorizationSupport.AllRoles)
            {
                await testFunction(addTokenHeader, name);
            }
        }

        public static async Task IterateAllRolesAndAnonymousAsync(
            Func<Action<HttpRequestMessage>, string, Task> testFunction
        )
        {
            foreach (var (addTokenHeader, name) in AuthorizationSupport.AllRolesAndAnonymous)
            {
                await testFunction(addTokenHeader, name);
            }
        }
    }
}
