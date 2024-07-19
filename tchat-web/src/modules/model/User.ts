export default class User {
  private _name: string
  private _id: string
  private _firstname: string
  private _email: string
  private _userName: string
  private _token: string
  private _roles: string[]

  public get lastName(): string {
    return this._name
  }

  public get id(): string {
    return this._id
  }

  public get firstName(): string {
    return this._firstname
  }

  public get email(): string {
    return this._email
  }

  public get userName(): string {
    return this._userName
  }

  public get token(): string {
    return this._token
  }

  public get isAdmin(): boolean {
    return this._roles.findIndex((v) => v.toLowerCase() === 'admin') !== -1
  }

  public get roles(): string[] {
    return Array.from(this._roles)
  }

  constructor(
    name: string,
    id: string,
    firstname: string,
    userName: string,
    email: string,
    token: string,
    roles: string[] = []
  ) {
    this._name = name
    this._id = id
    this._firstname = firstname
    this._email = email
    this._userName = userName
    this._token = token
    this._roles = roles
  }
}
