using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CollateralCreatorAdminWeb.Service;

namespace CollateralCreatorAdminWeb.Models
{
    public class Notification
    {
        /// <summary>
        /// Gets or sets the status of the notification message.
        /// </summary>
        public NotificationStatus NotificationStatus { get; set; }

        /// <summary>
        /// Gets or sets the message text.
        /// </summary>
        public string Message { get; set; }
    }
}