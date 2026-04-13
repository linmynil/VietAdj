using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace triluatsoft.tls.CrystalReports
{
    public class CNum2Char
    {
        //Định nghĩa dãy phát âm các ký số
        static string[] chuSo = new string[10] { "", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
        //Định nghĩa dãy phát âm các đơn vị tỷ, ngàn, triệu
        static string[] chuCach = new string[4] { "", "tỷ", "ngàn", "triệu" };
        ///<SUMMARY>
        /// Hàm đổi 1 số có tối đa 3 ký số thành chuỗi tương ứng
        ///</SUMMARY>

        ///
        //</PARAM></P>///<RETURNS></RETURNS>
        private static string SoTram2Chu(uint soCanDoi)
        {
            uint soHangChuc, soHangTram, soHangDonVi;
            string str = null;
            //Xác định 3 ký số miêu tả hàng trăm, hàng chục và đơn vị
            soHangTram = soCanDoi / 100;
            soCanDoi = soCanDoi % 100;
            soHangChuc = soCanDoi / 10;
            soHangDonVi = soCanDoi % 10;
            if (soHangTram >= 1)
                str = chuSo[soHangTram] + " trăm";
            if (soHangChuc >= 2)
                str = str + " " + chuSo[soHangChuc] + " mươi";
            if (soHangChuc == 1)
                str = str + " mười";
            if (soHangDonVi == 0) return str;
            if (soHangChuc == 0)
            {
                if (soHangTram != 0)
                    str = str + " lẻ " + chuSo[soHangDonVi];
                else str = str + " " + chuSo[soHangDonVi];
                return str;
            }
            if (soHangChuc == 1)
            {
                if (soHangDonVi != 5)
                    str = str + " " + chuSo[soHangDonVi];
                else
                    str = str + " lăm";
                return str;
            }
            if (soHangDonVi == 1)
                str = str + " mốt";
            else if (soHangDonVi == 5) str = str + " lăm";
            else str = str + " " + chuSo[soHangDonVi];
            return str;

        }
        ///<SUMMARY>
        /// Hàm đổi 1 số nguyên bất kỳ thành chuỗi phát âm tương ứng
        ///</SUMMARY>

        ///
        //</PARAM></P>///<RETURNS></RETURNS>
        //public static string So2Chu(long soCanDoi) {
        //  int idx = 0;//Vị trí dấu chấm phân cách từng 3 ký số
        //  long baKySo = 0;
        //  string str = null;
        //  string tram = null;
        //  string tuNganCach = null;
        //  if (soCanDoi == 0) {
        //    str = "không";
        //  } else {
        //    while (soCanDoi != 0) {
        //      baKySo = soCanDoi % 1000;
        //      soCanDoi = soCanDoi / 1000;
        //      tram = SoTram2Chu(Convert.ToUInt32(baKySo));
        //      if (idx == 0) {//vị trí đơn vị
        //        if (soCanDoi == 0)
        //          str = tram;
        //        else str = "" + tram;// lẻ
        //      } else if (tram == null) {

        //      } else if (tram.Length != 0) {//vị trí ngàn, triệu, tỷ
        //        tuNganCach = chuCach[(idx % 3) + 1];
        //        str = tram + " " + tuNganCach + " " + str;
        //      } else if ((idx % 3) == 0)
        //        str = " tỷ" + str;
        //      idx = idx + 1;
        //    }
        //  }
        //  str += " đồng";
        //  str = str.Trim();
        //  return char.ToUpper(str[0]) + str.Substring(1); ;
        //}
        ///<SUMMARY>
        /// Hàm kiểm tra xem điền số có hợp lệ hay không
        ///</SUMMARY>
        public static string So2Chu(object soCanDoi)
        {
            return So2Chu(CConvert.ToDouble(soCanDoi));
        }
        public static string Num2Words(object soCanDoi)
        {
            return NumberToWords(CConvert.ToInt(soCanDoi));
        }
        public static string So2Chu(double soCanDoi)
        {
            soCanDoi = Math.Abs(soCanDoi);//hvtam-18112014 - Chi lay gia tri duong de map qua chuoi
            int idx = 0;//Vị trí dấu chấm phân cách từng 3 ký số
            double baKySo = 0;
            string str = null;
            string tram = null;
            string tuNganCach = null;
            if (soCanDoi == 0)
            {
                str = "không";
            }
            else
            {
                while (soCanDoi != 0)
                {
                    baKySo = soCanDoi % 1000;
                    soCanDoi = soCanDoi / 1000;
                    tram = SoTram2Chu(Convert.ToUInt32(baKySo));
                    if (idx == 0)
                    {//vị trí đơn vị
                        if (soCanDoi == 0) str = "|" + soCanDoi + "|" + tram;
                        else str = "" + tram;// lẻ
                                             //str += "[vị trí đơn vị]";
                    }
                    else if (tram == null)
                    {

                    }
                    else if (tram.Length != 0)
                    {//vị trí ngàn, triệu, tỷ
                        tuNganCach = chuCach[(idx % 3) + 1];
                        str = tram + " " + tuNganCach + " " + str;
                    }
                    else if ((idx % 3) == 0)
                        str = " tỷ" + str;
                    idx = idx + 1;
                }
            }
            str = str.Trim();
            str += " đồng";
            return char.ToUpper(str[0]) + str.Substring(1);
        }

        public static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";
            if ((number / 1000000000) > 0)
            {
                if (number == 1)
                {
                    words += NumberToWords(number / 1000000000) + " billion ";
                } else
                {
                    words += NumberToWords(number / 1000000000) + " billions ";
                }                
                number %= 1000000000;
            }

            if ((number / 100000000) > 0)
            {
                if (number == 1)
                {
                    words += NumberToWords(number / 10000000) + " hundred ";
                } else
                {
                    words += NumberToWords(number / 10000000) + " hundreds ";
                }                

                number %= 10000000;

                if ((number / 1000000) > 0)
                {
                    words += "and ";
                } else
                {
                    words += "millions ";
                }
            }

            if ((number / 1000000) > 0)
            {
                if (number == 1)
                {
                    words += NumberToWords(number / 1000000) + " million ";
                } else
                {
                    words += NumberToWords(number / 1000000) + " millions ";
                }
                
                number %= 1000000;
            }
            
            if ((number / 100000) > 0)
            {
                if (number == 1)
                {
                    words += NumberToWords(number / 100000) + " hundred ";
                } else
                {
                    words += NumberToWords(number / 100000) + " hundreds ";
                }
                
                number %= 100000;
                if ((number / 1000) > 0)
                {
                    words += "and ";
                } else
                {
                    words += "thousands ";
                }
            }
            
            if ((number / 1000) > 0)
            {
                if (number == 1)
                {
                    words += NumberToWords(number / 1000) + " thousand ";
                } else
                {
                    words += NumberToWords(number / 1000) + " thousands ";
                }
                
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                if (number == 1)
                {
                    words += NumberToWords(number / 100) + " hundred ";
                } else
                {
                    words += NumberToWords(number / 100) + " hundreds ";
                }
                
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }
            return char.ToUpper(words[0]) + words.Substring(1);            
        }

        ///
        //</PARAM></P>///<RETURNS></RETURNS>
        private static bool IsInteger(string number)
        {
            Regex notIntPattern = new Regex("[^0-9-]");
            Regex intPattern = new Regex("^-[0-9]+$|^[0-9]+$");
            return !notIntPattern.IsMatch(number) && intPattern.IsMatch(number);
        }

        private static string AddSplitDot(string str)
        {
            string r = str;
            for (int i = r.Length - 1; i > 0; i--)
            {
                if (i + 3 == r.Length || (i + 3 < r.Length && r.Substring(i + 3, 1).Equals(".")))
                    r = r.Insert(i, ".");
            }
            return r;
        }
        private static string[] m_arrHundredOf = new string[10]{"không","một","hai","ba","bốn","năm","sáu",
                                                                            "bảy","tám","chín"};
        private static string[] m_arrTensOf = new string[10]{"lẻ","mười","hai mươi","ba mươi","bốn mươi","năm mươi",
                                                                        "sáu mươi", "bảy mươi","tám mươi","chín mươi"};

        private static string[] m_arrUnit = new string[10] {"","mốt","hai","ba","bốn","lăm","sáu",
                                                                      "bảy","tám","chín"};

        private static string[] m_arrUnitName = new string[4] { "đơn vị", "nghìn", "triệu", "tỷ" };

        /// <summary>
        /// Lấy tên của 1 số
        /// </summary>
        /// <param name="num">ký tự Số</param>
        /// <param name="pos">vị trí (3: trăm, 2: chục, 1: đơn vị</param>
        /// <param name="specify">cho biết đây có phải là dạng đọc đặc biệt không</param>
        /// <returns></returns>
        private static string GetNumberName(char num, int pos, bool specify)
        {
            string strResult = "";
            switch (pos)
            {
                case 3: strResult = m_arrHundredOf[num - 48] + " trăm"; break;
                case 2: strResult = m_arrTensOf[num - 48]; break;
                case 1:
                    strResult = m_arrUnit[num - 48];
                    if (num == '5')
                    {
                        if (specify)
                            strResult = "năm";
                    }

                    if (num == '1')
                    {
                        if (specify)
                            strResult = "một";
                    }
                    break;

            }
            return strResult;
        }

        /// <summary>
        /// Đọc từng phần của chuỗi
        /// </summary>
        /// <param name="p_strNum">chuỗi số (tối đa 3 số)</param>
        /// <param name="specify">Cho biết là có đọc dạng đặc biệt ko</param>
        /// <returns></returns>
        private static string ReadPart(string p_strNum, bool specify)
        {
            if ((p_strNum == "") || (int.Parse(p_strNum) == 0))
                return "";
            return GetNumberName(p_strNum[0], p_strNum.Length, specify) + " " + ReadPart(p_strNum.Remove(0, 1), specify);
        }

        /// <summary>
        /// Đọc 1 số nguyên
        /// </summary>
        /// <param name="p_intNumber"></param>
        /// <returns></returns>
        public static string ReadInt(string p_strNumber, string p_strUnitName)
        {
            // Check strNum
            for (int i = 0; i < p_strNumber.Length; i++)
                if (p_strNumber[i] < 48 || p_strNumber[i] > 57)
                    return "Chuỗi số không hợp lệ.";
            //
            m_arrUnitName[0] = p_strUnitName;
            string strTemp, strResult = "", strPartNum = "";
            bool specify = false;
            int count = -1, l = 0;

            // Delete '0' as 0 position
            while (p_strNumber.Length > 0 && p_strNumber[0] == '0')
                p_strNumber = p_strNumber.Remove(0, 1);

            // if (strNum is "") then return "khong strUnitName
            if (p_strNumber == "")
                return "Không " + m_arrUnitName[0];

            // Read
            while (p_strNumber.Length > 0)
            {
                // if strSource then get from right to left 3 char else get from to left l char
                l = p_strNumber.Length;
                if (l >= 3)
                    l = 3;
                // increment unitname
                count++;
                // Get l char rom right to left and read it
                strPartNum = p_strNumber.Substring(p_strNumber.Length - l, l);
                if ((strPartNum == "5") || (strPartNum == "1"))
                    specify = true;

                if ((l >= 2) && (strPartNum[l - 1] == '1') && ((strPartNum[l - 2] == '1') || (strPartNum[l - 2] == '0')))
                    specify = true;

                if ((l >= 2) && (strPartNum[l - 1] == '5') && (strPartNum[l - 2] == '0'))
                    specify = true;

                strTemp = ReadPart(strPartNum, specify);

                // Add unit name to read string
                if (strTemp != "" || count == 0)
                {
                    if (count != 0 && count % 3 == 0)
                        strTemp = strTemp + m_arrUnitName[count % 3 + 3] + " ";
                    else
                        strTemp = strTemp + m_arrUnitName[count % 3] + " ";
                }
                else
                {
                    if (count % 3 == 0)
                        strTemp = strTemp + m_arrUnitName[count % 3 + 3] + " ";
                }
                //
                strResult = strTemp + strResult;

                p_strNumber = p_strNumber.Remove(p_strNumber.Length - l, l);
                specify = false;
            }

            char c = char.ToUpper(strResult[0]);
            strResult = c + strResult.Remove(0, 1);
            return strResult;
        }

    }
}
