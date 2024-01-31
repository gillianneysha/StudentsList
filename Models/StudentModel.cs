using DataAccess;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;

namespace ISPRGG2_Finals_Dy_Lim_Esguerra.Models
{
    public class StudentModelLogin
    {
        [Required(ErrorMessage ="Please input username")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Please input password")]
        public string Password { get; set; }


        private DAL dl = new();
        public bool LogIn()
        {
            dl.SetSql("Select * from Accounts where Username = @Username and Password = @Password");
            dl.AddParam("@Username", Username);
            dl.AddParam("@Password", Password);
            bool success = dl.checkSuccess();
            if (success)
            {
                return (true);
            }
            else
            {
                return (false);

            }
        }

    }

    public class StudentModelRegister
    {
        [Required(ErrorMessage = "Please input username")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Please input password")]
        public string Password { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Please input confirm password")]
        [CompareAttribute("Password", ErrorMessage = "Confirm password doesn't match, Try again!")]
        public string ConfirmPassword { get; set; }

        private DAL dl = new();
        public bool RegisterOne()
        {

            dl.SetSql("Select * from Accounts where Username = @name");
            dl.AddParam("@name", Username);
            bool uniq = dl.checkAvailable();
            if (uniq == false)
            {
                return (true);
            }
            else
            {
                return (false);
            }
        }


        public void RegisterTwo()
        {
            dl.SetSql("Insert into Accounts Values (@name, @pw)");
            dl.AddParam("@name", Username);
            dl.AddParam("@pw", Password);
            dl.Execute();

        }

    }

    public class StudentModelChange
    {
        [Required(ErrorMessage = "Please input username")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Please input password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please input new password")]
        public string NewPassword { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "Please input confirm password")]
        [CompareAttribute("NewPassword", ErrorMessage = "Confirm password doesn't match, Try again!")]
        public string ConfirmPassword { get; set; }

        private DAL dl = new();

        public void ChangePass()
        {
            dl.SetSql("Update Accounts set Password = @pw where Username = @name");
            dl.AddParam("@pw", NewPassword);
            dl.AddParam("name", Username);
            dl.Execute();
        }

    }

    public class StudentModelRecords
    {
        public int StudentID { get; set; }
        [Required(ErrorMessage = "Last Name is required!")]
        public string Lastname { get; set; }
        [Required(ErrorMessage = "First Name is required!")]
        public string Firstname { get; set; }

        private DAL dl = new();

        public StudentModelRecords Get(int ID)
        {
            StudentModelRecords s = new();

            dl.Open();
            dl.SetSql("SELECT * FROM Students WHERE StudentID = @id");
            dl.AddParam("@id", ID);
            SqlDataReader dr = dl.GetReader();
            if (dr.Read() == true)
            {
                s.StudentID = (int)dr[0];
                s.Firstname = (string)dr[1];
                s.Lastname = (string)dr[2];
            }
            dr.Close();
            dl.Close();

            return s;

        }
        public List<StudentModelRecords> Get()
        {
            List<StudentModelRecords> slist = new();

            dl.Open();
            dl.SetSql("SELECT * FROM Students");
            SqlDataReader dr = dl.GetReader();
            while (dr.Read() == true)
            {
                StudentModelRecords s = new();
                s.StudentID = (int)dr[0];
                s.Firstname = (string)dr[1];
                s.Lastname = (string)dr[2];

                slist.Add(s);
            }
            dr.Close();
            dl.Close();

            return slist;

        }

		public List<StudentModelRecords> Get(string search)
		{
			List<StudentModelRecords> students = new List<StudentModelRecords>();
			string sql = "SELECT * FROM Students WHERE Lastname LIKE '%' + @search + '%' OR Firstname LIKE '%' + @search + '%'";

			DAL dal = new DAL();
			dal.SetSql(sql);
			dal.AddParam("@search", search);

			try
			{
				dal.Open();

				SqlDataReader dr = dal.GetReader();

				while (dr.Read())
				{
					StudentModelRecords student = new StudentModelRecords();
					student.StudentID = Convert.ToInt32(dr["StudentID"]);
					student.Lastname = Convert.ToString(dr["Lastname"]);
					student.Firstname = Convert.ToString(dr["Firstname"]);
					students.Add(student);
				}

				dr.Close();
			}
			catch (Exception ex)
			{
                throw ex;
			}
			finally
			{
				dal.Close();
			}

			return students;


		}

		public void Add()
        {
            dl.SetSql("INSERT INTO Students VALUES (@ln, @fn)");
            dl.AddParam("@ln", Lastname);
            dl.AddParam("@fn", Firstname);
            dl.Execute();
        }

        public void Delete()
        {
            dl.SetSql("DELETE FROM Students WHERE StudentID = @id");
            dl.AddParam("@id", StudentID);
            dl.Execute();
        }

        public void Update()
        {
            dl.SetSql("UPDATE Students SET Lastname = @ln, Firstname = @fn " +
                "WHERE StudentID = @id");
            dl.AddParam("@ln", Lastname);
            dl.AddParam("@fn", Firstname);
            dl.AddParam("@id", StudentID);
            dl.Execute();
        }
    }

}
