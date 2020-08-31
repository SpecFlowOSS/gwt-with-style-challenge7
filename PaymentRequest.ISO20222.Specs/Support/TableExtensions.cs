using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace PaymentRequest.ISO20222.Specs.Support
{
    public static class TableExtensions
    {
        public static IDictionary<string, string> AsSingleRow(this Table table)
        {
            if (table.RowCount == 0)
                throw new InvalidOperationException("The table contains no rows.");

            if (table.RowCount > 1)
                throw new InvalidOperationException("The table contains multiple rows.");

            return table.Rows.Single();
        }

        public static IEnumerable<IDictionary<string, string>> AsAllRows(this Table table)
        {
            return table.Rows;
        }
    }
}
