export interface ILoginRequest {
    email: string | null
    password: string | null
}

export interface IVerifyTokenRequest {
    token: string[]
}

export interface IUser {
    userId: string;
    firstName: string | null;
    lastName: string | null;
    email: string | null;
    accessToken: string | null;
    refreshToken: string | null;
    tokenExpiration: string | null;
    isAuthenticated: boolean | null;
}