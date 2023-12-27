namespace AnalysisParalysis.Services.Definitions;

public interface IAppSettingService
{
    T? Setting<T>(string key);
}
