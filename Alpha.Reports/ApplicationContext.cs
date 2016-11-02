namespace Alpha.Reports
{
    using System.Collections.Generic;
    using Models;

    public interface ApplicationContext
    {
        IEnumerable<Product> Products { get; }        

        string OldMainAppPath { get; }
    }
}
