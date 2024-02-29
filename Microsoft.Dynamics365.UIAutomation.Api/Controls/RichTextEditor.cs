using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Microsoft.Dynamics365.UIAutomation.Api.Controls
{
    public class RichTextEditor
    {
        public class RichTextEditorReference
        {
            public const string RichTextEditor = "RichTextEditor";
            //private string 
        }
        public string Name { get; set; }
        public string Value { get; set; }

        public RichTextEditor(string name)
        {
            Name = name;
        }

        public string TryGetValue(WebClient client, RichTextEditor control)
        {
            Trace.TraceInformation("RichTextEditor.TryGetValue initiatied for " + control.Name);
            string field = control.Name;
            return client.Execute(client.GetOptions("TryGetValue"), browser =>
            {
                

                return "";

            });

        }
    
        public bool SetValue(WebClient client, string value) {
            return client.Execute(client.GetOptions("SetValue"), browser =>
            {
                browser.SwitchToFrame("//*[@class='fullPageContentEditorFrame']", browser.FindElement("//*[@class='fullPageContentEditorFrame']"));
                if (browser.HasElement("//iframe"))
                {
                    var innerFrame = browser.FindElement("//iframe");
                    browser.SwitchToFrame("//iframe", browser.FindElement("//iframe"));
                    var textInput = browser.FindElement("//body");
                    textInput.Click(client);
                    textInput.SetValue(client, value);
                    browser.SwitchToFrame("0");

                }
                return true;
            });

        }
    }
}
