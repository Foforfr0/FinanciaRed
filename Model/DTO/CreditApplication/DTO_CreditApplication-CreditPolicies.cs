using System.ComponentModel;

namespace FinanciaRed.Model.DTO.CreditApplication{
    internal class DTO_CreditApplication_CreditPolicies : INotifyPropertyChanged{
        public int IdCreditApplication {
            get; set;
        }
        public int IdCreditPolicy {
            get; set;
        }
        public string NameCreditPolicy {
            get; set;
        }
        private bool? _isApplied;
        public bool? IsApplied {
            get => _isApplied;
            set {
                if (_isApplied != value) {
                    _isApplied = value;
                    OnPropertyChanged (nameof (IsApplied));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged (string propertyName) {
            PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (propertyName));
        }
    }
}
