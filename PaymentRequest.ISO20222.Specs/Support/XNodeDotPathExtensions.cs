using System;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace PaymentRequest.ISO20222.Specs.Support
{
    public static class XNodeDotPathExtensions
    {
        public static XElement DotPathSelectElementOrThrow(this XNode node, DotPath dotPath, XmlNamespaceManager nsMgr)
        {
            var element = node.XPathSelectElement(dotPath.ElementXPath, nsMgr);

            if (element == null)
                throw new InvalidOperationException($"Element {dotPath.DotPathString} not found in node {node} using xpath {dotPath.ElementXPath}");
            return element;
        }

        public static XObject DotPathSelectElementOrAttributeOrThrow(this XNode node, DotPath dotPath, XmlNamespaceManager nsMgr)
        {
            var element = node.DotPathSelectElementOrThrow(dotPath, nsMgr);

            if (dotPath.AttributeName == null)
                return element;

            var attribute = element.Attribute(dotPath.AttributeName);
            if (attribute == null)
                throw new InvalidOperationException($"Attribute {dotPath.AttributeName} not found in element {element}");

            return attribute;
        }
    }
}