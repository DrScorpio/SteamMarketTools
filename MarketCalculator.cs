using System;

public class SteamMarketCalculator
{
    private const double marketTax = 0.8696;
    
    public static double TaxedPrice(double sellingPrice)
    {
        return sellingPrice * marketTax;
    }

    public static double GetDiscount(double primeCost,double sellingprice) 
    {
        var taxed = TaxedPrice(sellingprice);
        return Math.Round(primeCost/sellingprice,2);
    }

}
