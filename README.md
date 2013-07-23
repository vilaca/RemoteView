RemoteView
==========

Desktop sharing HTTP server.

No need for a dedicated client, any modern browser will do.

Licensed as GPL. See license file or <http://www.gnu.org/licenses/>.


Running the Server Application
==============================

Syntax: RemoteView [Port to listen] [Options]

Example: RemoteView 6060 -b

Options:

-ip :  Bind ip;

 -b  :  Don't show banner message;
         
 -m  :  Allow multiple instances;
         
 -h  :  Help (This screen);


Requirements
============

Server has been tested in both Windows XP 32 bits and Windows 7 64 bits. DotNet Framework 2.0 or better needed.

Client has been tested with Chrome on the same Windows configurations as above but "should" work in any browser/OS combination. 


Project Status
==============

This application is already somewhat functional but a few features and code quality assurance are still needed.

See [TODO file](https://github.com/vilaca/RemoteView/blob/master/TODO.md) for more info.


Contributors Wanted
===================

If you find this project interesting or that in any wait it could be useful for you and want to contribute please do.

The following roles are needed and open for contributors:

- C# developer;
- Beta tester;
- Code reviewer.

See [TODO file](https://github.com/vilaca/RemoteView/blob/master/TODO.md) for more info on pending tasks.


How does the Application work ?
===============================

This Application allows a user with a browser (client) to control a remote computer (server).

The Application is basically an embedded HTTP server that generates a simple HTML5 page with an image. That image is a representation of the current Screen Device of the Server.

The Client sees the remote computer Desktop as a webpage on the browser. Clicks on the browser are sent to the remote computer.





