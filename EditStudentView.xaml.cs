using System.Text.RegularExpressions;
using StudentSearch.models;

namespace StudentSearch;

public partial class EditStudentView : ContentPage {
    

    private Dat154Context _dx;
    private Student _student;
    
    public EditStudentView(Dat154Context dx, Student student) {
        InitializeComponent();

        _dx = dx;
        _student = student;

        CurrId.Text = student.Id.ToString();
        NameEntry.Text = student.Studentname;
        AgeEntry.Text = student.Studentage.ToString();
    }
    
    private void OnChangeStudent_Clicked(object? sender, EventArgs e) {

        if (string.IsNullOrWhiteSpace(NameEntry.Text)) {
            DisplayAlertAsync("Error", "Name cannot be empty", "Ok");
            return;
        }

        int age = int.Parse(AgeEntry.Text);

        if (age < 0 || age > 150) {
            DisplayAlertAsync("Error", "Please enter an age between 0 and 150", "OK"); 
            return;
        }

        _student.Studentname = NameEntry.Text;
        _student.Studentage = age;

        _dx.SaveChanges();

        DisplayAlertAsync("Success", "Student edited", "Ok");
        Navigation.PopToRootAsync();
    }
    
    private void OnAddStudent_Clicked(object? sender, EventArgs e) {
        
    }
}