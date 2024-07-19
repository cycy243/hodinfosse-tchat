import redaxios, { type Response } from 'redaxios'

import Message from '../model/Message'

export default class MessageRepository {
  private _axiosCli: any

  constructor(axiosCli: any) {
    this._axiosCli = axiosCli
  }

  async sendMessage(
    msgContent: string,
    userId: string,
    token: string
  ): Promise<Message | undefined> {
    try {
      const newMsg = new Message(Math.random().toString(), msgContent, new Date(), userId)
      const response: Response<Message> = await this._axiosCli.post(
        '/api/message',
        {
          content: newMsg.content,
          sendDateTime: (newMsg.sendDateTime as Date).toISOString(),
          userId
        },
        {
          headers: {
            Authorization: `Bearer ${token}`
          }
        }
      )
      console.log(response.data)
      return newMsg
    } catch (err) {
      console.log(err)
      return undefined
    }
  }

  async getMessages(token: string): Promise<Message[] | undefined> {
    try {
      const response: Response<Message[]> = await this._axiosCli.get('/api/message?count=100', {
        headers: {
          Authorization: `Bearer ${token}`
        }
      })

      return response.data
    } catch (err) {
      console.log(err)
      return undefined
    }
  }

  private randomIntFromInterval(min: number, max: number) {
    return Math.floor(Math.random() * (max - min) + min)
  }
}
