using MapLoafServer;
using System.Net;
using System.Web;

public class HttpServer
{
    public int Port = 8065;
    string path = "/ml/maps/";
    private HttpListener _listener = new();
    private TextMap? _mapdata;

    public void Start(bool local, TextMap MapData)
    {
        _mapdata = MapData;

        if (local)
        {
            Port = 80;
            path = "/test/";
        }

        _listener.Prefixes.Add("http://+:" + Port.ToString() + path);
        _listener.Start();

        Console.WriteLine("Server listening on " + _listener.Prefixes.ElementAt(0).ToString());

        Receive();
    }

    public void Stop()
    {
        _listener.Stop();
    }

    private void Receive()
    {
        _listener.BeginGetContext(new AsyncCallback(ListenerCallback), _listener);
    }

    private void ListenerCallback(IAsyncResult result)
    {
        if (_listener.IsListening)
        {
            var context = _listener.EndGetContext(result);
            var request = context.Request;

            Console.WriteLine($"Received request [{request.Url}] from [{request.RemoteEndPoint}] with method [{request.HttpMethod}]");

            var response = context.Response;
            response.StatusCode = (int)HttpStatusCode.OK;
            response.ContentType = "application/octet-stream";
            response.SendChunked = true;
            response.KeepAlive = true;

            byte[] response_bytes = [];

            if (request.HttpMethod == HttpMethod.Get.ToString())
            {
                if (request.RawUrl == null) return;

                if (request.RawUrl.StartsWith(path + "map.prg"))
                {
                    string parsedUrl = request.RawUrl.Split("?")[1];
                    var paramsCollection = HttpUtility.ParseQueryString(parsedUrl);

                    // The foreach loop will iterate over the params collection and print the key and value for each param
                    foreach (var key in paramsCollection.AllKeys)
                    {
                        Console.WriteLine($"Key: {key} => Value: {paramsCollection[key]}");
                    }

                    uint x = UInt32.Parse(paramsCollection.GetValues("x")[0]);
                    uint y = UInt32.Parse(paramsCollection.GetValues("y")[0]);

                    if (_mapdata != null)
                    {
                        response_bytes = [0x00, 0x04, .. _mapdata.GetScreen(x, y, 40, 25)]; //  Assume C64
                    }                        
                }

                if (response_bytes.Length > 0)
                {
                    response.OutputStream.Write(response_bytes);
                    response.OutputStream.Flush();
                }                
            }

            response.OutputStream.Close();
            Receive();
        }
    }
}