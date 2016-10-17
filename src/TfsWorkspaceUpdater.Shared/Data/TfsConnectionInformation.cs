namespace TfsWorkspaceUpdater.Shared.Data
{
    using System;
    using System.ComponentModel;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography;
    using System.Text;
    using System.Xml.Serialization;
    
    public class TfsConnectionInformation : INotifyPropertyChanged
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Fields

        private string _tfsAddress;
        private bool _integratedSecurity;
        private string _username;
        private string _password;

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Properties

        [XmlElement]
        public string TfsAddress
        {
            get { return _tfsAddress; }
            set
            {
                if (value == _tfsAddress) return;
                _tfsAddress = value;
                OnPropertyChanged();
            }
        }

        [XmlElement]
        public bool IntegratedSecurity
        {
            get { return _integratedSecurity; }
            set
            {
                if (value == _integratedSecurity) return;
                _integratedSecurity = value;
                OnPropertyChanged();
            }
        }

        [XmlElement]
        public string Username
        {
            get { return _username; }
            set
            {
                if (value == _username) return;
                _username = value;
                OnPropertyChanged();
            }
        }

        [XmlIgnore]
        public string Password
        {
            get { return _password; }
            set
            {
                if (value == _password) return;
                _password = value;
                OnPropertyChanged();
            }
        }

        [XmlElement(ElementName = "Password")]
        public string EncodedPassword
        {
            get { return string.IsNullOrWhiteSpace(Password) ? string.Empty : EncodePassword(Password); }
            set { Password = string.IsNullOrWhiteSpace(value) ? string.Empty : DecodePassword(value); }
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Private Methods

        private static string EncodePassword(string password)
        {
            var entropy = Encoding.ASCII.GetBytes(Assembly.GetExecutingAssembly().FullName);
            var userData = Encoding.ASCII.GetBytes(password);
            return Convert.ToBase64String(ProtectedData.Protect(userData, entropy, DataProtectionScope.CurrentUser));
        }

        private static string DecodePassword(string password)
        {
            var entropy = Encoding.ASCII.GetBytes(Assembly.GetExecutingAssembly().FullName);
            var encryptedData = Convert.FromBase64String(password);
            return Encoding.ASCII.GetString(ProtectedData.Unprotect(encryptedData, entropy, DataProtectionScope.CurrentUser));
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region INotifyPropertyChanged Members & Extension

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
