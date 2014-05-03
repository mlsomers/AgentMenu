AgentMenu
=========

Menu navigation framework for the Secretlabs Agent Smartwatch.
Maybe a little more than just a menu, it also abstracts some other low level stuff away like handling button events.

Though there may not be much "green" in the main demo app, there are more comments in the lib.
Note that I have not religiously followed best practices regarding encapsulation.
I have no problem with public fields instead of properties, especially not in a micro-framework project.

There are also other glitches like RenderTitle made static so it can be called from anywhere (should move to somewhere else, and would be nice to have a virtual one that calls the static one, but... enough to do still..)

