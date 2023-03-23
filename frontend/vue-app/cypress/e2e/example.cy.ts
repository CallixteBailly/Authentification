// https://docs.cypress.io/api/introduction/api.html

describe('My First Test', () => {
  it('visits the app root url', () => {
    cy.visit('/')
    cy.contains('h1', 'Hello World !')
  })
})

describe('Test Page login', () => {
  it('visits the app root url', () => {
    cy.visit('/login')
    cy.contains('h1', 'Se connecter')
  })
})
