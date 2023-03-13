using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_IOS
using Unity.Notifications.iOS;
#endif

#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif

public class schedule_script : MonoBehaviour
{
    #if UNITY_IOS

    // Start is called before the first frame update
    void Start()
    {
        //sets the base playerprefs for my two notification managing playerprefs 
        if (!(PlayerPrefs.HasKey("LastFiredAM")))
        {
            PlayerPrefs.SetInt("LastFiredAM", -2);
        }

        if (!(PlayerPrefs.HasKey("LastFiredPM")))
        {
            PlayerPrefs.SetInt("LastFiredPM", -2);
        }

        if (!(PlayerPrefs.HasKey("WasMorningA")))
        {
            PlayerPrefs.SetInt("WasMorningA", 1);
        }

        if (!(PlayerPrefs.HasKey("WasNightA")))
        {
            PlayerPrefs.SetInt("WasNightA", 1);
        }

        // 1 minutetime interval trigger
        var nowTrigger = new iOSNotificationTimeIntervalTrigger()
        {
            TimeInterval = new TimeSpan(0, 1, 0),
            Repeats = false
        };

        //one day time interval trigger
        var timeTrigger = new iOSNotificationTimeIntervalTrigger()
        {
            TimeInterval = new TimeSpan(24, 0, 0),
            Repeats = false
        };

        var water_notif_AM_A = new iOSNotification()
        {
            // You can optionally specify a custom identifier which can later be 
            // used to cancel the notification, if you don't set one, a unique 
            // string will be generated automatically.
            Identifier = "waterer",
            Title = "Water thine plant", //top line, bolded
            Body = "Notification scheduled at " + DateTime.Now.ToShortTimeString(),
            //Body = "You know you want to", // Main body, non-bolded
            Subtitle = "Not even sure what this one is", // Second line, also bolded
            ShowInForeground = true,
            ForegroundPresentationOption = PresentationOption.Alert,
            CategoryIdentifier = "category_a",
            ThreadIdentifier = "thread1",
            Trigger = timeTrigger,
        };

        var water_notif_AM_B = new iOSNotification()
        {
            // You can optionally specify a custom identifier which can later be 
            // used to cancel the notification, if you don't set one, a unique 
            // string will be generated automatically.
            Identifier = "waterer",
            Title = "Water thine plant", //top line, bolded
            Body = "Notification scheduled at " + DateTime.Now.ToShortTimeString(),
            //Body = "You know you want to", // Mainy body, non-bolded
            Subtitle = "Not even sure what this one is", // second line, also bolded
            ShowInForeground = true,
            ForegroundPresentationOption = PresentationOption.Alert,
            CategoryIdentifier = "category_a",
            ThreadIdentifier = "thread1",
            Trigger = timeTrigger,
        };

        var water_notif_PM_A = new iOSNotification()
        {
            Identifier = "waterer",
            Title = "Water thine plant",
            Body = "You know you want to",
            Subtitle = "Still couldn't tell you",
            ShowInForeground = true,
            ForegroundPresentationOption = PresentationOption.Alert,
            CategoryIdentifier = "category_a",
            ThreadIdentifier = "thread1",
            Trigger = timeTrigger,
        };

        var water_notif_PM_B= new iOSNotification()
        {
            Identifier = "waterer",
            Title = "Water thine plant",
            Body = "You know you want to",
            Subtitle = "Still couldn't tell you",
            ShowInForeground = true,
            ForegroundPresentationOption = PresentationOption.Alert,
            CategoryIdentifier = "category_a",
            ThreadIdentifier = "thread1",
            Trigger = timeTrigger,
        };

        var sun_notif_A = new iOSNotification()
        {
            Identifier = "sun",
            Title = "Sun thine plant",
            Body = "shouldn't be too difficult ",
            Subtitle = "Not even sure what this one is",
            ShowInForeground = true,
            ForegroundPresentationOption = PresentationOption.Alert,
            CategoryIdentifier = "category_a",
            ThreadIdentifier = "thread1",
            Trigger = timeTrigger,
        };

        var sun_notif_B = new iOSNotification()
        {
            Identifier = "sun",
            Title = "Sun thine plant",
            Body = "shouldn't be too difficult ",
            Subtitle = "Not even sure what this one is",
            ShowInForeground = true,
            ForegroundPresentationOption = PresentationOption.Alert,
            CategoryIdentifier = "category_a",
            ThreadIdentifier = "thread1",
            Trigger = timeTrigger,
        };

        //If it is afternoon
        if (DateTime.Now.Hour >= 12)
        {
            // If there hasn't been a notification scheduled yet in the afternoon today 
            if (PlayerPrefs.GetInt("LastFiredPM") != DateTime.Now.Day)
            {
                // Checks to see which notification was scheduled last and schedules the other one 
                if(PlayerPrefs.GetInt("WasNightA") == 1)
                {
                    iOSNotificationCenter.ScheduleNotification(water_notif_PM_A);
                    PlayerPrefs.SetInt("WasNightA", 0);
                }
                else 
                {
                    iOSNotificationCenter.ScheduleNotification(water_notif_PM_B);
                    PlayerPrefs.SetInt("WasNightA", 1);
                }
            

                PlayerPrefs.SetInt("LastFiredPM", DateTime.Now.Day);
            }
        }
        else // If it is morning 
        {
            // This is the exact same setup as the afternoon notifs
            if (PlayerPrefs.GetInt("LastFiredAM") != DateTime.Now.Day)
            {
                if(PlayerPrefs.GetInt("WasMorningA") == 1)
                {
                    iOSNotificationCenter.ScheduleNotification(water_notif_AM_A);
                    iOSNotificationCenter.ScheduleNotification(sun_notif_A);
                    PlayerPrefs.SetInt("WasMorningA", 0);
                }
                else
                {
                    iOSNotificationCenter.ScheduleNotification(water_notif_AM_B);
                    iOSNotificationCenter.ScheduleNotification(sun_notif_B);
                    PlayerPrefs.SetInt("WasMorningA", 1);
                }

                PlayerPrefs.SetInt("LastFiredAM", DateTime.Now.Day);
            }
        }

        // Am doing it this way because it seemed like it would be the most effective at the time
        // This does cause notifications to potentially go off after they've opened the app on a given day
        // But at the time, that seemed fine to me. To undo this, all you would need to do is remove the if/else check
        // Notifications overwrite themselves if you schedule one while there's an instance of it already scheduled 
        // So removing the if/else would cause a notif to overwrite itself and instead just go off the next day
    }

