using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
/*
 * test ciagu pojedynczych bitow 9725 < xi < 10275
 * test dlugiej serii 000000....0000 nie wiecej niz 26 bitow
 * test serii 0 00 000 6 i wiecej
 * test pokerowy 4 bit 4 bit na reprezezentacje dziesietna od 0 do 15
 * test listu
 */
namespace POD_lab5
{
    class Program
    {
        static bool Czy_pierwsza(int n)
        {
            if (n < 2)
                return false;
            for (int i = 2; i * i <= n; i++)
                if (n % i == 0)
                    return false;
            return true;
        }
        static int NWD(int a, int b)
        {
            while (a != b)
            {
                if (a > b)
                    a -= b;
                else
                    b -= a;
            }
            return a;
        }
        static string generator_BBS(int p, int q, int t)
        {
            string ciag_pseudolosowy = "";
            int m = p * q;
            int s;
            int x;
            Random losuj = new Random(Environment.TickCount);
            do
            {
                s = losuj.Next(1, m);
            } while (NWD(s,m)!=1);
            for (int i = 0; i < t; i++)
            {
                x = (s * s) % m;
                if(x%2==0)
                {
                    ciag_pseudolosowy = ciag_pseudolosowy + "0";
                }
                else
                    ciag_pseudolosowy = ciag_pseudolosowy + "1";
                s = x;
            }
            return ciag_pseudolosowy;
        }
        static byte[] generator_BBS_byte(int p, int q, int t)
        {
            byte[] ciag = new byte[t];
            int m = p * q;
            int s;
            int x;
            Random losuj = new Random(Environment.TickCount);
            do
            {
                s = losuj.Next(1, m);
            } while (NWD(s, m) != 1);
            for (int i = 0; i < t; i++)
            {
                x = (s * s) % m;
                if (x % 2 == 0)
                {
                    ciag[i] = 0;
                }
                else
                    ciag[i] = 1;
                s = x;
            }
            return ciag;
        }
        static String generator_Blum_Micali(int a, int p, int n)
        {
            String ciag_pseudolosowy = "";
            Random losuj = new Random(Environment.TickCount);
            BigInteger x = new BigInteger(losuj.Next(1, p));
            for (int i = 0; i < n; i++)
            {
                if (x > (p - 1) / 2)
                {
                    ciag_pseudolosowy = ciag_pseudolosowy + "1";
                }
                else
                {
                    ciag_pseudolosowy = ciag_pseudolosowy + "0";
                }
                x = BigInteger.Pow(x, a);
                x = x % p;

            }
            return ciag_pseudolosowy;
        }
        static void test_pojedynczych_bitow(String wiadomosc)
        {
            int zera = 0;
            int jedynki = 0;
            for (int i = 0; i < wiadomosc.Length; i++)
            {
                if (wiadomosc[i] == '0')
                    zera++;
                else
                    jedynki++;
            }
            Console.WriteLine("Ilosc zer = " + zera);
            Console.WriteLine("Ilosc jedynek = " + jedynki);
        }
        static void test_dlugiej_serii(String wiadomosc)
        {
            int seria_zer = 0;
            int seria_jedynek = 0;
            int zera = 0;
            int jedynki = 0;
            for (int i = 0; i < wiadomosc.Length; i++)
            {
                if (wiadomosc[i] == '0')
                {
                    zera++;
                    if (zera > seria_zer)
                    {
                        seria_zer = zera;
                    }
                    jedynki = 0;
                }
                else
                {
                    jedynki++;
                    if (jedynki > seria_jedynek)
                    {
                        seria_jedynek = jedynki;
                    }
                    zera = 0;
                }
            }
            Console.WriteLine("Najdluzszy ciag zer = " + seria_zer);
            Console.WriteLine("Najdluzszy ciag jedynek = " + seria_jedynek);
        }
        static void test_serii(String wiadomosc)
        {
            int[] seria_zer = new int[6];
            int[] seria_jedynek = new int[6];
            int zera = 0;
            int jedynki = 0;
            for (int i = 0; i < wiadomosc.Length; i++)
            {
                if (wiadomosc[i] == '0')
                {
                    zera++;
                    if (jedynki > 0)
                    {
                        if (jedynki <= 6)
                            seria_jedynek[jedynki - 1]++;
                        else
                            seria_jedynek[5]++;
                    }
                    jedynki = 0;
                }
                else
                {
                    jedynki++;
                    if (zera > 0)
                    {
                        if (zera <= 6)
                            seria_zer[zera - 1]++;
                        else
                            seria_zer[5]++;
                    }
                    zera = 0;
                }
            }
            for (int i = 0; i <= 4; i++)
            {
                Console.WriteLine("Liczba zer powtarzajacych sie " + (i + 1) + " wynosi " + seria_zer[i]);
            }
            Console.WriteLine("Liczba zer powtarzajacych sie 6 i wiecej wynosi " + seria_zer[5]);
            for (int i = 0; i <= 4; i++)
            {
                Console.WriteLine("Liczba jedynek powtarzajacych sie " + (i + 1) + " wynosi " + seria_jedynek[i]);
            }
            Console.WriteLine("Liczba jedynek powtarzajacych sie 6 i wiecej wynosi " + seria_jedynek[5]);
        }
        static float test_pokerowy(String wiadomosc)
        {
            int[] tablica = new int[16];
            int i = 0;
            int decValue;
            while (i < wiadomosc.Length)
            {
                decValue = Convert.ToInt32(wiadomosc.Substring(i, 4), 2);
                tablica[decValue]++;
                if ((i + 4) < (wiadomosc.Length - 1))
                    i += 4;
                else
                    break;

            }
            float suma = 0;
            for (int x = 0; x < 16; x++)
            {
                Console.WriteLine("Liczba " + x + " wystepuje " + tablica[x] + " razy");
                suma = suma + tablica[x] * tablica[x];
            }
            suma = suma * 16 / 5000 - 5000;
            return suma;
        }
        static Byte[] GetBytesFromBinaryString(String binary)
        {
            var list = new List<Byte>();

            for (int i = 0; i < binary.Length; i += 7)
            {
                String t = binary.Substring(i, 7);

                list.Add(Convert.ToByte(t, 2));
            }

            return list.ToArray();
        }
        static string szyfr_strumieniowy(String wiadomosc, String klucz)
        {
            StringBuilder temp = new StringBuilder();
            byte[] wiadomosc_byte = Encoding.ASCII.GetBytes(wiadomosc);
            string bits = "";
            for (int i=0;i<wiadomosc_byte.Length;i++)
            {
                if (wiadomosc_byte[i] < 2)
                    bits = bits + "000000" + Convert.ToString(wiadomosc_byte[i], 2);
                if (wiadomosc_byte[i] < 4 && wiadomosc_byte[i] >= 2)
                    bits = bits + "00000" + Convert.ToString(wiadomosc_byte[i], 2);
                if (wiadomosc_byte[i] < 8 && wiadomosc_byte[i] >= 4)
                    bits = bits + "0000" + Convert.ToString(wiadomosc_byte[i], 2);
                if (wiadomosc_byte[i] < 16 && wiadomosc_byte[i] >= 8)
                    bits = bits + "000" + Convert.ToString(wiadomosc_byte[i], 2);
                if (wiadomosc_byte[i] < 32 && wiadomosc_byte[i] >= 16)
                    bits = bits + "00" + Convert.ToString(wiadomosc_byte[i], 2);
                if (wiadomosc_byte[i] < 64 && wiadomosc_byte[i] >= 32)
                {
                    bits = bits + "0" + Convert.ToString(wiadomosc_byte[i], 2);
                }
                if (wiadomosc_byte[i] < 128 && wiadomosc_byte[i] >= 64)
                    bits = bits + Convert.ToString(wiadomosc_byte[i], 2);
                
            }


            for (int i = 0; i < bits.Length; i++)
            {
                temp.Append((bits[i] ^ klucz[i]));
            }

            string wynik = temp.ToString();
            wynik = Encoding.ASCII.GetString(GetBytesFromBinaryString(wynik));
            return wynik;
        }

