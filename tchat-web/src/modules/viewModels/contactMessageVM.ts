import type {
  GridOptions,
  GridProperty,
  GridTextOptions
} from '@/components/grid/GridComponent.vue'
import ContactMessage from '../model/ContactMessage'
import type ContactRepository from '../repository/ContactRepository'
import type ApiResponse from '../repository/ApiResponse'
import type User from '../model/User'

export class ContactMessageVM {
  messages: Array<ContactMessage>
  cols: Array<GridProperty>
  gridOptions: GridOptions
  gridTextOptions: GridTextOptions
  showResponseForm: boolean
  selectedMessage: ContactMessage
  contactRepo: ContactRepository
  user: User

  constructor(contactRepository: ContactRepository, user: User) {
    this.messages = new Array<ContactMessage>()
    this.cols = new Array<GridProperty>(
      { name: 'id', colName: '', toShow: true, isUnique: true },
      { name: 'subject', colName: 'Subject', toShow: true },
      { name: 'senderEmail', colName: "Sender's mail", toShow: true },
      { name: 'content', colName: 'Content', toShow: true },
      { name: 'response', colName: 'Response', inForm: true }
    )
    this.gridOptions = {
      isEditable: true,
      isAddable: false,
      isDeletable: true,
      isRefreshable: true
    }
    this.gridTextOptions = {
      editTxt: 'Answer'
    }
    this.showResponseForm = false
    this.selectedMessage = new ContactMessage()
    this.contactRepo = contactRepository
    this.user = user
  }
}
