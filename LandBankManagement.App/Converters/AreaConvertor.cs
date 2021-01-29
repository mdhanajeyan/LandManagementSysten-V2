using System;
using LandBankManagement.Data.Data;
using LandBankManagement.Extensions;

namespace LandBankManagement.Converters
{
    public class AreaConvertor
    {
        public static Area ConvertArea(decimal acres, decimal guntas, decimal aanas)
        {
            var totalGuntas = acres.AcreToGuntas() + guntas + aanas.AanasToGunta();
           
            var totalSqFt = totalGuntas.GuntasToSqft();
            var area = new Area()
            {
                Acres = totalSqFt.SqftToAcres(),
                Guntas = totalSqFt.SqftToGuntas(),
                SqMeters = totalSqFt.SqftToSqMts(),
                Anas=aanas,
                SqFt = totalSqFt
            };

            if (area.Anas >= 16)
            {
                var remainder = area.Anas % 16 ;
                int quotient = Convert.ToInt32(area.Anas / 16);
                
                area.Anas = remainder;
                area.Guntas += quotient;
            }

            if (area.Guntas >= 40)
            {
                var remainder = area.Guntas % 40;
                int quotient = Convert.ToInt32(area.Guntas / 40);

                area.Guntas = remainder;
                area.Acres += quotient;
            }
            
            return area;
        }
    }
}
