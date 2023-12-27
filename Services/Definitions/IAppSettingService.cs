namespace AnalysisParalysis.Services.Definitions;

/// <summary>
/// Used to pull settings from IConfiguration. Can support multiple setting locations.
/// </summary>
public interface IAppSettingService
{
    /// <summary>
    /// Attempt to find a setting with the name provided in <paramref name="key"/>.
    /// </summary>
    /// <typeparam name="T">The expected type of the setting's value.</typeparam>
    /// <param name="key">The setting name to look for.</param>
    /// <returns>The setting value as T if possible, otherwise null.</returns>
    T? Setting<T>(string key);
}
