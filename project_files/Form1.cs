using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TeamProject
{
    public partial class Form1 : Form
    {
        //global variables Objects
        Member thismember;
        Event thisEvent;
        DateTime date;

        string selectedDay; //Date for ViewEvent
        string currentButtonSelected;

        //Global ArrayList to store all the events once they have been retrieved.
        ArrayList listOfMemberEvents = new ArrayList();

        public Form1()
        {
            InitializeComponent();
            PasswordTextBox.PasswordChar = '*';                         //Add a Password Security character
            //The AddEvent and EditForm should be hiding until the user clicks Add or Edit Event 
            AddFormPanel.Visible = false;                               //AddEventPanel is Disable.
            //Button will be disable until the user selects an event.
            DeleteButton.Enabled = false;                               //DeleteButton is disable.
            EditButton.Enabled = false;                                 //EditButton is disable.
            selectedDay = DatePicker.Value.Date.ToString("MM/dd/yyyy"); //initialize selectedDate with todaysDate as default 
            AlertLabel.Text = "";
            MainMenuPanel.Visible = false;
            //DisplayPanel should be visible once viewEvent is clicked 
            DetailPanel.Visible = false;
            SelectionDatePanel.Visible = false;
        }

        //###### SignIn Function #######
        private void SignInButton_Click(object sender, EventArgs e)
        {
            string userName = UserNameTextBox.Text;
            string password = PasswordTextBox.Text;
            bool isValidUser = Member.verifyUserNameAndPassword(userName, password);

            //Log in and display member main menu
            if (isValidUser == true)
            {
                thismember = new Member(userName,password);
                //Console.WriteLine("Success!!"); 
                MainMenuPanel.Visible = true;       //make main menu visible
                                                    //After a successful LogIn
                UserNameTextBox.Text = "";          //reset UserNameTextBox
                PasswordTextBox.Text = "";          //reset PasswordTextBox
                AlertLabel.Text = "";               //reset AlertLabel
            }
            else
            {
                AlertLabel.Text = "No matching member found, please try again! ";
            }
        }       //##### SingInFunction End #######

        
        //##### SignOut Function ##### 
        private void SignOutButtonClicked(object sender, EventArgs e)
        {
            //clear everything just in case a user want to return.
            EventDetailLabel.Text = "";
            EventListBox.Items.Clear();
            SelectionAlertLabel.Text = "";
            //end of clear

            MainMenuPanel.Visible = false;          //return to sigIn menu
        }//##### SignOut Function End ######


        //####### Everything regarding ViewEvent start form here ##########
        private void ViewEventClicked(object eventButton, EventArgs e)  //viewEvent Button clicked
        {
            AddFormPanel.Visible = false ;          //AddForm Panel only shows until user selects AddEvent.
            disableDeleteButtons();                 //Disable Deletion functionality.
            //Buttons will be disable -> Enable them once a user selects an Event.
            DeleteButton.Enabled = false;           //Disable DeleteButton.
            EditButton.Enabled = false;             //Disable EditButton.
            //Every time we clicked a new button

            //Get the currentButtonTitle to -> Retrieve events based on a specific day.
            currentButtonSelected = "ViewEvent";                        
            InputDateLabel.Text = "Please select a day to view events"; //Display correct format label to user.
            SelectionDatePanel.Visible = true;    //make the SelectionDatePanel visible.
            // #### If we come from another window we need to display everything new #### 
            EventListBox.Items.Clear();           //clear The items from the EventListBox.
            EventDetailLabel.Text = "";           //Clear the EventDetails. 
            SelectionAlertLabel.Text = "";        //Clear AlertLabel
        }

        //DatePicker Function
        private void dayPickerValueChanged(object sender, EventArgs e)
        {
            AddFormPanel.Visible = false;           //AddForm Panel only shows until user selects AddEvent.
            disableDeleteButtons();                 //Disable Deletion functionality.
            //Buttons will be disable -> Enable them once a user selects an Event.
            DeleteButton.Enabled = false;           //Disable DeleteButton.
            EditButton.Enabled = false;             //Disable EditButton.

            selectedDay = DatePicker.Value.Date.ToString("MM/dd/yyyy"); //get the currentDate as the user selects one day
            selectedDay = correctFormatDate(selectedDay);               //format the date for the database
            //Console.WriteLine(DatePicker.Value.Date.ToString("MM/dd/yyyy")); //debugging only

            //## Reset Values Before and After the Selection is Done#####
            SelectionAlertLabel.Text = "";
            EventListBox.Items.Clear();     //Clear items on EventListBox when a new day is selected.
            EventDetailLabel.Text = "";     //Clear the event details if any when a new day is selected.

        }//End DatePicker Function

        //correct format date -> sometimes we miss a Zero
        private string correctFormatDate(string date) //in order to have the same format date perform the function
        {
            switch (date.Substring(0, 1))
            {
                case "0":
                    return date;
                default:
                    return "0" + date;             //when zero is missing, add it and return.
            }
        }
        //Get the member events based on the selectedDay
        private void FindEventButtonClicked(object sender, EventArgs e)
        {
            AddFormPanel.Visible = false;           //AddForm Panel only shows until user selects AddEvent.
            disableDeleteButtons();                 //Disable Deletion functionality.
            //Buttons will be disable -> Enable them once a user selects an Event.
            DeleteButton.Enabled = false;           //Disable DeleteButton.
            EditButton.Enabled = false;             //Disable EditButton.
            EventDetailLabel.Text = "";             //Clear the EventDetail Label. 
            listOfMemberEvents = TeamProject.Event.RetrieveEvents(selectedDay.ToString().Substring(6,4), selectedDay.ToString().Substring(0, 2), selectedDay.ToString().Substring(3, 2), thismember.getUserName(), currentButtonSelected);
            displayAccountsUsing(listOfMemberEvents);
        }//end of Finding button

        private void displayAccountsUsing(ArrayList listOfMemberEvent)           //display Event function
        {
            if(listOfMemberEvent.Count > 0) //If list not empty                                           
            {
                EventListBox.Items.Clear(); //Clear any existing events before we add more. 
                foreach (Event item in listOfMemberEvent)
                {
                    //format and display lists of accounts in the table layout
                    Console.WriteLine(item.getEventName());
                    EventListBox.Items.Add(item.getEventName());                 //Populate EventListBox.
                }
            }
            else
            {
                updateAlertLabelWith();                   //ErrorMessage
                EventListBox.Items.Clear();
            }
            
        }//end of displayEvents Function

                    //##### Function to handle the right Alert based on the currentButtonSelected #####. 
        private void updateAlertLabelWith()
        {
            switch (currentButtonSelected)      
            {
                case "ViewEvent":
                    SelectionAlertLabel.Text = "Sorry, no Events for this day!";
                    break;
                case "ViewMonthlyEvent":
                    SelectionAlertLabel.Text = "Sorry, no Events for this Month!";
                    break;
            }
        }           //##### End of updateAlertLabelUsing Function #####

        //get the selected Event to display details
        private void getSelectedEventFromEventListBox(object sender, EventArgs e)
        {
            if(EventListBox.SelectedIndex < listOfMemberEvents.Count)               //Only displayed event are allowed to be selected.
            {
                disableButtonsToAddEvent(false,currentButtonSelected);//debugging Only
                //AddFormPanel.Visible = false;    DebugginOLy                      //AddForm Panel only shows until user selects AddEvent.
                disableDeleteButtons();                                             //Disable Deletion functionality.
                DetailPanel.Visible = true;                                         //Show detail Panel.
                thisEvent = (Event)listOfMemberEvents[EventListBox.SelectedIndex];  //Update the currentEvent Based on the user selection.
                EventDetailLabel.Text = thisEvent.showEventDetails(thisEvent);      //Display Event Details to user when Event is selected.
                //Console.WriteLine("current index "+ EventListBox.SelectedIndex); ;  //for debugging Only
                DeleteButton.Enabled = true;                                        //Enable DeleteButton to allow Functionality.
                EditButton.Enabled = true;                                          //Enable EditButton to allow Functionality.
            }        
        }//End of SelectEvent from the EventListBox

                          //  ####### End of ViewEvent ##########



                        //####### Everything regarding ViewMonthlyEvent start form here ##########
        private void ViewmonthlyEventClicked(object sender, EventArgs e)
        {
            AddFormPanel.Visible = false;            //AddForm Panel only shows until user selects AddEvent.
            disableDeleteButtons();
            //Buttons will be disable -> Enable them once a user selects an Event.
            DeleteButton.Enabled = false;           //Disable DeleteButton.
            EditButton.Enabled = false;             //Disable EditButton.
            //Every time we clicked a new button.

            //Get the currentButtonTitle to -> Retrieve events based on a specific month.
            currentButtonSelected = "ViewMonthlyEvent";
            //Change InputDate label to month selection
            InputDateLabel.Text = "Plese select a month to view events";//Display correct format label to user.
            SelectionDatePanel.Visible = true;                          //Display SelectiondatePanel.
            EventListBox.Items.Clear();                                 //Clear EventListBox.
            SelectionAlertLabel.Text = "";                              //Clear the alert label.
            EventDetailLabel.Text = "";                                 //Clear the EventDetail Label.
        }

                        //####### End ViewMonthlyEvent Ends here ##########

        //####### Everything reguarding DeleteEvent start form here ##########
        private void DeleteButtonClicked(object sender, EventArgs e)
        {
            AddFormPanel.Visible = false;          //AddForm Panel only shows until user selects AddEvent.
            disableAllOhterFunctionalityUntilDeleteGetsCancel(false);
            //Buttons and Labels to Confirm or Cancel Deletion will be visible when DeleteButton is clicked
            YesButton.Visible = true;              //YesButton is not visible.
            NoButton.Visible = true;               //NoButton is not visible.
            DeleteAlertLabel.Text = "Are you sure you want to delete the current selected event?"; //Clear DeleteAlertLabel.  
        }

        //This function will be use over and over.
        private void disableDeleteButtons()
        {
            //Buttons and Labels to Confirm or Cancel Deletion will not be visible until DeleteButton is clicked
            YesButton.Visible = false;              //YesButton is not visible.
            NoButton.Visible = false;               //NoButton is not visible.
            DeleteAlertLabel.Text = "";             //Clear DeleteAlertLabel.
        }//End of DisableDeleteButtons Function.

        //Function to disable SelectionDatePanel,ViewEventButton,ViewMonthlyEventButton,AddButton to allow a proper user interaction.
        private void disableAllOhterFunctionalityUntilDeleteGetsCancel(Boolean willAppear)
        {
            SelectionDatePanel.Enabled = willAppear;
            ViewButton.Enabled = willAppear;
            ViewMonthlyButton.Enabled = willAppear;
            AddButton.Enabled = willAppear;
            SignOutButton.Enabled = willAppear;
            EditButton.Enabled = willAppear;
        }

        //All Functionality should go back to normal -> Enable the Buttons we disabled to allow user interaction.
        private void NoButtonClicked(object sender, EventArgs e)
        {
            disableAllOhterFunctionalityUntilDeleteGetsCancel(true);        //Enable the functionality of all other Buttons.
            YesButton.Visible = false;                                      //Hide NoButton
            NoButton.Visible = false;                                       //Hide YesButton
            DeleteAlertLabel.Text = "";                                     //Clear Text.
        }

        //Deletion should be perform once the user acknowledge the operation.
        private void YesButtonClicked(object sender, EventArgs e)
        {
            thisEvent.deleteEvent(thisEvent);                               //Delete Event from the database.
            disableAllOhterFunctionalityUntilDeleteGetsCancel(true);        //Enable Other functionalities.
            YesButton.Visible = false;                                      //Hide NoButton
            NoButton.Visible = false;                                       //Hide YesButton
            DeleteAlertLabel.Text = "";                                     //Clear Text.
            EventDetailLabel.Text = "";                                     //Clear EventDetailLabel.
            SelectionAlertLabel.Text = "Event deleted successfully!";       //Display Successful message.
            
            //Clear the deleted items from the listBox by retrieving all the events from the database after the deletion was completed.
            listOfMemberEvents = TeamProject.Event.RetrieveEvents(selectedDay.ToString().Substring(6, 4), selectedDay.ToString().Substring(0, 2), selectedDay.ToString().Substring(3, 2), thismember.getUserName(), currentButtonSelected);
            displayAccountsUsing(listOfMemberEvents);                       //Display all the events.

        }
                    //####### End of DeleteEvent ##########



        //##### Everything regarding AddEvent Starts from here #####/////
        private void AddEventCliecked(Object sender, EventArgs e)
        {
            //Display the right messages to the user to interact with the form.
            EventListBox.Items.Clear();                                 //Clear the EventListBox                               
            clearTextBoxes();                                           //Clear the textboxes to allow new input
            addEventAlertLabel.Text = "";                               //Reset the alert label
            currentButtonSelected = "AddEvent";                         //update the currentselectedButton
            DetailPanel.Visible = false;                                //Disable the current DetailEventPanel first.
            AddFormPanel.Visible = true;                                //Make the AddForm Panel Visible.
            //Console.WriteLine("you clicked add event");                 //for debugging only

            //When the user clicks AddEvent for the first time, allow them to interact with the form right away.
            EventDetailLabel.Text = "";                                 //Clear any previous EventDetails.
            InputDateLabel.Text = "Please select a day to add an Event"; //Display a message.
            disableButtonsToAddEvent(false,currentButtonSelected);      //Disable other functionalities for the user to complete the AddForm or until cancel is clicked.
            SelectionAlertLabel.Text = "";                              //Reset the alert label after deletion.

        }//End of AddEventClicked.

        //Function to disable Buttons that are related to AddEvent 
        private void disableButtonsToAddEvent(Boolean willAppear, string selectedButton)
        {
            switch (selectedButton)
            {
                case "AddEvent":                                //Disable other Buttons when Add Event in progress
                    SelectionDatePanel.Visible = !willAppear;   //Initializes as true if willAppear is false
                    FindButton.Enabled = willAppear;            //Based on the value the user it at the moment
                    EventListBox.Enabled = willAppear;
                    ViewMonthlyButton.Enabled = willAppear;
                    ViewButton.Enabled = willAppear;
                    DeleteButton.Enabled = willAppear; ;
                    EditButton.Enabled = willAppear;
                    break;
                case "Cancel":                                  //Enable the necessary button the user need to interact with
                    AddFormPanel.Visible = !willAppear;         //Disable AddForm Panel
                    FindButton.Enabled = willAppear;            //Based on the value the user it at the moment
                    EventListBox.Enabled = willAppear;
                    ViewMonthlyButton.Enabled = willAppear;
                    ViewButton.Enabled = willAppear;
                    currentButtonSelected = "";
                    InputDateLabel.Text = "Please make a selection from the menu";
                    break;
                default:
                    break;
            }
        }//End of disableButtonsToAddEvent

        //Cancel AddEventForm is Clicked
        private void cancelEventButtonIsClicked(object sender, EventArgs e)
        {
            //Enable Buttons functionalities based on currentButtonSelected
            if (currentButtonSelected.Equals("AddEvent"))
            {
                disableButtonsToAddEvent(true, "Cancel");                       //Enable all other Buttons.
            }
            else if (currentButtonSelected.Equals("EditEvent"))
            {
                disableAllOhterFunctionalityUntilEditGetsCancel(true);          //Enable all other Buttons.
                clearTextBoxes();                                               //Clear TextBoxes.
                AddFormPanel.Visible = false;                                   //Hide the current Panel.
                DetailPanel.Visible = true;                                     //Enable DetailPanel.
                
            }
        }//End of AddEvent is Clicked.

        //Function to find overlapping times.
        private void AddConfiramtionButtonClicked(object sender, EventArgs e)
        {
            if (currentButtonSelected.Equals("AddEvent"))
            {
                if (EventNameTextBox.Text != "" && EventLocationTextBox.Text != "" && EventStartTimeTextBox.Text != "" && EventEndTimeTextBox.Text != "" && EventDescriptionTextBox.Text != "")
                {
                    //Read the Information from each textbox to add the event
                    string eventName = EventNameTextBox.Text;                                   //Read eventNameTextBox.
                    string eventLocation = EventLocationTextBox.Text;                           //Read eventLocationTextBox.
                    string eventStartTime = changeTimeFormat(EventStartTimeTextBox.Text);       //Read and change the startTimeFormat.
                    string eventEndTime = changeTimeFormat(EventEndTimeTextBox.Text);           //Read and change the endTimeFormat.
                    string eventDescription = EventDescriptionTextBox.Text;                     //Read eventDescriptionTextBox.

                    Console.WriteLine(eventStartTime);

                    //create variables for selectedDay.
                    string day = selectedDay.ToString().Substring(3, 2);                        //Store day.
                    string month = selectedDay.ToString().Substring(0, 2);                      //Store month.
                    string year = selectedDay.ToString().Substring(6, 4);                       //Store year.

                    //valid date and time format for the database
                    string correctEventStarTime = year + "/" + month + "/" + day + " " + eventStartTime;
                    string correctEventEndTime = year + "/" + month + "/" + day + " " + eventEndTime;

                    //if overlapping occurs, display a message to the user -> Event cannot be add it due to overlapping! 
                    Event findOverlapping = new Event();                                        //Create a new object for potential New Event. 
                    Boolean eventOverLaps = findOverlapping.findOverLappingEventsWith(eventStartTime, eventEndTime, year, month, day, thismember.getUserName());
                    if (eventOverLaps == true)                                                 //check for overlapping events
                    {
                        Console.WriteLine("there is an overlapping event at this frame time");  //Debugging.
                        addEventAlertLabel.Text = "Sorry, There is an overlapping event time"; //Update the corresponding label Alert;
                    }//End of if; 

                    else
                    {   //Create new Event Object that will be add it to the database.
                        Event addEventToDatabase = new Event(eventName, eventLocation, correctEventStarTime, correctEventEndTime, eventDescription, day, month, year, thismember.getUserName());
                        addEventToDatabase.addEvent();                                            //AddEvent to database
                        addEventAlertLabel.Text = "Your event was add it successfully";          //Display a successful Message.
                        clearTextBoxes();                                                       //Clear textBoxes to allow new input.
                    }
                }
                else
                {
                    addEventAlertLabel.Text = "You must fill out all the information";          //Display a successful Message.
                }
            }

            else if (currentButtonSelected.Equals("EditEvent"))
            {
                //Do not allow update unless all textBoxes are filled out
                if(EventNameTextBox.Text != "" && EventLocationTextBox.Text != "" && EventStartTimeTextBox.Text != "" && EventEndTimeTextBox.Text != "" && EventDescriptionTextBox.Text != "")
                {
                    //update the database after reading the textBoxes data from AddFormPanel. 
                    Console.WriteLine("reading changes!");
                    //Read the Information from each textbox to add the event
                    string eventName = EventNameTextBox.Text;                                   //Read eventNameTextBox.
                    string eventLocation = EventLocationTextBox.Text;                           //Read eventLocationTextBox.
                    string eventStartTime = changeTimeFormat(EventStartTimeTextBox.Text);       //Read and change the startTimeFormat.
                    string eventEndTime = changeTimeFormat(EventEndTimeTextBox.Text);           //Read and change the endTimeFormat.
                    string eventDescription = EventDescriptionTextBox.Text;                     //Read eventDescriptionTextBox.

                    Console.WriteLine(eventStartTime);
                    Console.WriteLine(eventName);

                    //create variables for selectedDay.
                    string day = selectedDay.ToString().Substring(3, 2);                        //Store day.
                    string month = selectedDay.ToString().Substring(0, 2);                      //Store month.
                    string year = selectedDay.ToString().Substring(6, 4);                       //Store year.

                    //valid date and time format for the database
                    string correctEventStarTime = year + "/" + month + "/" + day + " " + eventStartTime;
                    string correctEventEndTime = year + "/" + month + "/" + day + " " + eventEndTime;

                    //Set the attributes for the currentEvent
                    thisEvent.setEventName(eventName);
                    thisEvent.setLocation(eventLocation);
                    thisEvent.setStartTime(correctEventStarTime);
                    thisEvent.setEndTime(correctEventEndTime);
                    thisEvent.setDescription(eventDescription);
                    //Perform Update
                    thisEvent.updateEvents(thisEvent);      
                    EventNameTextBox.Text = "";
                    EventLocationTextBox.Text = "";
                    EventStartTimeTextBox.Text = "";
                    EventEndTimeTextBox.Text = "";
                    EventDescriptionTextBox.Text = "";
                    addEventAlertLabel.Text = "Event has been updated, click cancel now.";
                }
                else
                {
                    addEventAlertLabel.Text = "You must fillout all the information";          //Display a successful Message.
                }

            }
            
            
        }//End of overlapping Function

        //function to clear the textBox from AddForm Panel
        private void clearTextBoxes()
        {
            EventNameTextBox.Text = "";
            EventLocationTextBox.Text = "";
            EventStartTimeTextBox.Text = "";
            EventEndTimeTextBox.Text = "";
            EventDescriptionTextBox.Text = "";
        }//End of clearBoxes function.

        //Function change the time format
        private string changeTimeFormat(string givenTimeFormat)
        {
            string rightFormat = "";
            if (givenTimeFormat.Length != 0)
            {
                rightFormat =  DateTime.Parse(givenTimeFormat).ToString("HH:mm");       //return the valid start/endTime in the right format.
            }
            else
            {
                //the text box is empty
            }
            return rightFormat;
        }//End of changeTimeFormat TextBox.


                        //####### End of AddEvent ##########




        //####### Everything regarding EditEvent start form here ##########

        private void EditEventButtonClicked(object sender, EventArgs e)             
        {
            SelectionAlertLabel.Text = "";                                          //Reset label after deletion.
            currentButtonSelected = "EditEvent";                                    //Keep track of the current Button selected
            DetailPanel.Visible = false;                                            //Disable detail label.
            AddFormPanel.Visible = true;                                            //enable AddForm. 
            disableAllOhterFunctionalityUntilEditGetsCancel(false);                 //Disable all other buttons.
            addEventAlertLabel.Text = "";                                           //Clear label.

            //populate the textBoxes whit the correct Data Information -> still need formatting
            EventNameTextBox.Text = thisEvent.getEventName();
            EventLocationTextBox.Text = thisEvent.getLocation();
            EventStartTimeTextBox.Text = changeDateFormatWhenReadingfromDatabase(thisEvent.getStartTime()); //Format time
            EventEndTimeTextBox.Text = changeDateFormatWhenReadingfromDatabase(thisEvent.getEndTime());     
            EventDescriptionTextBox.Text = thisEvent.getDescription();
        }//End of eventClicked

        //function to change the date format when reading from the database.
        private string changeDateFormatWhenReadingfromDatabase(string usingTime)
        {                        
            string newTime = formatTime(usingTime.Substring(9));                //Get the proper time format to display and save to the database.
            return newTime;
        }
        private string formatTime(string withTime)
        {
            DateTime myTime = DateTime.Parse(withTime);                         //Format the givenTime to display it. 
            Console.WriteLine(myTime);
            return myTime.ToString("hh:mm tt");                                 //Return normal hours + AM or PM.
        }

        //Disable otherButton until the user cancels or saves the Event -> to allow better interaction.
        private void disableAllOhterFunctionalityUntilEditGetsCancel(Boolean willAppear)
        {
            ViewButton.Enabled = willAppear;
            ViewMonthlyButton.Enabled = willAppear;
            AddButton.Enabled = willAppear;
            SignOutButton.Enabled = willAppear;
            DeleteButton.Enabled = willAppear;
            EventListBox.Enabled = willAppear;
            FindButton.Enabled = willAppear;
            DatePicker.Enabled = willAppear;
        }
                        //####### End of EditEvent ##########

    }//End Class
}//end Project
