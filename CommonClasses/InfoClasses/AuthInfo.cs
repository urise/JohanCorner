using System;
using System.Globalization;
using System.Linq;
using CommonClasses.DbClasses;
using CommonClasses.Helpers;
using CommonClasses.Roles;
using Interfaces.DbInterfaces;
using Interfaces.MiscInterfaces;

namespace CommonClasses.InfoClasses
{
    public class AuthInfo: IEncryptor
    {
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public DateTime LastActiveDate { get; set; }
        public bool FinanceKeyIsEntered { get; set; } //can't be readonly as we need to set finance key on change key form
        public bool FinanceKeyIsNeeded { get; set; }
        public UserAccess UserAccess { get; set; }
        private TripleDesProvider _tripleDesProvider;

        public int AuthTokenId { get; set; }
        public string Token { get; set; }
        public int LinkedCompanyId
        {
            get { return CompanyId; }
            set { CompanyId = value; }
        }

        public AuthInfo()
        {
            UserAccess = new UserAccess();
        }

        public AuthInfo(int userId, int companyId, bool financeKeyIsEntered, bool financeKeyIsNeeded)
        {
            UserId = userId;
            CompanyId = companyId;
            FinanceKeyIsEntered = financeKeyIsEntered;
            FinanceKeyIsNeeded = financeKeyIsNeeded;
        }

        public void SetFinanceKey(string financeKey)
        {
            if (FinanceKeyIsEntered && FinanceKeyIsNeeded)
                _tripleDesProvider = new TripleDesProvider(financeKey);
        }

        public void ReSetFinanceKey(string financeKey)
        {
            if(string.IsNullOrEmpty(financeKey))
            {
                FinanceKeyIsNeeded = false;
                FinanceKeyIsEntered = false;
                return;
            }
            FinanceKeyIsNeeded = true;
            FinanceKeyIsEntered = true;
            _tripleDesProvider = new TripleDesProvider(financeKey);
        }

        public string EncryptDecimal(decimal value)
        {
            if (FinanceKeyIsNeeded)
            {
                if (!FinanceKeyIsEntered)
                    throw new Exception(Messages.WrongFinanceKey);
                return _tripleDesProvider.Encrypt(value.ToString());
            }
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public decimal DecryptDecimal(string str)
        {
            if (FinanceKeyIsNeeded)
            {
                if (!FinanceKeyIsEntered)
                    return 0;
                return decimal.Parse(_tripleDesProvider.Decrypt(str));
            }
            return str.ToDecimal();
        }

        public bool AccessGranted(System.Reflection.MethodBase method)
        {
            Attribute[] attributes = Attribute.GetCustomAttributes(method).Where(r => r is AccessTier).ToArray();
            if (attributes.Length == 0) return true;

            foreach(var attr in attributes)
            {
                var accessTier = (AccessTier)attr;
                if (UserAccess.IsGranted(accessTier.Component, accessTier.Level))
                {
                    return true;
                }
            }
            return false;
        }
    }
}