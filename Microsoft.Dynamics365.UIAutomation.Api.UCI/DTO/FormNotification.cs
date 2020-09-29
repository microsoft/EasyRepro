using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI.DTO
{
    public class FormNotification
    {
        private static readonly Dictionary<string, FormNotificationType> _classToTypeMap = new Dictionary<string, FormNotificationType>{
            {"markaslost-symbol", FormNotificationType.Error},
            {"warning-symbol", FormNotificationType.Warning},
            {"informationicon-symbol", FormNotificationType.Information},
            {"locked-symbol", FormNotificationType.Locked}
        };


        public FormNotificationType Type { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return $"{Type}: {Message}";
        }

        public void SetTypeFromClass(string classes)
        {
            string[] classesArr = classes.ToLower().Split(' ');
            string classFound = classesArr.FirstOrDefault(c => _classToTypeMap.ContainsKey(c));

            if (classFound == null)
                throw new InvalidOperationException($"Unknown notification type. Current class: {classes}");

            Type = _classToTypeMap[classFound];
        }
        
    }
}
