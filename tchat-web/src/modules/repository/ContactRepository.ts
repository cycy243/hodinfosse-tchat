import ContactMessage from '../model/ContactMessage'
import ApiResponse from './ApiResponse'
import { ApiResponseType } from './ApiResponseType'
import { apiReponseTypeFromStatusCode } from './Utils'

import { type Response } from 'redaxios'

export default class ContactRepository {
  private _axiosCli: any

  constructor(axiosCli: any) {
    this._axiosCli = axiosCli
  }

  async sendMessage(
    senderMail: string,
    senderFirstname: string,
    senderLastname: string,
    content: string,
    subject: string
  ): Promise<ApiResponse<boolean>> {
    try {
      const response: Response<boolean> = await this._axiosCli.post('/api/contact/message', {
        senderEmail: senderMail,
        senderName: senderLastname,
        senderFirstname,
        subject,
        content
      })
      return new ApiResponse(ApiResponseType.SUCCESS, 'Message successfully sent', true)
    } catch (err) {
      return new ApiResponse(
        apiReponseTypeFromStatusCode(parseInt(`${(err as any).status}`)),
        (err as any)?.data?.detail,
        false
      )
    }
  }

  async getMessages(
    count: number | undefined = undefined,
    token: string,
    getDeleted: boolean = false
  ): Promise<ApiResponse<ContactMessage[]>> {
    try {
      const response: Response<ContactMessage[]> = await this._axiosCli.get(`/api/contact/all`, {
        params: {
          count: count || '',
          isDeleted: getDeleted
        },
        headers: {
          Authorization: `Bearer ${token}`
        }
      })
      return new ApiResponse(ApiResponseType.SUCCESS, 'Message successfully sent', response.data)
    } catch (err) {
      return new ApiResponse(
        apiReponseTypeFromStatusCode(parseInt(`${(err as any).status}`)),
        (err as any)?.data?.detail,
        []
      )
    }
  }

  async addResponse(
    message: ContactMessage,
    msgResponse: string,
    token: string
  ): Promise<ApiResponse<ContactMessage | undefined>> {
    try {
      const response: Response<ContactMessage> = await this._axiosCli.put(
        `/api/contact/message`,
        {
          ...message,
          msgResponse
        },
        {
          headers: {
            Authorization: `Bearer ${token}`
          }
        }
      )
      return new ApiResponse(ApiResponseType.SUCCESS, 'Message successfully sent', response.data)
    } catch (err) {
      return new ApiResponse(
        apiReponseTypeFromStatusCode(parseInt(`${(err as any).status}`)),
        (err as any)?.data?.detail,
        undefined
      )
    }
  }

  async deleteMessage(message: ContactMessage, token: string): Promise<ApiResponse<boolean>> {
    try {
      await this._axiosCli.delete(`/api/contact/message/${message.id}`, {
        headers: {
          Authorization: `Bearer ${token}`
        }
      })
      return new ApiResponse(ApiResponseType.SUCCESS, 'Message successfully sent', true)
    } catch (err) {
      return new ApiResponse(
        apiReponseTypeFromStatusCode(parseInt(`${(err as any).status}`)),
        (err as any)?.data?.detail,
        false
      )
    }
  }
}
