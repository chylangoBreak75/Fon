
///////////////////////   README     ///////////////////////////////////

* NOTE: I used the technologies that I am more familiar with.

* I used SQL Server version 2019.
	DB Name is: dbFon

* For de API I used .Net Core 8.0

* I used Angular CLI: 16.2.16 (npm version 10.8.2).

* I didn't use Kafka.

There are too many things that can be inproved , I did everything this way to finish as quick as possible.
	* Passwords in DB should be stored encoded, like this detail many other things can be changed.
	* Front can be beautify, but takes time.
	* Also security can be improved.

///////////////////////   Steps to use     ///////////////////////////////////
	1) Run angular, landing page is login
	2) Type username "Rene" and password "123"
	3) Will go to Applications page where we can see the applications lit and we can also add more rows through a modal window.


//////////////////////    GIVEN INSTRUCTION    ////////////////////////////////////////////////////////////

	.Net core + Angular + SQL

2) Login form, which is connected with a username and password. (user data is stored in the DB)
3) After logging in, the application list form opens. List fields (id, date, application type (request/proposal/complaint), status (submitted/completed)
4) The form allows you to fill out a new application. By clicking the "New application" button, a Popup opens for filling out the application.
5) Filling fields:
	a) Application type
	b) Message field
6) The application is registered in Kafka.
7) A hosted service runs in the app background, which listens to the Kafka topic and waits for applications. After taking the application from Kafka, it changes the status to processed and sends the result to the Database.
8) In the application list, the application status is updated without reloading the form.

///////////////////////   Tables Quick Review     ///////////////////////////////////

Database: dbFon
	tables:		
		tblUsers
			usrId
			usrName
			usrPwd
		catAppType   (Request,Proposal,Complaint)
			idAppType
			TypeAppName
		catAppStatus
			idAppStatus
			AppStatusName		
		tblApp
			AppId
			AppDate
			AppIdStatus (ref catAppStatus)
			AppIdType (ref catAppType)
			AppDescription

///////////////////////    Services Quick Review //////////////////////////////////////////////////////////		

API SERVICES
	/login			POST
	/ListarApps		GET
	/GetTypeApps		GET
	/GetStatusApps		GET
	/AddApp			POST
