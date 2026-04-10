import { useMemo, useState } from 'react'
import type { FormEvent } from 'react'
import { Link, useLocation, useNavigate } from 'react-router-dom'
import { useAuth } from '../../app/state/auth'
import { apiFetch } from '../../api'

export function LoginPage() {
  const auth = useAuth()
  const nav = useNavigate()
  const loc = useLocation()
  const from = (loc.state as any)?.from as string | undefined

  const [email, setEmail] = useState('test@example.com')
  const [password, setPassword] = useState('Test@1234')
  const [msg, setMsg] = useState<string>('')
  const [busy, setBusy] = useState(false)

  const canSubmit = useMemo(
    () => email.trim().length > 3 && password.trim().length > 3 && !busy,
    [busy, email, password],
  )

  async function onSubmit(e: FormEvent) {
    e.preventDefault()
    setBusy(true)
    setMsg('')
    try {
      const res = await auth.login(email, password)
      if (!res.success) {
        setMsg(res.message)
        return
      }
      const redirectTo = res.data?.redirectTo
      nav(redirectTo || from || '/', { replace: true })
    } finally {
      setBusy(false)
    }
  }

  async function forgotPassword() {
    setBusy(true)
    setMsg('')
    try {
      const res = await apiFetch<any>('/api/auth/forgot-password', {
        method: 'POST',
        body: JSON.stringify({ email }),
      })
      setMsg(res.message + (res.data?.resetToken ? ` (token: ${res.data.resetToken})` : ''))
    } finally {
      setBusy(false)
    }
  }

  async function social(provider: 'google' | 'facebook') {
    setMsg(`External login (${provider}) يحتاج إعداد OAuth على السيرفر.`)
    await apiFetch<any>(`/api/auth/external/${provider}`, {
      method: 'POST',
      body: JSON.stringify({ token: 'frontend-demo-token' }),
    })
  }

  return (
    <div style={styles.wrap}>
      <div style={styles.card}>
        <h1 style={styles.h1}>Login</h1>
        <p style={styles.p}>ادخلي بياناتك عشان توصلي للـDashboard حسب دورك.</p>

        <form onSubmit={onSubmit} style={{ display: 'grid', gap: 10 }}>
          <label style={styles.label}>
            Email
            <input
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              style={styles.input}
              autoComplete="email"
            />
          </label>
          <label style={styles.label}>
            Password
            <input
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              style={styles.input}
              type="password"
              autoComplete="current-password"
            />
          </label>

          <button disabled={!canSubmit} style={styles.primary}>
            {busy ? 'Loading…' : 'Login'}
          </button>
        </form>

        <div style={{ display: 'flex', justifyContent: 'space-between', marginTop: 10 }}>
          <button onClick={forgotPassword} style={styles.linkBtn} disabled={busy}>
            Forgot password?
          </button>
          <Link to="/signup" style={styles.link}>
            Create account
          </Link>
        </div>

        <div style={styles.divider} />

        <div style={{ display: 'grid', gap: 10 }}>
          <button onClick={() => social('google')} style={styles.oauth}>
            Continue with Google
          </button>
          <button onClick={() => social('facebook')} style={styles.oauth}>
            Continue with Facebook
          </button>
        </div>

        {msg ? <div style={styles.msg}>{msg}</div> : null}
      </div>
    </div>
  )
}

const styles: Record<string, React.CSSProperties> = {
  wrap: { display: 'grid', placeItems: 'center', minHeight: '70vh' },
  card: {
    width: 'min(520px, 100%)',
    border: '1px solid rgba(2,6,23,0.08)',
    background: '#fff',
    borderRadius: 18,
    padding: 16,
  },
  h1: { margin: 0, color: '#0f172a' },
  p: { margin: '6px 0 12px', color: '#475569' },
  label: { display: 'grid', gap: 6, fontWeight: 700, color: '#0f172a' },
  input: {
    padding: '10px 12px',
    borderRadius: 12,
    border: '1px solid rgba(2,6,23,0.12)',
    outline: 'none',
  },
  primary: {
    padding: '10px 12px',
    borderRadius: 12,
    border: 'none',
    background: '#0f766e',
    color: '#fff',
    fontWeight: 900,
    cursor: 'pointer',
  },
  link: { color: '#0f766e', fontWeight: 800, textDecoration: 'none' },
  linkBtn: {
    border: 'none',
    background: 'transparent',
    color: '#0f766e',
    fontWeight: 800,
    cursor: 'pointer',
    padding: 0,
  },
  divider: { height: 1, background: 'rgba(2,6,23,0.08)', margin: '14px 0' },
  oauth: {
    padding: '10px 12px',
    borderRadius: 12,
    border: '1px solid rgba(2,6,23,0.12)',
    background: '#f8fafc',
    color: '#0f172a',
    fontWeight: 850,
    cursor: 'pointer',
  },
  msg: {
    marginTop: 12,
    padding: 10,
    borderRadius: 12,
    background: '#f0fdfa',
    border: '1px solid rgba(15,118,110,0.25)',
    color: '#0f766e',
    fontWeight: 750,
    whiteSpace: 'pre-wrap',
  },
}

