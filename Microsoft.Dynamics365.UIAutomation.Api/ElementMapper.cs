using Microsoft.Dynamics365.UIAutomation.Api.Controls;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    public class ElementMapper
    {
        #region elements
        public Lookup.AdvancedLookupReference AdvancedLookupReference;
        public Navigation.ApplicationReference ApplicationReference;
        public BusinessProcessFlow.BusinessProcessFlowReference BusinessProcessFlowReference;
        public CommandBar.CommandBarReference CommandBarReference;
        public CustomerServiceCopilot.CustomerServiceCopilotReference CustomerServiceCopilotReference;
        public Dashboard.DashboardReference DashboardReference;
        public Dialogs.DialogsReference DialogsReference;
        public Entity.EntityReference EntityReference;
        public GlobalSearch.GlobalSearchReference GlobalSearchReference;
        public Grid.GridReference GridReference;
        public OnlineLogin.LoginReference LoginReference;
        public Lookup.LookupReference LookupReference;
        public Navigation.NavigationReference NavigationReference;
        public Telemetry.PerformanceWidgetReference PerformanceWidgetReference;
        public PowerApp.PowerAppReference PowerAppReference;
        public QuickCreate.QuickCreateReference QuickCreateReference;
        public RelatedGrid.RelatedReference RelatedGridReference;
        public SubGrid.SubGridReference SubGridReference;
        public Timeline.TimelineReference TimelineReference;
        #endregion
        #region controls
        public RichTextEditor.RichTextEditorReference RichTextEditorReference;
        #endregion
        public ElementMapper(IConfiguration config)
        {
            MapControls(config);
            MapElements(config);
        }

        private void MapControls(IConfiguration config)
        {
            RichTextEditorReference = new RichTextEditor.RichTextEditorReference();
            config.GetSection(RichTextEditor.RichTextEditorReference.RichTextEditor).Bind(RichTextEditorReference);
        }

        private void MapElements(IConfiguration config)
        {
            ApplicationReference = new Navigation.ApplicationReference();
            config.GetSection(Navigation.ApplicationReference.Application).Bind(ApplicationReference);
            AdvancedLookupReference = new Lookup.AdvancedLookupReference();
            config.GetSection(Lookup.AdvancedLookupReference.AdvancedLookup).Bind(AdvancedLookupReference);
            BusinessProcessFlowReference = new BusinessProcessFlow.BusinessProcessFlowReference();
            config.GetSection(BusinessProcessFlow.BusinessProcessFlowReference.BusinessProcessFlow).Bind(BusinessProcessFlowReference);
            CommandBarReference = new CommandBar.CommandBarReference();
            config.GetSection(CommandBar.CommandBarReference.CommandBar).Bind(CommandBarReference);
            CustomerServiceCopilotReference = new CustomerServiceCopilot.CustomerServiceCopilotReference();
            config.GetSection(CustomerServiceCopilot.CustomerServiceCopilotReference.CustomerServiceCopilot).Bind(CustomerServiceCopilotReference);
            DashboardReference = new Dashboard.DashboardReference();
            config.GetSection(Dashboard.DashboardReference.Dashboard).Bind(DashboardReference);
            DialogsReference = new Dialogs.DialogsReference();
            config.GetSection(Dialogs.DialogsReference.Dialogs).Bind(DialogsReference);
            EntityReference = new Entity.EntityReference();
            config.GetSection(Entity.EntityReference.Entity).Bind(EntityReference);
            GlobalSearchReference = new GlobalSearch.GlobalSearchReference();
            config.GetSection(GlobalSearch.GlobalSearchReference.GlobalSearch).Bind(GlobalSearchReference);
            GridReference = new Grid.GridReference();
            config.GetSection(Grid.GridReference.Grid).Bind(GridReference);
            LoginReference = new OnlineLogin.LoginReference();
            config.GetSection(OnlineLogin.LoginReference.Login).Bind(LoginReference);
            LookupReference = new Lookup.LookupReference();
            config.GetSection(Lookup.LookupReference.Lookup).Bind(LookupReference);
            NavigationReference = new Navigation.NavigationReference();
            config.GetSection(Navigation.NavigationReference.Navigation).Bind(NavigationReference);
            PerformanceWidgetReference = new Telemetry.PerformanceWidgetReference();
            config.GetSection(Telemetry.PerformanceWidgetReference.PerformanceWidget).Bind(PerformanceWidgetReference);
            PowerAppReference = new PowerApp.PowerAppReference();
            config.GetSection(PowerApp.PowerAppReference.PowerApp).Bind(PowerAppReference);
            QuickCreateReference = new QuickCreate.QuickCreateReference();
            config.GetSection(QuickCreate.QuickCreateReference.QuickCreate).Bind(QuickCreateReference);
            RelatedGridReference = new RelatedGrid.RelatedReference();
            config.GetSection(RelatedGrid.RelatedReference.RelatedGrid).Bind(RelatedGridReference);
            SubGridReference = new SubGrid.SubGridReference();
            config.GetSection(SubGrid.SubGridReference.SubGrid).Bind(SubGridReference);
            TimelineReference = new Timeline.TimelineReference();
            config.GetSection(Timeline.TimelineReference.Timeline).Bind(TimelineReference);
        }
    }
}
