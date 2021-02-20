using System;
using LandBankManagement.Data.Data;
using LandBankManagement.Extensions;

namespace LandBankManagement.ViewModels
{
    public class AreaConvertor
    {
        public static Area ConvertArea(decimal acres, decimal guntas, decimal aanas)
        {
            var totalSqFt = acres.AcreToSqft() + guntas.GuntasToSqft() + aanas.AanasToSqft();
            
            
            var area = new Area()
            {
                Acres = acres,
                Guntas = guntas,
                Anas = aanas,
                SqFt = totalSqFt,
                SqMeters = totalSqFt.SqftToSqMts(),
            };

            if (area.Anas >= 16)
            {
                var remainder = area.Anas % 16 ;
               // int quotient = Convert.ToInt32(area.Anas / 16);
                var quotient = Math.Truncate(area.Anas / 16);

                area.Anas = remainder;
                area.Guntas += quotient;
            }

            if (area.Guntas >= 40)
            {
                var remainder = area.Guntas % 40;
                //int quotient = Convert.ToInt32(area.Guntas / 40);
                var quotient = Math.Truncate(area.Guntas / 40);
                area.Guntas = remainder;
                area.Acres += quotient;
            }
            
            return area;
        }
    }
}
