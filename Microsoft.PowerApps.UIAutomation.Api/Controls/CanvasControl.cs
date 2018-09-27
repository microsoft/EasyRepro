using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.PowerApps.UIAutomation.Api
{
    public class CanvasControl
    {
        public String Name { get; set; }
        public String ControlType { get; set; }
        public List<CanvasControlProperty> ControlProperties { get; set; }

        public CanvasControl()
        {
            this.ControlProperties = new List<CanvasControlProperty>();
        }
        
    }
    public class CanvasControlProperty
    {
        public string Label { get; set; }
        public object Value { get; set; }
        public CanvasControlProperty(string label, string value)
        {
            this.Label = label;
            this.Value = value;
        }
    }
}
