/*
0.120.01 - 2016 Feb 24 - half custom artwork
0.120.10 - 2016 Feb 25 - mostly custom artwork (still undone: clouds)
0.120.2	- 2016 Feb 29 - now even the splash screen transition has broken... seriously annoying: all for simply tyrying to add a UI slider to said splash
0.120.3 - 2016 Mar 5 - okay, seriously: fuck Unity3d scene management for failing to load scene states saved with full prefabs, et. al.  W T F... fixed splash at least....
0.120.4 - 2016 Mar 7 - Unity3D can go to hell... more inconsistencies drove me to presently remove even the splash scene... considering new code for WebGL load delayer
0.120.5 - 2016 Mar 16 - splash scene replaced... viable mini-game status achieved.

Development Notes: 0.120.5
==================
things to do:

@ lecture 119

	alternative enemy prefabs?
	alternative enemy projectiles?
	alternative enemy formations?
	bosses? intermissions?

	double-check distro audio levels

	have enemies test for active player before droppping bombs? not working yet.....

	the Sky def. needs to be redone for OCD-TIDY but seems to be 100% nominal at this point.
		modify CloudFlow to support a notion of near or far for the micro-clouds... keep near ones low and far ones high when pooling

	fix formation motion - still working out squelch delta for deceleration

	complete object pooling for: projectiles, effects?

	tidy IDE
	tidy C#
	tidy game interface

	create my own art assets for clouds too? if not, it still needs more work anyway
	replace cheesy button graphics with "crayola"?

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

/* explaining things to classmates:


	public float fireDelay = 2f; // public to adjust in inspector if desired
	private float fireTime = Time.time - fireDelay;

	void Fire () {
	if (fireTime + fireDelay <= Time.time) {
		AudioSource.PlayClipAtPoint (bombSound, transform.position);
		GameObject myBomb = Instantiate(bomb, transform.position,  Quaternion.identity) as GameObject;
		myBomb.GetComponent<Rigidbody2D>().velocity += Vector2.down * bombSpeed;
		fireTime = Time.time;
	}



*/