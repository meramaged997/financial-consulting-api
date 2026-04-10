import { Outlet } from 'react-router-dom'
import { AuthProvider } from '../state/auth'

export function AppShell() {
  return (
    <AuthProvider>
      <Outlet />
    </AuthProvider>
  )
}

