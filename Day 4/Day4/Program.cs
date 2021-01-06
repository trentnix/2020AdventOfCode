using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Day4
{
    class Program
    {
        static void Main(string[] args)
        {
            //file in disk
            var FileUrl = @"D:\Source\Advent of Code\Day 4\day4.txt";

            //file lines
            string[] lines = File.ReadAllLines(FileUrl);

            int numValidPassports = 0;
            int numValidPassportsAdvanced = 0;

            string passport = String.Empty;
            int lineCount = 0;
            foreach(string line in lines)
            {
                lineCount++;

                if (line.Length > 0)
                {
                    passport += line + " ";
                }

                if (((line.Length == 0) || (lineCount == lines.Length)) &&
                    (passport.Length > 0)
                   )
                {
                    Passport myPassport = new Passport(passport);
                    if (myPassport.IsValidPassport())
                    {
                        numValidPassports++;
                    }

                    if (myPassport.IsValidPassportWithDataValidation())
                    {
                        numValidPassportsAdvanced++;
                    }

                    passport = String.Empty;
                }
            }

            Console.WriteLine("Step 1 - Valid Passports: {0}", numValidPassports);
            Console.WriteLine("Step 2 - Valid Passports (with Validation): {0}", numValidPassportsAdvanced);
        }
    }

    public class Passport
    {
        public Passport(string passport)
        {
            /*
                byr (Birth Year)
                iyr (Issue Year)
                eyr (Expiration Year)
                hgt (Height)
                hcl (Hair Color)
                ecl (Eye Color)
                pid (Passport ID)
                cid (Country ID)
             */

            byr = getValue("byr:", passport);
            iyr = getValue("iyr:", passport);
            eyr = getValue("eyr:", passport);
            hgt = getValue("hgt:", passport);
            hcl = getValue("hcl:", passport);
            ecl = getValue("ecl:", passport);
            pid = getValue("pid:", passport);
            cid = getValue("cid:", passport);
        }

        private string getValue(string type, string passport)
        {
            int index = passport.IndexOf(type);
            if (index < 0)
                return String.Empty;

            index += type.Length;
            
            string value = passport.Substring(index);
            value = value.Substring(0, value.IndexOf(' '));

            return value;
        }

        public string byr = String.Empty;
        public string iyr = String.Empty;
        public string eyr = String.Empty;
        public string hgt = String.Empty;
        public string hcl = String.Empty;
        public string ecl = String.Empty;
        public string pid = String.Empty;
        public string cid = String.Empty;

        public bool IsValidPassport()
        {
            if ((byr.Length == 0) ||
                (iyr.Length == 0) ||
                (eyr.Length == 0) ||
                (hgt.Length == 0) ||
                (hcl.Length == 0) ||
                (ecl.Length == 0) ||
                (pid.Length == 0))
            {
                return false;
            }

            return true;
        }

        public bool IsValidPassportWithDataValidation()
        {
            /* 
                byr (Birth Year) - four digits; at least 1920 and at most 2002.
                iyr (Issue Year) - four digits; at least 2010 and at most 2020.
                eyr (Expiration Year) - four digits; at least 2020 and at most 2030.
                hgt (Height) - a number followed by either cm or in:
                If cm, the number must be at least 150 and at most 193.
                If in, the number must be at least 59 and at most 76.
                hcl (Hair Color) - a # followed by exactly six characters 0-9 or a-f.
                ecl (Eye Color) - exactly one of: amb blu brn gry grn hzl oth.
                pid (Passport ID) - a nine-digit number, including leading zeroes.
                cid (Country ID) - ignored, missing or not.
             */

            if (!isValidYear(byr, 1920, 2002))
            {
                return false;
            }

            if (!isValidYear(iyr, 2010, 2020))
            {
                return false;
            }

            if (!isValidYear(eyr, 2020, 2030))
            {
                return false;
            }

            if (!isValidHeight(hgt))
            {
                return false;
            }

            if (!isValidHairColor(hcl))
            {
                return false;
            }

            if (!isValidEyeColor(ecl))
            {
                return false;
            }

            if (!isValidPassportID(pid))
            {
                return false;
            }

            return true;
        }

        private bool isValidPassportID(string value)
        {
            if (value.Length != 9)
                return false;

            string pattern = @"[0-9]{9}";
            Regex rgx = new Regex(pattern);
            Match match = rgx.Match(value);
            if (match.Success)
            {
                return true;
            }

            return false;
        }

        private bool isValidHairColor(string value)
        {
            if (value.Length != 7)
                return false;

            string pattern = @"^[#][a-f0-9]{6}";
            Regex rgx = new Regex(pattern);
            Match match = rgx.Match(value);
            if (match.Success)
            {
                return true;
            }

            return false;
        }

        private bool isValidEyeColor(string value)
        {
            switch (value)
            {
                case "amb":
                case "blu":
                case "brn":
                case "gry":
                case "grn":
                case "hzl":
                case "oth":
                    return true;
                default:
                    return false;
            }
        }

        private bool isValidHeight(string value)
        {
            if (value.Length <= 0)
            {
                return false;
            }

            int mincm = 150; int maxcm = 193;
            int minin = 59; int maxin = 76;

            string height = String.Empty;
            int nHeight = -1;

            try
            {
                int index = value.IndexOf("in");
                if (index > 0)
                {
                    height = value.Substring(0, index);
                    nHeight = int.Parse(height);

                    if (nHeight >= minin && nHeight <= maxin)
                    {
                        return true;
                    }
                }
                else
                {
                    index = value.IndexOf("cm");
                    if (index > 0)
                    {
                        height = value.Substring(0, index);
                        nHeight = int.Parse(height);

                        if (nHeight >= mincm && nHeight <= maxcm)
                        {
                            return true;
                        }

                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        private bool isValidYear(string value, int minYear, int maxYear)
        {
            if (value.Length <= 0)
            {
                return false;
            }

            try
            {
                int year = int.Parse(value);
                if (year >= minYear && year <= maxYear)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }

            return false;
        }
    }
}

/*
 * 
 * --- Day 4: Passport Processing ---
You arrive at the airport only to realize that you grabbed your North Pole Credentials instead of your passport. 
While these documents are extremely similar, North Pole Credentials aren't issued by a country and therefore 
aren't actually valid documentation for travel in most of the world.

It seems like you're not the only one having problems, though; a very long line has formed for the automatic 
passport scanners, and the delay could upset your travel itinerary.

Due to some questionable network security, you realize you might be able to solve both of these problems 
at the same time.

The automatic passport scanners are slow because they're having trouble detecting which passports have all 
required fields. The expected fields are as follows:

byr (Birth Year)
iyr (Issue Year)
eyr (Expiration Year)
hgt (Height)
hcl (Hair Color)
ecl (Eye Color)
pid (Passport ID)
cid (Country ID)
Passport data is validated in batch files (your puzzle input). Each passport is represented as a sequence 
of key:value pairs separated by spaces or newlines. Passports are separated by blank lines.

Here is an example batch file containing four passports:

ecl:gry pid:860033327 eyr:2020 hcl:#fffffd
byr:1937 iyr:2017 cid:147 hgt:183cm

iyr:2013 ecl:amb cid:350 eyr:2023 pid:028048884
hcl:#cfa07d byr:1929

hcl:#ae17e1 iyr:2013
eyr:2024
ecl:brn pid:760753108 byr:1931
hgt:179cm

hcl:#cfa07d eyr:2025 pid:166559648
iyr:2011 ecl:brn hgt:59in
The first passport is valid - all eight fields are present. The second passport is invalid - it is missing 
hgt (the Height field).

The third passport is interesting; the only missing field is cid, so it looks like data from North Pole 
Credentials, not a passport at all! Surely, nobody would mind if you made the system temporarily ignore 
missing cid fields. Treat this "passport" as valid.

The fourth passport is missing two fields, cid and byr. Missing cid is fine, but missing any other field is 
not, so this passport is invalid.

According to the above rules, your improved system would report 2 valid passports.

Count the number of valid passports - those that have all required fields. Treat cid as optional. In your 
batch file, how many passports are valid?

Your puzzle answer was 196.

--- Part Two ---
The line is moving more quickly now, but you overhear airport security talking about how passports with 
invalid data are getting through. Better add some data validation, quick!

You can continue to ignore the cid field, but each other field has strict rules about what values are valid 
for automatic validation:

byr (Birth Year) - four digits; at least 1920 and at most 2002.
iyr (Issue Year) - four digits; at least 2010 and at most 2020.
eyr (Expiration Year) - four digits; at least 2020 and at most 2030.
hgt (Height) - a number followed by either cm or in:
If cm, the number must be at least 150 and at most 193.
If in, the number must be at least 59 and at most 76.
hcl (Hair Color) - a # followed by exactly six characters 0-9 or a-f.
ecl (Eye Color) - exactly one of: amb blu brn gry grn hzl oth.
pid (Passport ID) - a nine-digit number, including leading zeroes.
cid (Country ID) - ignored, missing or not.
Your job is to count the passports where all required fields are both present and valid according to the 
above rules. Here are some example values:

byr valid:   2002
byr invalid: 2003

hgt valid:   60in
hgt valid:   190cm
hgt invalid: 190in
hgt invalid: 190

hcl valid:   #123abc
hcl invalid: #123abz
hcl invalid: 123abc

ecl valid:   brn
ecl invalid: wat

pid valid:   000000001
pid invalid: 0123456789
Here are some invalid passports:

eyr:1972 cid:100
hcl:#18171d ecl:amb hgt:170 pid:186cm iyr:2018 byr:1926

iyr:2019
hcl:#602927 eyr:1967 hgt:170cm
ecl:grn pid:012533040 byr:1946

hcl:dab227 iyr:2012
ecl:brn hgt:182cm pid:021572410 eyr:2020 byr:1992 cid:277

hgt:59cm ecl:zzz
eyr:2038 hcl:74454a iyr:2023
pid:3556412378 byr:2007
Here are some valid passports:

pid:087499704 hgt:74in ecl:grn iyr:2012 eyr:2030 byr:1980
hcl:#623a2f

eyr:2029 ecl:blu cid:129 byr:1989
iyr:2014 pid:896056539 hcl:#a97842 hgt:165cm

hcl:#888785
hgt:164cm byr:2001 iyr:2015 cid:88
pid:545766238 ecl:hzl
eyr:2022

iyr:2010 hgt:158cm hcl:#b6652a ecl:blu byr:1944 eyr:2021 pid:093154719
Count the number of valid passports - those that have all required fields and valid values. Continue to 
treat cid as optional. In your batch file, how many passports are valid?

Your puzzle answer was 114.
 * 
 */