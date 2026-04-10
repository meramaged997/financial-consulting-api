import type { ReactNode } from 'react'
import { Navigate } from 'react-router-dom'
import { useAuth } from '../state/auth'

type Role = 'StartupFounder' | 'FinancialConsultant' | 'Administrator'

export function RoleGate({
  allow,
  children,
}: {
  allow: Role[]
  children: ReactNode
}) {
  const auth = useAuth()
  const role = (auth.user?.role ?? '') as Role

  if (!role) return <Navigate to="/login" replace />
  if (!allow.includes(role)) return <Navigate to="/role-switcher" replace />
  return <>{children}</>
}

