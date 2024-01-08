'use strict';
import fetch from 'node-fetch';

console.log('Hello world');

var workbookId = "1WCXD3m8lKbrhlAkJa6cHlWXUSzqFqCSpCL9sSZEk4Uo";
var sheetName = "Sheet2";
var url = 'https://docs.google.com/spreadsheets/d/' + workbookId + '/gviz/tq?tqx=out:csv&sheet=' + sheetName;
console.log('URL = ' + url);
  
const fetchResponse = await fetch(url);
const csvText = await fetchResponse.text(); 