using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfHello
{
    public class Student
    {
        public string StudentName { get; set; }
        public bool IsEnrolled { get; set; }
        public Student(string name, bool ch)
        {
            StudentName = name;
            IsEnrolled = ch;
        }
        public string FullStudentData
        {
            get { return StudentName + "\t" + IsEnrolled; }
        }
    }

    public class StudentList : ObservableCollection<Student>
    {
        public StudentList()
        {
            Add(new Student("Lorin Kanev", true));
            Add(new Student("Ivan Petrov", true));
            Add(new Student("Sergey Masov", false));
            Add(new Student("Tais Frolova", true));
            Add(new Student("Elena Diva", false));
        }
    }
}
