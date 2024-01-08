import fetch from 'node-fetch';

export const handler = async (event) => {

  var workbookId = event.queryStringParameters.GoogleWorkbookId;
  var sheetName = event.queryStringParameters.GoogleSheetName;
  var url = 'https://docs.google.com/spreadsheets/d/' + workbookId + '/export?format=csv&gid=' + sheetId;
  console.log('URL = ' + url);
  
  const fetchResponse = await fetch(url);
  const csvText = await fetchResponse.text(); 

  // TODO implement
  const response = {
    statusCode: 200,
    body: csvText
  };
  return response;
};
