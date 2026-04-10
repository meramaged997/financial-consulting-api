import {
  createContext,
  useContext,
  useEffect,
  useMemo,
  useState,
} from 'react'
import type { ReactNode } from 'react'
import { apiFetch } from '../../api'
import type { ApiEnvelope } from '../../api'

type Role = 'StartupFounder' | 'FinancialConsultant' | 'Administrator'

export type AuthUser = {
  userId: string
  fullName: string
  email: string
  phoneNumber: string
  role: Role
  redirectTo?: string
  token: string
  tokenExpiry?: string
}

type AuthState = {
  token: string
  user: AuthUser | null
  login: (email: string, password: string) => Promise<ApiEnvelope<AuthUser>>
  register: (payload: {
    fullName: string
    email: string
    password: string
    confirmPassword: string
    phoneNumber?: string
    role: 1 | 2 | 3
  }) => Promise<ApiEnvelope<AuthUser>>
  logout: () => void
}

const AuthCtx = createContext<AuthState | null>(null)

const LS_TOKEN = 'startawy.token'
const LS_USER = 'startawy.user'

export function AuthProvider({ children }: { children: ReactNode }) {
  const [token, setToken] = useState('')
  const [user, setUser] = useState<AuthUser | null>(null)

  useEffect(() => {
    const t = localStorage.getItem(LS_TOKEN) ?? ''
    const u = localStorage.getItem(LS_USER)
    setToken(t)
    if (u) {
      try {
        setUser(JSON.parse(u) as AuthUser)
      } catch {
        setUser(null)
      }
    }
  }, [])

  const api = useMemo<AuthState>(() => {
    return {
      token,
      user,
      async login(email: string, password: string) {
        const res = await apiFetch<AuthUser>('/api/auth/login', {
          method: 'POST',
          body: JSON.stringify({ email, password }),
        })
        if (res.success && res.data?.token) {
          setToken(res.data.token)
          setUser(res.data)
          localStorage.setItem(LS_TOKEN, res.data.token)
          localStorage.setItem(LS_USER, JSON.stringify(res.data))
        }
        return res
      },
      async register(payload) {
        const res = await apiFetch<AuthUser>('/api/auth/register', {
          method: 'POST',
          body: JSON.stringify(payload),
        })
        if (res.success && res.data?.token) {
          setToken(res.data.token)
          setUser(res.data)
          localStorage.setItem(LS_TOKEN, res.data.token)
          localStorage.setItem(LS_USER, JSON.stringify(res.data))
        }
        return res
      },
      logout() {
        setToken('')
        setUser(null)
        localStorage.removeItem(LS_TOKEN)
        localStorage.removeItem(LS_USER)
      },
    }
  }, [token, user])

  return <AuthCtx.Provider value={api}>{children}</AuthCtx.Provider>
}

export function useAuth() {
  const v = useContext(AuthCtx)
  if (!v) throw new Error('useAuth must be used within AuthProvider')
  return v
}

