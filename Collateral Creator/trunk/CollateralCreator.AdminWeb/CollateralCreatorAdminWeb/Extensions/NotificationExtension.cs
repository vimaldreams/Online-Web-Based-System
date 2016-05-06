﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CollateralCreatorAdminWeb.Models;
using CollateralCreatorAdminWeb.Service;

namespace CollateralCreatorAdminWeb.Extensions
{
    public static class NotificationExtensions
    {
        // Action result extension method
        // Used within actions to add Notifications to results 
        public static ActionResult WithNotification(this ActionResult actionResult)
        {
            return new WithNotificationResult(actionResult);
        }

        // Action result extension method
        // Used within actions to add Notifications to results 
        public static ActionResult WithNotification(this ActionResult actionResult, NotificationStatus status, string message)
        {
            return new WithNotificationResult(actionResult, status, message);
        }

        // Controller extension method
        // Used to hold retrieve the notifications from tempdata
        public static Notifications Notifications(this ControllerBase controller)
        {
            return new Notifications(controller.TempData);
        }

        // Html Helper extensions method
        // Used within Views to get the notifications THefrom temp data
        public static Notifications Notifications(this HtmlHelper htmlHelper)
        {
            return new Notifications(htmlHelper.ViewContext.TempData);
        }
    }
}