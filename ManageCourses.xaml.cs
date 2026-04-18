using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using StudentSearch.models;

namespace StudentSearch;

/**
 * This class shows a list of all the students with a grade.
 * It will show all grades less than or equals the one chosen.
 * It can also filter by course.
 * Only takes Dat154Context as parameter.
 */
public partial class ManageCourses : ContentPage {
    
    private readonly List<string> _possibleGrades = new() { "A", "B", "C", "D", "E", "F"};
    
    private readonly ObservableCollection<Student> _studentView;
    private readonly ObservableCollection<Grade> _grades;
    private readonly ObservableCollection<Course> _courses;
    private ObservableCollection<StudentGradeModel> studentGrades;
    public ManageCourses(Dat154Context dx) {
        InitializeComponent();
        
        dx.Students.Load();
        dx.Grades.Load();
        dx.Courses.Load();

        _studentView = dx.Students.Local.ToObservableCollection();
        _grades = dx.Grades.Local.ToObservableCollection();
        _courses = dx.Courses.Local.ToObservableCollection();
        
        GradePicker.ItemsSource = _possibleGrades;
        CoursePicker.ItemsSource = _courses;

        var query = _studentView.Join(_grades,
            student => student.Id, grade => grade.Studentid,
            ((student, grade) =>
                new StudentGradeModel {
                    Name = student.Studentname, 
                    Course = grade.Coursecode, 
                    Grade = grade.Grade1
                })).OrderBy(g => g.Grade);

        studentGrades = new ObservableCollection<StudentGradeModel>(query);
        
        StudentGradeView.ItemsSource = studentGrades;

    }

     private void OnSelectionFilter(object? sender, EventArgs e) {
         var gradeSelected =(string?) GradePicker.SelectedItem;
         var courseSelected = (Course?) CoursePicker.SelectedItem;

         var filtered = studentGrades.AsEnumerable();

         if (gradeSelected != null) {

             filtered = filtered.Where(s =>
                 string.Compare(s.Grade, gradeSelected, StringComparison.OrdinalIgnoreCase) <= 0);

         }

         if (courseSelected != null) {
             filtered = filtered.Where(sg => 
                 sg.Course.Equals(courseSelected.Coursecode, StringComparison.OrdinalIgnoreCase));
         }

         StudentGradeView.ItemsSource = filtered.OrderBy(s => s.Grade)
             .ToList();
     }

}