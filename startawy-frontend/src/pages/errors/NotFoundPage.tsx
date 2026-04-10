import { Link } from 'react-router-dom'

export function NotFoundPage() {
  return (
    <div style={{ maxWidth: 720, margin: '0 auto' }}>
      <h1 style={{ marginTop: 0 }}>404</h1>
      <p style={{ color: '#475569' }}>الصفحة غير موجودة.</p>
      <Link to="/" style={styles.link}>
        Back to home
      </Link>
    </div>
  )
}

const styles: Record<string, React.CSSProperties> = {
  link: {
    display: 'inline-block',
    marginTop: 10,
    textDecoration: 'none',
    fontWeight: 850,
    padding: '10px 12px',
    borderRadius: 12,
    background: '#0f766e',
    color: '#fff',
  },
}

