using System.ComponentModel;

namespace StudentSearch.models;

public partial class Student : INotifyPropertyChanged {
    
    public Student() {
        Grades = [];
    }
    
    public int Id { get; set; }

    public string Studentname {
        get;
        set {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    } = null!;

    public int Studentage {
        get;
        set {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
    
    public virtual ICollection<Grade> Grades { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;
}