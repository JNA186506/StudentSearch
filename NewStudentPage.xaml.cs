using StudentSearch.models;

namespace StudentSearch;

public partial class NewStudentPage : ContentPage {
    private readonly Dat154Context _dx;

    public NewStudentPage(Dat154Context dx) {
        InitializeComponent();

        _dx = dx;
    }

    private void OnSaveStudentClicked(object? sender, EventArgs e) {
        if (string.IsNullOrWhiteSpace(IdEntry.Text) || !int.TryParse(IdEntry.Text, out var studentId)) {
            DisplayAlertAsync("Error", "Please enter a valid student ID (0-9999).", "OK");
            return;
        }

        if (studentId is < 0 or > 9999) DisplayAlertAsync("Error", "Invalid id entry", "Ok");

        var oldStudent = _dx.Students.FirstOrDefault(s => s.Id == studentId);
        if (oldStudent != null) {
            DisplayAlertAsync("Error", "Student with this ID already exists", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(NameEntry.Text)) {
            DisplayAlertAsync("Error", "Name cannot be empty", "Ok");
            return;
        }

        if (string.IsNullOrWhiteSpace(AgeEntry.Text) || !int.TryParse(AgeEntry.Text, out var age)) {
            DisplayAlertAsync("Error", "Please enter a valid age.", "OK");
            return;
        }

        if (age < 0 || age > 150) {
            DisplayAlertAsync("Error", "Please enter an age between 0 and 150", "OK");
            return;
        }

        var name = NameEntry.Text;

        var newStudent = new Student { Id = studentId, Studentname = name, Studentage = age };

        _dx.Students.Add(newStudent);

        NameEntry.Text = string.Empty;
        AgeEntry.Text = string.Empty;

        DisplayAlertAsync("Success", "Student added", "Ok");
        Navigation.PopToRootAsync();
    }
}