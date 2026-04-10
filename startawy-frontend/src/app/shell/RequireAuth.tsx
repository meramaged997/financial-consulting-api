import { useEffect } from 'react'
import type { ReactNode } from 'react'
import { useLocation, useNavigate } from 'react-router-dom'
import { useAuth } from '../state/auth'

export function RequireAuth({ children }: { children: ReactNode }) {
  const auth = useAuth()
  const nav = useNavigate()
  const loc = useLocation()

  useEffect(() => {
    if (!auth.token) {
      nav('/login', { replace: true, state: { from: loc.pathname } })
    }
  }, [auth.token, loc.pathname, nav])

  if (!auth.token) return null
  return <>{children}</>
}

