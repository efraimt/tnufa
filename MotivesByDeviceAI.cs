using System;
using System.Collections.Generic;
using System.Text;

namespace ViPi2.Shared
{


    public class MotivesByDeviceAI
    {
        public DeviceQualityRating Rating { get; set; }

        /// <summary>
        /// דירוג מניע השלווה/ רוגע/ צורך במוצר איכותי הנובע מצורך ברוגע
        /// </summary>
        public int CalmnessQualityIndex
        {
            get
            {
                //מקדם החברה משפיע משמעותית על מדד האיכות-רוגע
                // יש להמיר את הערכים של דירוגי החברה המדורגים מ1-3 לערכים של מדד האיכות המדורגים מ1-5, מכאן נובעים ערכי המקדמים
                var companyCoefficient = 1.33;
                //מקדם הסדרה
                var seriesCoefficient = 0.33;
                var rating = (int)Math.Round(Rating.CompanyRating * companyCoefficient + Rating.SeriesQuality * seriesCoefficient);

                if (rating < 1) rating = 1;
                if (rating > 5) rating = 5;

                return rating;
            }
        }

        /// <summary>
        /// מדד המציין את היחס בין איכות החברה, כפי שהיא נתפסת, לבין הצורך ברוגע
        /// כלומר, כמה מלמד רכישת מכשיר מחברה זו ללא נתונים נוספים על מדד הרוגע
        /// מדובר על מדד הנגזר מכך שהאנשים ששלווה חשובה להם, יקנו וישקיעו במוצר איכותי יותר (או נחשב כזה) ו
        /// </summary>
        public Double CalmnessImpactFromCompanyQualityIndex
        {
            get
            {
                switch (Rating.CompanyRating)
                {
                    case 3: return 1;
                    case 2: return 0.8;
                    case 1: return 0.5;
                    default:
                        return 0.0;
                };
            }
        }

        /// <summary>
        /// מקדם המגדיר ההערכה לחשיבות של החדשנות בעיני הלקוח 
        /// (מדד המציין את חדשנות החברה כפי שהיא נתפסת, רמת החדשנות נגזרת מרמת האיכות)
        /// כנגזרת מהמכשיר שהוא רכש ללא נתונים נוספים
        /// </summary>
        public Double InnovationImpactFromCompanyQualityLevel
        {
            get
            {
                switch (Rating.CompanyRating)
                {
                    case 3: return 1;
                    case 2: return 0.8;
                    case 1: return 0.5;
                    default:
                        return 0.0; ;
                };
            }
        }


        /// <summary>
        /// מקדם המגדיר ההערכה לחשיבות של המחיר בעיני הלקוח, כנגזרת מהמכשיר שהוא רכש ללא נתונים נוספים
        /// </summary>
        public Double PriceImpactFromCompanyQualityLevel
        {
            get
            {
                switch (Rating.CompanyRating)
                {
                    case 3: return 1;
                    case 2: return 1.2;
                    case 1: return 1.5;
                    default:
                        return 0.0;
                };
            }
        }

        public int SocialRating
        {
            get
            {
                var level = (int)(
                             ((Rating.SeriesQuality + 2) * CalmnessImpactFromCompanyQualityIndex)
                           - ((3 - Rating.ModelLevel) * PriceImpactFromCompanyQualityLevel)
                           - ((3 - (int)Rating.AgeRating) * InnovationImpactFromCompanyQualityLevel)
                           );
                if (level < 1) level = 1;
                if (level > 5) level = 5;

                return level;
            }
        }


        public int InnovationIndex
        {
            get
            {
                var level = (int)(
                             ((Rating.SeriesQuality + 2) * CalmnessImpactFromCompanyQualityIndex)
                           - ((3 - Rating.ModelLevel) * PriceImpactFromCompanyQualityLevel)
                           - ((3 - (int)Rating.AgeRating) * InnovationImpactFromCompanyQualityLevel)
                           );
                if (level < 1) level = 1;
                if (level > 5) level = 5;

                return level;
            }
        }

        public string CalcString
        {
            get
            {
                string s;

                s = "((Device.SeriesQuality + 2) * CalmnessImpactFromCompanyQualityLevel) \n"
              + " - ((3 - Device.PriceLevel) * PriceImpactFromCompanyQualityLevel) \n"
             + " - ((3 - Device.PublishDate) * InnovationImpactFromCompanyQualityLevel) \n";

                return s;
            }
        }

    }

}