    // Update is called once per frame
    #endif

    #if UNITY_ANDROID

    void Start()
    {
        // Remove Notifications currently displayed 
        AndroidNotificationCenter.CancelAllDisplayedNotifications();

        //Make the notification channel and initializes it
        var channel = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Notifications Channel",
            Importance = Importance.Default,
            Description = "Scheduled notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);

        //Creates our notifications that we will send using the app
        var sunlight_and_morning_water = new AndroidNotification();
        sunlight_and_morning_water.Title = "Give your PlantWear some sun and water it!";
        sunlight_and_morning_water.Text = "Water and sun are very important!";
        sunlight_and_morning_water.FireTime = System.DateTime.Now.AddHours(24);
        var morning = AndroidNotificationCenter.SendNotification(sunlight_and_morning_water, "channel_id");

        var night_water = new AndroidNotification();
        night_water.Title = "Give your PlantWear some sun and water it!";
        night_water.Text = "Water and sun are very important!";
        night_water.FireTime = System.DateTime.Now.AddHours(24);
        var night = AndroidNotificationCenter.SendNotification(night_water, "channel_id");

        // Schedules the morning notification iff there isn't one already and it's the morning 
        if (!(AndroidNotificationCenter.CheckScheduledNotificationStatus(morning) == NotificationStatus.Scheduled) && DateTime.Now.Hour < 12)
        {
            // This should literally never come up but it's here just in case 
            //AndroidNotifications.CancelNotification(morning);

            // Schedules the thing 
            AndroidNotificationCenter.SendNotification(sunlight_and_morning_water, "channel_id");
        }

        // Schedules the night notification iff there isn't one already and it's afternoon 
        if (!(AndroidNotificationCenter.CheckScheduledNotificationStatus(night) == NotificationStatus.Scheduled) && DateTime.Now.Hour >= 12)
        {
            // This should literally never come up but it's here just in case 
            //AndroidNotifications.CancelNotification(night);

            // Schedules the thing 
            AndroidNotificationCenter.SendNotification(night_water, "channel_id");
        }
        // Never actually got the chance to test these (I am a sad iOS user), but I checked pretty rigorously agains the documentation
        // So I would be surprised if it didn't
        // This one works like the iOS one would if you took out the if/else checks 
    }

    #endif
}
