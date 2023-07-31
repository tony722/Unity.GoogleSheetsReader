# AET.Unity.GoogleSheetsReader

### Provides a Crestron Simpl# library for reading a Google Sheets worksheet that has been published as a csv. 
It also automatically saves the sheet to a cache file locally so the processor can work offline.

[Download latest Demo .zip Crestron here: v1.0.2](https://github.com/tony722/Unity.GoogleSheetsReader/releases/download/v1.0.2/GoogleSheetsReader.Demo_compiled.zip)

To publish a Google Sheet as a csv:
1. In Google Sheets go to File->Share->Publish to the Web. 
2. Under "Link" select the sheet you want to publish.
3. Under "Embed" select Comma-separated values (.csv) 
4. Hit Publish
5. Copy the provided URL and use that in your code.

_**Note: column A in your Google Sheet is reserved exclusively for the section name(s)**. Do not put other values in Column A--and even if you only need a single section, give it a name._

[See example google sheet here](https://docs.google.com/spreadsheets/d/1WCXD3m8lKbrhlAkJa6cHlWXUSzqFqCSpCL9sSZEk4Uo/edit?usp=sharing). This example sheet has two sections.

A | B | C | D
--|--|--|--
**Section 1 Name** | **Name** | **Date** | **Value**
 &#65279; | Item 1 | 12/1/2022 | 105.3
 &#65279; | Item 2 | 12/13/2022 | 99.7 
**My 2nd Section** | **Description** | **Qty** | **Price**
 &#65279; | Item Type A | 100 | 25.95
 &#65279; | Item Type B | 50 | 14.95


See the SIMPL+ example in the Demo .zip file, or use this library as followings in Simpl#Pro (simple example):

````C#
string googleSheetsUrl = "https://docs.google.com/spreadsheets/d/e/2PACX-1vQKY0ZBdIi3xGXwtzHt57Z4rsUNvYL-CQg34sVGct5C5h0VAQvHfYYn-YEUSLnaJ1PKk84Ksp7XK2UF/pub?gid=0&single=true&output=csv";
string cacheFile = "\\User\\SampleSheet.csv";
var reader = new GoogleReader(googleSheetsUrl, cacheFile);
string csvText = reader.ReadPublishedGoogleSheetCsv();

Sheet sheet = new SheetReader().ReadCsvText(csvText);

Section section1 = sheet.Sections[0];
Section section2 = sheet.Sections["My 2nd Section"];

foreach (var cells in section1.Rows) {
  string name = cells[0]; //name = "Item 1". Note: column A is not accessible here: it's reserved exclusively for section names. So [0] is column B.
  string date = cells["Date"]; //Can also access by column name
  string value = cells["Value"];
  
  //do something with these values now
}

var description = section2.Rows[0].Cells["Description"];
````
