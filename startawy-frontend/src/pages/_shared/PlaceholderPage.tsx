import { useLocation, useParams } from 'react-router-dom'

export function PlaceholderPage({ title }: { title: string }) {
  const loc = useLocation()
  const params = useParams()
  return (
    <div style={{ maxWidth: 900 }}>
      <h1 style={{ marginTop: 0 }}>{title}</h1>
      <p style={{ color: '#475569' }}>
        الصفحة دي جاهزة للـUI والربط بالـAPI. (Route: <code>{loc.pathname}</code>)
      </p>
      {Object.keys(params).length ? (
        <pre style={styles.pre}>{JSON.stringify(params, null, 2)}</pre>
      ) : null}
    </div>
  )
}

const styles: Record<string, React.CSSProperties> = {
  pre: {
    background: '#0b1020',
    color: '#d6e2ff',
    padding: 12,
    borderRadius: 12,
    overflowX: 'auto',
  },
}

