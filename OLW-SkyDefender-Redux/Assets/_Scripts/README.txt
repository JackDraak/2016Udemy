/*

Development Notes:
==================
things to do:

@ lecture 119

	have enemies test for active player before droppping bombs?

	the Sky def. needs to be redone for OCD-TIDY but seems to be 100% nominal at this point.
		modify CloudFlow to support a notion of near or far for the micro-clouds... keep near ones low and far ones high when pooling

	fix formation motion - still working out squelch for delta motion

	complete object pooling for: projectiles, effects?

	??? take player health out of level manager and put in player

	optimize collision layers
	tidy IDE
	tidy C#
	tidy interface

	difficulty scaling: more/faster enemies? more bombs? more formations?
	drop power-ups - double fiting rate for 20 seconds? health boost?

	replace the rocket graphics with my own art?
	get or create some HUD graphcis for the score board, shipboard, etc... buttons...
	create my own art assets for everything? at the least, tidy-up the sky assets

	fix singleton (menuing) issue with levelmanager destroying | losing links on scene reloads

*/

// *Reference: 2D Sky FREE version: 1.0
// Disclaimer:
// I have altered the art assets from this package* slightly, and while
// at first glance it may appear to be used mostly "as-is", you will NOT
// get the results you see in my game from this asset package "right out 
// of the box."
//
// ^JackDraak
