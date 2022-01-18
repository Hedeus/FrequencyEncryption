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
        private Dictionary<char, double> _StandardDict;        

        public Dictionary<char, double> StandardDict       
        {
            get => _StandardDict;
            set => Set(ref _StandardDict, value);
        }
        #endregion

        #region TestdDict : TestdDict        
        private Dictionary<char, char> _TestdDict;

        public Dictionary<char, char> TestdDict
        {
            get => _TestdDict;
            set => Set(ref _TestdDict, value);
        }
        #endregion

        /*------------------------------------------------------------------------------------*/

        //Отримання частотного розподілення літер
        private Dictionary<char, double> GetEtalonDict()
        {
            Dictionary<char, double> dict = new Dictionary<char, double>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            FileInfo info = new FileInfo("Частотность.xlsx");
            using (ExcelPackage xlPackage = new ExcelPackage(info))
            {
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[0];                
                for (int iRow = 1; iRow <= 34; iRow++)
                {
                    dict[Convert.ToChar(worksheet.GetValue(iRow, 1))] = Convert.ToDouble(worksheet.GetValue(iRow, 2));
                }                
            }
            return dict;
        }

        #region Функція Шифрування
        private void Encrypt()
        {
            Dictionary<char, char> cipher = new Dictionary<char, char>();
            BaseText = BaseText.ToUpper();
            foreach (var c in BaseText)
            {
                cipher[c] = '0';
            }            

            double d;
            string CharsForRemoving = "";
            foreach (var pair in cipher)
            {
                if (!StandardDict.TryGetValue(pair.Key, out d))
                    CharsForRemoving += pair.Key;                    
            }
            foreach (var c in CharsForRemoving)
            {
                cipher.Remove(c);
            }
            cipher = cipher.OrderBy(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value);

            Dictionary<char, int> cipherRandomizer = new Dictionary<char, int>();
            Random rnd = new Random();
            foreach (var pair in StandardDict)
            {
                cipherRandomizer.Add(pair.Key, rnd.Next(0, 1000));
            }
            cipherRandomizer = cipherRandomizer.OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

            for (int i = 0; i < cipher.Count; i++)
            {
                cipher[cipher.ElementAt(i).Key] = cipherRandomizer.ElementAt(i).Key;               
            }

            TestdDict = cipher;

            char litera;
            EncryptedText = "";
            foreach (var c in BaseText)
            {
                if (!cipher.TryGetValue(c, out litera))
                    litera = c;
                EncryptedText += litera;
            }

        }
        #endregion

        #region Функція розшифрування
        private void Decipher()
        {

            return;
        }
        #endregion

        #region EncryptCommand : EncryptCommand - Команда зашифрувати
        public ICommand EncryptCommand { get; }
        private bool CanEncryptCommandExecute(object p) => true;
        private void OnEncryptCommandExecuted(object p)
        {
            Encrypt();
            ActiveTab = 1;
            return;
        }
        #endregion

        #region DecipherCommand : DecipherCommand - Команда зашифрувати
        public ICommand DecipherCommand { get; }
        private bool CanDecipherCommandExecute(object p) => true;
        private void OnDecipherCommandExecuted(object p)
        {
            return;
        }
        #endregion

        /*------------------------------------------------------------------------------------*/

        public MainWindowViewModel()
        {
            Dictionary<char, double> standardFrequency = new Dictionary<char, double>();
            standardFrequency = GetEtalonDict();            
            standardFrequency[' '] = standardFrequency['_'];
            standardFrequency.Remove('_');
            StandardDict = standardFrequency;

            EncryptCommand = new LambdaCommand(OnEncryptCommandExecuted, CanEncryptCommandExecute);
            DecipherCommand = new LambdaCommand(OnDecipherCommandExecuted, CanDecipherCommandExecute);

        }
    }
}
