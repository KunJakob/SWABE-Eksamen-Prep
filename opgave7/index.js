 'use strict';

 const functions = require('firebase-functions');
 const fetch = require('node-fetch');
 

 const WEBHOOK_URL = 'http://requestb.in/1mqw97l1';
 
 export const webhook = functions.database.ref('/hooks/{hookId}').onCreate(async (content) => {
   const response = await fetch(WEBHOOK_URL, {
     method: 'post',
     body: JSON.stringify(content.val()),
     headers: {'Content-Type': 'application/json'}
   });
 
   if (!response.ok) {
     throw new Error(`HTTP Error: ${response.statusCode}`);
   }
   functions.logger.log('SUCCESS! Posted', content.ref);
 });