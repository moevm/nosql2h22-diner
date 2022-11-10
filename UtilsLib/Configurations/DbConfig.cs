namespace UtilsLib.Configurations;

public class DbConfig
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string UsersCollectionName { get; set; } = null!;
    public string PaymentsCollectionName { get; set; } = null!;
    public string DishesCollectionName { get; set; } = null!;
    public string ShiftsCollectionName { get; set; } = null!;
    public string CommentsCollectionName { get; set; } = null!;
    public string ResourcesCollectionName { get; set; } = null!;
    public string DishResourcesCollectionName { get; set; } = null!;
    public string WeeksCollectionName { get; set; } = null!;
    public string AuthInfoCollectionName { get; set; } = null!;
}