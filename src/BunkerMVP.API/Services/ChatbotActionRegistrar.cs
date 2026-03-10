using System.Text;
using System.Text.Json;
using BunkerMVP.Application.DTOs.Auth;
using BunkerMVP.Application.DTOs.Dashboard;
using BunkerMVP.Application.DTOs.Locations;
using BunkerMVP.Application.DTOs.Orders;
using BunkerMVP.Application.DTOs.Products;
using BunkerMVP.Application.DTOs.Quotes;
using BunkerMVP.Application.DTOs.Requests;
using BunkerMVP.Application.DTOs.Suppliers;
using BunkerMVP.Application.DTOs.Vessels;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace BunkerMVP.API.Services;

public class ChatbotActionRegistrar : IHostedService
{
    private readonly IApiDescriptionGroupCollectionProvider _apiDescriptions;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ChatbotActionRegistrar> _logger;

    // Response type map keyed by action name
    private static readonly Dictionary<string, Type> ResponseTypeMap = new()
    {
        ["Vessels_GetAll"]       = typeof(List<VesselDto>),
        ["Vessels_GetById"]      = typeof(VesselDto),
        ["Vessels_Create"]       = typeof(VesselDto),
        ["Vessels_Update"]       = typeof(VesselDto),

        ["Products_GetAll"]      = typeof(List<ProductDto>),
        ["Products_GetById"]     = typeof(ProductDto),
        ["Products_Create"]      = typeof(ProductDto),
        ["Products_Update"]      = typeof(ProductDto),

        ["Locations_GetAll"]     = typeof(List<LocationDto>),
        ["Locations_GetById"]    = typeof(LocationDto),
        ["Locations_Create"]     = typeof(LocationDto),
        ["Locations_Update"]     = typeof(LocationDto),

        ["Suppliers_GetAll"]     = typeof(List<SupplierDto>),
        ["Suppliers_GetById"]    = typeof(SupplierDto),
        ["Suppliers_Create"]     = typeof(SupplierDto),
        ["Suppliers_Update"]     = typeof(SupplierDto),

        ["Requests_GetAll"]      = typeof(List<BunkerRequestDto>),
        ["Requests_GetById"]     = typeof(BunkerRequestDto),
        ["Requests_Create"]      = typeof(BunkerRequestDto),
        ["Requests_Update"]      = typeof(BunkerRequestDto),
        ["Requests_GetQuotes"]   = typeof(List<SupplierQuoteDto>),
        ["Requests_AddQuote"]    = typeof(SupplierQuoteDto),
        ["Requests_CreateOrder"] = typeof(BunkerOrderDto),

        ["Auth_Login"]           = typeof(LoginResponse),
        ["Dashboard_GetStats"]   = typeof(DashboardStatsDto),
    };

