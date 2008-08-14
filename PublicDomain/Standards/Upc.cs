using System;
using System.Collections.Generic;
using System.Text;
using PublicDomain;

namespace PublicDomain
{
    /// <summary>
    /// http://www.codeproject.com/KB/graphics/upc_a_barcode.aspx
    /// </summary>
    public class Upc
    {
        /// <summary>
        /// 
        /// </summary>
        public const UpcProductType DefaultProductType = UpcProductType.RegularUpcCodes;

        /// <summary>
        /// Gets the upc code.
        /// </summary>
        /// <param name="manufacturerCode">Maximum 5 digits</param>
        /// <returns></returns>
        public static string GetUpcCode(int manufacturerCode)
        {
            return GetUpcCode(manufacturerCode, RandomGenerationUtilities.GetRandomInteger(1, 99999));
        }

        /// <summary>
        /// Gets the upc code.
        /// </summary>
        /// <param name="manufacturerCode">Maximum 5 digits</param>
        /// <param name="productCode">Maximum 5 digits</param>
        /// <returns></returns>
        public static string GetUpcCode(int manufacturerCode, int productCode)
        {
            return GetUpcCode(manufacturerCode, productCode, DefaultProductType);
        }

        /// <summary>
        /// Gets the upc code.
        /// </summary>
        /// <param name="manufacturerCode">Maximum 5 digits</param>
        /// <param name="productCode">Maximum 5 digits</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string GetUpcCode(int manufacturerCode, int productCode, UpcProductType type)
        {
            string mfg = StringUtilities.PadIntegerLeft(manufacturerCode, 5);
            if (mfg.Length > 5)
            {
                mfg = mfg.Substring(0, 5);
            }
            string pc = StringUtilities.PadIntegerLeft(productCode, 5);
            if (pc.Length > 5)
            {
                pc = pc.Substring(0, 5);
            }
            int typeVal = (int)type;

            if (typeVal < 0 || typeVal > 9)
            {
                typeVal = (int)DefaultProductType;
            }

            string result = typeVal + mfg + pc;

            int checksum = GetChecksum(result);

            return result + checksum;
        }

        private static int GetChecksum(string result)
        {
            int x = 0;
            for (int i = 0; i < result.Length; i++)
            {
                int y = int.Parse(result[i].ToString());
                if ((y % 2) == 0)
                {
                    x += y;
                }
                else
                {
                    x += (y * 3);
                }
            }
            x = (10 - (x % 10)) % 10;
            return x;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum UpcProductType
    {
        /// <summary>
        /// Regular UPC codes
        /// </summary>
        RegularUpcCodes = 0,

        /// <summary>
        /// Reserved
        /// </summary>
        Reserved1 = 1,

        /// <summary>
        /// Weight items marked at the store
        /// </summary>
        WeightItems = 2,

        /// <summary>
        /// National Drug/Health-related code
        /// </summary>
        NationalDrugHealth = 3,

        /// <summary>
        /// No format restrictions, in-store use on non-food items
        /// </summary>
        InStoreNonFood = 4,

        /// <summary>
        /// Coupons
        /// </summary>
        Coupons = 5,

        /// <summary>
        /// Reserved
        /// </summary>
        Reserved6 = 6,

        /// <summary>
        /// Regular UPC codes
        /// </summary>
        RegularUpcCodes2 = 7,

        /// <summary>
        /// Reserved
        /// </summary>
        Reserved8 = 8,

        /// <summary>
        /// Reserved
        /// </summary>
        Reserved9 = 9
    }
}
