<h1>Dependencys</h1>
<p>MySQL Server.</p>
<p>Mono.</p>
<p>Any sort of C Sharp editor or IDE.<p>
<p>Sequel Pro</p>
<p>git</p>
<p>Adobe Flash</p>
<p>Adobe Projector</p>

<h1>Installation</h1>
<p>Step 1. Change your MySql root password to "botmaker" without the quotes (If you don't know how, look it up)</p>
<p>Step 2. Open up terminal and type: git clone https://github.com/atomboy2005/ROTMGServer.</p>
<p>Step 3. navigate to: ROTMGSever -> db</p>
<p>Step 4. Open up Database.cs with your preferred C sharp editor (I recommend MonoDevelop or Xamarin Studio)</p>
<p>Step 5. On line 22 "change password=" too "password=botmaker" without the quotes</p>
<p>Step 6. Start MySQL Server if it's not started alrealdy.</p>
<p>Step 7. Save that and open up Sequel Pro</p>
<p>Step 8.  In the name column put "Localhost" without the quotes.</p>
<p>Step 9.  In the host column put "127.0.0.1" without the quotes.</p>
<p>Step 10. In the password column put "botmaker" without the quotes.</p>
<p>Step 11. Click connect.</p>
<p>Step 12. In the mac top bar click database -> Add Database.</p>
<p>Step 13. In the Database Name type "ROTMG" without the quotes.</p>
<p>Step 14. In the mac top bar go to File -> Import.</p>
<p>Step 15. Navigate to ROTMGServer -> db select struct.sql and click open.</p>
<p>Step 16. Open 2 terminal windows.</p>
<p>Step 17. In the first terminal window type cd -Path to ROTMGServer-/bin/debug/Server then type mono server.exe</p>
<p>Step 18. In the second terminal window type cd -Path To ROTMGServer-/bin/debug/wServer then type sudo mono wServer.exe and then enter your password.</p>
<p>Step 19. Open up client.swf with Adobe Projector, you will get a load error, this is normal.</p>
<p>Step 20. Right click on the window and click Global Settings.</p>
<p>Step 21. It should of opened up a System Preferences window, in that window click Trusted Location Settings.</p>
<p>Step 22. In that window click the plus button and navigate the ROTMGServer file and select client.swf and click open.</p>
<p>Step 23. Close and reopen client.swf.</p>
<p>Step 24. Congratulations, you've Successfully set up the ROTMGServer give yourself a pat on the back!</p>