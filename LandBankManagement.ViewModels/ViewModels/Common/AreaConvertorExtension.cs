using System;

namespace LandBankManagement.Extensions
{
    static public class AreaConvertor
    {

        static public decimal AcreToGuntas(this decimal acre)
        {
            return acre * Convert.ToDecimal(40);
        }

        static public decimal AcreToSqft(this decimal acre)
        {
            return acre * Convert.ToDecimal(43560d);
        }

        static public decimal GuntasToSqft(this decimal acre)
        {
            return acre * Convert.ToDecimal(1089);
        }

        static public decimal AanasToSqft(this decimal aana)
        {
            return aana * Convert.ToDecimal(342.25);
        }

        static public decimal AanasToGunta(this decimal aana)
        {
            return aana / Convert.ToDecimal(16);
        }

        static public decimal SqftToAcres(this decimal sqft)
        {
            return sqft / Convert.ToDecimal(43560d);
        }

        static public decimal SqftToGuntas(this decimal sqft)
        {
            return sqft / Convert.ToDecimal(1089);
        }

        static public decimal SqftToSqMts(this decimal sqft)
        {
            return sqft / Convert.ToDecimal(10.764d);
        }


    }
}
