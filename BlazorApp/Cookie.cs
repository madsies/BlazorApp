using System;
using System.Timers;

namespace BlazorApp;

public class Cookie
{
    private static System.Timers.Timer clock;
    private int cookieCount;
    private int clickStrength;


    public static void Main()
    {
        clock = new System.Timers.Timer(1000);
        clock.Elapsed += OnTimedEvent;
    }

    public static void onClick()
    {
        
    }

    private static void OnTimedEvent(Object source, ElapsedEventArgs e)
    {

    }
}
