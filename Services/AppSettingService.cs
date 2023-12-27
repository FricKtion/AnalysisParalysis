using AnalysisParalysis.Services.Definitions;
using System.Globalization;

namespace AnalysisParalysis.Services;

/// <inheritdoc />
public class AppSettingService : IAppSettingService
{
    private readonly IConfiguration _config;

    public AppSettingService(IConfiguration config)
        => (_config) = (config); 

    /// <inheritdoc />
    public T? Setting<T>(string key)
    {
        var envSetting = Environment.GetEnvironmentVariable(key);
        if(!string.IsNullOrEmpty(envSetting))
            return ChangeType<T>(envSetting);

        var configSetting = _config[key];
        if(!string.IsNullOrEmpty(configSetting))
            return ChangeType<T>(configSetting);

        var localSetting = _config[$"Values:{key}"];
        if(!string.IsNullOrEmpty(localSetting))
            return ChangeType<T>(localSetting);

        return default(T);
    }

    /// <summary>
    /// Attempt to change type of <paramref name="value"/> from string to T.
    /// </summary>
    /// <typeparam name="T">The type to convert to.</typeparam>
    /// <param name="value">The value to convert.</param>
    /// <returns>The converted value if possible.</returns>
    private T ChangeType<T>(string value)
        => (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
}
