using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif

public class MobileNotificationsManager : MonoBehaviour
{


    public AndroidNotificationChannel defaultNotificationChannel; 

    TimeSpan now = DateTime.Now.TimeOfDay;
    TimeSpan eightmorning = new TimeSpan(8, 00, 00); //8:00 AM
    TimeSpan twelvenoon = new TimeSpan(12, 00, 00); //12:00 NN
    TimeSpan sevenevening = new TimeSpan(19, 00, 00); //19:00 NN


    private int identifier;
    private string savePath;

    // Start is called before the first frame update
    void Start()
    {
        AndroidNotificationCenter.CancelAllDisplayedNotifications();

        defaultNotificationChannel = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Default Channel",
            Importance = Importance.High,
            Description = "Generic notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(defaultNotificationChannel);

        
        savePath = Application.persistentDataPath + "/gamesave.anmls";
        if (File.Exists(savePath)) {
            CheckUpNotif();
        } else {
            morningnotif();
            noonnotif();
            evenotif();
            // trialnotif();
        }
        
        
    }

    public void morningnotif() {
        var notification = new AndroidNotification();
        notification.Title = "Good Morning!";
        notification.Text = "Let's have some food to start the day right!";
        notification.SmallIcon = "my_custom_icon_id";
        notification.LargeIcon = "my_custom_large_icon_id";
        
        long elapsedTicks = eightmorning.Ticks - now.Ticks;
        TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
        float p = Convert.ToSingle(elapsedSpan.TotalSeconds);
        if (p < 0) {  
            notification.FireTime = new System.DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0).AddDays(1);
        } else {            
            notification.FireTime = new System.DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);
        }

        notification.RepeatInterval = new TimeSpan(1, 0, 0, 0);
        AndroidNotificationCenter.SendNotification(notification, "channel_id");
    }

    public void noonnotif() {
        var notification = new AndroidNotification();
        notification.Title = "It's noon time baby!";
        notification.Text = "Don't forget to feed me lunch too~";
        notification.SmallIcon = "my_custom_icon_id";
        notification.LargeIcon = "my_custom_large_icon_id";
        
        long elapsedTicks = twelvenoon.Ticks - now.Ticks;
        TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
        float p = Convert.ToSingle(elapsedSpan.TotalSeconds);
        if (p < 0) {  
            notification.FireTime = new System.DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 0, 0).AddDays(1);
        } else {            
            notification.FireTime = new System.DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 0, 0);
        }

        notification.RepeatInterval = new TimeSpan(1, 0, 0, 0);
        AndroidNotificationCenter.SendNotification(notification, "channel_id");
    }

    public void evenotif() {
        var notification = new AndroidNotification();
        notification.Title = "Aaaah finally!";
        notification.Text = "Let's have a good meal to cap off the day!";
        notification.SmallIcon = "my_custom_icon_id";
        notification.LargeIcon = "my_custom_large_icon_id";
        
        long elapsedTicks = sevenevening.Ticks - now.Ticks;
        TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
        float p = Convert.ToSingle(elapsedSpan.TotalSeconds);
        if (p < 0) {  
            notification.FireTime = new System.DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 19, 0, 0).AddDays(1);
        } else {            
            notification.FireTime = new System.DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 19, 0, 0);
        }

        notification.RepeatInterval = new TimeSpan(1, 0, 0, 0);
        AndroidNotificationCenter.SendNotification(notification, "channel_id");
    }

    public void salenotif() {
        DateTime regDate = Convert.ToDateTime(PlayerPrefs.GetString("isRegisteredKeyName"));

        var notification = new AndroidNotification();
        notification.Title = "50% Off on ALL Items!";
        notification.Text = "Don't lose your chance! Our 2nd week sale is only for today!";
        notification.SmallIcon = "my_custom_icon_id";
        notification.LargeIcon = "my_custom_large_icon_id";
        notification.FireTime = regDate.AddDays(14);
        AndroidNotificationCenter.SendNotification(notification, "channel_id");
        var id = AndroidNotificationCenter.SendNotification(notification, "channel_id");

    }

    public void trialnotif() {
        var notification = new AndroidNotification();
        notification.Title = "Trial Notif";
        notification.Text = "Choi Seungcheol Best Leader";
        notification.SmallIcon = "my_custom_icon_id";
        notification.LargeIcon = "my_custom_large_icon_id";
        notification.FireTime = System.DateTime.Now.AddSeconds(10);
        AndroidNotificationCenter.SendNotification(notification, "channel_id");
    }

    public void CheckUpNotif() {
        var notification = new AndroidNotification();
        notification.Title = "Are you still there?";
        notification.Text = "I'm really hungry :(";
        notification.SmallIcon = "my_custom_icon_id";
        notification.LargeIcon = "my_custom_large_icon_id";
        notification.FireTime = System.DateTime.Now.AddHours(20);
        
        var id = AndroidNotificationCenter.SendNotification(notification, "channel_id");


        if (AndroidNotificationCenter.CheckScheduledNotificationStatus(id)== NotificationStatus.Scheduled) {
            AndroidNotificationCenter.CancelNotification(id);
            AndroidNotificationCenter.SendNotification(notification, "channel_id");
        }

    }
}