    public ChatbotActionRegistrar(
        IApiDescriptionGroupCollectionProvider apiDescriptions,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<ChatbotActionRegistrar> logger)
    {
        _apiDescriptions = apiDescriptions;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var registrationUrl = _configuration["ChatbotSystem:RegistrationUrl"];
        if (string.IsNullOrWhiteSpace(registrationUrl))
        {
            _logger.LogWarning("ChatbotSystem:RegistrationUrl is not configured. Skipping chatbot action registration.");
            return;
        }

        var hostAppId = _configuration["ChatbotSystem:HostAppId"] ?? "bunker-mvp";
        var apiBaseUrl = (_configuration["ChatbotSystem:ApiBaseUrl"] ?? "http://localhost:5005").TrimEnd('/');
        var apiKey = _configuration["ChatbotSystem:ApiKey"];

        var client = _httpClientFactory.CreateClient();
        int registered = 0;

        var allDescriptions = _apiDescriptions.ApiDescriptionGroups.Items
            .SelectMany(g => g.Items);

        foreach (var desc in allDescriptions)
        {
            try
            {
                var actionName = GetActionName(desc);
                if (actionName == null) continue;

                var payload = new
                {
                    name = actionName,
                    description = GetDescription(actionName),
                    endpointUrl = apiBaseUrl + "/" + desc.RelativePath!.TrimStart('/'),
                    httpMethod = desc.HttpMethod ?? "GET",
                    parameterSchema = GetParameterSchema(desc),
                    responseSchema = GetResponseSchema(actionName),
                    hostAppId = hostAppId,
                    fewShotExamples = "[]",
                    authType      = !string.IsNullOrWhiteSpace(apiKey) ? "ApiKey" : null,
                    authToken     = !string.IsNullOrWhiteSpace(apiKey) ? apiKey   : null,
                    authHeaderName = !string.IsNullOrWhiteSpace(apiKey) ? "X-Api-Key" : null,
                    isActive = true
                };

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(registrationUrl, content, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Registered: {ActionName}", actionName);
                    registered++;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    _logger.LogInformation("Already registered: {ActionName}", actionName);
                    registered++;
                }
                else
                {
                    _logger.LogWarning("Failed to register {ActionName}: HTTP {StatusCode}", actionName, (int)response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Exception while registering endpoint {RelativePath}", desc.RelativePath);
            }
        }

        _logger.LogInformation("Registration complete: {Count} actions registered.", registered);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private static string? GetActionName(ApiDescription desc)
    {
        var actionDescriptor = desc.ActionDescriptor;
        var routeValues = actionDescriptor.RouteValues;

        if (!routeValues.TryGetValue("controller", out var controller) || controller == null)
            return null;
        if (!routeValues.TryGetValue("action", out var action) || action == null)
            return null;

        return $"{controller}_{action}";
    }

    private static string GetDescription(string actionName)
    {
        var parts = actionName.Split('_', 2);
        var controller = parts.Length > 0 ? parts[0] : "";
        var action = parts.Length > 1 ? parts[1] : "";
        var singular = Singularize(controller);

        return action switch
        {
            "GetAll"      => $"Retrieve all {controller} records.",
            "GetById"     => $"Retrieve a single {singular} by its integer ID.",
            "Create"      => $"Create a new {singular} record.",
            "Update"      => $"Update an existing {singular} by its integer ID.",
            "Delete"      => $"Delete a {singular} by its integer ID.",
            "GetStats"    => "Retrieve dashboard statistics (total requests, open requests, pending quotes, total orders).",
            "GetQuotes"   => "Retrieve all supplier quotes for a specific bunker request.",
            "AddQuote"    => "Add a supplier quote to an existing bunker request.",
            "CreateOrder" => "Create a purchase order from an accepted supplier quote.",
            "Login"       => "Authenticate with username and password to start a session.",
            "Logout"      => "End the current session and log out.",
            "Me"          => "Retrieve the currently authenticated user's information.",
            _             => $"Perform {action} on {controller}."
        };
    }

    private static string Singularize(string word)
    {
        if (word.EndsWith("ies", StringComparison.OrdinalIgnoreCase))
            return word[..^3] + "y";
        if (word.EndsWith("s", StringComparison.OrdinalIgnoreCase) && word.Length > 1)
            return word[..^1];
        return word;
    }

    private static string GetParameterSchema(ApiDescription desc)
    {
        // Find body parameter
        var bodyParam = desc.ParameterDescriptions
            .FirstOrDefault(p => p.Source?.Id == "Body");

        // Find route/query parameters
        var routeParams = desc.ParameterDescriptions
            .Where(p => p.Source?.Id == "Path" || p.Source?.Id == "Query")
            .ToList();

        if (bodyParam == null && !routeParams.Any())
            return "{}";

        Dictionary<string, object> properties;

        if (bodyParam?.Type != null)
        {
            // Start from the body DTO schema
            var bodySchema = JsonSchemaGenerator.Generate(bodyParam.Type);
            var bodyObj = JsonSerializer.Deserialize<Dictionary<string, object>>(bodySchema);
            properties = new Dictionary<string, object>();

            if (bodyObj != null && bodyObj.TryGetValue("properties", out var propsElem))
            {
                var propsJson = JsonSerializer.Serialize(propsElem);
                var propsDict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(propsJson);
                if (propsDict != null)
                    foreach (var kv in propsDict)
                        properties[kv.Key] = kv.Value;
            }
        }
        else
        {
            properties = new Dictionary<string, object>();
        }

        // Merge route params
        foreach (var rp in routeParams)
        {
            var paramName = JsonNamingPolicy.CamelCase.ConvertName(rp.Name);
            var paramType = rp.Type ?? typeof(string);
            var schema = JsonSchemaGenerator.Generate(paramType);
            properties[paramName] = JsonSerializer.Deserialize<JsonElement>(schema);
        }

        var merged = new Dictionary<string, object>
        {
            ["type"] = "object",
            ["properties"] = properties
        };

        return JsonSerializer.Serialize(merged);
    }

    private static string GetResponseSchema(string actionName)
    {
        if (ResponseTypeMap.TryGetValue(actionName, out var responseType))
            return JsonSchemaGenerator.Generate(responseType);

        return "{}";
    }
}
