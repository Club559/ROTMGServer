# ROTMGServer
A fork of Club559's Private Server that I edited to work for Mac OSX and maybe Linux.

<h1>Dependencys</h1>
MySQL Server.
Mono.
Any sort of cs edditer or IDE.
Sequel Pro
git
Adobe Flash
Adobe Projector

<h1>Installation</h1>
Step 1. Change your MySql root password to "botmaker" wthout the qoutes (If you don't know how, look it up)
Step 2. Open up terminal and type: git clone https://github.com/atomboy2005/ROTMGServer.
Step 3. navigate to: ROTMGSever -> db
Step 4. Open up Database.cs with your prefered c sharp editor (I reccomend MonoDevelop or Xamarin Studio)
Step 5. On line 22 "change password=" too "password=botmaker" without the quotes
Step 6. Start MySQL Server if it's not started arealdy.
Step 7. Save that and open up Sequel Pro
Step 8. In the name collum put "Localhost" without the quotes.
Step 9. In the host collum put "127.0.0.1" without the quotes.
Step 10. In the password collum put "botmaker" without the quotes.
Step 11. Click connect.
Step 12. In the mac top bar click database -> Add Database.
Step 13. In the Datbase Name type "ROTMG" without the quotes.
Step 14. In the mac tob par go to File -> Import.
Step 15. Navigate to ROTMGServer -> db select struct.sql and click open.
Step 16. Open 2 terminal windows.
Step 17. In the first terminal window type cd -Path to ROTMGServer-/bin/debug/Server then type mono server.exe
Step 18. In the second terminal window type cd -Path To ROTMGServer-/bin/debug/wServer then type sudo mono wServer.exe and then enter yor password.
Step 19. Open up client.swf with Adobe Projector, you will get a load error, this is normal.
Step 20. Right click on the window and click Global Settings.
Step 21. It should of opened up a system prefences window, in that window click Trusted Location Settings.
Step 22. In that window click the plus button and navigate the ROTMGServer file and select client.swf and click open.
Step 23. Now click close and reopen client.swf.
Step 24. Congratualtions, you've Sucsessfully set up the ROTMGServer give yourself a pat on the back!

<h1>Notes</h1>
If you use linux the installtion might be a bit different.

<h1>Credit/s</h1>
I'm only going to give one credit, and that's to Club559/Travoos he made the orignal code for the ROTMG Server, all credit goes to him I just edited it to work for mac I'll link his account and the orignal project below:
Club559/Travoos: <a href="https://github.com/Club559/">https://github.com/Club559/</a>
Orignal Project: <a href="https://github.com/Club559/ROTMGServer">https://github.com/Club559/ROTMGServer</a>
