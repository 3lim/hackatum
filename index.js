const express = require('express');
const bodyParser = require('body-parser');
const fetch = require('node-fetch');
const twilio = require('twilio');
const { escape } = require('querystring');

const client = twilio();
const TwimlResponse = twilio.TwimlResponse;

const SERVER_BASE_URL = 'http://dk.ngrok.io';

const app = express();

app.use(bodyParser.urlencoded({ extended: false }));

app.post('/messages', (req, res, next) => {
  const message = req.body.Body;
  const from = req.body.From;
  const to = req.body.To;
  res.send('<Response><Message>Thanks for sending "' + message + '"</Message></Response>');

app.listen(3000, () => {
  console.log('Listening on 3000');
});