        static void Main(string[] args)
        {
            String wynik;
            for (int i = 0; i <= 2; i++)
            {
                wynik = generator_BBS(5651, 5623, 20000);
                StreamWriter SW;
                SW = File.AppendText("plik20000.txt");
                SW.WriteLine(wynik);
                SW.WriteLine("\n\n");
                SW.Close();
            }
            FileInfo f = new FileInfo("Binfile.bin");
            using (BinaryWriter bw = new BinaryWriter(f.OpenWrite()))
            {
                wynik = generator_BBS(5651, 5623, 1000000);
                bw.Write(wynik);
                
            }


            for (int i = 0; i <= 2; i++)
            {
                wynik = generator_BBS(5651, 5623, 20000);
                Console.WriteLine("Test pojedynczych bitow\n");
                test_pojedynczych_bitow(wynik);
                Console.WriteLine("\n\n\n\nTest dlugiej serii\n");
                test_dlugiej_serii(wynik);
                Console.WriteLine("\n\n\n\nTest serii\n");
                test_serii(wynik);
                Console.WriteLine("\n\n\n\nTest pokerowy\n");
                float suma = test_pokerowy(wynik);
                Console.WriteLine("Wynik testu pokerowego " + suma + "\n\n\n\n");
                Console.ReadKey();
            }
           

            wynik = generator_BBS(5651, 5623, 200000);
            string szyfrowana_wiadomosc = "Test1234567890";
            string zaszyfrowana_wiadomosc = szyfr_strumieniowy(szyfrowana_wiadomosc, wynik);
            Console.WriteLine("Szyfrowana wiadomosc " + szyfrowana_wiadomosc + " zaszyfrowana wiadomosc " + zaszyfrowana_wiadomosc);
            Console.WriteLine("Zaszyfrowana wiadomosc " + zaszyfrowana_wiadomosc + " odszyfrowana wiadomosc " + szyfr_strumieniowy(zaszyfrowana_wiadomosc, wynik));
            Console.ReadKey();
        }
    }
}
