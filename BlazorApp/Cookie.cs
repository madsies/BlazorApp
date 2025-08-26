namespace BlazorApp;

public class CookieHelper
{
    public int basePrice;
    public string helperName;
    public int cps;
    public int amount;

    public CookieHelper(int price, string name, int cookiesPerSec)
    {
        basePrice = price;
        helperName = name;
        cps = cookiesPerSec;
        amount = 0;
    }


    public void addHelper(int count)
    {
        amount += count;
    }

}

public class Cookie
{
    private int cookieCount = 0;
    private int clickStrength = 1;
    private int CPS = 0;
    public List<CookieHelper> helpers = new List<CookieHelper>();

    public Cookie()
    {
        initialiseHelpers();
    }

    public void upgradeHelper(CookieHelper helper)
    {
        if (cookieCount >= helper.basePrice)
        {
            cookieCount -= helper.basePrice;
            helper.addHelper(1);
        }
        calculateCPS();
    }

    public int getCPS()
    {
        return CPS;
    }

    private void calculateCPS()
    {
        int count = 0;
        foreach (CookieHelper helper in helpers)
        {
            count += helper.cps * helper.amount;
        }
        CPS = count;
    }

    private void initialiseHelpers()
    {
        helpers.Add(new CookieHelper(10, "Grandma", 1));
        helpers.Add(new CookieHelper(100, "Farm", 3));
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
