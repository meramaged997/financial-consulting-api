import { Link } from 'react-router-dom'
import { useAuth } from '../../app/state/auth'

export function RoleSwitcherPage() {
  const auth = useAuth()
  return (
    <div style={{ maxWidth: 720, margin: '0 auto' }}>
      <h1 style={{ marginTop: 0 }}>Role switcher (Demo)</h1>
      <p style={{ color: '#475569' }}>
        حسابك الحالي: <b>{auth.user?.role ?? '—'}</b>
      </p>
      <div style={{ display: 'grid', gap: 10 }}>
        <Link style={tile} to="/dashboard">
          Founder routes
        </Link>
        <Link style={tile} to="/consultant/dashboard">
          Consultant routes
        </Link>
        <Link style={tile} to="/admin/dashboard">
          Admin routes
        </Link>
      </div>
      <p style={{ marginTop: 14, color: '#475569' }}>
        لو فتحتي صفحة مش بتاعة دورك هيرجعك هنا.
      </p>
    </div>
  )
}

const tile: React.CSSProperties = {
  display: 'block',
  padding: '12px 14px',
  borderRadius: 14,
  background: '#f0fdfa',
  border: '1px solid rgba(15,118,110,0.25)',
  color: '#0f766e',
  fontWeight: 850,
  textDecoration: 'none',
}

