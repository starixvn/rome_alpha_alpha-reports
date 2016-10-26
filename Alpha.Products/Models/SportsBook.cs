namespace Alpha.Products.Models
{
    using System;

    public class SportsBook : Product
    {
        public override int Id
        {
            get
            {
                return 1;
            }
        }

        public override bool Visible
        {
            get
            {
                return true;
            }
        }
    }
}
