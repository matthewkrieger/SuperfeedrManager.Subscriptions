# SuperfeedrManager.Subscriptions

### SuperfeedrManager.Subscriptions (sm.subs) is a Windows (currently, will support Mac in future) command line utility for managing your [Superfeedr](http://superfeedr.com) [subscriptions](https://documentation.superfeedr.com/subscribers.html).

sm.subs has the following functions: create (implemented shortly), list, export, replay, replayall, unsubscribe, unsubscribeall

To configure sm.subs for your account, add your Superfeedr username and password into app.config.

##### Command line options:  

`sm.subs subscribe` *`hubtopic`* *`hubcallback`* [*`hubsecret`*] - Create subscription with hub.topic *`hubtopic`*, hub.callback *`hubcallback`* and hub.secret *`hubsecret`* (optional)  

`sm.subs list` - List all subscriptions  

`sm.subs export` - Export subscriptions  

`sm.subs replay` *`hubtopic`* *`hubcallback`* - Replay subscription with hub.topic *`hubtopic`* and hub.callback *`hubcallback`*  

`sm.subs replay all` - Replay all subscriptions  

`sm.subs unsubscribe` *`hubtopic`* *`hubcallback`* - Delete subscription with hub.topic *`hubtopic`* and hub.callback *`hubcallback`*  

`sm.subs unsubscribeall` *`hubtopic`* *`hubcallback`* - Delete all subscriptions

This is a VERY early release - no error handling, little configurability, no validation of command line parameters and has only been casually tested...so far.
