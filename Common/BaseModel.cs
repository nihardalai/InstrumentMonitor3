using System.ComponentModel;

namespace Common
{
    public abstract class BaseModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public abstract void UpdateWith(BaseModel newModel);
    }
}
