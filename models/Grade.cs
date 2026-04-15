namespace StudentSearch.models;

public partial class Grade {
    public int Studentid { get; set; }
    public string Coursecode { get; set; } = null!;
    public string Grade1 { get; set; }

    public virtual Course CourseCodeNavigation { get; set; } = null!;
    public virtual Student Student { get; set; } = null!;
}