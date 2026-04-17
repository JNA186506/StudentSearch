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

    }


    private void StudentList_SelectionChanged(object? sender, SelectionChangedEventArgs e) {
        var selected = e.CurrentSelection.FirstOrDefault() as Student;
 
    }

    private void DoClearStudent(object? sender, EventArgs e) {
        StudentList.SelectedItem = null;
    }

    private void DoSearchStudent(object? sender, EventArgs e) {
        StudentList.BindingContext = _students
            .Where(s => s.Studentname.Contains(SearchField.Text, StringComparison.CurrentCultureIgnoreCase))
            .OrderBy(s => s.Studentname);
    }

    private void ChangeToGradeReports(object? sender, EventArgs e) {
        Navigation.PushAsync(new ManageCourses(_dx));
    }

    private void ChangeToEditStudentPage(object? sender, EventArgs e) {
        var selected = (Student)StudentList.SelectedItem;
        if (selected == null) {
            DisplayAlertAsync("Error", "Please select the student you want to change", "OK");
        }
        if (selected != null) {
            Navigation.PushAsync(new EditStudentView(_dx, selected));
        }
    }

    private void ChangeToNewStudentPage(object? sender, EventArgs e) {
        Navigation.PushAsync(new NewStudentPage(_dx));
    }
}