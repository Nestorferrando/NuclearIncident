using UnityEngine;
using System.Collections;

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
        console.ClearGlyphColor = Color.blue;
		console.ClearBackgroundColor	= Color.black;
		
		console.GraphicsOnly			= true;	// draw methods will ...
												// ... only draw glyph and colors
		
	}

    private void Update()
    {

        // Drawing in the console begins here :
        console.PrepareUpdate();

        console.Clear(); // clearing the console

        // writes 'a' in blue over red at ( 10, 10 ) :
        console.Glyph = 97;
        console.GlyphColor = Color.blue;
        console.BackgroundColor = Color.black;
        for (int i = 0; i < 10; i++) 
        {
            for (int j = 0; j < 10; j++)
            {
                console.SetTile(i, j);
            }
        }

  
		
		// Drawing in the console ends here :
		console.EndUpdate ();
	
	}

}
