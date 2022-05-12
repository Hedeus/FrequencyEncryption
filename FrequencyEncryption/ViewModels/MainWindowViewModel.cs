using FrequencyEncryption.Infrastructure.Commands.Base;
using FrequencyEncryption.ViewModels.Base;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace FrequencyEncryption.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {

        /*------------------------------------------------------------------------------------*/

        #region Title : Title - Заголовок вікна
        private string _Title = "Точність розшифровки";

        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

        #endregion

        #region P : P - Перше число для розрахунку ключа
        private int _P = 3;

        public int P
        {
            get => _P;
            set => Set(ref _P, value);
        }
        #endregion

        #region Q : Q - Друге число для розрахунку ключа
        private int _Q = 7;

        public int Q
        {
            get => _Q;
            set => Set(ref _Q, value);
        }
        #endregion

        #region N : N - Модуль для розрахунку ключа
        private int _N = 0;

        public int N
        {
            get => _N;
            set => Set(ref _N, value);
        }
        #endregion

        #region Fi : Fi - Функція Ейлера
        private int _Fi = 0;

        public int Fi
        {
            get => _Fi;
            set => Set(ref _Fi, value);
        }
        #endregion

        #region OpenKey : OpenKey - Відкритий Ключ
        private int _OpenKey = 0;

        public int OpenKey
        {
            get => _OpenKey;
            set => Set(ref _OpenKey, value);
        }
        #endregion

        #region SecretKey : SecretKey - Закритий ключ
        private int _SecretKey = 0;

        public int SecretKey
        {
            get => _SecretKey;
            set => Set(ref _SecretKey, value);
        }
        #endregion

        #region PrimeList : PrimeList - Список простих чисел
        private List<int> _PrimeList;

        public List<int> PrimeList
        {
            get => _PrimeList;
            set => Set(ref _PrimeList, value);
        }
        #endregion



        #region ActiveTab : ActiveTab - номер вкладки
        private int _ActiveTab = 0;

        public int ActiveTab
        {
            get => _ActiveTab;
            set => Set(ref _ActiveTab, value);
        }
        #endregion

        #region BaseText : BaseText - початковий текст
        private string _BaseText = "Тут буде незашифрований текст";

        public string BaseText
        {
            get => _BaseText;
            set => Set(ref _BaseText, value);
        }

        #endregion

        #region EncryptedText : EncryptedText - зашифрований текст
        private string _EncryptedText = "Тут буде зашифрований текст";

        public string EncryptedText
        {
            get => _EncryptedText;
            set => Set(ref _EncryptedText, value);
        }

        #endregion

        #region EncryptedMessage : EncryptedMessage - зашифрованe повідомлення
        private IEnumerable<int> _EncryptedMessage;

        public IEnumerable<int> EncryptedMessage
        {
            get => _EncryptedMessage;
            set => Set(ref _EncryptedMessage, value);
        }

        #endregion

        #region DecipheredText : DecipheredText - розшифрований текст
        private string _DecipheredText = "Тут буде розшифрований текст";

        public string DecipheredText
        {
            get => _DecipheredText;
            set => Set(ref _DecipheredText, value);
        }

        #endregion

        #region StandardDict : StandardDict Еталонний розподіл літер
        private Dictionary<char, int> _StandardDict;        

        public Dictionary<char, int> StandardDict       
        {
            get => _StandardDict;
            set => Set(ref _StandardDict, value);
        }
        #endregion

        #region CipherdDict : CipherDict        
        private Dictionary<char, char> _CipherDict;

        public Dictionary<char, char> CipherDict
        {
            get => _CipherDict;
            set => Set(ref _CipherDict, value);
        }
        #endregion

        #region RSADict : RSADict        
        private Dictionary<char, int> _RSADict;

        public Dictionary<char, int> RSADict
        {
            get => _RSADict;
            set => Set(ref _RSADict, value);
        }
        #endregion

        #region DeCipherDict : DeCipherDict        
        private Dictionary<char, char> _DeCipherDict;

        public Dictionary<char, char> DeCipherDict
        {
            get => _DeCipherDict;
            set => Set(ref _DeCipherDict, value);
        }
        #endregion

        /*------------------------------------------------------------------------------------*/

        #region GetEtalonDict Отримання частотного розподілення літер 
        //Отримання частотного розподілення літер
        private Dictionary<char, double> GetEtalonDict()
        {
            Dictionary<char, double> dict = new Dictionary<char, double>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            //FileInfo info = new FileInfo("Частотность.xlsx");
            //FileInfo info = new FileInfo("My_Frequency.xlsx");
            FileInfo info = new FileInfo("..\\..\\Data\\My_Frequency.xlsx");
            using (ExcelPackage xlPackage = new ExcelPackage(info))
            {
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[0];
                for (int iRow = 1; iRow <= 34; iRow++)
                {
                    dict[Convert.ToChar(worksheet.GetValue(iRow, 1))] = Convert.ToDouble(worksheet.GetValue(iRow, 2));
                }
            }
            return dict;

            ////Створення власного словника частотності
            //int totalKeyCount = 0;
            //Dictionary<char, double> cipheranalysis = new Dictionary<char, double>();
            //double keycount;
            //foreach (var c in BaseText)
            //{
            //    if (!cipheranalysis.TryGetValue(c, out keycount))
            //        keycount = 0.0;
            //    cipheranalysis[c] = keycount + 1;
            //    totalKeyCount += 1;
            //}
            //Dictionary<char, double> FD = new Dictionary<char, double>();
            //foreach (var pair in cipheranalysis)
            //{
            //    FD[pair.Key] = pair.Value / totalKeyCount;
            //}
            //FD['_'] = FD[' '];
            //FD.Remove(' ');
            //cipheranalysis = FD;

            //// Dictionary<char, double> dict = new Dictionary<char, double>();
            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            //FileInfo info = new FileInfo("My_Frequency.xlsx");
            //using (ExcelPackage xlPackage = new ExcelPackage(info))
            //{
            //    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[0];
            //    int iRow = 1;
            //    foreach (var pair in cipheranalysis)
            //    {
            //        worksheet.Cells[iRow, 1].Value = pair.Key.ToString();
            //        worksheet.Cells[iRow, 2].Value = pair.Value.ToString();
            //        iRow++;
            //    }
            //    xlPackage.Save();                
            //}
            //return cipheranalysis;
        }
        #endregion

        #region GetRSADict Отримання RSA словника
        private Dictionary<char, int> GetRSADict()
        {
            Dictionary<char, int> dict = new Dictionary<char, int>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            //FileInfo info = new FileInfo("Частотность.xlsx");
            //FileInfo info = new FileInfo("My_Frequency.xlsx");
            FileInfo info = new FileInfo("..\\..\\Data\\RSA_Dictionary.xlsx");
            using (ExcelPackage xlPackage = new ExcelPackage(info))
            {
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[0];
                for (int iRow = 1; iRow <= 34; iRow++)
                {
                    dict[Convert.ToChar(worksheet.GetValue(iRow, 1))] = Convert.ToInt32(worksheet.GetValue(iRow, 2));
                }
            }
            return dict;
        }
        #endregion

        #region CreatePrimeList - Створення списка простих чисел
        private List<int> CreatePrimeList()
        {
            int max = 100;
            List<int> Primes = new List<int>();
            Primes.Add(2);
            Primes.Add(3);
            //for (int i = 5; i < int.MaxValue; i += 2)
            for (int i = 5; i < max; i += 2)
            {
                if ((i > 10) && (i % 10 == 5))
                {
                    continue;
                }
                for (int j = 1; j < Primes.Count; j++)
                {
                    if (Primes[j] * Primes[j - 1] > i)
                    {
                        Primes.Add(i);
                        break;
                    }
                    if (i % Primes[j] == 0)
                    {
                        break;
                    }
                    //else
                    //{
                    //    Primes.Add(i);
                    //}
                }
            }
            return Primes;
        }
        #endregion

        #region IsCoprime - Перевірка на взаємну простоту 2 чисел
        //public static bool IsCoprime(int num1, int num2)
        //{
        //    if (num1 == num2)
        //    {
        //        return num1 == 1;
        //    }
        //    else
        //    {
        //        if (num1 > num2)
        //        {
        //            return IsCoprime(num1 - num2, num2);
        //        }
        //        else
        //        {
        //            return IsCoprime(num2 - num1, num1);
        //        }
        //    }
        //}
        public static bool IsCoprime(int a, int b)
        {
            return a == b
                ? a == 1
                : a > b
                    ? IsCoprime(a - b, b)
                    : IsCoprime(b - a, a);
        }
        #endregion

        #region invmod(int a, int m) - Пошук числа, зворотньго до заданного числа "а" за модулем "m"
        private static (int, int, int) gcdex(int a, int b)
        {
            if (a == 0)
                return (b, 0, 1);
            (int gcd, int x, int y) = gcdex(b % a, a);
            return (gcd, y - (b / a) * x, x);
        }

        private static int invmod(int a, int m)
        {
            (int g, int x, int y) = gcdex(a, m);
            return g > 1 ? 0 : (x % m + m) % m;
        }
        #endregion

        #region powmod - Зведення у ступінь по модулю
        private int powmod(int a, int n, int m)
        {
            int x = a;
            while (n > 1)
            {
                x = (x * a) % m;
                n--;
            }
            return x;
        }
        #endregion

        #region RSAEncrypt - Шифрування за RSA
        private void RSAEncrypt()
        {
            Random rnd = new Random();

            int e = PrimeList[rnd.Next(0, PrimeList.Count)];
            int d = PrimeList[rnd.Next(0, PrimeList.Count)];
            int n = e * d;
            int f = (e - 1) * (d - 1);
            while (f < 100 || e == d)
            {
                e = PrimeList[rnd.Next(0, PrimeList.Count)];
                d = PrimeList[rnd.Next(0, PrimeList.Count)];
                n = e * d;
                f = (e - 1) * (d - 1);
            }
            P = e;
            Q = d;
            N = n;
            Fi = f;

            int x = Fi;
            if (x > PrimeList[PrimeList.Count - 1])
            {
                x = PrimeList[PrimeList.Count - 1];
            }
            else
            {
                while (!PrimeList.Contains(x))
                {
                    x--;
                }
            }

            e = PrimeList[rnd.Next(0, PrimeList.IndexOf(x))];
            while (!IsCoprime(e, Fi) || e > Fi)
            {
                e = e = PrimeList[rnd.Next(0, PrimeList.IndexOf(x))];
            }
            OpenKey = e;

            d = invmod(e, Fi);
            SecretKey = d;

            //BaseText = BaseText.ToUpper();
            IEnumerable<char> textChars = BaseText.Distinct();

            Dictionary<char, int> cipher = new Dictionary<char, int>();
            d = 1;
            foreach (var c in textChars)
            {
                cipher[c] = d;
                d++;
            }
            RSADict = cipher;

            IEnumerable<int> crypto = BaseText.Select(c => powmod(RSADict[c], OpenKey, N));
            EncryptedMessage = crypto;

            string s = "";
            foreach (var c in EncryptedMessage)
            {
                s += c.ToString() + ",";
            }
            EncryptedText = s;
        }
        #endregion

        #region RSADecipher - RSA розшифровування
        private void RSADecipher()
        {
            Dictionary<int, char> decipherdict = new Dictionary<int, char>();
            for (int i = 0; i < RSADict.Count; i++)
            {
                decipherdict[RSADict.ElementAt(i).Value] = RSADict.ElementAt(i).Key;
            }

            string s = "";
            int e;
            foreach (var i in EncryptedMessage)
            {
                e = powmod(i, SecretKey, N);
                if (decipherdict.ContainsKey(e))
                {
                    s += decipherdict[e];
                }
                else
                    s += "?";
            }
            DecipheredText = s;
        } 
        #endregion

        #region Функція Шифрування
        private void Encrypt()
        {
            BaseText = BaseText.ToUpper();

            //Dictionary<char, double> standardFrequency = new Dictionary<char, double>();
            //standardFrequency = GetEtalonDict();
            //standardFrequency[' '] = standardFrequency['_'];
            //standardFrequency.Remove('_');
            //StandardDict = standardFrequency;

            IEnumerable<char> textChars = BaseText.Distinct();            

            Dictionary<char, char> cipher = new Dictionary<char, char>();            
            
            Dictionary<char, int> cipherRandomizer = new Dictionary<char, int>();
            Random rnd = new Random();
            foreach (var pair in StandardDict)
            {
                cipherRandomizer.Add(pair.Key, rnd.Next(0, 1000));
            }
            cipherRandomizer = cipherRandomizer.OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

            for (int i = 0; i < StandardDict.Count; i++)
            {
                cipher[StandardDict.ElementAt(i).Key] = cipherRandomizer.ElementAt(i).Key;               
            }

            cipher = cipher.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            CipherDict = cipher;             
                       
            IEnumerable<char> strCharCode = BaseText.Select(c => (cipher.ContainsKey(c) ? cipher[c] : c));                             
            EncryptedText = new string(strCharCode.ToArray());            
        }
        #endregion

        #region Функція розшифрування
        private void Decipher()
        {

            //підраховуємо кількість появ у зашифрованому тексті кожної літери
            Dictionary<char, int> cipheranalysis = new Dictionary<char, int>();
            int keycount;
            foreach (var c in EncryptedText)
            {
                if (!cipheranalysis.TryGetValue(c, out keycount))
                    keycount = 0;
                cipheranalysis[c] = keycount + 1;
            }

            //Видалення зі словника символів, що було вирішено не використовувати у шифруванні (як розділові знаки)
            int d;
            string CharsForRemoving = "";
            foreach (var pair in cipheranalysis)
            {
                if (!StandardDict.TryGetValue(pair.Key, out d))
                    CharsForRemoving += pair.Key;
            }
            foreach (var c in CharsForRemoving)
            {
                cipheranalysis.Remove(c);
            }

            //Сортування словників за частотою появ символів у зашифрованому тексті
            // і еталоні, а також побудова ключового словника
            cipheranalysis = cipheranalysis.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            StandardDict = StandardDict.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            Dictionary<char, char> keyDict = new Dictionary<char, char>();
            for (int i = 0; i < cipheranalysis.Count; i++)
            {
                keyDict[cipheranalysis.ElementAt(i).Key] = StandardDict.ElementAt(i).Key;
            }            

            //Розшифровка тексту
            IEnumerable<char> strCharCode = EncryptedText.Select(c => (keyDict.ContainsKey(c) ? keyDict[c] : c));
            DecipheredText = new string(strCharCode.ToArray());

            Dictionary<char, char> TD = new Dictionary<char, char>();
            foreach (var pair in CipherDict)
            {   
                if (keyDict.ContainsKey(pair.Value))
                    TD[pair.Key] = keyDict[pair.Value];
                //TD[pair.Key] = (keyDict.ContainsKey(pair.Value) ? keyDict[pair.Value] : '0');
            }
            TD = TD.OrderByDescending(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
            DeCipherDict = TD;

            int accordance = 0;
            foreach (var pair in TD)
            {
                if (pair.Key ==pair.Value)
                    accordance++;
            }
            double correctness = (double)accordance / TD.Count;
            Title = correctness.ToString();


            //keyDict = keyDict.OrderByDescending(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
            //DeCipherDict = keyDict;
        }
        #endregion

        #region EncryptCommand : EncryptCommand - Команда зашифрувати
        public ICommand EncryptCommand { get; }
        private bool CanEncryptCommandExecute(object p) => true;
        private void OnEncryptCommandExecuted(object p)
        {
            //Encrypt();
            RSAEncrypt();
            //RSADecipher();
            //ActiveTab = 1;
            return;
        }
        #endregion

        #region DecipherCommand : DecipherCommand - Команда розшифрувати
        public ICommand DecipherCommand { get; }
        private bool CanDecipherCommandExecute(object p) => true;
        private void OnDecipherCommandExecuted(object p)
        {
            //Decipher();
            RSADecipher();
            return;
        }
        #endregion

        /*------------------------------------------------------------------------------------*/

        public MainWindowViewModel()
        {
            //Dictionary<char, int> standardFrequency = new Dictionary<char, int>();
            //standardFrequency = GetEtalonDict();
            //standardFrequency = GetRSADict();
            //standardFrequency[' '] = standardFrequency['_'];
            //standardFrequency.Remove('_');
            //StandardDict = standardFrequency;
            List<int> listofprimes = new List<int>();
            listofprimes = CreatePrimeList();
            PrimeList = listofprimes;

            EncryptCommand = new LambdaCommand(OnEncryptCommandExecuted, CanEncryptCommandExecute);
            DecipherCommand = new LambdaCommand(OnDecipherCommandExecuted, CanDecipherCommandExecute);

        }
    }
}
