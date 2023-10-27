using SQLite;
using System.ComponentModel;
using System.Runtime.CompilerServices;

[Table("Produce")]
public class Produce : INotifyPropertyChanged
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public string Name { get; set; }
    public string ImagePath { get; set; }

    public ProduceType Type { get; set; }
    private int _quantity;
    public int Quantity
    {
        get { return _quantity; }
        set
        {
            if (_quantity != value)
            {
                _quantity = value;
                OnPropertyChanged();
            }
        }
    }

    public enum ProduceType
    {
        Fruit,
        Bakery,
        RedMeat,
        Deli,
        Cheese,
        Baking,
        Chip,
        Frozen,
        Drink,
        Personal,
        Cleaning,
        Pet
        // ... other types if necessary
    }





    public int UserId { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
