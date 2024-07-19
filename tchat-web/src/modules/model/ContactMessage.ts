export default class ContactMessage {
  private _id: string
  private _content: string
  private _subject: string
  private _senderMail: string
  private _response: string

  get response() {
    return this._response
  }

  get id() {
    return this._id
  }

  get content() {
    return this._content
  }

  get subject() {
    return this._subject
  }

  get senderMail() {
    return this._senderMail
  }

  constructor(args?: {
    subject: string
    senderMail: string
    id: string
    content: string
    response: string
  }) {
    this._id = args?.id || ''
    this._content = args?.content || ''
    this._subject = args?.subject || ''
    this._senderMail = args?.senderMail || ''
    this._response = args?.response || ''
  }
}
