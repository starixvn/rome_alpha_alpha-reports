namespace Alpha.Reports.Models
{
    public interface IUser
    {
        int Id { get; }

        string Name { get; }

        byte LevelId { get; }

        string Language { get; }

        bool GetHideProduct(int productId);
    }
}