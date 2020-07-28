# AET.Unity.GoogleSheetsReader

Provides a Crestron Simpl# library for reading a Google Sheets worksheet that has been published as a csv. 

The sheet should be formatted into sections--even if you only use a single section, with column A used exclusively for section name.

A | B | C | D
--|--|--|--
**Section 1 Name** | **Name** | **Date** | **Value**
 &#65279; | Item 1 | 12/1/2020 | 105.3
 &#65279; | Item 2 | 12/13/2020 | 99.7 
**My 2nd Section** | **Description** | **Qty** | **Price**
 &#65279; | Item Type A | 100 | 25.95
 &#65279; | Item Type B | 50 | 14.95

Simple example:
````C#
string csvData = SheetReader.ReadURL(googleSheetsUrl);
Sheet sheet = SheetReader.ReadCSVText(csvText);

Section section1 = sheet.Section("Section 1 Name");
Section section2 = sheet.Section("My 2nd Section");

foreach (var row in section1.Rows) {
  string name = row[0]; //Note: column A is not accessible here: it's reserved exclusively for section names. So [0] is column B.
  string date = row["Date"];  //Can also access   
  string value = row["Value"];
}

var description = section2.Rows[0].Cells["Description"];
````
