namespace Alpha.Reports.Models
{
    public interface IUser
    {
        int CustId { get; }

        string CustName { get; }

        byte UserLevelId { get; }

        string Language { get; }
    }
}