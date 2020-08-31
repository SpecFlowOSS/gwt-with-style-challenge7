using System;
using System.Linq;

namespace PaymentRequest.ISO20222.Specs.Support
{
    /// <summary>
    /// Encapsulates a relative path to an element or attribute within an XML document based on a user-readable "dot path" notation used in the Gherkin file
    /// </summary>
    /// <remarks>
    /// Child elements are separated with dots ('.').
    /// An attribute is separated with a space (' '). 
    /// For example the dot path "Amt.InstdAmt Ccy" represents a path in the XML document pointing to the "CCy" attribute in the following XML structure <![CDATA[<CurrentNode><Amt><InstdAmt Ccy="xxx">]]> in the context of a "current node".
    /// The dot path "Amt.InstdAmt" represents a path pointing to the "InstdAmt" element within the "Amt" element within the "current node".
    /// </remarks>
    public class DotPath
    {
        public const string NamespacePrefix = "ns";

        public string DotPathString { get; private set; }

        public string ElementXPath { get; private set; }

        public string AttributeName { get; private set; }

        public DotPath(string dotPathString)
        {
            if (string.IsNullOrEmpty(dotPathString))
                throw new ArgumentNullException(nameof(dotPathString));

            DotPathString = dotPathString;

            var elementPathAndAttributeParts = dotPathString.Split(' ');
            var elementPath = elementPathAndAttributeParts[0];

            ElementXPath = String.Join('/', elementPath.Split('.').Select(part => $"{NamespacePrefix}:{part}").ToArray());

            if (elementPathAndAttributeParts.Length > 1)
                AttributeName = elementPathAndAttributeParts[1];
        }
    }
}
