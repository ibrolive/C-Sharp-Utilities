using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Utilities
{
    public class EmailTemplateHelper
    {
        Dictionary<string, string> Templates = new Dictionary<string, string>()
        {
            // the last two templates will serve as a guide. please do not remove them.
            { "SampleTemplate", "This template contains two variables. The value of the first variable is {0}. And the value of the second variable is {1}."},
            {"TemplateName", "TemplateContent{0}{1}"},
        };

        public string GetTemplateContentFromFileSystem(string ServerMappedPath, string TemplateName)
        {
            string templateContent;
            //Read template file from the App_Data folder
            using (var sr = new StreamReader(ServerMappedPath + "Sample_Email_Template.txt"))
            {
                templateContent = sr.ReadToEnd();
            }
            if (string.IsNullOrEmpty(templateContent))
            {
                throw new Exception("Blank template or template not found.");
            }
            return templateContent;
        }


        public string GetTemplateContent(string TemplateName)
        {
            string templateContent = GetAzureFileContentAsString(TemplateName);
            if (string.IsNullOrEmpty(templateContent))
            {
                throw new Exception("Blank template or template not found.");
            }
            return templateContent;
        }

        private string GetAzureFileContentAsString(string FileName)
        {
            // TODO: Complete GetTemplateContent implementation
            string template = "";
            if(Templates.ContainsKey(FileName))
            {
                template = Templates[FileName];
            }
            return template;
        }
    }
}
