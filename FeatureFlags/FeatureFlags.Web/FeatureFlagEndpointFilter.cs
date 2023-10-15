using Microsoft.FeatureManagement;

namespace FeatureFlags.Web;

public static class FeatureFlagEndpointFilterExtensions
{
    public static TBuilder WithFeature<TBuilder>(this TBuilder builder, string endpointName) where TBuilder : IEndpointConventionBuilder
    {
        builder.AddEndpointFilter(new FeatureFlagEndpointFilter(endpointName));
        return builder;
    }
}

public class FeatureFlagEndpointFilter : IEndpointFilter
{
    private readonly string _endpointName;

    public FeatureFlagEndpointFilter(string endpointName)
    {
        _endpointName = endpointName;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var featureManager = context.HttpContext.RequestServices.GetRequiredService<IFeatureManager>();
        var isEnabled = await featureManager.IsEnabledAsync($"Endpoints_{_endpointName}");

        return !isEnabled 
            ? Results.NotFound() 
            : await next(context);
    }
}