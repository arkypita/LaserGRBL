//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Text;

namespace Tools
{
	/// <summary>
	/// Description of WordWrap.
	/// </summary>
	public class WordWrap
	{
		protected const string _newline = "\r\n";
		
		public static string WrapString(string the_string, int width) 
		{
		
		    // Lucidity check
		    if ( the_string == null || the_string.Length < 1 ||  width < 1)
		        return the_string;			    
			
		    int pos, next;
		    StringBuilder sb = new StringBuilder();
		

		
		    // Parse each line of text
		    for ( pos = 0; pos < the_string.Length; pos = next ) {
		        // Find end of line
		        int eol = the_string.IndexOf( _newline, pos );
		
		        if ( eol == -1 )
		            next = eol = the_string.Length;
		        else
		            next = eol + _newline.Length;
		
		        // Copy this line of text, breaking into smaller lines as needed
		        if ( eol > pos ) {
		            do {
		                int len = eol - pos;
		
		                if ( len > width )
		                    len = BreakLine( the_string, pos, width );
		
		                sb.Append( the_string, pos, len );
		                sb.Append( _newline );
		
		                // Trim whitespace following break
		                pos += len;
		
		                while ( pos < eol && Char.IsWhiteSpace( the_string[pos] ) )
		                    pos++;
		
		            } while ( eol > pos );
		        } else sb.Append( _newline ); // Empty line
		    }
		
		    return sb.ToString();
		}
		
		/// <summary>
		/// Locates position to break the given line so as to avoid
		/// breaking words.
		/// </summary>
		/// <param name="text">String that contains line of text</param>
		/// <param name="pos">Index where line of text starts</param>
		/// <param name="max">Maximum line length</param>
		/// <returns>The modified line length</returns>
		public static int BreakLine(string text, int pos, int max)
		{
		  // Find last whitespace in line
		  int i = max - 1;
		  while (i >= 0 && !Char.IsWhiteSpace(text[pos + i]))
		    i--;
		  if (i < 0)
		    return max; // No whitespace found; break at maximum length
		  // Find start of whitespace
		  while (i >= 0 && Char.IsWhiteSpace(text[pos + i]))
		    i--;
		  // Return length of text before whitespace
		  return i + 1;
		}
	}
}
