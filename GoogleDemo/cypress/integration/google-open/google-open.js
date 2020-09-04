import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
    Then('I should see title Google', () => {
        cy.title()
            .should('include','Google');
        });
	