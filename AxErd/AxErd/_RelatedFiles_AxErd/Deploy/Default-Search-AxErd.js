/***
2013/05/29  Wednesday  17:02pm
Default-Search-AxErd.js
AxErd\...\

http://www.microsoft.com/dynamics/ax/erd/ax2012r2/default.htm

http://go.microsoft.com/fwlink/p/?linkid=296623
***/


fOnKeyPress_Enter =
function ()
{
	var elemHasFocus;
	var sIdOfFocusElem;

	// 'Enter' key should be same as clicking the 'Search now' button,
	// but only if the idTex400SearchTxt current has focus.
	//
	if (13 == event.keyCode)
	{
		elemHasFocus = jQuery(':focus'); // UNdocumented ':focus' option of jQuery?
		if (elemHasFocus === undefined)
		{
			return true;
		}
		sIdOfFocusElem = elemHasFocus[0].id;
		if ('idTex400SearchTxt' == sIdOfFocusElem)
		{
			fAxErd_484_SearchNowMagnify_onclick('enterkey');
		}
	}
	return true;
};




// OLD version of this function....
// Unclear whether to eventually use these META elements for search.
//
fAxErd_484_OLD22_SearchNowMagnify_onclick_Meta_OLD =
function(_sFromOrigin)
{

 // As of 2013/05/13:  'ERD' content type attribute is Not yet well indexed.


	var sHttpUrlToBingCom;
	var sMetaAxEd;

	// <meta name="Search.MsDynAx.Erd.62.All" content="yes" />
	// <meta name="Search.MsDynAx.Erd.62.ContentType" content="MT" />
	//
	if ('(all)' == $('#idSel405ContentType').val())
	{
		sMetaAxErd = 'meta:Search.MsDynAx.Erd.62.All("yes")';
	}
	else if ('ERD' == $('#idSel405ContentType').val())
	{
 alert('(For ERD, the Search function is not yet ready. Probably ready by July 1 2013, or sooner.)');
 return true; // ??? Remove soon, I hope.
	}
	else
	{
		sMetaAxErd = 'meta:Search.MsDynAx.Erd.62.ContentType("';
		sMetaAxErd = sMetaAxErd + jQuery('#idSel405ContentType').val();
		sMetaAxErd = sMetaAxErd + '")';
	}

	sHttpUrlToBingCom = 'http://www.bing.com/search?q='
		+ jQuery('#idTex400SearchTxt').val() + ' '
		+ sMetaAxErd
		+ ' site:www.microsoft.com/dynamics/ax'
		;

	//location.replace(sHttpUrlToBingCom);
	window.open(sHttpUrlToBingCom);

	return true;
};




fAxErd_484_SearchNowMagnify_onclick =
function(_sFromOrigin)
{

 // As of 2013/05/20:  Recently added 'ERD' content type attribute is Not yet well indexed.
 // As of 2013/05/29:  Well indexed now.

	var sHttpUrlToBingCom;
	var sMetaAxEdAfter;
	var sUserText2, sUserText3Lower;

	//
	// This method IGNORES these Meta elements.
	//
	// <meta name="Search.MsDynAx.Erd.62.All" content="yes" />
	// <meta name="Search.MsDynAx.Erd.62.ContentType" content="MT" />
	//

	sMetaAxErdAfter = '';  //' %2BDynamics ';

	if ('(all)' == jQuery('#idSel405ContentType').val())
	{
 alert('(For All, only the ERD pages are not yet included in the search. Probably ready by July 1 2013, or sooner. Search will now proceed...)');
		//sMetaAxErdAfter = 'meta:Search.MsDynAx.Erd.62.All("yes")'; // TODO: ??? Yes DO activate this in July 2013.
	}
	else
	{
		sMetaAxErdAfter = sMetaAxErdAfter + ' %2B' + jQuery('#idSel405ContentType').val();
	}


	// Defensive edits of user's input text.
	//
	sUserText2 = jQuery('#idTex400SearchTxt').val();
	sUserText2 = sUserText2.replace(/[~`!@#$%^&*()+=?,.;:<>"\'\|\{\}\[\]\-\\\/]/gi, ' ');
	sUserText3Lower = sUserText2.toLowerCase();

	if (0 <= sUserText3Lower.indexOf('javascript',0))
	{
		sUserText2 = sUserText2.replace(/JavaScript/gi, 'JavXaSXcript');  // g=EveryOccurrence, i=CaseInsensitive.
	}
	if (0 <= sUserText3Lower.indexOf('vbscript',0))
	{
		sUserText2 = sUserText2.replace(/VBScript/gi, 'VXBSXcript');
	}

	jQuery('#idTex400SearchTxt').val(sUserText2);  // Update the text box.
	
	if ('' == sUserText2)
	{
		alert('Cannot search for an empty value. Type a table name into the search text box.');
		return true;
	}

	sHttpUrlToBingCom = 'http://www.bing.com/search?q=%2BAxErd '
		+ sUserText2 + ' '
		+ sMetaAxErdAfter
		+ ' site:www.microsoft.com/dynamics/ax'
		;

	//location.replace(sHttpUrlToBingCom);
	window.open(sHttpUrlToBingCom);

	return true;
};

//eof
