using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using StudentSearch.models;

namespace StudentSearch;

/**
 * This class handles the logic on the main page. You can make a new student.
 * Edit a student, and delete a student. You can also navigate to pages that give a
 * grade report for every student, and editing a selected students grades.
 */
public partial class MainPage : ContentPage {
    private readonly Dat154Context _dx;
    private readonly ObservableCollection<Student> _students;

    public MainPage(Dat154Context dx) {
        InitializeComponent();

        _dx = dx;
        _dx.Students.Load();
        _students = _dx.Students.Local.ToObservableCollection();


        StudentList.BindingContext = _students.OrderBy(s => s.Studentname);
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
        if (selected == null) DisplayAlertAsync("Error", "Please select the student you want to change", "OK");

        if (selected != null) Navigation.PushAsync(new EditStudentView(_dx, selected));
    }

    private void ChangeToNewStudentPage(object? sender, EventArgs e) {
        Navigation.PushAsync(new NewStudentPage(_dx));
    }

    private void ChangeToEditGradePage(object? sender, EventArgs e) {
        var selected = (Student)StudentList.SelectedItem;
        if (selected == null) DisplayAlertAsync("Error", "Please select a student to edit grades", "OK");

        if (selected != null) Navigation.PushAsync(new EditSubjectView(_dx, selected));
    }

    private async void OnDeleteStudent(object? sender, EventArgs e) {
        var selected = StudentList.SelectedItem as Student;
        if (selected == null) {
            await DisplayAlertAsync("Error", "Choose a student to delete", "OK");
            return;
        }

        var confirm = await DisplayAlertAsync("Confirm", "Do you want to remove the selected student?", "Yes", "No");

        if (!confirm) {
            return;
        }
        _dx.Students.Remove(selected);
        _dx.SaveChanges();
      
    }
}