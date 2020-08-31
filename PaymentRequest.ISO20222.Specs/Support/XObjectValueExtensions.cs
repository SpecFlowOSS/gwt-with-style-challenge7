using System;
using System.Xml.Linq;

namespace PaymentRequest.ISO20222.Specs.Support
{
    public static class XObjectValueExtensions
    {
        public static void SetValue(this XObject obj, string value)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            switch (obj)
            {
                case XElement element: element.Value = value;
                    break;
                case XAttribute attribute: attribute.Value = value;
                    break;
                default:
                    throw new InvalidOperationException($"XObject type {obj.NodeType} not supported");
            }
        }
    }
}
