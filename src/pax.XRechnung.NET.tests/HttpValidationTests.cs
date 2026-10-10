using System.Net;
using System.Net.Sockets;
using System.Text;

namespace pax.XRechnung.NET.tests;

[TestClass]
public sealed class HttpValidationTests
{
    [TestMethod]
    public async Task ExplicitUriUsesHttpAndPreservesDetailedFailure()
    {
        using var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        var endpoint = (IPEndPoint)listener.LocalEndpoint;
        var responseTask = Respond(listener);
        var result = await XmlInvoiceValidator.ValidateSchematron("<remote-only/>", new Uri($"http://127.0.0.1:{endpoint.Port}/"));
        await responseTask;
        Assert.IsFalse(result.IsValid);
        Assert.AreEqual("OK", result.HttpStatusCode);
        Assert.HasCount(1, result.Validations);
        Assert.AreEqual("TEST-ERROR", result.Validations.Single().Code);
        Assert.AreEqual("/Invoice", result.Validations.Single().Path);
        Assert.IsNull(result.Error);
    }

    private static async Task Respond(TcpListener listener)
    {
        using var client = await listener.AcceptTcpClientAsync();
        await using var stream = client.GetStream();
        using var reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
        Assert.AreEqual("POST / HTTP/1.1", await reader.ReadLineAsync());
        while (!string.IsNullOrEmpty(await reader.ReadLineAsync())) { }
        const string html = "<body><table><tr><th>Step</th></tr><tr><td>Rules</td><td>1</td><td>0</td><td>0</td></tr></table>"
            + "<table><tr><th>Messages</th></tr><tr><td>1</td><td>TEST-ERROR</td><td>ERROR</td><td>Rejected by reference</td></tr>"
            + "<tr><td>Pfad: /Invoice</td></tr></table></body>";
        var body = Encoding.UTF8.GetBytes(html);
        var headers = Encoding.ASCII.GetBytes($"HTTP/1.1 200 OK\r\nContent-Type: text/html\r\nContent-Length: {body.Length}\r\nConnection: close\r\n\r\n");
        await stream.WriteAsync(headers);
        await stream.WriteAsync(body);
    }
}
