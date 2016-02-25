/*
0.120.01 - 2016 Feb 24 - half custom artwork

Development Notes: 0.120.01
==================
things to do:

@ lecture 119

	drop power-ups - double damae and/or fiting rate for 20 seconds? health boost?

	have enemies test for active player before droppping bombs?

	the Sky def. needs to be redone for OCD-TIDY but seems to be 100% nominal at this point.
		modify CloudFlow to support a notion of near or far for the micro-clouds... keep near ones low and far ones high when pooling

	fix formation motion - still working out squelch for delta motion

	complete object pooling for: projectiles, effects?

	??? take player health out of level manager and put in player

	tidy IDE
	tidy C#
	tidy game interface

	create my own art assets for clouds too? if not, it still needs more work anyway

	fix singleton (menuing) issue with levelmanager destroying | losing links on scene reloads

	difficulty scaling: fast enough? out of sync between formation speed and bomb freqeuency?
*/

// *Reference: 2D Sky FREE version: 1.0
// Disclaimer:
// I have altered the art assets from this package* slightly, and while
// at first glance it may appear to be used mostly "as-is", you will NOT
// get the results you see in my game from this asset package "right out 
// of the box."
//
// ^JackDraak
