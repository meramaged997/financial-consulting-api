import { NavLink, Outlet } from 'react-router-dom'

export function PublicLayout() {
  return (
    <div>
      <header style={styles.header}>
        <div style={styles.brand}>Startawy</div>
        <nav style={styles.nav}>
          <NavLink to="/" style={styles.link}>
            Home
          </NavLink>
          <NavLink to="/plans" style={styles.link}>
            Plans
          </NavLink>
          <NavLink to="/login" style={styles.link}>
            Login
          </NavLink>
          <NavLink to="/signup" style={styles.primaryLink}>
            Sign up
          </NavLink>
        </nav>
      </header>
      <main style={styles.main}>
        <Outlet />
      </main>
    </div>
  )
}

const styles: Record<string, React.CSSProperties> = {
  header: {
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'space-between',
    padding: '14px 18px',
    borderBottom: '1px solid rgba(0,0,0,0.08)',
    position: 'sticky',
    top: 0,
    background: '#fff',
    zIndex: 10,
  },
  brand: { fontWeight: 800, letterSpacing: 0.2, color: '#0f766e' },
  nav: { display: 'flex', gap: 12, alignItems: 'center' },
  link: { color: '#0f172a', textDecoration: 'none', fontWeight: 600 },
  primaryLink: {
    color: '#fff',
    background: '#0f766e',
    textDecoration: 'none',
    fontWeight: 700,
    padding: '8px 12px',
    borderRadius: 10,
  },
  main: { maxWidth: 1100, margin: '0 auto', padding: 18 },
}

