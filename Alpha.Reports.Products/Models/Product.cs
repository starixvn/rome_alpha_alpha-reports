namespace Alpha.Reports.Products.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Sunplus.Agent.Model.RemotingRef;

    public abstract class Product
    {
        public abstract int Id { get; }

        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsHidden { get; }

        public bool IsDisabled { get; }

        public bool? IsSynced { get; }

        public abstract bool Visible { get; }
    }
}
