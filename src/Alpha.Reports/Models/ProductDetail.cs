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

        public string RefId { get; set; }

        public int ResourceId { get; set; }

        public int ProductId { get; set; }

        public int SportType { get; set; }

        public int BetType { get; set; }

        public int Order { get; set; }

        public string DisplayName { get; set; }

        public int LeagueGroupId { get; set; }

        public int TicketType { get; set; }

        public int ChildResourceId { get; set; }

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
