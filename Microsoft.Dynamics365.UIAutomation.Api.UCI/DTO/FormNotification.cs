namespace Microsoft.Dynamics365.UIAutomation.Api.UCI.DTO
{
    public class FormNotification
    {
        public FormNotificationType Type { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return $"{Type}: {Message}";
        }
    }
}
