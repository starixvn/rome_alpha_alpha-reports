namespace Alpha.Reports.Models
{
    public class ProductDetail
    {
        public ProductDetail()
        {
        }

        public ProductDetail(
            string refId,
            int resourceId,
            int productId,
            int sportType,
            int betType,
            int order,
            int leagueGroupId,
            int ticketType,
            int childResourceId)
        {
            RefId = refId;
            ResourceId = resourceId;
            ProductId = productId;
            SportType = sportType;
            BetType = betType;
            Order = order;
            LeagueGroupId = leagueGroupId;
            TicketType = ticketType;
            ChildResourceId = childResourceId;
        }

        public string RefId { get; }

        public int ResourceId { get; }

        public int ProductId { get; }

        public int SportType { get; }

        public int BetType { get; }

        public int Order { get; }

        public string DisplayName { get; set; }

        public int LeagueGroupId { get; }

        public int TicketType { get; }

        public int ChildResourceId { get; }

        public override string ToString()
            => string.Format(
                    "<ProductDetail ProductId=\"{0}\" Sporttype=\"{1}\" Bettype=\"{2}\" LeagueGroupId=\"{3}\" TicketType=\"{4}\"/>",
                    ProductId,
                    SportType,
                    BetType,
                    LeagueGroupId,
                    TicketType);
    }
}
