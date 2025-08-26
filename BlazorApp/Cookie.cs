using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace BlazorApp;

public class Cookie
{
    private int cookieCount = 0;
    private int clickStrength = 1;


    public void Main()
    {
        
    }

    public int getCookies()
    {
        return cookieCount;
    }

    public void addCookies(int amount)
    {
        cookieCount += amount;
    }

    public void onClick()
    {
        cookieCount += clickStrength;
    }

}
