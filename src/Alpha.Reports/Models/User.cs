namespace Alpha.Reports.Models
{
    public interface User
    {
        int CustId { get; }

        string CustName { get; }

        byte UserLevelId { get; }

        string Language { get; }
    }
}