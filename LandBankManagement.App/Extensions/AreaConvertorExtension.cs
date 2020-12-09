using System;

namespace LandBankManagement.Extensions
{
    static public class AreaConvertor
    {

        static public decimal AcreToGuntas(this decimal acre)
        {
            return acre * Convert.ToDecimal(40.0087146);
        }

        static public decimal AcreToSqft(this decimal acre)
        {
            return acre * Convert.ToDecimal(43560d);
        }

        static public decimal GuntasToSqft(this decimal acre)
        {
            return acre * Convert.ToDecimal(1089.087d);
        }

        static public decimal SqftToAcres(this decimal sqft)
        {
            return sqft / Convert.ToDecimal(43560d);
        }

        static public decimal SqftToGuntas(this decimal sqft)
        {
            return sqft / Convert.ToDecimal(1089.087d);
        }

        static public decimal SqftToSqMts(this decimal sqft)
        {
            return sqft / Convert.ToDecimal(10.7639104d);
        }


    }
}
