import GenericForm from './GenericForm.vue'

describe('<GenericForm />', () => {
  it('renders', () => {
    // see: https://on.cypress.io/mounting-vue
    cy.mount(GenericForm)
  })
})