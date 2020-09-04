import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
    Given('I visit google.com', () => {
        cy.visit('http://google.com')
    });
    