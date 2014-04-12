PortableLibraryProfiles
=======================

A simple library and console app to enumerate all PCL profiles for all frameworks installed on the local machine.

Usage
=====

Just execute the app.

All PCL profiles on the local machine (at C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETPortable) are examined, converted to JSON, and stored in a "profiles.json" file.

Also, if there are any folders in the same directory as the executable, they are treated as a .NETPortable folder, and they are processed separately into their own profiles.json file.

To see the results in a table, go here:

  <http://embed.plnkr.co/03ck2dCtnJogBKHJ9EjY>