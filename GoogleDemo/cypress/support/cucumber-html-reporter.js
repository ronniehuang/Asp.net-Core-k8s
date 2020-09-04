
const report = require('multiple-cucumber-html-reporter');
 
report.generate({
    jsonDir: './cypress/cucumber-json/',
    reportPath: './cypress/reports/',
    metadata:{
        browser: {
            name: 'chrome',
            version: '60'
        },
        device: 'Local test machine',
        platform: {
            name: 'ubuntu',
            version: '16.04'
        }
    }
});