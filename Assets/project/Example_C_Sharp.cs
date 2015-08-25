using System;
using UnityEngine;
using System.Collections;
using Random = System.Random;


public class Example_C_Sharp : MonoBehaviour {
       
	// Interface :
private Console console;

	void Start () {
	
		// Getting a reference on the Console component :
		console					= GetComponent<Console> ();
		console.GraphicsOnly	= true;
	
		// ... and for example :
		console.ClearGlyph				= 0;	// smiley is now ...
												// ... the clear glyph.
        console.ClearGlyphColor         = Color.black;
		console.ClearBackgroundColor	= Color.black;
		
		console.GraphicsOnly			= true;	// draw methods will ...
												// ... only draw glyph and colors
		
	}



	void Update ()
	{

		// Drawing in the console begins here :
		console.PrepareUpdate ();
	
		console.Clear ();	// clearing the console
		
		// writes 'a' in blue over red at ( 10, 10 ) :
        Random r = new Random();

        for (int i = 0; i < console.width; i++)
	    {
            for (int j = 0; j < console.height; j++)
	        {

	            console.Glyph = 1;
	            console.GlyphColor = Color.yellow;
	            console.BackgroundColor = Color.black;

                if (r.NextDouble()<0.1) console.SetTile(i, j);
	        }
	    }
	    // Drawing in the console ends here :
		console.EndUpdate ();
	
	}

}
