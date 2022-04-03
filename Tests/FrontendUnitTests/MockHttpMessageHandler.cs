using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FrontendUnitTests;

public class MockHttpMessageHandler : HttpMessageHandler
{
    private readonly Queue<(string, HttpResponseMessage)> _queue;

    public MockHttpMessageHandler()
    {
        _queue = new Queue<(string, HttpResponseMessage)>();
    }

    public void AddNewMockResponse(string requestMustContain, HttpStatusCode statusCode, string content = null, string mediaType = "text/plain")
    {
        var response = new HttpResponseMessage(statusCode);
        if (content != null)
            response.Content = new StringContent(content, System.Text.Encoding.UTF8, mediaType);
        _queue.Enqueue((requestMustContain, response));
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var next = _queue.Dequeue();
        if (request.RequestUri.ToString().Contains(next.Item1))
            return next.Item2;
        else throw new Exception($"Request URI did not contain '{next.Item1}'");
    }
}
