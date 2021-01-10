using JMBG_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace JMBG_App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult DisplayJmbgInformationView()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetJmbgInfo(string jmbg)
        {
            string final = CheckJmbg(jmbg);

            return Json(final);
        }

        public string CheckJmbg(string jmbg)
        {
            char[] jmbg_temp_array = jmbg.ToCharArray(0, jmbg.Length);

            int[] jmbg_array;

            jmbg_array = Array.ConvertAll(jmbg_temp_array, elem => (int)Char.GetNumericValue(elem));

            for (int i = 0; i < jmbg_array.Length; i++)
            {
                if (jmbg_array[i] < 0 || jmbg_array[i] > 9)
                    return "Invalid character in jmbg";
            }

            int month = MonthParser(jmbg_temp_array);

            if (month > 12 || month < 1)
                return "Invalid month";

            int day = DayParser(jmbg_temp_array);

            int year = YearParser(jmbg_temp_array);

            if (year == 0)
                return "Invalid year";

            if (DayValidation(day, month, year))
                return "Invalid day";

            int region_number = RegionParser(jmbg_temp_array);

            string region = CheckRegion(region_number);

            if (region == "Invalid region")
                return "Invalid region";

            int gender_number = GenderParser(jmbg_temp_array);

            string gender = CheckGender(gender_number);

            string control_number = CheckControlNumber(jmbg_temp_array);

            if (control_number == "Invalid")
                return "Invalid control number";

            return "{\"day\": \"" + day + "\"," + "\"month\": \"" + month + "\"," +
                "\"year\": \"" + year + "\"," + "\"region\": \"" + region + "\"," +
                "\"gender\": \"" + gender + "\"," + "\"control_number\": \"" + control_number + "\"}";

        }

        public bool DayValidation(int day, int month, int year)
        {
            if (day <= 31 && day >= 1)
            {
                if ((month == 4 || month == 6 || month == 9 || month == 11) && day == 31)
                    return true;
                else if (month == 2)
                {
                    if (!CheckFebruary(day, month, year))
                        return true;
                }
                else
                    return false;
            }

            else
                return true;

            return false;
        }

        public int DayParser(char[] jmbg_temp_array)
        {
            string day_string = "";

            for (int i = 0; i < 2; i++)
            {
                day_string = day_string.Insert(i, jmbg_temp_array[i].ToString());
            }

            return int.Parse(day_string);
        }

        public int MonthParser(char[] jmbg_temp_array)
        {
            string month_string = "";

            for (int i = 0; i < 2; i++)
            {
                month_string = month_string.Insert(i, jmbg_temp_array[i + 2].ToString());
            }

            return int.Parse(month_string);
        }

        public int YearParser(char[] jmbg_temp_array)
        {
            string year_string = "";

            for (int i = 0; i < 4; i++)
            {
                if (i == 0)
                {
                    if (int.Parse(jmbg_temp_array[i + 4].ToString()) >= 8)
                        year_string = year_string.Insert(i, "1");
                    else if (int.Parse(jmbg_temp_array[i + 4].ToString()) == 0)
                        year_string = year_string.Insert(i, "2");
                    else
                        return 0;
                }
                else
                    year_string = year_string.Insert(i, jmbg_temp_array[i + 3].ToString());
            }

            return int.Parse(year_string);
        }

        public int RegionParser(char[] jmbg_temp_array)
        {
            string region_string = "";

            for (int i = 0; i < 2; i++)
            {
                region_string = region_string.Insert(i, jmbg_temp_array[i + 7].ToString());
            }

            return int.Parse(region_string);
        }
        public bool CheckFebruary(int day, int month, int year)
        {
            if (day == 30 || day == 31)
                return false;
            else if (day == 29)
                if (year % 400 == 0 || (year % 100 != 0) && (year % 4 == 0))
                    return true;
                else
                    return false;
            return true;
        }
        public string CheckRegion(int region_number)
        {
            if (region_number < 10)
            {
                if (region_number == 1)
                    return "Stranac u BiH";
                else if (region_number == 2)
                    return "Stranac u Crnoj Gori";
                else if (region_number == 3)
                    return "Stranac u Hrvatskoj";
                else if (region_number == 4)
                    return "Stranac u Makedoniji";
                else if (region_number == 5)
                    return "Stranac u Sloveniji";
                else if (region_number == 7)
                    return "Stranac u Srbiji";
                else if (region_number == 8)
                    return "Stranac u Vojvodini";
                else if (region_number == 9)
                    return "Stranac na Kosovu i Metohiji";
            }
            else if (region_number < 20)
            {
                if (region_number == 10)
                    return "Banja Luka";
                else if (region_number == 11)
                    return "Bihać";
                else if (region_number == 12)
                    return "Doboj";
                else if (region_number == 13)
                    return "Goražde";
                else if (region_number == 14)
                    return "Livno";
                else if (region_number == 15)
                    return "Mostar";
                else if (region_number == 16)
                    return "Prijedor";
                else if (region_number == 17)
                    return "Sarajevo";
                else if (region_number == 18)
                    return "Tuzla";
                else if (region_number == 19)
                    return "Zenica";
            }
            else if (region_number < 30)
            {
                if (region_number == 21)
                    return "Podgorica";
                else if (region_number == 26)
                    return "Nikšić";
                else if (region_number == 29)
                    return "Pljevlja";
            }
            else if (region_number < 40)
            {
                if (region_number == 30)
                    return "Osijek, Slavonija";
                else if (region_number == 31)
                    return "Bjelovar, Virovitica, Koprivnica, Pakrac, Podravina region";
                else if (region_number == 32)
                    return "Varaždin, Međimurje region";
                else if (region_number == 33)
                    return "Zagreb";
                else if (region_number == 34)
                    return "Karlovac";
                else if (region_number == 35)
                    return "Gospić, Lika region";
                else if (region_number == 36)
                    return "Rijeka, Pula, Istra and Primorje region";
                else if (region_number == 37)
                    return "Sisak, Banovina region";
                else if (region_number == 38)
                    return "Split, Zadar, Dubrovnik, Dalmacija region";
                else if (region_number == 39)
                    return "Ostalo (Hrvatska)";
            }
            else if (region_number < 50)
            {
                if (region_number == 41)
                    return "Bitolj";
                else if (region_number == 42)
                    return "Kumanovo";
                else if (region_number == 43)
                    return "Ohrid";
                else if (region_number == 44)
                    return "Prilep";
                else if (region_number == 45)
                    return "Skoplje";
                else if (region_number == 46)
                    return "Strumica";
                else if (region_number == 47)
                    return "Tetovo";
                else if (region_number == 48)
                    return "Veles";
                else if (region_number == 49)
                    return "Štip";
            }
            else if (region_number < 60)
            {
                if (region_number == 50)
                    return "Slovenija";
            }
            else if (region_number < 70)
            {
                return "Invalid region";
            }
            else if (region_number < 80)
            {
                if (region_number == 71)
                    return "Beograd";
                else if (region_number == 72)
                    return "Šumadija";
                else if (region_number == 73)
                    return "Niš";
                else if (region_number == 74)
                    return "Južna Morava";
                else if (region_number == 75)
                    return "Zaječar";
                else if (region_number == 76)
                    return "Podunavlje";
                else if (region_number == 77)
                    return "Podrinje i Kolubara";
                else if (region_number == 78)
                    return "Kraljevo";
                else if (region_number == 79)
                    return "Užice";
            }
            else if (region_number < 90)
            {
                if (region_number == 80)
                    return "Novi Sad";
                else if (region_number == 81)
                    return "Sombor";
                else if (region_number == 82)
                    return "Subotica";
                else if (region_number == 85)
                    return "Zrenjanin";
                else if (region_number == 86)
                    return "Pančevo";
                else if (region_number == 87)
                    return "Kikinda";
                else if (region_number == 88)
                    return "Ruma";
                else if (region_number == 89)
                    return "Sremska Mitrovica";
            }
            else if (region_number < 100)
            {
                if (region_number == 91)
                    return "Priština";
                else if (region_number == 92)
                    return "Kosovska Mitrovica";
                else if (region_number == 93)
                    return "Peć";
                else if (region_number == 94)
                    return "Đakovica";
                else if (region_number == 95)
                    return "Prizren";
                else if (region_number == 96)
                    return "Kosovsko Pomoravski";
            }

            return "Invalid region";
        }
        public int GenderParser(char[] jmbg_temp_array)
        {
            string gender_string = "";

            for (int i = 0; i < 3; i++)
            {
                gender_string = gender_string.Insert(i, jmbg_temp_array[i + 9].ToString());
            }

            return int.Parse(gender_string);
        }

        public string CheckGender(int gender_number)
        {
            if (gender_number < 500)
                return "male";
            else
                return "female";
        }

        public string CheckControlNumber(char[] jmbg_temp_array)
        {
            string jmbg_string = "";

            for (int m = 0; m < 12; m++)
            {
                jmbg_string = jmbg_string.Insert(m, jmbg_temp_array[m].ToString());
            }

            int[] jmbg = Array.ConvertAll(jmbg_temp_array, s => (int)Char.GetNumericValue(s));

            int a = jmbg[0]; int b = jmbg[1]; int v = jmbg[2]; int g = jmbg[3]; int d = jmbg[4];
            int dj = jmbg[5]; int e = jmbg[6]; int zj = jmbg[7]; int z = jmbg[8]; int i = jmbg[9];
            int j = jmbg[10]; int k = jmbg[11]; int l = jmbg[12];

            l = 11 - ((7 * (a + e) + 6 * (b + zj) + 5 * (v + z) + 4 * (g + i) + 3 * (d + j) + 2 * (dj + k)) % 11);

            if (l > 9)
            {
                l = 0;
            }

            if (l == jmbg[12])
                return "Valid";

            return "Invalid";
        }
    }
}
