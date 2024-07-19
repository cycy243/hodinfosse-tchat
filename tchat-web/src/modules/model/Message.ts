export default class Message {
  private _id: string
  private _content: string
  private _sentDateTime: Date
  private _userId: string
  private _username: string

  public get userId(): string {
    return this._userId
  }

  public get content(): string {
    return this._content
  }

  public get id(): string {
    return this._id
  }

  public get sendDateTime(): Date {
    return this._sentDateTime
  }

  public get username(): string {
    return this._username
  }

  constructor(
    id: string,
    content: string,
    sentDateTime: Date,
    userId: string = '',
    username: string = ''
  ) {
    this._id = id
    this._content = content
    this._sentDateTime = sentDateTime
    this._userId = userId
    this._username = username
  }
}
