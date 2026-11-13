using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SIMS.Data;
using SIMS.Models.ViewModels;
using SIMS.Services.Interfaces;

namespace SIMS.Services.Implementations
{
    public class SqlFacultyService : IFacultyService
    {
        private readonly SimsDbContext _context;

        public SqlFacultyService(SimsDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<FacultyGradingViewModel> GetGradingSheetAsync(Guid? courseId = null)
        {
            var dbCourses = await _context.Courses.AsNoTracking().ToListAsync();
            var dbStudents = await _context.Students.AsNoTracking().ToListAsync();

            var assignedCourses = new List<CourseSelectionViewModel>();

            if (dbCourses.Any())
            {
                assignedCourses = dbCourses.Select(c => new CourseSelectionViewModel
                {
                    CourseId = c.CourseId,
                    CourseCode = c.CourseCode,
                    CourseName = c.CourseName,
                    TotalEnrolled = dbStudents.Count,
                    MaxCapacity = 40
                }).ToList();
            }
            else
            {
                assignedCourses.Add(new CourseSelectionViewModel
                {
                    CourseId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    CourseCode = "APDP301",
                    CourseName = "Applied Programming & Design Principles",
                    TotalEnrolled = dbStudents.Count,
                    MaxCapacity = 40
                });
            }

            var selectedCourse = courseId.HasValue
                ? assignedCourses.FirstOrDefault(c => c.CourseId == courseId.Value) ?? assignedCourses.First()
                : assignedCourses.First();

            var studentGrades = new List<StudentGradeEntryViewModel>();

            if (dbStudents.Any())
            {
                foreach (var s in dbStudents)
                {
                    // Map score derived from student's current GPA (e.g., GPA 3.82 -> ~95.5)
                    double totalScore = Math.Round((s.Gpa / 4.0) * 100.0, 1);
                    double attendance = 10.0;
                    double midterm = Math.Round(totalScore * 0.3, 1);
                    double assignment = Math.Round(totalScore * 0.2, 1);
                    double finalExam = Math.Round(totalScore * 0.4, 1);

                    studentGrades.Add(new StudentGradeEntryViewModel
                    {
                        EnrollmentId = Guid.NewGuid(),
                        StudentId = s.Id,
                        StudentCode = s.StudentCode,
                        StudentName = s.FullName,
                        AvatarUrl = "https://images.unsplash.com/photo-1534528741775-53994a69daeb?auto=format&fit=crop&w=128&q=80",
                        AttendanceScore = attendance,
                        MidtermScore = midterm,
                        AssignmentScore = assignment,
                        FinalScore = finalExam,
                        TotalCalculatedScore = totalScore,
                        LetterGrade = GetLetterGrade(totalScore),
                        Status = "Submitted",
                        Note = $"Record loaded from database for student {s.StudentCode}."
                    });
                }
            }

            return new FacultyGradingViewModel
            {
                FacultyId = Guid.NewGuid(),
                FacultyName = "Dr. Michael Vance",
                Department = "Department of Software Engineering & Architecture",
                SelectedCourseId = selectedCourse.CourseId,
                SelectedCourseCode = selectedCourse.CourseCode,
                SelectedCourseName = selectedCourse.CourseName,
                Semester = "Fall Semester 2026",
                GradingStrategy = "Standard-Weighted (10-30-20-40)",
                AutoCalculateGpa = true,
                AssignedCourses = assignedCourses,
                StudentGrades = studentGrades
            };
        }

        public async Task<bool> SaveGradesAsync(FacultyGradingViewModel model)
        {
            if (model?.StudentGrades == null || !model.StudentGrades.Any())
            {
                return true;
            }

            // Update GPA for each student in the database based on their submitted score
            foreach (var grade in model.StudentGrades)
            {
                var student = await _context.Students.FindAsync(grade.StudentId);
                if (student != null)
                {
                    if (grade.TotalCalculatedScore.HasValue)
                    {
                        student.Gpa = Math.Round((grade.TotalCalculatedScore.Value / 100.0) * 4.0, 2);
                    }
                    else if (grade.FinalScore.HasValue)
                    {
                        var attendance = grade.AttendanceScore ?? 0;
                        var midterm = grade.MidtermScore ?? 0;
                        var assignment = grade.AssignmentScore ?? 0;
                        var finalExam = grade.FinalScore.Value;
                        var total = attendance + midterm + assignment + finalExam;
                        student.Gpa = Math.Round((total / 100.0) * 4.0, 2);
                    }
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }

        private static string GetLetterGrade(double score)
        {
            return score switch
            {
                >= 90 => "A+",
                >= 85 => "A",
                >= 80 => "A-",
                >= 75 => "B+",
                >= 70 => "B",
                >= 65 => "C+",
                >= 60 => "C",
                >= 50 => "D",
                _ => "F"
            };
        }
    }
}

