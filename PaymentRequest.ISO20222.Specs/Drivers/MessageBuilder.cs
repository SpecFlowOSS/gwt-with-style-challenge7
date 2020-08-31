using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using PaymentRequest.ISO20222.Specs.Support;

namespace PaymentRequest.ISO20222.Specs.Drivers
{
    public class MessageBuilder
    {
        private readonly MessageTemplatesRegistry _messageTemplatesRegistry;
        private readonly HashSet<string> _updatedNodes = new HashSet<string>();
        private readonly XmlNamespaceManager _nsMgr;

        private string _templateName;
        private XDocument _document;

        public MessageBuilder(MessageTemplatesRegistry messageTemplatesRegistry)
        {
            _messageTemplatesRegistry = messageTemplatesRegistry;

            _nsMgr = new XmlNamespaceManager(new NameTable());
        }

        public void CreateMessage(string templateName)
        {
            _templateName = templateName;

            using var templateStream = _messageTemplatesRegistry.GetTemplateStream(templateName);
            _document = XDocument.Load(templateStream);

            if (_document?.Root == null)
                throw new InvalidOperationException($"Template {templateName} is empty.");

            var defaultNamespace = _document.Root.GetDefaultNamespace();
            _nsMgr.AddNamespace(DotPath.NamespacePrefix, defaultNamespace.NamespaceName);
        }

        public void SetupNodes(DotPath nodeDotPath, IEnumerable<IDictionary<string, string>> tokensList)
        {
            foreach (var tokens in tokensList)
            {
                SetupNode(nodeDotPath, tokens);
            }
        }

        public void SetupNode(DotPath nodeDotPath, IDictionary<string, string> tokens)
        {
            var node = _document.Root.DotPathSelectElementOrThrow(nodeDotPath, _nsMgr);

            if (_updatedNodes.Contains(nodeDotPath.DotPathString))
            {
                //This node has been already updated in the template with test data
                //Let's clone it, add the clone as next sibling and use the cloned node for updating
                var clone = new XElement(node);
                node.AddAfterSelf(clone);
                node = clone;
            }
            else
            {
                _updatedNodes.Add(nodeDotPath.DotPathString);
            }

            foreach (var token in tokens)
            {
                var tokenDotPath = new DotPath(token.Key);

                var elementOrAttribute = node.DotPathSelectElementOrAttributeOrThrow(tokenDotPath, _nsMgr);

                elementOrAttribute.SetValue(token.Value);
            }
        }

        public string GetMessage()
        {
            var message = _document.ToString();

            //Make the final message visible in the test report
            Console.WriteLine($"Final {_templateName} message: \n{message}");

            return message;
        }
    }
}
