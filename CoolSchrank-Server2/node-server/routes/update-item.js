var express = require('express');
var router = express.Router();
var sql = require('mssql');
var connectionString = "Driver=msnodesql;Server=tcp:coolschrankdbserver.database.windows.net,1433;Database=coolschrankDB;Uid=coolschrank@coolschrankdbserver;Pwd=hackaTUM-2016;Encrypt=yes;TrustServerCertificate=no;Connection Timeout=30;";
var accountSid = 'SKfad8a61beb60a92f0993267f0921c4b2';
var authToken = 'YKq7ukK3P0eDyipzY6UaEaAR6EUE2lqK';
var accountRealSid = 'ACb44609cbe49e73fe7beefab010c29c3e';
var telNumberTo = "+4917643424166";
var telNumberFrom = "+4915735994166";
var client = require('twilio')(accountSid, authToken);
/* GET users listing. */
router.get('/:slotNumber/:sensorValue', function(req, res) {
  sql.connect(connectionString).then(function() {
    new sql.Request();
    .input('slotNumber', sql.Int, req.params.slotNumber)
    .query('select * from slots where slots.slotNumber = @slotNumber')
    .then(function(recordSet) {
      if(recordSet.length == 0) {
        client.accounts(accountRealSid).messages.create({
            to: telNumberTo,
            from: telNumberFrom,
            body: "What's the name of the item in slot " + req.params.slotNumber+"?",
        }, function(err, message) {
            console.log(message.sid);
        });
        // wait for answer
      } else {
        new sql.Request();
        .input('slotNumber', sql.Int, req.params.slotNumber)
        .input('sensorValue', sql.Int, req.params.sensorValue)
        .query('update slots set slots.sensorValue=@sensorValue where slots.slotNumber=@slotNumber');
      }
    });
  });
});

module.exports = router;
