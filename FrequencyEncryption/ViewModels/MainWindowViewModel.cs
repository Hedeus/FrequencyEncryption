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
            int max = 300;
            List<int> Primes = new List<int>();
            Primes.Add(2);
            //for (int i = 3; i < int.MaxValue; i += 2)
            for (int i = 3; i < max; i += 2)
            {
                if ((i > 10) && (i % 10 == 5))
                {
                    continue;
                }
                for (int j = 0; j < Primes.Count; j++)
                {
                    if (j * j - 1 > i)
                    {
                        Primes.Add(i);
                        break;
                    }
                    if (i % j == 0)
                    {
                        break;
                    }
                    else
                    {
                        Primes.Add(i);
                    }
                }
            }
            return Primes;
        }
        #endregion

        #region IsCoprime - Перевірка на взаємну простоту 2 чисел
        public static bool IsCoprime(int a, int b)
        {
            return a == b
                ? a == 1
                : a > b
                    ? IsCoprime(a - b, b)
                    : IsCoprime(b - a, a);
        } 
        #endregion

        private void RSAEncrypt()
        {
            BaseText = BaseText.ToUpper();
            IEnumerable<char> textChars = BaseText.Distinct();

            N = P * Q;
            Fi = (P - 1) * (Q - 1);
            Random rnd = new Random();            
            int x = Fi;
            while (!PrimeList.Contains(x))
            {
                x--;
            }
            int e;

            e = rnd.Next(0, PrimeList.IndexOf(x));

        }

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
            //ActiveTab = 1;
            return;
        }
        #endregion

        #region DecipherCommand : DecipherCommand - Команда зашифрувати
        public ICommand DecipherCommand { get; }
        private bool CanDecipherCommandExecute(object p) => true;
        private void OnDecipherCommandExecuted(object p)
        {
            //Decipher();
            return;
        }
        #endregion

        /*------------------------------------------------------------------------------------*/

        public MainWindowViewModel()
        {
            Dictionary<char, int> standardFrequency = new Dictionary<char, int>();
            //standardFrequency = GetEtalonDict();
            standardFrequency = GetRSADict();
            standardFrequency[' '] = standardFrequency['_'];
            standardFrequency.Remove('_');
            StandardDict = standardFrequency;
            List<int> listofprimes = new List<int>();
            listofprimes = CreatePrimeList();
            PrimeList = listofprimes;

            EncryptCommand = new LambdaCommand(OnEncryptCommandExecuted, CanEncryptCommandExecute);
            DecipherCommand = new LambdaCommand(OnDecipherCommandExecuted, CanDecipherCommandExecute);

        }
    }
}
