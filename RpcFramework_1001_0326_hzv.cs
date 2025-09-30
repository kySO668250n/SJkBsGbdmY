// 代码生成时间: 2025-10-01 03:26:30
using System;

using System.Net.Http;

using System.Text.Json;

using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Microsoft.Extensions.Hosting;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using Newtonsoft.Json.Linq;


// RpcClient is responsible for making remote procedure call (RPC)

public class RpcClient

{

    private readonly HttpClient _httpClient;

    private readonly ILogger<RpcClient> _logger;

    private readonly string _rpcServiceUrl;

    private readonly JsonSerializerSettings _jsonSerializerSettings;


    public RpcClient(ILogger<RpcClient> logger, IConfiguration configuration)

    {

        _logger = logger;

        _rpcServiceUrl = configuration.GetValue<string>("RpcServiceUrl");

        _httpClient = new HttpClient();

        _jsonSerializerSettings = new JsonSerializerSettings {

            TypeNameHandling = TypeNameHandling.Auto,

            PreserveReferencesHandling = PreserveReferencesHandling.None,

            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,

            NullValueHandling = NullValueHandling.Ignore,

            Formatting = Formatting.Indented,

            DateFormatHandling = DateFormatHandling.IsoDateFormat

        };

    }


    // Make a synchronous RPC call to the remote service

    public async Task<JObject> RpcCallAsync(string method, params object[] parameters)

    {

        try

        {

            var requestJson = JsonConvert.SerializeObject(new { Method = method, Parameters = parameters }, _jsonSerializerSettings);

            var content = new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_rpcServiceUrl, content);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            var responseJson = JObject.Parse(responseContent);

            return responseJson;

        }

        catch (Exception ex)

        {

            _logger.LogError($"An error occurred during RPC call: {ex.Message}");

            throw new ApplicationException($"RPC call failed: {ex.Message}");

        }

    }

}


// RpcService is the class that handles incoming RPC requests and executes the appropriate method

public class RpcService : IHostedService

{

    private readonly IServiceProvider _serviceProvider;

    private readonly ILogger<RpcService> _logger;

    private readonly HttpClient _httpClient;


    public RpcService(IServiceProvider serviceProvider, ILogger<RpcService> logger)

    {

        _serviceProvider = serviceProvider;

        _logger = logger;

        _httpClient = new HttpClient();

    }


    // This method is called when the application starts

    public Task StartAsync(CancellationToken cancellationToken)

    {

        return Task.CompletedTask;

    }


    // This method is called when the application stops

    public async Task StopAsync(CancellationToken cancellationToken)

    {

        await Task.CompletedTask;

    }


    // Handle the RPC request

    public async Task HandleRpcRequestAsync(HttpRequestMessage request)

    {

        try

        {

            var requestJson = await request.Content.ReadAsStringAsync();

            var requestObj = JObject.Parse(requestJson);

            var methodName = requestObj["Method"].ToString();

            var parameters = requestObj["Parameters"].ToObject<object[]>(JsonSerializer.Create(_jsonSerializerSettings));

            var method = typeof(_serviceProvider.GetService<IRpcHandler>()).GetMethod(methodName);

            var result = method.Invoke(_serviceProvider.GetService<IRpcHandler>(), parameters);

            var responseJson = new JObject { { "Result", result } };

            var response = new HttpResponseMessage(HttpStatusCode.OK)

            {

                Content = new StringContent(responseJson.ToString(), System.Text.Encoding.UTF8, "application/json")

            };

            await request.RequestUri.CreateResponse(response);

        }

        catch (Exception ex)

        {

            _logger.LogError($"An error occurred during RPC handling: {ex.Message}");

            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)

            {

                Content = new StringContent($"RPC handling failed: {ex.Message}", System.Text.Encoding.UTF8, "text/plain")

            };

            await request.RequestUri.CreateResponse(response);

        }

    }

}


// RpcHandler is an interface that defines the contract for handling RPC requests

public interface IRpcHandler

{

    // Define the methods that can be remotely called

    // Example method

    int Add(int a, int b);

}


// RpcHandlerImplementation is a concrete implementation of the IRpcHandler interface

public class RpcHandlerImplementation : IRpcHandler

{

    public int Add(int a, int b)

    {

        return a + b;

    }

    // Add more methods as needed

}

