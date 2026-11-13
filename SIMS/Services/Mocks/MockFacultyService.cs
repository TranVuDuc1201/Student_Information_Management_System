using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SIMS.Models.ViewModels;
using SIMS.Services.Interfaces;

namespace SIMS.Services.Mocks
{
    public class MockFacultyService : IFacultyService
    {
        public Task<FacultyGradingViewModel> GetGradingSheetAsync(Guid? courseId = null)
        {
            var defaultCourseId = courseId ?? Guid.Parse("11111111-1111-1111-1111-111111111111");

            var vm = new FacultyGradingViewModel
            {
                FacultyId = Guid.NewGuid(),
                FacultyName = "Dr. Michael Vance",
                Department = "Department of Software Engineering & Architecture",
                SelectedCourseId = defaultCourseId,
                SelectedCourseCode = "APDP301",
                SelectedCourseName = "Applied Programming & Design Principles",
                Semester = "Fall Semester 2026",
                GradingStrategy = "Standard-Weighted (10-30-20-40)",
                AutoCalculateGpa = true,
                AssignedCourses = new List<CourseSelectionViewModel>
                {
                    new CourseSelectionViewModel
                    {
                        CourseId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        CourseCode = "APDP301",
                        CourseName = "Applied Programming & Design Principles",
                        TotalEnrolled = 35,
                        MaxCapacity = 40
                    },
                    new CourseSelectionViewModel
                    {
                        CourseId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                        CourseCode = "OOP202",
                        CourseName = "Object-Oriented Programming with C#",
                        TotalEnrolled = 38,
                        MaxCapacity = 40
                    },
                    new CourseSelectionViewModel
                    {
                        CourseId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                        CourseCode = "DS101",
                        CourseName = "Data Structures & Algorithms",
                        TotalEnrolled = 40,
                        MaxCapacity = 40
                    }
                },
                StudentGrades = new List<StudentGradeEntryViewModel>
                {
                    new StudentGradeEntryViewModel
                    {
                        EnrollmentId = Guid.NewGuid(),
                        StudentId = Guid.NewGuid(),
                        StudentCode = "BH02383",
                        StudentName = "Tran Vu Duc",
                        AvatarUrl = "https://images.unsplash.com/photo-1534528741775-53994a69daeb?auto=format&fit=crop&w=128&q=80",
                        AttendanceScore = 10,
                        MidtermScore = 28.5,
                        AssignmentScore = 19.0,
                        FinalScore = 38.0,
                        TotalCalculatedScore = 95.5,
                        LetterGrade = "A+",
                        Status = "Submitted",
                        Note = "Outstanding architectural documentation."
                    },
                    new StudentGradeEntryViewModel
                    {
                        EnrollmentId = Guid.NewGuid(),
                        StudentId = Guid.NewGuid(),
                        StudentCode = "BH02190",
                        StudentName = "Nguyen Thi Mai",
                        AvatarUrl = "https://images.unsplash.com/photo-1494790108377-be9c29b29330?auto=format&fit=crop&w=128&q=80",
                        AttendanceScore = 9.5,
                        MidtermScore = 25.0,
                        AssignmentScore = 18.0,
                        FinalScore = 34.0,
                        TotalCalculatedScore = 86.5,
                        LetterGrade = "A-",
                        Status = "Submitted",
                        Note = "Solid implementation of clean code patterns."
                    },
                    new StudentGradeEntryViewModel
                    {
                        EnrollmentId = Guid.NewGuid(),
                        StudentId = Guid.NewGuid(),
                        StudentCode = "BH02411",
                        StudentName = "Le Hoang Nam",
                        AvatarUrl = "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?auto=format&fit=crop&w=128&q=80",
                        AttendanceScore = 8.0,
                        MidtermScore = 22.0,
                        AssignmentScore = 16.5,
                        FinalScore = 31.0,
                        TotalCalculatedScore = 77.5,
                        LetterGrade = "B+",
                        Status = "Draft",
                        Note = "Good progress, needs minor refactor in DIP."
                    },
                    new StudentGradeEntryViewModel
                    {
                        EnrollmentId = Guid.NewGuid(),
                        StudentId = Guid.NewGuid(),
                        StudentCode = "BH02505",
                        StudentName = "Pham Quoc Bao",
                        AvatarUrl = "https://images.unsplash.com/photo-1500648767791-00dcc994a43e?auto=format&fit=crop&w=128&q=80",
                        AttendanceScore = 10.0,
                        MidtermScore = 27.0,
                        AssignmentScore = 18.5,
                        FinalScore = 36.0,
                        TotalCalculatedScore = 91.5,
                        LetterGrade = "A",
                        Status = "Submitted",
                        Note = "Excellent understanding of S.O.L.I.D."
                    },
                    new StudentGradeEntryViewModel
                    {
                        EnrollmentId = Guid.NewGuid(),
                        StudentId = Guid.NewGuid(),
                        StudentCode = "BH02620",
                        StudentName = "Dang Thu Trang",
                        AvatarUrl = "https://images.unsplash.com/photo-1438761681033-6461ffad8d80?auto=format&fit=crop&w=128&q=80",
                        AttendanceScore = 9.0,
                        MidtermScore = 24.0,
                        AssignmentScore = 17.0,
                        FinalScore = null,
                        TotalCalculatedScore = null,
                        LetterGrade = "Incomplete",
                        Status = "Draft",
                        Note = "Awaiting final capstone project submission."
                    }
                }
            };

            return Task.FromResult(vm);
        }

        public Task<bool> SaveGradesAsync(FacultyGradingViewModel model)
        {
            return Task.FromResult(true);
        }
    }
}

