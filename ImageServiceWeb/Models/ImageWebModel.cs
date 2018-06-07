using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Communication.Client;

namespace ImageServiceWeb.Models
{
    /// <summary>
    /// the image web model
    /// </summary>
    public class ImageWebModel
    {
        private static IImageServiceClient Client { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageWebModel"/> class.
        /// </summary>
        public ImageWebModel()
        {
            Client = ImageServiceClient.Instance;
            IsConnected = Client.IsConnected;
            NumOfPics = 0;
            Students = GetStudents();
        }

        [Required]
        [Display(Name = "Is Connected")]
        public bool IsConnected { get; set; }

        [Required]
        [Display(Name = "Number of Pictures")]
        public int NumOfPics { get; set; }

        /// <summary>
        /// Gets the students details from the file within the app data
        /// </summary>
        /// <returns></returns>
        public static List<Student> GetStudents()
        {
            List<Student> students = new List<Student>();
            StreamReader file = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Details.txt"));
            string line;

            while ((line = file.ReadLine()) != null)
            {
                string[] param = line.Split(' ');
                students.Add(new Student() { FirstName = param[0], LastName = param[1], ID = param[2] });
            }
            file.Close();
            return students;
        }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Students")]
        public List<Student> Students { get; set; }

        /// <summary>
        /// definition of each student.
        /// </summary>
        public class Student
        {
            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [Display(Name = "ID")]
            public string ID { get; set; }
        }
    }
}