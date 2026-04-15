using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using StudentSearch.models;

namespace StudentSearch;

public partial class MainPage : ContentPage {

    private readonly Dat154Context _dx;
    private readonly ObservableCollection<Student> _students;
    private readonly ObservableCollection<Course> _courses;
    
    public MainPage(Dat154Context dx) {
        InitializeComponent();
        
        _dx = dx;
        _students = _dx.Students.Local.ToObservableCollection();
        _courses = _dx.Courses.Local.ToObservableCollection();
        
        _dx.Students.Load();
        _dx.Courses.Load();
        
        StudentList.BindingContext = _students.OrderBy(s => s.Studentname);
        CoursePicker.BindingContext = _courses;

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
        if (string.IsNullOrWhiteSpace(IdEntry.Text) || !int.TryParse(IdEntry.Text, out int studentId)) {
            DisplayAlertAsync("Error", "Please enter a valid student ID (0-9999).", "OK");
            return;
        }
        if (studentId is < 0 or > 9999) {
            DisplayAlertAsync("Error", "Invalid id entry", "Ok");
        }

        Student? oldStudent = _students.FirstOrDefault(s => s.Id == studentId);
        if (oldStudent != null) {
            DisplayAlertAsync("Error", "Student with this ID already exists", "OK");
            return;
        }
        
        if (string.IsNullOrWhiteSpace(NameEntry.Text)) {
            DisplayAlertAsync("Error", "Name cannot be empty", "Ok");
            return;
        }

        if (string.IsNullOrWhiteSpace(AgeEntry.Text) || !int.TryParse(AgeEntry.Text, out int age)) {
            DisplayAlertAsync("Error", "Please enter a valid age.", "OK");
            return;
        }
        if (age < 0 || age > 150) {
            DisplayAlertAsync("Error", "Please enter an age between 0 and 150", "OK"); 
            return;
        }

        string name = NameEntry.Text;

        Student newStudent = new Student { Id = studentId, Studentname = name, Studentage = age };
        
        _students.Add(newStudent);
        _dx.Students.Add(newStudent);
        

        IdEntry.Text = string.Empty;
        NameEntry.Text = string.Empty;
        AgeEntry.Text = string.Empty;

        DisplayAlertAsync("Success", "Student added", "Ok");
    }

    private void OnChangeStudent_Clicked(object? sender, EventArgs e) {

        if (string.IsNullOrWhiteSpace(NameEntry.Text)) {
            DisplayAlertAsync("Error", "Name cannot be empty", "Ok");
            return;
        }


        if (!Regex.IsMatch(IdEntry.Text, @"\d")) {
            DisplayAlertAsync("Error", "Id has to be number", "Ok");
            return;
        }
        
        int? id = int.Parse(IdEntry.Text);
        Student? student = _students.FirstOrDefault(s => s.Id == id);
        if (student == null) {
            DisplayAlertAsync("Error", "Could not find student", "Ok");
            return;
        }

        int age = int.Parse(AgeEntry.Text);

        if (id != student.Id) {
            DisplayAlertAsync("Error", "Cannot change ID", "Ok");
            return;
        }
        
        if (age < 0 || age > 150) {
            DisplayAlertAsync("Error", "Please enter an age between 0 and 150", "OK"); 
            return;
        }

        student.Studentname = NameEntry.Text;
        student.Studentage = age;

        _dx.SaveChanges();
    }

    private void DoSearchStudent(object? sender, EventArgs e) {
        StudentList.BindingContext = _students
            .Where(s => s.Studentname.Contains(SearchField.Text, StringComparison.CurrentCultureIgnoreCase))
            .OrderBy(s => s.Studentname);
    }
}