using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace TeamProject
{
    class Member
    {
        private int memberID;
        private string userName;
        private string password;
        private string firstName;
        private string lastName;
        private bool managerStatus;

        public Member()
        {

        }
        //constructor to logIn 
        public Member(string username, string password)
        {
            this.userName = username;
            this.password = password;
        }

        public Member(int memberID, string userName, string password, string firstName, string lastName, bool managerStatus)
        {
            this.memberID = memberID;
            this.userName = userName;
            this.password = password;
            this.firstName = firstName;
            this.lastName = lastName;
            this.managerStatus = managerStatus;
        }

        public void setmemberID(int memberID)
        {
            this.memberID = memberID;
        }

        public int getMemebrID()
        {
            return this.memberID;
        }
        public void setuserName(string userName)
        {
            this.userName = userName;
        }

        public String getUserName()
        {
            return this.userName;
        }

        public void setPassword(string password)
        {
            this.password = password;
        }

        public String getPassword()
        {
            return this.password;
        }

        public void setFirstName(string firstName)
        {
            this.firstName = firstName;
        }

        public String getFirstName()
        {
            return this.firstName;
        }

        public void setLastName(string lastName)
        {
            this.lastName = lastName;
        }

        public String getLastName()
        {
            return this.lastName;
        }

        public void setManagerStatus(bool status)
        {
            this.managerStatus = status;
        }

        public bool getManagerStatus()
        {
            return this.managerStatus;
        }

        public static bool verifyUserNameAndPassword(string username, string password)
        {
            //compare the input text to the database customer info 
            //first access the database and pull the custID based on the input
            bool result = true;
            DataTable myTable = new DataTable(); //will read all rows from the returning values
            string connStr = "server=157.89.28.130;user=ChangK;database=csc340;port=3306;password=Wallace#409;";

            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);

            try
            {

                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                Console.WriteLine("WE made it here");
                string sql = "SELECT * FROM ijmember WHERE username=@un AND password=@pw;";
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@un", username);
                cmd.Parameters.AddWithValue("@pw", password);
                MySqlDataReader myReader = cmd.ExecuteReader();
                if (myReader.Read())
                {
                    //Console.WriteLine("we here");
                    //Console.WriteLine("maybe");
                    //textBox1.Text = myReader["FirstName"].ToString();
                    //textBox2.Text = myReader["LastName"].ToString();
                    //textBox3.Text = myReader["ID"].ToString();
                   
                    result = true;
                }
                else
                {
                    result = false;
                }
                myReader.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            Console.WriteLine("Done.");
            return result;
        }

        public static bool checkManagerStatusWith(String givenString )
        {
            if (givenString[0] == '0')
            {
                return false;
            }
            return true; 
        }
    } //end of class member
}
