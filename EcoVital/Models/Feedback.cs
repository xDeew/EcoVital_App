using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EcoVital.Models
{
    public sealed class Feedback : INotifyPropertyChanged
    {
        private int _feedbackId;
        private string _email = string.Empty;
        private string _type;
        private string _message = string.Empty;

        public int FeedbackId
        {
            get => _feedbackId;
            set
            {
                _feedbackId = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public string Type
        {
            get => _type;
            set
            {
                _type = value;
                OnPropertyChanged();
            }
        }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}