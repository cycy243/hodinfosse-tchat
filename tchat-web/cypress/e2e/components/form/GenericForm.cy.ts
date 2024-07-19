import { mount } from 'cypress/vue'
import { GenericForm } from '../../../../src/components/form/GenericForm.vue'

describe('GenericForm cypress tests', () => {
  it('when mounted then there is a form', () => {
    mount(GenericForm<{ id: string; name: string }>())

    cy.contains('form')
  })
})
