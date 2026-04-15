using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using StudentSearch.models;

namespace StudentSearch;

public partial class MainPage : ContentPage {

    private readonly Dat154Context _dx;
    private readonly ObservableCollection<Student> _students;
    
    public MainPage(Dat154Context dx) {
        InitializeComponent();
        
        _dx = dx;
        _students = _dx.Students.Local.ToObservableCollection();
        
        _dx.Students.Load();
        StudentList.BindingContext = _students.OrderBy(s => s.Studentname);

        ChangeStudentButton.Clicked += OnAddStudent_Clicked;
    }


    private void StudentList_SelectionChanged(object? sender, SelectionChangedEventArgs e) {
        var selected = e.CurrentSelection.FirstOrDefault() as Student;
        if (selected != null) {
            NameEntry.Text = selected.Studentname;
            IdEntry.Text = selected.Id.ToString();
            AgeEntry.Text = selected.Studentage.ToString();

            ChangeStudentButton.Text = "Endre Student";
            ChangeStudentButton.Clicked -= OnAddStudent_Clicked;
            ChangeStudentButton.Clicked += OnChangeStudent_Clicked;
        }
        else {
            NameEntry.Text = string.Empty; 
            IdEntry.Text = string.Empty;
            AgeEntry.Text = string.Empty;
            
            ChangeStudentButton.Text = "Ny Student";
            ChangeStudentButton.Clicked -= OnChangeStudent_Clicked;
            ChangeStudentButton.Clicked += OnAddStudent_Clicked;
        }
    }

    private void DoClearStudent(object? sender, EventArgs e) {
        StudentList.SelectedItem = null;
    }

    private void OnAddStudent_Clicked(object? sender, EventArgs e) {
        
    }

    private void OnChangeStudent_Clicked(object? sender, EventArgs e) {
        throw new NotImplementedException();
    }

    private void DoSearchStudent(object? sender, EventArgs e) {
        StudentList.BindingContext = _students
            .Where(s => s.Studentname.Contains(SearchField.Text, StringComparison.CurrentCultureIgnoreCase))
            .OrderBy(s => s.Studentname);
    }
}