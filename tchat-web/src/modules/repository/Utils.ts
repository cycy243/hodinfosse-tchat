import { ApiResponseType } from './ApiResponseType'

export function apiReponseTypeFromStatusCode(statusCode: number) {
  if (statusCode >= 200 && statusCode < 300) {
    return ApiResponseType.SUCCESS
  }
  if (statusCode === 403) {
    return ApiResponseType.UNAUTHORIZED
  }
  if (statusCode >= 400 && statusCode < 500) {
    return ApiResponseType.UNAUTHORIZED
  }
  return ApiResponseType.API_SERVER_ERROR
}
