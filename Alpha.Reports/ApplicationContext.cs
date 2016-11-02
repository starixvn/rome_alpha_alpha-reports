namespace Alpha.Reports
{
    using System;
    using System.Collections.Generic;
    using Models;

    public interface ApplicationContext
    {
        IEnumerable<Product> Products { get; }        

        string OldMainAppPath { get; }
    }
}
