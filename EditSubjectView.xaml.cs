using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentSearch.models;

namespace StudentSearch;

/**
 * This class and view handles adding and removing grades to the database.
 * It works by updating a variable that the class owns, and will dynamically update the list.
 * Takes the Dat154Context and the student to be updated as parameters.
 */
public partial class EditSubjectView : ContentPage {

    private Student _student;
    private Dat154Context _dx;

    private readonly ObservableCollection<Grade> _grades;
    private readonly ObservableCollection<Course> _courses;
    private ObservableCollection<Grade> _currStudentGrades;
    private readonly List<string> _possibleGrades = new() { "A", "B", "C", "D", "E", "F"};
    
    public EditSubjectView(Dat154Context dx, Student student) {
        InitializeComponent();

        _student = student;
        _dx = dx;
    
        _dx.Grades.Load();
        _dx.Courses.Load();

        _grades = _dx.Grades.Local.ToObservableCollection();
        _courses = _dx.Courses.Local.ToObservableCollection();

        var query = _grades.Where(g => g.Studentid == student.Id);
        _currStudentGrades = new ObservableCollection<Grade>(query);

        StudentGradeCollection.ItemsSource = _currStudentGrades;
        CoursePicker.ItemsSource = _courses;
        GradePicker.ItemsSource = _possibleGrades;
    }


    private void OnAddEntry(object? sender, EventArgs e) {
        var selectedCourse = CoursePicker.SelectedItem as Course;
        var selectedGrade = GradePicker.SelectedItem as string;
        
        if (selectedCourse is null) {
            DisplayAlertAsync("Error", "Please select a course", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(selectedGrade)) {
            DisplayAlertAsync("Error", "Please select a grade", "OK");
            return;
        }
        

        Grade newGrade = new Grade
            { Coursecode = selectedCourse.Coursecode, Grade1 = selectedGrade, Studentid = _student.Id };

        bool gradeExists = _currStudentGrades.Any(g => g.Coursecode == selectedCourse.Coursecode);

        if (gradeExists) {
            DisplayAlertAsync("Error", "Grade has already been set", "OK");
            return;
        }

        _dx.Add(newGrade);
        _currStudentGrades.Add(newGrade);
        _dx.SaveChanges();

    }

    private void OnDeleteEntry(object? sender, EventArgs e) {
        var btn = (Button)sender;
        var grade = btn.CommandParameter as Grade ?? btn.BindingContext as Grade;
    
        if (grade == null) {
            DisplayAlertAsync("Error", "Something went wrong, try again", "OK");
            return;
        }

        _dx.Remove(grade);
        _currStudentGrades.Remove(grade);
        _dx.SaveChanges();
    }
}