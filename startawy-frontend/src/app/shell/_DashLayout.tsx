import { NavLink, Outlet } from 'react-router-dom'
import { useAuth } from '../state/auth'

export function DashLayout({
  title,
  links,
}: {
  title: string
  links: { to: string; label: string }[]
}) {
  const auth = useAuth()
  return (
    <div style={styles.wrap}>
      <aside style={styles.aside}>
        <div style={styles.brand}>{title}</div>
        <div style={styles.user}>
          <div style={{ fontWeight: 800, color: '#0f172a' }}>
            {auth.user?.fullName ?? '—'}
          </div>
          <div style={{ color: '#475569', fontSize: 12 }}>{auth.user?.role}</div>
        </div>
        <nav style={styles.nav}>
          {links.map((l) => (
            <NavLink
              key={l.to}
              to={l.to}
              style={({ isActive }) => ({
                ...styles.link,
                ...(isActive ? styles.linkActive : null),
              })}
            >
              {l.label}
            </NavLink>
          ))}
        </nav>
        <button style={styles.logout} onClick={() => auth.logout()}>
          Logout
        </button>
      </aside>
      <main style={styles.main}>
        <Outlet />
      </main>
    </div>
  )
}

const styles: Record<string, React.CSSProperties> = {
  wrap: {
    display: 'grid',
    gridTemplateColumns: '270px 1fr',
    minHeight: '100vh',
    background: '#f8fafc',
  },
  aside: {
    padding: 16,
    borderRight: '1px solid rgba(2,6,23,0.08)',
    background: '#fff',
    position: 'sticky',
    top: 0,
    height: '100vh',
    overflow: 'auto',
  },
  brand: { fontWeight: 900, color: '#0f766e', marginBottom: 10 },
  user: {
    padding: 12,
    border: '1px solid rgba(2,6,23,0.08)',
    borderRadius: 14,
    marginBottom: 14,
    background: '#f0fdfa',
  },
  nav: { display: 'flex', flexDirection: 'column', gap: 6, marginBottom: 14 },
  link: {
    textDecoration: 'none',
    color: '#0f172a',
    padding: '10px 10px',
    borderRadius: 12,
    fontWeight: 650,
  },
  linkActive: { background: '#ccfbf1', color: '#0f766e' },
  main: { padding: 18 },
  logout: {
    width: '100%',
    padding: '10px 12px',
    borderRadius: 12,
    border: '1px solid rgba(2,6,23,0.12)',
    background: '#fff',
    fontWeight: 700,
    cursor: 'pointer',
  },
}

