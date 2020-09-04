import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
	Then('I should see title', () => {
        cy.title().should('include', 'Google')
        });