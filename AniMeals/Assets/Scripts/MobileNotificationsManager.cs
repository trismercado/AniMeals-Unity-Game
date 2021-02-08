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
        notification.Text = "It's a new day for us! Let's have some food to start the day right!";
        notification.SmallIcon = "my_custom_icon_id";
        notification.LargeIcon = "my_custom_large_icon_id";
        
        long elapsedTicks = eightmorning.Ticks - now.Ticks;
        TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
        float p = Convert.ToSingle(elapsedSpan.TotalSeconds);
        if (p < 0) {
            long nextDayTicks = elapsedTicks + TimeSpan.TicksPerDay;
            elapsedSpan = new TimeSpan(nextDayTicks);
            p = Convert.ToSingle(elapsedSpan.TotalSeconds);   
        }

        notification.FireTime = System.DateTime.Now.AddSeconds(p);
        notification.RepeatInterval = new TimeSpan(1, 0, 0, 0);
        AndroidNotificationCenter.SendNotification(notification, "channel_id");
    }

    public void noonnotif() {
        var notification = new AndroidNotification();
        notification.Title = "It's noon time baby!";
        notification.Text = "We're half way through the day. Don't forget to feed me lunch too~";
        notification.SmallIcon = "my_custom_icon_id";
        notification.LargeIcon = "my_custom_large_icon_id";
        
        long elapsedTicks = twelvenoon.Ticks - now.Ticks;
        TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
        float p = Convert.ToSingle(elapsedSpan.TotalSeconds);
        if (p < 0) {
            long nextDayTicks = elapsedTicks + TimeSpan.TicksPerDay;
            elapsedSpan = new TimeSpan(nextDayTicks);
            p = Convert.ToSingle(elapsedSpan.TotalSeconds);   
        }

        notification.FireTime = System.DateTime.Now.AddSeconds(p);
        notification.RepeatInterval = new TimeSpan(1, 0, 0, 0);
        AndroidNotificationCenter.SendNotification(notification, "channel_id");
    }

    public void evenotif() {
        var notification = new AndroidNotification();
        notification.Title = "Aaaah finally!";
        notification.Text = "Have you eaten? We definitely deserve some good meal to cap off today!";
        notification.SmallIcon = "my_custom_icon_id";
        notification.LargeIcon = "my_custom_large_icon_id";
        
        long elapsedTicks = sevenevening.Ticks - now.Ticks;
        TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
        float p = Convert.ToSingle(elapsedSpan.TotalSeconds);
        if (p < 0) {
            long nextDayTicks = elapsedTicks + TimeSpan.TicksPerDay;
            elapsedSpan = new TimeSpan(nextDayTicks);
            p = Convert.ToSingle(elapsedSpan.TotalSeconds);   
        }

        notification.FireTime = System.DateTime.Now.AddSeconds(p);
        notification.RepeatInterval = new TimeSpan(1, 0, 0, 0);
        AndroidNotificationCenter.SendNotification(notification, "channel_id");
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


}
