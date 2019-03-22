namespace ShoppingCartApi.IntegrationTests.Api
{
    public class ApiUrl
    {
        public ApiUrl(string host, int port)
        {
            Host = host;
            Port = port;
        }
        
        public string Host { get; }
        public int Port { get; }

        public string GetFor(string path)
        {
            return $"http://{Host}:{Port}{path}";
        }
    }
}