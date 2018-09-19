using Microsoft.Dynamics365.UIAutomation.Browser;

namespace Microsoft.Dynamics365.UIAutomation.Api.Extensions
{
    public static class EntityExtensions
    {
        public static BrowserCommandResult<string> GetLookupValue(this Entity entity, string attribute) =>
            entity.GetValue(new LookupItem {Name = attribute});

        public static void SetLookupValue(this Entity entity, string attribute, string value)
        {
            entity.SetValue(attribute, value);
            entity.SelectLookup(attribute, 0);
        }

        public static void OverrideLookupValue(this Browser xrmBrowser, string attribute, string value)
        {
            xrmBrowser.Entity.SelectLookup(attribute);
            xrmBrowser.Lookup.Search(value);
            xrmBrowser.Lookup.SelectItem(value);
            xrmBrowser.Lookup.Add(500);
        }
    }
}