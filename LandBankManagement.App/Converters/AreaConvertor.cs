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
            return area;
        }
    }
}
