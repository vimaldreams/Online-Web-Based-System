using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CollateralCreatorAdminWeb.Service;

namespace CollateralCreatorAdminWeb.Models
{
    public class Notifications
    {
        private const string DictionaryName = "NOTIFICATIONS";

        private IList<Notification> _notifications;

        public Notifications(TempDataDictionary tempDataDictionary)
        {
            if (!tempDataDictionary.ContainsKey(DictionaryName))
            {
                tempDataDictionary[DictionaryName] = new List<Notification>();
            }
            _notifications = tempDataDictionary[DictionaryName] as IList<Notification>;
        }

        public IEnumerable<Notification> Current
        {
            get { return _notifications; }
        }

        public void Add(NotificationStatus notificationStatus, string message)
        {
            _notifications.Add(new Notification { NotificationStatus = notificationStatus, Message = message });
        }
    }
}