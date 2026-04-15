namespace StudentSearch.models;

public partial class Course
{
    public Course() {
        Grades = [];
    }

    public string Coursecode { get; set; } = null!;
    public string Coursename { get; set; } = null!;
    public string Semester { get; set; } = null!;
    public string Teacher { get; set; } = null!;

    public virtual ICollection<Grade> Grades { get; set; }
}