using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject
{
    class Event
    {
        private int eventID;
        private string eventName;
        private string location;
        private string startTime;
        private string endTime;
        private int duration;
        private string description;
        private string day;
        private string month;
        private string year;
        private string userName;

        //Allow the right datetime for database
        private DateTime startTimeDateFormat;
        private DateTime endTimeDateFormat;
        //constructor to logIn 
        public Event()
        {
     
        }

        //constructor to create a new Event Object and save it to the database
        public Event(string EventName, string location, string startTime, string endTime, string description, string day, string month, string year, string userName)
        {
            this.eventName = EventName;
            this.location = location;
            this.startTime = startTime;
            this.endTime = endTime;
            this.description = description;
            this.day = day;
            this.month = month;
            this.year = year;
            this.userName = userName;
        }

        public Event(int eventID, string eventName, string location, string startTime, string endTime, int duration,
        string description,
        string day,
        string month,
        string year)
        {
            this.eventID = eventID;
            this.eventName = eventName;
            this.location = location;
            this.startTime = startTime;
            this.endTime = endTime;
            this.duration = duration;
            this.description = description;
            this.day = day;
            this.month = month;
            this.year = year;
        }

        public string getUserName()
        {
            return this.userName;
        }
        public void setUserName(string userName)
        {
            this.userName = userName;
        }
        public int getEventID()
        {
            return this.eventID;
        }
        public void setEventID(int eventID)
        {
            this.eventID = eventID;
        }
        public void setEventName(string eventName)
        {
            this.eventName = eventName;
        }

        public String getEventName()
        {
            return this.eventName;
        }

        public void setLocation(string location)
        {
            this.location = location;
        }

        public String getLocation()
        {
            return this.location;
        }

        public void setStartTime(string startTime)
        {
            this.startTime = startTime;
        }

        public String getStartTime()
        {
            return this.startTime;
        }

        public void setEndTime(string endtime)
        {
            this.endTime = endtime;
        }

        public String getEndTime()
        {
            return this.endTime;
        }

        public void setDuration(int duration)
        {
            this.duration = duration;
        }

        public int getDuration()
        {
            return this.duration;
        }

        public void setDescription(string description)
        {
            this.description = description;
        }

        public String getDescription()
        {
            return this.description;
        }

        public void setDay(string day)
        {
            this.day = day;
        }

        public String getDay()
        {
            return this.day;
        }

        public void setMonth(string month)
        {
            this.month = month;
        }

        public String getMonth()
        {
            return this.month;
        }

        public void setYear(string year)
        {
            this.year = year;
        }

        public String getYear()
        {
            return this.year;
        }

        //##### Function to determine which Type of viewEvent we need to display - > ViewEvent or ViewMonthlyEvent #####
        private static void viewSingleOrMonthlyEvents(string year, string month, string day, string usingUserName,string eventType, MySqlConnection conn, DataTable myTable)
        {
            switch (eventType)              //Perform the right selection based on the selectedButton -> viewEvent or ViewMonthlyEvent
            {
                case "ViewEvent":           //Find events for a specific day
                    conn.Open();
                    string selectSingleEventSQL = "SELECT eventID,eventName,location,startTime,endTime, duration,description,DAY,MONTH,YEAR from ijevent WHERE DAY = @day AND MONTH = @month AND YEAR = @year AND username = @userName ORDER BY eventID ASC;";
                    MySqlCommand cmd = new MySqlCommand(selectSingleEventSQL, conn);
                    cmd.Parameters.AddWithValue("@userName", usingUserName);
                    cmd.Parameters.AddWithValue("@day", day);
                    cmd.Parameters.AddWithValue("@month", month);
                    cmd.Parameters.AddWithValue("@year", year);
                    MySqlDataAdapter myAdapter = new MySqlDataAdapter(cmd);
                    myAdapter.Fill(myTable);
                    //Console.WriteLine("we are in viewEvents");      //Debugging only
                    break;
                case "ViewMonthlyEvent":    //Find events for a specific month   
                    conn.Open();
                    string selectMonthlyEventsSQL = "SELECT eventID,eventName,location,startTime,endTime, duration,description,DAY,MONTH,YEAR from ijevent WHERE MONTH = @month AND YEAR = @year AND username = @userName ORDER BY eventID ASC;";
                    MySqlCommand cmd1 = new MySqlCommand(selectMonthlyEventsSQL, conn);
                    cmd1.Parameters.AddWithValue("@userName", usingUserName);
                    cmd1.Parameters.AddWithValue("@month", month);
                    cmd1.Parameters.AddWithValue("@year", year);
                    MySqlDataAdapter myAdapter1 = new MySqlDataAdapter(cmd1);
                    myAdapter1.Fill(myTable);
                    //Console.WriteLine("we are in viewMonthlyEvents");       
                    break;
            }

        }           //##### End of typeEvent Function #####

        public static ArrayList RetrieveEvents(string year, string month, string day, string usingUserName, string currentButtonSelected)
        {
            ArrayList listsOfEvents = new ArrayList();
            DataTable myTable = new DataTable(); //will read all rows from the returning values
            string connectionToDatabase = "server=157.89.28.130;user=ChangK;database=csc340;port=3306;password=Wallace#409;";
            MySqlConnection conn = new MySqlConnection(connectionToDatabase);

            try
            {
                Console.WriteLine("Connecting to MySql...");    //Connect to the database
                viewSingleOrMonthlyEvents(year,month,day,usingUserName,currentButtonSelected,conn,myTable); //Call Function to display the right data

                Console.WriteLine("Table is ready");           
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            conn.Close();

            foreach (DataRow row in myTable.Rows)               //convert the retrieved data to Event and save them to the list
            {
                Event newEvent = new Event();                   //create default Account Class without initialization
                                                                //Populate an Event object for each event found in the database
                newEvent.eventID = Int32.Parse(row["eventID"].ToString());
                newEvent.eventName = row["eventName"].ToString();
                newEvent.location = row["location"].ToString();
                newEvent.startTime = row["startTime"].ToString();
                newEvent.endTime = row["endTime"].ToString();
                newEvent.duration = Int32.Parse(row["duration"].ToString());
                newEvent.description = row["description"].ToString();
                newEvent.day = row["day"].ToString();
                newEvent.month = row["month"].ToString();
                newEvent.year = row["year"].ToString();

                listsOfEvents.Add(newEvent);
            }
            return listsOfEvents;
        }
                        //##### End of Retrieve Events Function#####

        //##### ShowEventDetails Function #####
        public string showEventDetails(Event currentEvent)      //Display event Details to user
        {
            //Console.WriteLine("The current event has an Event ID of " + getEventID());
            return "Event Name: " + currentEvent.getEventName() + "\nLocation: " + currentEvent.getLocation() + "\nStart Time: " + currentEvent.getStartTime() + " \nEnd Time: " + currentEvent.getEndTime() + "\nDuration: " + currentEvent.getDuration()+" minutes" + "\nDescription: " + currentEvent.getDescription() + " ";
        }
                       //End of ShowEventDetails Function #####
        
            //##### DeleteEvent Function #####
        public void deleteEvent(Event currentEvent)
        {
            //Make the database connection. 
            string connectionToDatabase = "server=157.89.28.130;user=ChangK;database=csc340;port=3306;password=Wallace#409;";
            MySql.Data.MySqlClient.MySqlConnection conn = new MySqlConnection(connectionToDatabase);
            conn = new MySql.Data.MySqlClient.MySqlConnection(connectionToDatabase);

            try
            {
                Console.WriteLine("Connecting to MySql..."); 
                conn.Open();
                string selectSingleEventSQL = "DELETE FROM ijevent WHERE eventID = @eventID;";
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySqlCommand(selectSingleEventSQL, conn);

                cmd.Parameters.AddWithValue("@eventID", currentEvent.getEventID());
                cmd.ExecuteNonQuery();
                Console.WriteLine(currentEvent.getEventID());
                Console.WriteLine("Event has been deleted");     
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            conn.Close();
        }
                //##### End Of DeleteEvent Function #####

        //###### Function to Insert an Event into the database start from here ######

        public Boolean findOverLappingEventsWith(string eventStartTime, string eventEndTime, string year, string month, string day, string withCurrentUserName)
        {
            //Retrieve any event between the startTime and endTime -> If the reading is true -> Overlapping Occurs
            //otherwise we are good to add the event. 

            //first access the database and find if there are events going on between startTime and endTime
            bool overlappingHasNotoccur = true;
            DataTable myTable = new DataTable(); //will read all rows from the returning values
            string connStr = "server=157.89.28.130;user=ChangK;database=csc340;port=3306;password=Wallace#409;";

            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);

            try
            {

                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                Console.WriteLine("About to check for overlapping events");
                string sql = "SELECT * FROM ijevent WHERE(TIME_FORMAT(endTime, '%H:%i') >= @startTime) AND(DATE_FORMAT(startTime, '%H:%i') <= @endTime) AND(DAY = @day AND MONTH = @month AND YEAR = @year AND userName = @userName); ";
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@startTime", eventStartTime);
                cmd.Parameters.AddWithValue("@endTime", eventEndTime);
                cmd.Parameters.AddWithValue("@day", day);
                cmd.Parameters.AddWithValue("month", month);
                cmd.Parameters.AddWithValue("year", year);
                cmd.Parameters.AddWithValue("userName", withCurrentUserName);
                MySqlDataReader myReader = cmd.ExecuteReader();
                if (myReader.Read())
                {
                    //Console.WriteLine("we here");
                    //Console.WriteLine("maybe");
                    //textBox1.Text = myReader["FirstName"].ToString();
                    //textBox2.Text = myReader["LastName"].ToString();
                    //textBox3.Text = myReader["ID"].ToString();

                    overlappingHasNotoccur = true;
                }
                else
                {
                    overlappingHasNotoccur = false;
                }
                myReader.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();

            Console.WriteLine(day);
            Console.WriteLine(month);
            Console.WriteLine(year);
            return overlappingHasNotoccur;
        }//End Of Overlapping Events


        public void addEvent()
        {
            //insert new Event to the database
            string connStr = "server=157.89.28.130;user=ChangK;database=csc340;port=3306;password=Wallace#409;";
            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);

            conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql = "INSERT INTO ijevent(eventName,location,startTime,endTime,duration,description,DAY,MONTH,YEAR,userName) VALUES(@eventName, @eventLocation, @eventStartTime, @eventEndTime, TIMESTAMPDIFF(MINUTE, startTime, endTime), @description, @day, @month, @year, @userName);";
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@eventName", eventName);
                cmd.Parameters.AddWithValue("@eventLocation", location);
                cmd.Parameters.AddWithValue("@eventStartTime", startTime);
                cmd.Parameters.AddWithValue("@eventEndTime", endTime);
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@day", day);
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@userName", userName);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            Console.WriteLine("event added");
        }

                    //###### End of AddEvent to database #####

        //function to update any given Event
        public void updateEvent(Event currentEvent)
        {
            //Make the database connection
            string connectionToDatabase = "server=157.89.28.130;user=ChangK;database=csc340;port=3306;password=Wallace#409;";
            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connectionToDatabase);
            conn = new MySql.Data.MySqlClient.MySqlConnection(connectionToDatabase);

            try
            {
                Console.WriteLine("Connecting to MySql...");    //Connect to the database.
                conn.Open();
                string selectSingleEventSQL = "UPDATE ijevent SET eventName=@eventName, location=@location, startTime=@startTime, endTime=@endTime, duration = TIMESTAMPDIFF(MINUTE, startTime, endTime), description=@description WHERE eventID=@eventID AND userName=@userName;";
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(selectSingleEventSQL, conn);
                Console.WriteLine(startTime + "here we are");
                Console.WriteLine(endTime);
                cmd.Parameters.AddWithValue("@eventName", currentEvent.getEventName());
                cmd.Parameters.AddWithValue("@location", currentEvent.getLocation());
                cmd.Parameters.AddWithValue("@startTime", currentEvent.getStartTime());
                cmd.Parameters.AddWithValue("@endTime", currentEvent.getEndTime());
                cmd.Parameters.AddWithValue("@description", currentEvent.getDescription());
                cmd.Parameters.AddWithValue("@eventID", currentEvent.getEventID());
                cmd.Parameters.AddWithValue("@userName", currentEvent.getUserName());
                //cmd.ExecuteReader();
                cmd.ExecuteNonQuery();

                Console.WriteLine("Event has been updated");     
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            conn.Close();
        }//End of UpdateEvent

        public void updateEvents(Event currentEvent)
        {
            string connStr = "server = 157.89.28.130; user = ChangK; database = csc340; port = 3306; password = Wallace#409;";
            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql = "UPDATE ijevent SET eventName = @eventName, location = @location, startTime = @startTime, endTime = @endTime, duration = TIMESTAMPDIFF(MINUTE, startTime, endTime), description = @description WHERE eventID = @eventID AND userName = @userName;";
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@eventName", currentEvent.getEventName());
                cmd.Parameters.AddWithValue("@location", currentEvent.getLocation());
                cmd.Parameters.AddWithValue("@startTime", currentEvent.getStartTime());
                cmd.Parameters.AddWithValue("@endTime", currentEvent.getEndTime());
                cmd.Parameters.AddWithValue("@description", currentEvent.getDescription());
                cmd.Parameters.AddWithValue("@eventID", currentEvent.getEventID());
                cmd.Parameters.AddWithValue("@userName", currentEvent.getUserName());
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            Console.WriteLine("event updated");
        }
    } //end of event class
}//end Project
