USE csc340;

DROP TABLE ijevent;
DROP TABLE ijmanagerEvent;
DROP TABLE ijmember;

CREATE TABLE IJmember(
	memberID 		INT PRIMARY KEY AUTO_INCREMENT,
	userName 		VARCHAR(9),
	password		VARCHAR(8),
	firstName		VARCHAR(10), 
	lastName		VARCHAR(10),
 	managerStatus	BOOLEAN,
 	UNIQUE (userName)
);

CREATE TABLE IJevent(
	eventID 		INT(8) PRIMARY KEY AUTO_INCREMENT,
	eventName 	    VARCHAR(30),
 	location		VARCHAR(30),
	startTime	    DATETIME,
	endTime		    DATETIME,
	duration		INT(4),
	description	    VARCHAR(70),
	DAY			    VARCHAR(2),
	MONTH			VARCHAR(2),
	YEAR			VARCHAR(4),
	userName		VARCHAR(9),
	CONSTRAINT userName_FK FOREIGN KEY (userName) REFERENCES ijmember(userName)
);

CREATE TABLE IJmanagerEvent(
	eventID 		INT(8) PRIMARY KEY AUTO_INCREMENT,
	eventName 	    VARCHAR(30),
 	location		VARCHAR(30),
	startTime	    DATETIME,
	endTime		    DATETIME,
	duration		INT(4),
	description	    VARCHAR(70),
	DAY			    VARCHAR(2),
	MONTH			VARCHAR(2),
	YEAR			VARCHAR(4),
	userName		VARCHAR(9),
	attendees	    VARCHAR(70),
	CONSTRAINT userName_FK_M FOREIGN KEY (userName) REFERENCES ijmember(userName) 
);

INSERT INTO ijmember(userName, PASSWORD, firstName, lastName, managerStatus) VALUES('Bobby1','pw','Bobby','Lee',FALSE);
INSERT INTO ijmember(userName, PASSWORD, firstName, lastName, managerStatus) VALUES('John2','pw','John','Smith',true);

SELECT * FROM ijmember
WHERE username = 'jr' AND PASSWORD = 'pw';

##find a useerName
SELECT * FROM ijmember WHERE username= 'lo' AND PASSWORD= 'pw';
#--Insert Event to the table
INSERT INTO ijevent(eventName,location,startTime,endTime,duration,description,DAY,MONTH,YEAR,userName)
VALUES('study sesion','EKU','2021-04-29 08:30:00','2021-04-29 09:00:00',TIMESTAMPDIFF(MINUTE,startTime,endTime),'Need to finish strong','29','04','2021','io');
INSERT INTO ijevent(eventName,location,startTime,endTime,duration,description,DAY,MONTH,YEAR,userName)
VALUES('Soccer Game','Lexignton','2021-04-30 06:30:00','2021-04-30 08:45:00',TIMESTAMPDIFF(MINUTE,startTime,endTime),'Go to Lex and meet with friends to play','29','04','2021','io');


#insert events from different days in a month to test the selection
INSERT INTO ijevent(eventName,location,startTime,endTime,duration,description,DAY,MONTH,YEAR,userName)
VALUES('Amazon Interview','NewYork','2021/05/20 12:10:00','2021/05/20 14:59:00',TIMESTAMPDIFF(MINUTE,startTime,endTime),'Inperson code Interview to secure your spot at Amazon','20','05','2021','jr');
#

#select statement to find all event in a specific month
SELECT eventID, eventName,location,startTime,endTime, duration,description,DAY,MONTH,YEAR
from ijevent
WHERE MONTH = '05' AND YEAR = '2021' AND username = 'jr'
ORDER BY eventID ASC;
##End of Monthly Query

######------ Select statement to find if an event is between a given time
SELECT * FROM ijevent
WHERE (TIME_FORMAT(startTime,'%H:%i') >= '12:00' AND TIME_FORMAT(endTime,'%H,:%i') <= '14:30') 
AND (DAY = '20' AND MONTH = '05' AND YEAR ='2021' AND userName = 'jr'); 



##### Select overlapping events #####
SELECT * FROM ijevent
WHERE (TIME_FORMAT(endTime, '%H,%i') > '13:00') AND (DATE_FORMAT(startTime,'%H%i') < '14:00')
AND (DAY = '20' AND MONTH = '05' AND YEAR ='2021' AND userName = 'jr');;

 
((date_end > '" . $start_date . "') AND (date_start < '" . $end_date . "')) ";


####################################
SELECT * FROM ijevent
WHERE '12:00' BETWEEN '13:00' AND '14:00'; 

####### How to delete an Event from the table 
DELETE FROM ijevent WHERE eventID = 10;



#Select statements based on a specific day and userName 
SELECT eventID, eventName,location,startTime,endTime, duration,description,DAY,MONTH,YEAR
from ijevent
WHERE DAY = '29' AND MONTH = '04' AND YEAR = '2021' AND username = 'jr'
ORDER BY eventID ASC;
