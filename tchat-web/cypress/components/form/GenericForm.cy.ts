import { mount } from 'cypress/vue'

import { FieldProperty, GenericForm } from '../../../src/components/form/GenericForm.vue'

import { configure, defineRule } from 'vee-validate'

import { required, email } from '@vee-validate/rules'
import { nextTick } from 'vue'

defineRule('required', required)
defineRule('email', email)

configure({
  generateMessage: (ctx: { rule: { name: string }; field: string }) => {
    const messages = {
      required: `${ctx.field} is required`
    }

    return messages[ctx.rule.name] || 'This field is invalid'
  }
})

describe('GenericForm cypress tests', () => {
  let nameInputField: FieldProperty
  let emailInputField: FieldProperty

  beforeEach(() => {
    nameInputField = {
      inputLabel: 'Name',
      inputType: 'text',
      inputValue: 'Start value',
      inputName: 'name',
      rules: 'required'
    }
    emailInputField = {
      inputLabel: 'Email',
      inputType: 'text',
      inputName: 'email',
      rules: 'required|email'
    }
  })

  describe('form initialisation', () => {
    it('when form has one field then one input is present', () => {
      mount(GenericForm<{ id: string; name: string }>(), {
        props: {
          fieldProperties: [nameInputField]
        }
      })

      cy.get('input[name="name"]').should('exist')
    })
  })
  describe('submit form', () => {
    it('when a field is required and form is submitted then shows error symbol', () => {
      mount(GenericForm<{ id: string; name: string }>(), {
        props: {
          fieldProperties: [nameInputField]
        }
      })
      // cy.get('input[name="name"]').contains('Start value')

      cy.get('input[name="name"]').clear().invoke('val', '')
      cy.get('form').trigger('submit')
      cy.get('.error_message').should('exist')
    })

    it('when a field is not right formatted and form is submitted then shows error symbol', () => {
      mount(GenericForm<{ id: string; name: string }>(), {
        props: {
          fieldProperties: [emailInputField]
        }
      })

      cy.get('input[name="email"]').type('lmjhjkh')

      cy.get('form').trigger('submit')
      cy.get('.error_message').should('exist')
    })

    it("when form with validdata is submitted then it triggers 'itemSave' event", async () => {
      const ParentComponent = {
        components: { MyComponent: GenericForm },
        template: '<MyComponent @submit="handleSubmit" :field-properties="props" />',
        methods: {
          handleSubmit: cy.spy().as('submitSpy')
        },
        data() {
          return {
            props: [emailInputField]
          }
        }
      }
      mount(ParentComponent)

      await nextTick()

      // Wait for the component to be fully rendered
      cy.wait(5000) // Adjust the wait time as needed

      // Log the HTML to debug
      cy.get('form').then(($body) => {
        cy.log($body.html())
      })

      cy.get('input[name="email"]').type('test@test.com')
      cy.get('form').trigger('submit')

      cy.get('@submitSpy').should('have.been.called')
    })
  })
})
