namespace UtilsLib.Configurations;

/// <summary>
/// 
/// </summary>
public abstract class DbConfig
{
    /// <summary>
    /// 
    /// </summary>
    public string ConnectionString { get; set; } = null!;
    
    /// <summary>
    /// 
    /// </summary>
    public string DatabaseName { get; set; } = null!;
    
    public string UsersCollectionName { get; set; } = null!;
    public string PaymentsCollectionName { get; set; } = null!;
    public string DishesCollectionName { get; set; } = null!;
    public string ShiftsCollectionName { get; set; } = null!;
    public string CommentsCollectionName { get; set; } = null!;
    public string ResourcesCollectionName { get; set; } = null!;
    public string DishResourcesCollectionName { get; set; } = null!;
}