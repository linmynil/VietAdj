using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable
{
    public class CConvert
    {
        public static int ToInt(object obj)
        {
            if (obj == null || obj.ToString().Length == 0) return 0;
            try
            {
                return Convert.ToInt32(obj);
            }
            catch
            {
                return 0;
            }
        }
        public static int ToInt(object obj, int DefaultVal)
        {
            if (obj == null || obj.ToString().Length == 0) return DefaultVal;
            return ToInt(obj);
        }

        public static decimal ToDecimal(object obj)
        {
            if (obj == null || obj.ToString().Length == 0) return 0;
            decimal val = 0;
            try
            {
                val = Convert.ToDecimal(obj);
            }
            catch
            {
                return 0;
            }

            return val;
        }
        public static decimal ToDecimal(object obj, decimal DefaultVal)
        {
            if (obj == null || obj.ToString().Length == 0) return DefaultVal;
            decimal val = DefaultVal;
            try
            {
                val = ToDecimal(obj);
            }
            catch
            {
                return DefaultVal;
            }

            return val;
        }

        public static double ToDouble(object obj)
        {
            if (obj == null || obj.ToString().Length == 0) return 0;
            double val = 0;
            try
            {
                val = Convert.ToDouble(obj);
            }
            catch
            {
            }

            return val;
        }
        public static double ToDouble(object obj, double DefaultVal)
        {
            if (obj == null || obj.ToString().Length == 0) return DefaultVal;
            double val = 0;
            try
            {
                val = Convert.ToDouble(obj);
            }
            catch
            {
            }

            return val;
        }

        public static float ToFloat(object obj)
        {
            if (obj == null || obj.ToString().Length == 0) return 0;
            float val = 0;
            try
            {
                val = float.Parse(obj.ToString());
            }
            catch
            {
            }

            return val;
        }
        public static float ToFloat(object obj, float DefaultVal)
        {
            if (obj == null || obj.ToString().Length == 0) return DefaultVal;
            float val = 0;
            try
            {
                val = float.Parse(obj.ToString());
            }
            catch
            {
            }

            return val;
        }

        public static long ToLong(object obj)
        {
            if (obj == null || obj.ToString().Length == 0) return 0;
            long val = 0;
            try
            {
                val = long.Parse(obj.ToString());
            }
            catch
            {
            }

            return val;
        }
        public static long ToLong(object obj, long DefaultVal)
        {
            if (obj == null || obj.ToString().Length == 0) return DefaultVal;
            long val = 0;
            try
            {
                val = Convert.ToInt64(obj);
            }
            catch
            {
            }
            return val;
        }

        public static string ToStr(object obj)
        {
            if (obj == null || obj.ToString().Length == 0) return "";
            return Convert.ToString(obj).Trim();
        }
        public static string ToStr(object obj, string DefaultVal)
        {
            if (obj == null || obj.ToString().Length == 0) return DefaultVal;
            return Convert.ToString(obj).Trim();
        }
        public static string ToTimeSpan(object obj)
        {
            if (obj == null || obj.ToString().Length == 0) return "";
            try
            {
                return DateTime.Parse(obj.ToString()).ToString("HH:mm");
            }
            catch
            {
                return "";
            }
        }
        public static DateTime ToDateTime(object obj)
        {
            if (obj == null || obj.ToString().Length == 0) return DateTime.Now;
            try
            {
                return DateTime.Parse(obj.ToString());
            }
            catch
            {
                return DateTime.Now;
            }
        }
        public static DateTime ToDateTime(object obj, DateTime DefaultVal)
        {
            if (obj == null || obj.ToString().Length == 0) return DefaultVal;
            try
            {
                return DateTime.Parse(obj.ToString());
            }
            catch
            {
                return DefaultVal;
            }
        }

        public static bool ToBool(object obj)
        {
            if (obj == null || obj.ToString().Length == 0) return false;
            try
            {
                return Boolean.Parse(obj.ToString());
            }
            catch
            {
                return false;
            }
        }
        public static bool ToBool(object obj, bool DefaultVal)
        {
            if (obj == null || obj.ToString().Length == 0) return DefaultVal;
            try
            {
                return Boolean.Parse(obj.ToString());
            }
            catch
            {
                return DefaultVal;
            }
        }
    }
}
