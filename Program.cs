using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace StudentApiClient
{
    class Program
    {
        static readonly HttpClient httpClient = new HttpClient();

        static async Task Main(string[] args)
        {
            httpClient.BaseAddress = new Uri("http://localhost:5215/api/Students/");

            await GetAllStudents();
            await GetPassedStudents();
            await GetAverageGrade();
            await GetStudentById(1);
            await GetStudentById(20);

            var newStudent = new Student { Name = "Mazen Abdullah", Age = 20, Grade = 85 };
            await AddStudent(newStudent);
            await GetAllStudents();

            await DeleteStudent(1);
            await GetAllStudents();

            await UpdateStudent(2, new Student { Name = "Salma", Age = 22, Grade = 90 });
            await GetAllStudents();
        }

        static async Task GetAllStudents()
        {
            Console.WriteLine("\nFetching all students...");
            var response = await httpClient.GetAsync("All");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var students = JsonSerializer.Deserialize<List<Student>>(json);
                students?.ForEach(s => Console.WriteLine($"ID: {s.Id}, Name: {s.Name}, Age: {s.Age}, Grade: {s.Grade}"));
            }
        }

        static async Task GetPassedStudents()
        {
            Console.WriteLine("\nFetching passed students...");
            var response = await httpClient.GetAsync("Passed");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var students = JsonSerializer.Deserialize<List<Student>>(json);
                students?.ForEach(s => Console.WriteLine($"ID: {s.Id}, Name: {s.Name}, Age: {s.Age}, Grade: {s.Grade}"));
            }
        }

        static async Task GetAverageGrade()
        {
            Console.WriteLine("\nFetching average grade...");
            var response = await httpClient.GetAsync("AverageGrade");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var avg = JsonSerializer.Deserialize<double>(json);
                Console.WriteLine($"Average Grade: {avg}");
            }
        }

        static async Task GetStudentById(int id)
        {
            Console.WriteLine($"\nFetching student with ID {id}...");
            var response = await httpClient.GetAsync($"{id}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var student = JsonSerializer.Deserialize<Student>(json);
                if (student != null)
                {
                    Console.WriteLine($"ID: {student.Id}, Name: {student.Name}, Age: {student.Age}, Grade: {student.Grade}");
                }
            }
        }

        static async Task AddStudent(Student student)
        {
            Console.WriteLine("\nAdding a new student...");
            var json = JsonSerializer.Serialize(student);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("", content);
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                var addedStudent = JsonSerializer.Deserialize<Student>(responseJson);
                Console.WriteLine($"Added Student - ID: {addedStudent?.Id}, Name: {addedStudent?.Name}, Age: {addedStudent?.Age}, Grade: {addedStudent?.Grade}");
            }
        }

        static async Task DeleteStudent(int id)
        {
            Console.WriteLine($"\nDeleting student with ID {id}...");
            var response = await httpClient.DeleteAsync($"{id}");
            Console.WriteLine(response.IsSuccessStatusCode
                ? $"Student with ID {id} has been deleted."
                : $"Failed to delete student with ID {id}.");
        }

        static async Task UpdateStudent(int id, Student student)
        {
            Console.WriteLine($"\nUpdating student with ID {id}...");
            var json = JsonSerializer.Serialize(student);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync($"{id}", content);
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                var updatedStudent = JsonSerializer.Deserialize<Student>(responseJson);
                Console.WriteLine($"Updated Student: ID: {updatedStudent?.Id}, Name: {updatedStudent?.Name}, Age: {updatedStudent?.Age}, Grade: {updatedStudent?.Grade}");
            }
        }
    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Grade { get; set; }
    }
}
