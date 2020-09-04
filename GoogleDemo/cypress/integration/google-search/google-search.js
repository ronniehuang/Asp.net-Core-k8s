import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
	Given('I visit google.com', () => {
        cy.visit('http://google.com')
    });
    When('I type in a search query', () => {
        cy.get('[type="text"]')
            .type('BNZ')
            .type('{enter}');
        });
    Then('I should see related results', () => {
        cy.get('#search')
            .contains('BNZ');
        });
	