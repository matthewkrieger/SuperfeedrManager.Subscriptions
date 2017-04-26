SuperfeedrManager.Subscriptions (sm.subs) is a Windows (currently, will support Mac in future) command line utility for managing your Superfeedr subscriptions.

sm.subs has the following functions: list, export, replay, replayall, delete, deleteall

To configure sm.subs for your account, add your Superfeedr username and password into app.config.

Command line options:

`sm.subs list` - List all subscriptions
`sm.subs export' - Export subscriptions
`sm.subs replay *hubTopic* *hubCallback*` - Replay subscription with hub.topic `*hubTopic*` and hub.Callback `*hubCallback*`
`sm.subs replay all` - Replay all subscriptions
`sm.subs delete *hubTopic* *hubCallback*` - Delete subscription with hub.topic `*hubTopic*` and hub.Callback `*hubCallback*`
`sm.subs deleteall *hubTopic* *hubCallback*` - Delete all subscriptions

This is a VERY early release - there's no error handling, no validation of command line parameters and has only been casually tested...so far.
