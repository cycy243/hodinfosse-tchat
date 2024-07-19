import { ApiResponseType } from './ApiResponseType'

export default class ApiResponse<T> {
  apiResponseType: ApiResponseType = ApiResponseType.SUCCESS
  message: string = ''
  data: T | undefined
  get isSuccess(): boolean {
    return this.apiResponseType === ApiResponseType.SUCCESS
  }

  constructor(apiResponseType: ApiResponseType, message: string, data: T | undefined = undefined) {
    this.apiResponseType = apiResponseType
    this.message = message
    this.data = data
  }
}
