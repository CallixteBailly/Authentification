import axios from 'axios';
import { AuthenticationApiUrl, key } from '../utils/config';
import CryptoJS from 'crypto-js';

interface Login {
  password: string;
  email: string;
}

interface Token {
  accessToken: string;
  refreshToken: string;
  expiration: Date;
}

interface User {
  firstName: string;
  lastName: string;
  token: string;
  isAuthenticated: boolean;
}

const api = axios.create({
  baseURL: AuthenticationApiUrl,
  timeout: 30000,
  headers: {
    'Content-Type': 'application/json',
  },
});

export const loginUser = async (login: Login): Promise<void> => {
  try {
    const response = await api.post('/authentication/login', { Email: login.email, Password: login.password });
    const { firstName, lastName, token } = response.data;
    const encryptedData = CryptoJS.AES.encrypt(JSON.stringify({ firstName, lastName, token, isAuthenticated: true }), key).toString();
    localStorage.setItem('user', encryptedData);

    api.defaults.headers.common['Authorization'] = `Bearer ${token.accessToken}`;
  } catch (error: any) {
    if (error.response) {
      throw new Error(`Login failed with status ${error.response.status}`);
    } else if (error.request) {
      throw new Error('No response received from server');
    } else {
      throw new Error('Failed to make login request');
    }
  }
};

export const getIsAuthenticated = async (): Promise<User | null> => {
  try {
    const encryptedData = localStorage.getItem('user');
    if (encryptedData) {
      // Decrypt the user data from localStorage
      const decryptedData = CryptoJS.AES.decrypt(encryptedData, key).toString(CryptoJS.enc.Utf8);
      const userData = JSON.parse(decryptedData);
      const response = await api.post('/authentication/verifyToken', { token: userData.token.accessToken });

      return {
        firstName: userData.firstName,
        lastName: userData.lastName,
        token: userData.token.accessToken,
        isAuthenticated: response.data,
      };
    }
    return null;
  } catch (error) {
    throw error;
  }
};
