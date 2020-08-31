using System;
using System.Collections.Generic;
using System.IO;
using TechTalk.SpecRun;

namespace PaymentRequest.ISO20222.Specs.Drivers
{
    public class MessageTemplatesRegistry
    {
        private readonly TestRunContext _testRunContext;

        private readonly Dictionary<string, string> _templates = new Dictionary<string, string>();

        public MessageTemplatesRegistry(TestRunContext testRunContext)
        {
            _testRunContext = testRunContext;
        }

        public void Register(string templateName, string templateFileName)
        {
            if (string.IsNullOrEmpty(templateName))
                throw new ArgumentNullException(nameof(templateName));

            if (string.IsNullOrEmpty(templateFileName))
                throw new ArgumentNullException(nameof(templateFileName));

            _templates.Add(templateName, templateFileName);
        }

        public Stream GetTemplateStream(string templateName)
        {
            if (!_templates.TryGetValue(templateName, out var templateFileName))
            {
                throw new InvalidOperationException($"Template not registered: {templateName}");
            }

            return File.OpenRead(Path.Combine(_testRunContext.TestDirectory, "MessageTemplates", templateFileName));
        }
    }
}
