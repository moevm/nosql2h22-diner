namespace UtilsLib.Configurations;

/// <summary>
/// 
/// </summary>
public abstract class BaseDbConfig
{
    /// <summary>
    /// 
    /// </summary>
    public string ConnectionString { get; set; } = null!;
    
    /// <summary>
    /// 
    /// </summary>
    public string DatabaseName { get; set; } = null!;
    
    public string CollectionName { get; set; } = null!;
}