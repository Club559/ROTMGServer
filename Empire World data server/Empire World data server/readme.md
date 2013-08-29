EmpireWorld launcher server documentation
-
### Basic listener documentation
Based on the HttpListener class, uses a multithreading method to speedup the work. Actually uses 5 workers. Listens on every IP available to the machine, at the port 8889, at the /launcher/ prefix (example: http://*:8889/launcher/whatever).
Has 2 default responses: the Bad request one (400) or the Internal Server Error one: the first is given when the request is invalid, the second when the server encountered errors when trying to execute the request's code.
#### Actual coded requests ####
The requests actually coded are the ones to retrieve data about the RotMG client version, the launcher's News (and a specific one), and to get the download link for the client.  
1. Version retrieval: actually it does simply check for the last value of the   cliVersion column in the database and returns it as plain text.  
2. News retrieval: gets all rows from the clinews table in the database, and produces an XML file (pseudo) with the structure specified in the source code.