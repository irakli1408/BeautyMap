using System.ComponentModel;

namespace BeautyMap.NotificationManager.Enums
{
    public enum SendNotificationTypes
    {
        [Description("პაროლის აღდგენა")]
        ResetPassword = 1,
        [Description("ადმინის რეგისტრაცია")]
        SetAdminPassword,
        [Description("ემაილის დადასტურება")]
        ConfirmEmail,
        [Description("მომხმარებლის დაბლოკვა")]
        AccountIsBlocked,
        [Description("შეტყობინება ჩარიცხვაზე")]
        NotifyAdminAboutPayment,
        [Description("შეტყობინება ჩარიცხვაზე მომხმარებელს")]
        NotifyUserAboutPayment,
    }
}
