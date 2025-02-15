using System;
using System.Collections.Generic;

namespace AspCoreWebAPICRUD.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public int? Age { get; set; }

    public string? Grade { get; set; }

    public DateOnly? EnrollmentDate { get; set; }
}
