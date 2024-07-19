import { type Response } from 'redaxios'
import type User from '../model/User'
import type RegisterDto from '../dto/RegisterDto'
import ApiResponse from './ApiResponse'
import { ApiResponseType } from './ApiResponseType'
import { apiReponseTypeFromStatusCode } from './Utils'

export default class UserRepository {
  public token: string | undefined
  private _axiosCli: any

  constructor(axiosCli: any) {
    this._axiosCli = axiosCli
  }

  async authUserWithGoogle(token: string): Promise<ApiResponse<User | undefined>> {
    try {
      const response: Response<User> = await this._axiosCli.post('/api/auth/google', {
        token
      })
      return new ApiResponse(ApiResponseType.SUCCESS, 'Sucessfull register', response.data)
    } catch (err) {
      return new ApiResponse(
        apiReponseTypeFromStatusCode(parseInt(`${(err as any).status}`)),
        (err as any)?.data?.detail,
        undefined
      )
    }
  }

  async login(login: string, password: string): Promise<ApiResponse<User | undefined>> {
    try {
      const response: Response<User> = await this._axiosCli.post('/api/auth/login', {
        email: login,
        password
      })
      return new ApiResponse(ApiResponseType.SUCCESS, 'Sucessfull register', response.data)
    } catch (err) {
      return new ApiResponse(
        apiReponseTypeFromStatusCode(parseInt(`${(err as any).status}`)),
        (err as any)?.data?.detail,
        undefined
      )
    }
  }

  async register(dto: RegisterDto): Promise<ApiResponse<User | undefined>> {
    try {
      const date = new Date(dto.birthdate)
      const response: Response<User> = await this._axiosCli.post('/api/auth/register', {
        lastname: dto.lastname,
        firstname: dto.firstname,
        birthdate: `${date.getDate() < 10 ? '0' + date.getDate() : date.getDate()}/${date.getMonth() < 10 ? '0' + date.getMonth() : date.getMonth()}/${date.getFullYear()}`,
        username: dto.username,
        email: dto.email,
        password: dto.password
      })
      return new ApiResponse(ApiResponseType.SUCCESS, 'Sucessfull register', response.data)
    } catch (err) {
      return new ApiResponse(
        apiReponseTypeFromStatusCode(parseInt(`${(err as any).status}`)),
        (err as any)?.data?.detail,
        undefined
      )
    }
  }

  async requestResetPassword(email: string): Promise<Boolean | undefined> {
    try {
      const response: Response<User> = await this._axiosCli.get(`/api/user/password?email=${email}`)
      return response.status === 200
    } catch (err) {
      return undefined
    }
  }

  async getAll(token: string): Promise<ApiResponse<User[] | undefined>> {
    try {
      const response: Response<User[]> = await this._axiosCli.get(`/api/user`, {
        headers: {
          Authorization: `Bearer ${token}`
        }
      })
      return new ApiResponse(ApiResponseType.SUCCESS, 'success', response.data)
    } catch (err) {
      return new ApiResponse(
        apiReponseTypeFromStatusCode(parseInt(`${(err as any).status}`)),
        (err as any)?.data?.detail,
        undefined
      )
    }
  }

  async delete(id: string, token: string): Promise<ApiResponse<boolean>> {
    try {
      await this._axiosCli.delete(`/api/user/${id}`, {
        headers: {
          Authorization: `Bearer ${token}`
        }
      })
      return new ApiResponse(ApiResponseType.SUCCESS, 'success', true)
    } catch (err) {
      return new ApiResponse(
        apiReponseTypeFromStatusCode(parseInt(`${(err as any).status}`)),
        (err as any)?.data?.detail,
        false
      )
    }
  }

  async confirmResetPassword(
    email: string,
    code: string,
    newPassword: string
  ): Promise<Boolean | undefined> {
    try {
      const response: Response<User> = await this._axiosCli.post(`/api/user/password`, {
        email,
        code,
        newPassword
      })
      return response.status === 200
    } catch (err) {
      return undefined
    }
  }

  async addUser(user: User, token: string): Promise<ApiResponse<User | undefined>> {
    try {
      const response: Response<User> = await this._axiosCli.post(
        '/api/user',
        {
          userName: user.userName,
          lastName: user.lastName,
          firstName: user.firstName,
          email: user.email
        },
        {
          headers: {
            Authorization: `Bearer ${token}`
          }
        }
      )
      return new ApiResponse(ApiResponseType.SUCCESS, 'Sucessfull register', response.data)
    } catch (err) {
      return new ApiResponse(
        apiReponseTypeFromStatusCode(parseInt(`${(err as any).status}`)),
        (err as any)?.data?.detail,
        undefined
      )
    }
  }

  async editUser(user: User, token: string): Promise<ApiResponse<User | undefined>> {
    try {
      const response: Response<User> = await this._axiosCli.post(
        `/api/user/${user.id}`,
        {
          userName: user.userName,
          lastName: user.lastName,
          firstName: user.firstName,
          email: user.email
        },
        {
          headers: {
            Authorization: `Bearer ${token}`
          }
        }
      )
      return new ApiResponse(ApiResponseType.SUCCESS, 'Sucessfull register', response.data)
    } catch (err) {
      return new ApiResponse(
        apiReponseTypeFromStatusCode(parseInt(`${(err as any).status}`)),
        (err as any)?.data?.detail,
        undefined
      )
    }
  }
}
